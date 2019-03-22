/****************************************************************************
*项目名称：SAEA.DSClient
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DSClient
*类 名 称：DistributedTransaction
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 14:48:36
*描述：
*=====================================================================
*修改时间：2019/3/21 14:48:36
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Diagnostics;

namespace SAEA.DSClient
{
    /// <summary>
    /// 分布式事务
    /// </summary>
    public class DistributedTransaction : IDisposable
    {
        string _id = string.Empty;

        public DistributedTransaction(string rpcUrl)
        {
            var st1 = new StackTrace(1, true);

            var method = st1.GetFrame(0).GetMethod();

            var className = method.DeclaringType.FullName;

            _id = $"{className}_{method.Name}";

            DSConnectionManager.Init(rpcUrl);
        }

        public void Commit(Func<bool> tryFunc, Func<bool> confirmFunc, Func<bool> cancelFunc = null)
        {
            if (string.IsNullOrWhiteSpace(_id)) throw new Exception("当前事务已被释放!");

            var result = false;

            if (DSConnectionManager.Try(_id))
            {
                result = tryFunc.Invoke();
            }

            if (result)
            {
                if (confirmFunc.Invoke())
                {
                    if (!DSConnectionManager.Confirm(_id))
                    {
                        if (cancelFunc != null && cancelFunc.Invoke())
                        {
                            DSConnectionManager.Cancel(_id);
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            _id = string.Empty;
        }
    }
}
