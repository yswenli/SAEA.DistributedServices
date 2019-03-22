/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2019-03-22 09:48:50
*******/

using SAEA.Common;
using SAEA.DSClient.Consumer.Model;
using SAEA.DSClient.Consumer.Service;
using SAEA.RPC.Consumer;
using System;

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
            _DS = new DSService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr(name, ex);
        }
        DSService _DS;
        public DSService DSService
        {
            get { return _DS; }
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
        public Boolean RegistTransaction(Transaction transaction)
        {
            return _serviceConsumer.RemoteCall<Boolean>("DSService", "RegistTransaction", transaction);
        }
    }
}


namespace SAEA.DSClient.Consumer.Model
{
    public class Transaction
    {
        public String ID
        {
            get; set;
        }
        public TransactionStatus TransactionStatus
        {
            get; set;
        }
        public DateTime Created
        {
            get; set;
        }
    }
}

namespace SAEA.DSClient.Consumer.Model
{
    public enum TransactionStatus : Int32
    {
        None = 0,
        Try = 1,
        Confirm = 2,
        Cancel = 3,
    }
}