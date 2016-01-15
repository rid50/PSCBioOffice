using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Threading;
using System.Collections;
using Microsoft.ApplicationServer.Caching;

namespace FillAppFabricCache
{
    class StateObject
    {
        public int LoopCounter;
        public CancellationToken ct;
        public DataCache cache;
        public string regionName;
    }

    class Program
    {
        private static DataCache _cache;

        static Program()
        {
            DataCacheFactory factory = new DataCacheFactory();
            _cache = factory.GetCache("default");
            //Debug.Assert(_cache == null);
        }

        //[STAThread]
        static void Main(string[] args)
        {
            try
            {
                Run(args);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());

                //MessageBox.Show("Error. Details: " + ex.Message, "Fingers Sample", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //static void Run(NFExtractor NFExtractor)
        static void Run(string[] args)
        {
            Stopwatch st = new Stopwatch();

            Int32 rowcount = 0;
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    st.Start();
                    rowcount = FillAppFabricCache.rowcount();
                    break;
                }
                catch (SqlException)
                {
                    Console.WriteLine("Time out, try again ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                finally
                {
                    st.Stop();
                }
            }

            if (rowcount == 0)
                return;

            Console.WriteLine("Row count: " + rowcount);

            int limit = 100;
            int topindex = (int)(rowcount/limit + 1);
            //topindex = 100;
            Task<int>[] taskArray = new Task<int>[topindex];
            //Task[] taskArray = new Task[1];
            int offset = 0;

            Stopwatch stw = new Stopwatch();
            stw.Start();

            bool go = false;
            if (args != null && args.Length != 0)
            {
                if (Int32.TryParse(args[0], out offset))
                {
                    if (offset < topindex)
                    {
                        offset *= limit;
                        limit = 1000;
                        taskArray = new Task<int>[10];
                        limit = 10000;
                        taskArray = new Task<int>[1];
                        go = true;
                    }

                    //Console.WriteLine(offset);
                }

                if (!go)
                {
                    Console.WriteLine(" --- Wrong parameter value, press any key to close ---");
                    Console.ReadKey();
                    return;
                }
            }

            var tokenSource = new CancellationTokenSource();
            CancellationToken ct = tokenSource.Token;

            ArrayList regionNameList;

            if (_cache.Get("regionNameList") != null)
            {
                regionNameList = _cache.Get("regionNameList") as ArrayList;
            }
            else
            {
                throw new Exception("Cache is empty");
            }

            taskArray = new Task<int>[regionNameList.Count];
            //Task<UInt32>[] taskArray = new Task<UInt32>[2];
            int retcode = 0;
            //int i = 0;
            if (true)
            {
                int i = 0;
                //for (int i = 0; i < taskArray.Length; i++)
                foreach (string regionName in regionNameList)
                {
                    taskArray[i++] = Task.Factory.StartNew((Object obj) =>
                    {
                        StateObject state = obj as StateObject;

                        var process = new FillAppFabricCache(state.ct, state.cache);
                        //try
                        //{
                        //process.run(state.LoopCounter * limit + offset, state.LoopCounter * limit + limit, limit - offset, Thread.CurrentThread.ManagedThreadId);
                        //process.run(state.LoopCounter * limit + 90000, state.LoopCounter * limit + limit, limit);

                        //retcode = process.run(state.LoopCounter * limit + offset, state.LoopCounter * limit + limit, limit);
                        //return retcode;
                        //string name = regionName;
                        return process.iterateCache(state.regionName);

                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}
                        //Console.WriteLine(process.run(1, 2, Thread.CurrentThread.ManagedThreadId));
                    },
                    new StateObject() { LoopCounter = i, ct = ct, cache = _cache, regionName = regionName },
                    tokenSource.Token,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
                }

                try
                {
                    Task.WaitAll(taskArray);
                    foreach (var t in taskArray)
                    {
                        if (t.Status == TaskStatus.RanToCompletion && (UInt32)t.Result != 0)
                            retcode = (int)t.Result;
                    }

                    //Console.WriteLine(" ----- Time elapsed: {0}", stw.Elapsed);
                }
                catch (Exception ex)
                {
                    foreach (var t in taskArray)
                    {
                        if (t.Status == TaskStatus.RanToCompletion && (UInt32)t.Result != 0)
                            retcode = (int)t.Result;
                        else if (t.Status == TaskStatus.Faulted)
                        {
                            while ((ex is AggregateException) && (ex.InnerException != null))
                                ex = ex.InnerException;
                            throw new Exception(ex.Message);
                        }

                        //Console.WriteLine("{0,10} {1,20} {2,14}",
                        //                  t.Id, t.Status,
                        //                  t.Status != TaskStatus.Canceled ? t.Status != TaskStatus.Faulted ? t.Result.ToString("N0") : "n/a" : "falted");
                    }

                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    //Console.WriteLine(" ------------------ Press any key to close -----------------------");
                    //Console.ReadKey();
                }
            }
            else
            {
                try
                {
                    var process = new FillAppFabricCache(ct, _cache);
                    for (int i = 0; i < taskArray.Length; i++)
                    {
                        //process.run(state.LoopCounter * limit + offset, state.LoopCounter * limit + limit, limit - offset, Thread.CurrentThread.ManagedThreadId);
                        //process.run(state.LoopCounter * limit + 90000, state.LoopCounter * limit + limit, limit);

                        process.run(i * limit + offset, i * limit + limit, limit);
                    }

                    //stw.Stop();
                    //Console.WriteLine(" ----- Count(*) time elapsed: {0}", st.Elapsed);
                    //Console.WriteLine(" ----- Loop time elapsed: {0}", stw.Elapsed);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Flatten().Message);
                    //throw ex.Flatten();
                    while ((ex is AggregateException) && (ex.InnerException != null))
                        ex = ex.InnerException;

                    Console.WriteLine(ex.ToString());
                }
                finally
                {
//                    Console.WriteLine(" ------------------ Press any key to close -----------------------");
                }
            }

            stw.Stop();
            Console.WriteLine(" ----- Count(*) time elapsed: {0}", st.Elapsed);
            Console.WriteLine(" ----- Loop time elapsed: {0}", stw.Elapsed);
            Console.WriteLine(" ------------------ Press any key to close -----------------------");
            Console.ReadKey();
        }
    }
}
