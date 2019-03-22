/****************************************************************************
*项目名称：SAEA.DistributedServices
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices
*类 名 称：DSConfigBuilder
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/21 17:29:56
*描述：
*=====================================================================
*修改时间：2019/3/21 17:29:56
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using SAEA.Common;
using SAEA.DSModel;
using System;

namespace SAEA.DistributedServices
{
    public class DSConfigBuilder
    {

        #region 配置工具类

        static string _filePath = PathHelper.GetFullName("DSConfig.json");

        public static DSConfig Read()
        {
            DSConfig result = null;

            try
            {
                var json = FileHelper.ReadString(_filePath);

                result = SerializeHelper.Deserialize<DSConfig>(json);
            }
            catch (Exception ex)
            {
                LogHelper.Error("DSConfigBuilder.Read", ex);
            }
            return result;
        }


        public static void Save(DSConfig dsConfig)
        {
            try
            {
                if (dsConfig != null)
                {
                    var json = SerializeHelper.Serialize(dsConfig);

                    FileHelper.WriteString(_filePath, json);

                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DSConfigBuilder.Save", ex);
            }
        }

        #endregion

        DSConfig _dsConfig = new DSConfig();


        public DSConfigBuilder UsePort(int port = 16883)
        {
            _dsConfig.Port = port;
            return this;
        }

        public DSConfigBuilder SetMaster()
        {
            _dsConfig.IsMaster = true;
            return this;
        }

        public DSConfigBuilder SetSlave(string ipPortStr)
        {
            if (string.IsNullOrEmpty(ipPortStr)) throw new Exception("设置丛操作配置有误！");
            _dsConfig.IsMaster = false;
            _dsConfig.MasterIpPortStr = ipPortStr;
            return this;
        }


        public DSConfig Build()
        {
            Save(_dsConfig);
            return _dsConfig;
        }
    }
}
