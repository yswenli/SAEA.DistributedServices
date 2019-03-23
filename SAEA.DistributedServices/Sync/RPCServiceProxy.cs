/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2019-03-22 09:48:50
*******/

using System;
using System.Collections.Generic;
using SAEA.Common;
using SAEA.RPC.Consumer;
using SAEA.DSClient.Consumer.Service;
using SAEA.DSModel;

namespace SAEA.DSClient.Consumer
{
    public class RPCServiceProxy
    {
        public event ExceptionCollector.OnErrHander OnErr;

        ServiceConsumer _serviceConsumer;

        public RPCServiceProxy(string uri = "rpc://127.0.0.1:39654") : this(uri, 4, 5, 10 * 1000) { }
        public RPCServiceProxy(string uri, int links = 4, int retry = 5, int timeOut = 10 * 1000)
        {
            ExceptionCollector.OnErr += ExceptionCollector_OnErr;
            _serviceConsumer = new ServiceConsumer(new Uri(uri), links, retry, timeOut);
            _Sy = new SyncService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr(name, ex);
        }
       
        SyncService _Sy;
        public SyncService SyncService
        {
            get { return _Sy; }
        }
        public bool IsConnected
        {
            get
            {
                return _serviceConsumer.IsConnected;
            }
        }

        public void Dispose()
        {
            _serviceConsumer.Dispose();
        }
    }
}

namespace SAEA.DSClient.Consumer.Service
{
    public class SyncService
    {
        ServiceConsumer _serviceConsumer;
        public SyncService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public Boolean HeartBean()
        {
            return _serviceConsumer.RemoteCall<Boolean>("SyncService", "HeartBean");
        }
    }
}


