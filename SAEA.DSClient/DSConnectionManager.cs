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
    class DSConnectionManager
    {
        RPCServiceProxy _rpcServiceProxy = null;

        public DSConnectionManager(string rpcUrl)
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

        private static void _rpcServiceProxy_OnErr(string name, Exception ex)
        {
            LogHelper.Error("DSConnectionManager.Init._rpcServiceProxy_OnErr", ex);
        }

        public bool Regist(string id, string location)
        {
            return _rpcServiceProxy.DSService.RegistTransaction(new SAEA.DSModel.Transaction() { ID = id, Location = location, TransactionStatus = SAEA.DSModel.TransactionStatus.Try });
        }

        public bool Commit(string id, string location)
        {
            return _rpcServiceProxy.DSService.Commit(new SAEA.DSModel.Transaction() { ID = id, Location = location, TransactionStatus = SAEA.DSModel.TransactionStatus.Confirm });
        }

        public void Clear()
        {
            if (_rpcServiceProxy != null && _rpcServiceProxy.IsConnected)
            {
                _rpcServiceProxy.Dispose();
            }
        }
    }
}
