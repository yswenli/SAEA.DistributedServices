/****************************************************************************
*项目名称：SAEA.DistributedServices
*CLR 版本：4.0.30319.42000
*机器名称：WENLI-PC
*命名空间：SAEA.DistributedServices
*类 名 称：Locker
*版 本 号：V1.0.0.0
*创建人： yswenli
*电子邮箱：wenguoli_520@qq.com
*创建时间：2019/3/22 15:51:42
*描述：
*=====================================================================
*修改时间：2019/3/22 15:51:42
*修 改 人： yswenli
*版 本 号： V1.0.0.0
*描    述：
*****************************************************************************/
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SAEA.DistributedServices
{
    public static class Locker
    {
        static HashSet<string> _IDs = new HashSet<string>();

        static object _locker = new object();

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static bool Lock(string key, int timeOut = 60 * 1000)
        {
            lock (_locker)
            {
                if (!_IDs.Contains(key))
                {
                    _IDs.Add(key);

                    return true;
                }
                else
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    while (_IDs.Contains(key))
                    {
                        if(stopwatch.ElapsedMilliseconds>= timeOut)
                        {
                            stopwatch.Stop();
                            return false;
                        }
                        Thread.Sleep(10);
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// 开锁
        /// </summary>
        /// <param name="key"></param>
        public static void UnLock(string key)
        {
            _IDs.Remove(key);
        }
    }
}
