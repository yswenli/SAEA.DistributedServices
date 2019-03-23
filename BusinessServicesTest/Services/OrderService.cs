using SAEA.RPC.Common;
using System;

namespace BusinessServicesTest.Services
{
    [RPCService]
    public class OrderService
    {

        #region tcc

        public bool AddOrderTry(string oderID, int inventoryCount)
        {
            if (TestData.Current.ContainsKey(oderID) && (TestData.Current[oderID] - inventoryCount) >= 0)
                return true;
            else
                return false;
        }

        public bool AddOrderConfirm(string oderID, int inventoryCount)
        {
            TestData.Current[oderID] -= inventoryCount;

            Console.WriteLine("Confirm TestData.Current[oderID]：" + TestData.Current[oderID]);
            return true;
        }


        public bool AddOrderCancel(string oderID, int inventoryCount)
        {
            TestData.Current[oderID] += inventoryCount;

            Console.WriteLine("Cancel TestData.Current[oderID]：" + TestData.Current[oderID]);
            return true;
        }

        #endregion
    }
}
