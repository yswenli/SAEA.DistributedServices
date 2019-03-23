/****************************************************************************
*项目名称：SAEA.DistributedServices.Host
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices.Host
*类 名 称：DistributedHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/22 14:03:19
*描述：
*=====================================================================
*修改时间：2019/3/22 14:03:19
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.DistributedServices.Host
{
    static class DistributedHelper
    {
        public static void TansactionServiceInit()
        {
            //开发时，用于saea.rpc 客户端代码生成
            GenerateCodeHelper.GenerateCode();

            var config = DSConfigBuilder.Read();

            if (config == null)
            {
                config = new DSConfigBuilder().UsePort().SetMaster().Build();
            }

            TransactionServiceManager.Init(config);
        }

    }
}
