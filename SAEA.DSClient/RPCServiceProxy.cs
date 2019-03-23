/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2019-03-22 09:48:50
*******/

using SAEA.Common;
using SAEA.DSClient.Consumer.Service;
using SAEA.RPC.Consumer;
using System;

namespace SAEA.DSClient.Consumer
{
    public class RPCServiceProxy : IDisposable
    {
        public event ExceptionCollector.OnErrHander OnErr;

        ServiceConsumer _serviceConsumer;

        public RPCServiceProxy(string uri = "rpc://127.0.0.1:39654") : this(uri, 4, 5, 10 * 1000) { }

        public RPCServiceProxy(string uri, int links = 4, int retry = 5, int timeOut = 10 * 1000)
        {
            ExceptionCollector.OnErr += ExceptionCollector_OnErr;
            _serviceConsumer = new ServiceConsumer(new Uri(uri), links, retry, timeOut);
            _DS = new DSService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr?.Invoke(name, ex);
        }
        DSService _DS;
        public DSService DSService
        {
            get { return _DS; }
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
    public class DSService
    {
        ServiceConsumer _serviceConsumer;
        public DSService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public Boolean IsMaster()
        {
            return _serviceConsumer.RemoteCall<Boolean>("DSService", "IsMaster", null);
        }
        public Boolean RegistTransaction(SAEA.DSModel.Transaction transaction)
        {
            return _serviceConsumer.RemoteCall<Boolean>("DSService", "RegistTransaction", transaction);
        }

        public bool Commit(SAEA.DSModel.Transaction transaction)
        {
            return _serviceConsumer.RemoteCall<Boolean>("DSService", "Commit", transaction);
        }
    }
}