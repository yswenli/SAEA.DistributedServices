using SAEA.Common;
using System.Reflection;

namespace SAEA.DistributedServices.Host
{
    static class Program
    {
        private static readonly string Name = "SAEA.DistributedServices.Host";
        private static readonly string Display = "SAEA.DistributedServices.Host";
        private static readonly string Description = "这是SAEA.DistributedServices 服务，关闭此服务将无法使用SAEA.DistributedServices。";
        private static readonly string FilePath = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
#if DEBUG
            ConsoleHelper.Title = "SAEA.DistributedServices.Host";

            DistributedHelper.TansactionServiceInit();

            ConsoleHelper.WriteLine("SAEA.DistributedServices 已启动...");

            ConsoleHelper.WriteLine("回车结束服务");

            ConsoleHelper.ReadLine();
#else


            if (args.Length != 0)
            {
                switch (args[0].ToUpper())
                {
                    case "/I":
                        WinService.WinServiceHelper.InstallAndStart(FilePath, Name, Display, Description);
                        return;
                    case "/U":
                        WinService.WinServiceHelper.Unstall(Name);
                        return;
                    default:
                        ConsoleHelper.WriteLine("args:");
                        ConsoleHelper.WriteLine("\t/i\t\t 安装服务");
                        ConsoleHelper.WriteLine("\t/u\t\t 卸载服务");
                        return;
                }
            }
            else
                new Service1().Run();
#endif

        }
    }
}
