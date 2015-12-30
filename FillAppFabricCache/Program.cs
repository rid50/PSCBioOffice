using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace FillAppFabricCache
{
    class StateObject
    {
        public int LoopCounter;
    }

    class Program
    {
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
            Task[] taskArray = new Task[topindex];
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
                        taskArray = new Task[10];
                        limit = 10000;
                        taskArray = new Task[1];
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

            if (true)
            {
                for (int i = 0; i < taskArray.Length; i++)
                {
                    taskArray[i] = Task.Factory.StartNew((Object obj) =>
                    {
                        StateObject state = obj as StateObject;

                        var process = new FillAppFabricCache();
                        //try
                        //{
                        //process.run(state.LoopCounter * limit + offset, state.LoopCounter * limit + limit, limit - offset, Thread.CurrentThread.ManagedThreadId);
                        //process.run(state.LoopCounter * limit + 90000, state.LoopCounter * limit + limit, limit);

                        process.run(state.LoopCounter * limit + offset, state.LoopCounter * limit + limit, limit);

                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}
                        //Console.WriteLine(process.run(1, 2, Thread.CurrentThread.ManagedThreadId));
                    },
                    new StateObject() { LoopCounter = i });
                }

                try
                {
                    Task.WaitAll(taskArray);
                    //                    Console.WriteLine(" ----- Time elapsed: {0}", stw.Elapsed);
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
                    //Console.WriteLine(" ------------------ Press any key to close -----------------------");
                    //Console.ReadKey();
                }
            }
            else
            {
                try
                {
                    var process = new FillAppFabricCache();
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
