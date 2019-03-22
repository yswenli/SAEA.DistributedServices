using SAEA.Common;
using SAEA.DSClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAEA.DSTest
{
    class Program
    {
        static int Max = 100;

        static int Total = 0;


        static void Main(string[] args)
        {
            ConsoleHelper.Title = "SAEA.DistributedServices Test";

            var rpcUrl = ConfigurationManager.AppSettings["dsUrl"];

            ConsoleHelper.WriteLine(rpcUrl);

            ConsoleHelper.WriteLine($"Max:{Max} Total:{Total}");

            ConsoleHelper.WriteLine($"正在进行并行测试...");

            Parallel.For(0, 1000, (i) =>
            {
                Work(rpcUrl);
            });

            ConsoleHelper.WriteLine($"并行测试完毕!");

            ConsoleHelper.WriteLine($"Max:{Max} Total:{Total}");

            Console.ReadLine();
        }

        static void Calc()
        {
            Total++;
            Max--;
        }

        static void Cancel()
        {
            Total--;
            Max++;
        }

        static void Work(string rpcUrl)
        {
            //使用分布式事务
            using (DistributedTransaction distributedTransaction = new DistributedTransaction(rpcUrl))
            {
                distributedTransaction.Commit(() =>
                {
                    if (Max > 0) return true;
                    return false;
                }, () =>
                {
                    Calc();
                    return true;
                }, () =>
                {
                    Cancel();
                    return true;
                });
            }

        }

    }
}
