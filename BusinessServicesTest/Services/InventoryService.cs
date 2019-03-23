using SAEA.RPC.Common;
using System;

namespace BusinessServicesTest.Services
{
    [RPCService]
    public class InventoryService
    {
        #region tcc

        public bool ReduceInventoryTry(string goodsID, int count)
        {
            if (TestData.Current.ContainsKey(goodsID) && (TestData.Current[goodsID] - count) >= 0)
                return true;
            else
                return false;
        }

        public bool ReduceInventoryConfirm(string goodsID, int count)
        {
            TestData.Current[goodsID] -= count;

            Console.WriteLine("Confirm TestData.Current[goodsID]：" + TestData.Current[goodsID]);

            return true;
        }


        public bool ReduceInventoryCancel(string goodsID, int count)
        {
            TestData.Current[goodsID] += count;

            Console.WriteLine("Cancel TestData.Current[goodsID]：" + TestData.Current[goodsID]);
            return true;
        }

        #endregion
    }
}
