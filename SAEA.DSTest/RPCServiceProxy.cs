/*******
* 此代码为SAEA.RPC.Generater生成
* 尽量不要修改此代码 2019-03-23 17:31:42
*******/

using SAEA.Common;
using SAEA.DSTest.Consumer.Service;
using SAEA.RPC.Consumer;
using System;

namespace SAEA.DSTest
{
    public class RPCServiceProxy
    {
        public event ExceptionCollector.OnErrHander OnErr;
        ServiceConsumer _serviceConsumer;
        public RPCServiceProxy(string uri = "rpc://127.0.0.1:16886") : this(uri, 4, 5, 10 * 1000) { }
        public RPCServiceProxy(string uri, int links = 4, int retry = 5, int timeOut = 10 * 1000)
        {
            ExceptionCollector.OnErr += ExceptionCollector_OnErr;
            _serviceConsumer = new ServiceConsumer(new Uri(uri), links, retry, timeOut);
            _In = new InventoryService(_serviceConsumer);
            _Or = new OrderService(_serviceConsumer);
        }
        private void ExceptionCollector_OnErr(string name, Exception ex)
        {
            OnErr?.Invoke(name, ex);
        }
        InventoryService _In;
        public InventoryService InventoryService
        {
            get { return _In; }
        }
        OrderService _Or;
        public OrderService OrderService
        {
            get { return _Or; }
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

namespace SAEA.DSTest.Consumer.Service
{
    public class InventoryService
    {
        ServiceConsumer _serviceConsumer;
        public InventoryService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public Boolean ReduceInventoryTry(String goodsID, Int32 count)
        {
            return _serviceConsumer.RemoteCall<Boolean>("InventoryService", "ReduceInventoryTry", goodsID, count);
        }
        public Boolean ReduceInventoryConfirm(String goodsID, Int32 count)
        {
            return _serviceConsumer.RemoteCall<Boolean>("InventoryService", "ReduceInventoryConfirm", goodsID, count);
        }
        public Boolean ReduceInventoryCancel(String goodsID, Int32 count)
        {
            return _serviceConsumer.RemoteCall<Boolean>("InventoryService", "ReduceInventoryCancel", goodsID, count);
        }
    }
}

namespace SAEA.DSTest.Consumer.Service
{
    public class OrderService
    {
        ServiceConsumer _serviceConsumer;
        public OrderService(ServiceConsumer serviceConsumer)
        {
            _serviceConsumer = serviceConsumer;
        }
        public Boolean AddOrderTry(String oderID, Int32 inventoryCount)
        {
            return _serviceConsumer.RemoteCall<Boolean>("OrderService", "AddOrderTry", oderID, inventoryCount);
        }
        public Boolean AddOrderConfirm(String oderID, Int32 inventoryCount)
        {
            return _serviceConsumer.RemoteCall<Boolean>("OrderService", "AddOrderConfirm", oderID, inventoryCount);
        }
        public Boolean AddOrderCancel(String oderID, Int32 inventoryCount)
        {
            return _serviceConsumer.RemoteCall<Boolean>("OrderService", "AddOrderCancel", oderID, inventoryCount);
        }
    }
}

