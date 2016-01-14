using System;
using System.Windows.Forms;

namespace PSCBioIdentification
{
    class CallbackFromAppFabricCacheService : CachePopulateService.IPopulateCacheServiceCallback
    {
        private static int totalRecords;
        private static double runningSum;

        public event EventHandler<MyEventArgs> MyEvent;

        public CallbackFromAppFabricCacheService()
        {
            totalRecords = 0;
            runningSum = 0;
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
                args = new MyEventArgs { Message = string.Format(" --- {0:0}%", runningSum / totalRecords * 100), Error = "" };
            }
            //args.Message = str;

            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void RespondWithResult(string str)
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

        public void CacheComplete()
        {
            EventHandler<MyEventArgs> handler = MyEvent;
            if (handler != null)
            {
                handler(this, null);
            }            
        }    
    }

    public class MyEventArgs : EventArgs
    {
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
