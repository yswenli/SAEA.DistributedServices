using SAEA.Common;
using SAEA.DSClient;
using SAEA.DSModel;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace SAEA.DSTest
{
    class Program
    {
        static RPCServiceProxy _rpcServiceProxy;


        static void Main(string[] args)
        {
            ConsoleHelper.Title = "SAEA.DistributedServices Test";

            _rpcServiceProxy = new RPCServiceProxy($"rpc://127.0.0.1:16886");

            var rpcUrl = ConfigurationManager.AppSettings["dsUrl"];

            ConsoleHelper.WriteLine(rpcUrl);

            ConsoleHelper.WriteLine($"正在进行并行测试...");

            for (int i = 0; i < 1000; i++)
            {
                Work(rpcUrl);
            }

            ConsoleHelper.WriteLine($"并行测试完毕!");

            Console.ReadLine();
        }

        static void Work(string rpcUrl)
        {
            //使用分布式事务
            using (DistributedTransaction distributedTransaction = new DistributedTransaction(rpcUrl))
            {
                var list = new List<ParticipantInfo>();

                var pi1 = new ParticipantInfo()
                {
                    Participant = _rpcServiceProxy.OrderService,
                    Method = "AddOrderTry",
                    Args = new object[] { "aaa", 10 }
                };


                var pi2 = new ParticipantInfo()
                {
                    Participant = _rpcServiceProxy.InventoryService,
                    Method = "ReduceInventoryTry",
                    Args = new object[] { "bbb", 10 }
                };

                list.Add(pi1);

                list.Add(pi2);

                distributedTransaction.Regist(list);
            }
        }

    }
}
