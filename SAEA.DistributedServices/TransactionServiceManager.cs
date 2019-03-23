/****************************************************************************
*项目名称：SAEA.DistributedServices
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices
*类 名 称：TransactionServiceManager
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/20 13:58:11
*描述：
*=====================================================================
*修改时间：2019/3/20 13:58:11
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.DSClient.Consumer;
using SAEA.DSModel;
using SAEA.RPC.Provider;
using System;

namespace SAEA.DistributedServices
{
    public static class TransactionServiceManager
    {
        public static DSConfig DSConfig
        {
            get; private set;
        }

        static ServiceProvider _serviceProvider;

        static RPCServiceProxy _rpcServiceProxy;

        static string MasterRpcUrl = string.Empty;

        public static void Init(DSConfig dsConfig)
        {
            DSConfig = dsConfig;

            _serviceProvider = new ServiceProvider(dsConfig.Port);
            _serviceProvider.OnErr += Sp_OnErr;
            _serviceProvider.Start();

            #region 从同步

            if (!dsConfig.IsMaster)
            {
                MasterRpcUrl = $"rpc://{dsConfig.MasterIpPortStr}";

                _rpcServiceProxy = new RPCServiceProxy(MasterRpcUrl);
                _rpcServiceProxy.OnErr += _rpcServiceProxy_OnErr;

                TaskHelper.Start(() =>
                {
                    try
                    {
                        while (_rpcServiceProxy.SyncService.HeartBean())
                        {
                            ThreadHelper.Sleep(1000);
                        }
                    }
                    catch { }
                    //若断开，则自动升为主
                    dsConfig.IsMaster = true;
                    DSConfigBuilder.Save(dsConfig);
                });
            }

            #endregion
        }

        private static void Sp_OnErr(Exception ex)
        {
            ConsoleHelper.WriteLine("TransactionManager.Init.Sp_OnErr:" + ex.Message);
            LogHelper.Error("TransactionManager.Init.Sp_OnErr", ex);
        }

        private static void _rpcServiceProxy_OnErr(string name, Exception ex)
        {
            ConsoleHelper.WriteLine("TransactionManager.Init._rpcServiceProxy_OnErr:" + ex.Message);
            LogHelper.Error("TransactionManager.Init._rpcServiceProxy_OnErr", ex);
        }
    }
}
