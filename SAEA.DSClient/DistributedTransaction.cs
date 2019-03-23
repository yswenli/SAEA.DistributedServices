/****************************************************************************
*项目名称：SAEA.DSClient
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DSClient
*类 名 称：DistributedTransaction
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 14:48:36
*描述：
*=====================================================================
*修改时间：2019/3/21 14:48:36
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.DSModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SAEA.DSClient
{
    /// <summary>
    /// 分布式事务
    /// </summary>
    public class DistributedTransaction : IDisposable
    {
        string _id = Guid.NewGuid().ToString("N");

        string _location = string.Empty;

        bool hasTransaction = false;

        List<ParticipantInfo> _participantInfos = new List<ParticipantInfo>();

        DSConnectionManager DSConnectionManager;

        public DistributedTransaction(string rpcUrlStr)
        {
            var st1 = new StackTrace(1, true);

            var method = st1.GetFrame(0).GetMethod();

            var className = method.DeclaringType.FullName;

            DSConnectionManager = new DSConnectionManager(rpcUrlStr);

            _location = $"{className}_{method.Name}";

        }

        public bool Regist(List<ParticipantInfo> participantInfos)
        {
            var result = false;

            var id = Guid.NewGuid().ToString("N");


            if (DSConnectionManager.Regist(_id, _location))
            {
                _participantInfos = participantInfos;

                foreach (var item in _participantInfos)
                {
                    item.CanConfirm = (bool)item.Participant.GetType().GetMethod(item.Method).Invoke(item.Participant, item.Args);
                }

                result = hasTransaction = true;
            }

            return result;
        }

        public void Commit()
        {
            if (hasTransaction)
            {
                try
                {
                    foreach (var item in _participantInfos)
                    {
                        if (item.CanConfirm)
                        {
                            var c = (bool)item.Participant.GetType().GetMethod(item.Method.Substring(0, item.Method.Length - 3) + "Confirm").Invoke(item.Participant, item.Args);
                            if (!c)
                            {
                                item.Participant.GetType().GetMethod(item.Method.Substring(0, item.Method.Length - 3) + "Cacnel").Invoke(item.Participant, item.Args);
                            }
                        }
                    }
                }
                catch { }

                try
                {
                    DSConnectionManager.Commit(_id, _location);
                }
                catch { }
            }

        }

        public void Dispose()
        {
            Commit();
            _location = string.Empty;
            DSConnectionManager.Clear();
        }
    }
}
