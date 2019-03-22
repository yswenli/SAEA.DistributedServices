/****************************************************************************
*项目名称：SAEA.DSClient
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DSClient
*类 名 称：DSConnectionManager
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 14:50:46
*描述：
*=====================================================================
*修改时间：2019/3/21 14:50:46
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.DSClient.Consumer;
using System;
using System.Net.Sockets;

namespace SAEA.DSClient
{
    static class DSConnectionManager
    {
        static object _locker = new object();

        static RPCServiceProxy _rpcServiceProxy = null;

        public static void Init(string rpcUrl)
        {
            lock (_locker)
            {
                var rpcUrls = rpcUrl.Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (_rpcServiceProxy == null)
                {
                    foreach (var item in rpcUrls)
                    {
                        try
                        {
                            _rpcServiceProxy = new RPCServiceProxy(item);
                            _rpcServiceProxy.OnErr += _rpcServiceProxy_OnErr;
                            if (_rpcServiceProxy.DSService.IsMaster())
                            {
                                break;
                            }
                        }
                        catch (SocketException sex)
                        {
                            LogHelper.Info($"{item} 连接失败！");
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error("连接到远程服务器失败", ex);
                            throw ex;
                        }
                    }
                }
            }
        }

        private static void _rpcServiceProxy_OnErr(string name, Exception ex)
        {
            LogHelper.Error("DSConnectionManager.Init._rpcServiceProxy_OnErr", ex);
        }

        public static bool Try(string id)
        {
            return _rpcServiceProxy.DSService.RegistTransaction(new Consumer.Model.Transaction() { ID = id, TransactionStatus = Consumer.Model.TransactionStatus.Try });
        }

        public static bool Confirm(string id)
        {
            return _rpcServiceProxy.DSService.RegistTransaction(new Consumer.Model.Transaction() { ID = id, TransactionStatus = Consumer.Model.TransactionStatus.Confirm });
        }

        public static bool Cancel(string id)
        {
            return _rpcServiceProxy.DSService.RegistTransaction(new Consumer.Model.Transaction() { ID = id, TransactionStatus = Consumer.Model.TransactionStatus.Cancel });
        }
    }
}
