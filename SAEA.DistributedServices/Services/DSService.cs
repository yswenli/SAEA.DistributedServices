/****************************************************************************
*项目名称：SAEA.DistributedServices.Services
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices.Services
*类 名 称：DSService
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 14:53:41
*描述：
*=====================================================================
*修改时间：2019/3/21 14:53:41
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.DSModel;
using SAEA.RPC.Common;

namespace SAEA.DistributedServices.Services
{
    [RPCService]
    public class DSService
    {
        public bool IsMaster()
        {
            return TransactionServiceManager.DSConfig.IsMaster;
        }


        public bool RegistTransaction(Transaction transaction)
        {
            if (Locker.Lock(transaction.Location))
            {
                LogHelper.Info(SerializeHelper.Serialize(transaction));

                return true;
            }

            return false;
        }

        public bool Commit(Transaction transaction)
        {
            LogHelper.Info(SerializeHelper.Serialize(transaction));

            Locker.UnLock(transaction.Location);

            return true;
        }
    }
}
