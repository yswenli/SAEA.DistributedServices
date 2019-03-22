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
            var rpcUrl = ConfigurationManager.AppSettings["dsUrl"];

            //使用分布式事务
            using (DistributedTransaction distributedTransaction = new DistributedTransaction(rpcUrl))
            {
                distributedTransaction.Commit(() =>
                {
                    if (Max > 0 && Total < Max) return true;
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

    }
}
