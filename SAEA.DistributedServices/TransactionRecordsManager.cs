/****************************************************************************
*项目名称：SAEA.DistributedServices
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices
*类 名 称：TransactionRecordsManager
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/20 14:37:55
*描述：
*=====================================================================
*修改时间：2019/3/20 14:37:55
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.DSModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAEA.DistributedServices
{
    static class TransactionRecordsManager
    {
        static string _filePath;

        static List<TransactionRecord> _transactionRecords;

        public static List<TransactionRecord> TransactionRecords { get => _transactionRecords; private set => _transactionRecords = value; }

        static TransactionRecordsManager()
        {
            _transactionRecords = new List<TransactionRecord>();
            _filePath = PathHelper.GetFullName("TransactionRecords.json");
        }

        public static void Init()
        {
            _transactionRecords = Read();
        }

        public static void Init(List<TransactionRecord> transactionRecords)
        {
            _transactionRecords = transactionRecords;
        }

        public static string Set(Transaction transaction)
        {
            var result = string.Empty;
            try
            {
                if (transaction != null)
                {
                    transaction.Created = DateTime.Now;

                    var old = _transactionRecords.Where(b => b.ID == transaction.ID).FirstOrDefault();

                    if (old != null && old.Transactions != null && old.Transactions.Any())
                    {
                        var et = old.Transactions.Where(b => b.ID == transaction.ID && b.TransactionStatus == transaction.TransactionStatus).FirstOrDefault();

                        if (et == null)
                        {
                            old.Transactions.Add(transaction);
                            if (Write(_transactionRecords))
                            {
                                result = transaction.ID;

                                var topt = new TransactionOpt()
                                {
                                    Opt = 1,
                                    ID = transaction.ID,
                                    Created = transaction.Created,
                                    TransactionStatus = transaction.TransactionStatus
                                };

                                OnChanged?.BeginInvoke(topt, null, null);
                            }
                        }
                        else
                        {
                            result = transaction.ID;
                        }
                    }
                    else
                    {
                        var tr = new TransactionRecord();
                        tr.ID = transaction.ID;
                        tr.Transactions.Add(transaction);
                        _transactionRecords.Add(tr);
                        if (Write(_transactionRecords))
                        {
                            result = transaction.ID;

                            var topt = new TransactionOpt()
                            {
                                Opt = 0,
                                ID = transaction.ID,
                                Created = transaction.Created,
                                TransactionStatus = transaction.TransactionStatus
                            };

                            OnChanged?.BeginInvoke(topt, null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("TransactionRecordsManager.Add transaction:" + (transaction == null ? "null" : SerializeHelper.Serialize(transaction)), ex);
            }
            return result;
        }


        public static List<Transaction> GetList(string id)
        {
            List<Transaction> result = null;
            try
            {
                var tr = _transactionRecords.Where(b => b.ID == id).FirstOrDefault();

                if (tr != null && tr.Transactions != null && tr.Transactions.Any())
                {
                    result = tr.Transactions;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("TransactionRecordsManager.Get id:" + id, ex);
            }
            return result;
        }


        public static Transaction Get(string id, TransactionStatus status)
        {
            var list = GetList(id);

            if (list != null && list.Any())
            {
                return list.Where(b => b.TransactionStatus == status).FirstOrDefault();
            }

            return null;
        }


        public static void Del(string id)
        {
            try
            {
                var tr = _transactionRecords.Where(b => b.ID == id).FirstOrDefault();

                if (tr != null && tr.Transactions != null && tr.Transactions.Any())
                {
                    var topt = new TransactionOpt()
                    {
                        Opt = 2,
                        ID = tr.ID
                    };

                    OnChanged?.BeginInvoke(topt, null, null);

                    _transactionRecords.Remove(tr);

                    Write(_transactionRecords);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("TransactionRecordsManager.Del id:" + id, ex);
            }
        }

        #region 事务变化相关
        /// <summary>
        /// 将改变同步到本地
        /// </summary>
        /// <param name="transactionOpt"></param>
        public static void Change(TransactionOpt transactionOpt)
        {
            if (transactionOpt != null)
                switch (transactionOpt.Opt)
                {
                    default:
                    case 0:
                    case 1:
                        Set(transactionOpt);
                        break;
                    case 2:
                        Del(transactionOpt.ID);
                        break;
                }
        }


        public static event Action<TransactionOpt> OnChanged;


        static List<TransactionOpt> _changedList = new List<TransactionOpt>();

        static string _changedListLockerKey = "changedListLockerKey";

        public static void AddChangedList(TransactionOpt transactionOpt)
        {
            if (Locker.Lock(_changedListLockerKey))
            {
                _changedList.Add(transactionOpt);
                Locker.UnLock(_changedListLockerKey);
            }
        }

        public static List<TransactionOpt> ChangedList
        {
            get
            {
                if (Locker.Lock(_changedListLockerKey))
                {
                    var list = _changedList.ToList();

                    Locker.UnLock(_changedListLockerKey);

                    return list;
                }
                return null;
            }
        }        

        public static void DelChangedList(List<TransactionOpt> list)
        {
            if (Locker.Lock(_changedListLockerKey))
            {
                foreach (var item in list)
                {
                    _changedList.Remove(item);
                }
            }
        }


        #endregion





        #region read/write

        public static List<TransactionRecord> Read()
        {
            List<TransactionRecord> result;

            try
            {
                var json = FileHelper.ReadString(_filePath);

                result = SerializeHelper.Deserialize<List<TransactionRecord>>(json);
            }
            catch (Exception ex)
            {
                LogHelper.Error("TransactionRecordsManager.Read", ex);
                result = new List<TransactionRecord>();
            }
            return result;
        }


        public static bool Write(List<TransactionRecord> transactionRecords)
        {
            var result = false;
            try
            {
                if (transactionRecords != null)
                {
                    var json = SerializeHelper.Serialize(transactionRecords);
                    FileHelper.WriteString(_filePath, json);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("TransactionRecordsManager.Write", ex);
            }
            return result;
        }

        #endregion



    }
}
