/****************************************************************************
*项目名称：SAEA.DistributedServices
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices
*类 名 称：GenerateCodeHelper
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/22 9:44:27
*描述：
*=====================================================================
*修改时间：2019/3/22 9:44:27
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;

namespace BusinessServicesTest
{
    /// <summary>
    /// 开发时，用于saea.rpc 客户端代码生成
    /// </summary>
    public static class GenerateCodeHelper
    {
        public static void GenerateCode(string nameSpace= "SAEA.DSTest")
        {
            SAEA.RPC.Generater.CodeGnerater.Generate(PathHelper.GetCurrentPath(), nameSpace);
        }
    }
}
