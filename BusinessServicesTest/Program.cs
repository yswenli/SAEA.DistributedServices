using SAEA.Common;
using SAEA.RPC.Provider;
using System;

namespace BusinessServicesTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //开发时，用于saea.rpc 客户端代码生成
            GenerateCodeHelper.GenerateCode();

            ConsoleHelper.Title = "tcc测试业务服务";

            var port = 16886;

            var _serviceProvider = new ServiceProvider(port: port);
            _serviceProvider.OnErr += Sp_OnErr;
            _serviceProvider.Start();

            ConsoleHelper.WriteLine("tcc测试业务服务已启动，url:rpc://127.0.0.1:" + port);

            ConsoleHelper.WriteLine("回车结束服务");

            ConsoleHelper.ReadLine();
        }

        private static void Sp_OnErr(Exception ex)
        {
            ConsoleHelper.WriteLine("tcc测试业务服务异常", ex);
        }
    }
}
