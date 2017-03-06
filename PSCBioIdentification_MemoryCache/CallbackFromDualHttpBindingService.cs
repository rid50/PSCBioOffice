using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSCBioIdentification
{
    //interface CallbackFromUnmanagedCacheService : MatchingService.IMatchingServiceCallback
    //{
    //}

    //interface CallbackFromManagedCacheService : CachePopulateService.IPopulateCacheServiceCallback
    //{
    //}

    //class CallbackFromCacheFillingService : CallbackFromUnmanagedCacheService, CallbackFromManagedCacheService
    //class CallbackFromDualHttpBindingService : UnmanagedMatchingService.IMatchingServiceCallback, MemoryCachePopulateService.IPopulateCacheServiceCallback, AppFabricCachePopulateService.IPopulateCacheServiceCallback, MemoryCacheMatchingService.IMatchingServiceCallback
    [CallbackBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    class CallbackFromDualHttpBindingService : UnmanagedMatchingService.IMatchingServiceCallback, MemoryCachePopulateService.IPopulateCacheServiceCallback, AppFabricCachePopulateService.IPopulateCacheServiceCallback
    {
        private static int totalRecords;
        private static double runningSum;

        public event EventHandler<MyEventArgs> MyEvent;

        public CallbackFromDualHttpBindingService()
        {
            //totalRecords = -1;
            //runningSum = 0;
        }

        public int TotalRecords {
            set { totalRecords = value; }
        }
        public int RunningSum
        {
            set { runningSum = value; }
        }

        public void RespondWithRecordNumbers(int num)
        {
            MyEventArgs args;
            if (totalRecords == 0)
            {
                totalRecords = num;
                args = new MyEventArgs { Message = string.Format(" --- Number of records read: {0}", num.ToString()), Error = "" };
            }
            else
            {
                runningSum += num;
                args = new MyEventArgs { Message = string.Format(" --- {0:0.00}%", runningSum / totalRecords * 100), Error = "" };
            }
            //args.Message = str;

            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {

                handler(this, args);

                ////Task.Factory.StartNew(async delegate (Object arg)
                //Task.Run(async () =>
                //{
                //    await Task.Delay(10);
                //    handler(this, args);
                //    //await Task.Factory.StartNew(delegate() { handler(this, args); });
                //});
            }
        }

        public void RespondWithText(string str)
        {
            MyEventArgs args = new MyEventArgs { Message = str, Error = "" };
            //args.Message = str;

            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void RespondWithError(String str)
        {
            MyEventArgs args = new MyEventArgs { Error = str };
            //args.Message = str;

            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void CacheOperationComplete()
        {
            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {
                //Task.Run(async () =>
                //{
                //    //await Task.Delay(10);
                //    //handler(this, null);
                //    await Task.Factory.StartNew(delegate() { handler(this, null); });
                //});
                handler(this, null);
            }            
        }

        //public void MatchingComplete()
        //{
        //    EventHandler<MyEventArgs> handler = MyEvent;
        //    if (handler != null)
        //    {
        //        handler(this, null);
        //    }
        //}
    }

    public class MyEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
