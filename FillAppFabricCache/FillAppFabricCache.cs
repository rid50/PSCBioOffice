using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Microsoft.ApplicationServer.Caching;
using System.Collections.Generic;
using System.Threading;
using System.Collections;

namespace FillAppFabricCache
{
    class FillAppFabricCache
    {
        //private static DataCacheFactory _factory = null;
        private static DataCache _cache;
        private static CancellationToken _ct;
        private static byte[] _probeTemplate;
        private static ArrayList _fingerList;

        //static FillAppFabricCache()
        //{
        //    DataCacheFactory factory = new DataCacheFactory();
        //    _cache = factory.GetCache("default");
        //    //Debug.Assert(_cache == null);
        //}

        public FillAppFabricCache(CancellationToken ct, DataCache cache, byte[] probeTemplate, ArrayList fingerList)
        {
            _ct = ct;
            _cache = cache;
            _probeTemplate = probeTemplate;
            _fingerList = fingerList;
        }


        public static Int32 rowcount()
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(getConnectionString());
                conn.Open();
                cmd = new SqlCommand();
                cmd.CommandTimeout = 300;
                cmd.Connection = conn;
                //cmd.CommandText = "SELECT count(*) FROM Egy_T_FingerPrint WHERE datalength(AppWsq) IS NOT NULL";
                cmd.CommandText = "SELECT count(*) FROM Egy_T_FingerPrint";
                reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32(0);
            }
            //catch (SqlException)
            //{
            //    return 0;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();

                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    
        //public void run(int from, int to, int count, int threadId)
        //public void run(int from, int to)
        public int run(int from, int to, int count)
        {
            //string fingerFields = "ri,rm,rr,rl";
            string fingerFields = "li,lm,lr,ll,ri,rm,rr,rl,lt,rt";
            string[] fingerFieldsArray = fingerFields.Split(new char[] { ',' });
            string dbFingerTable = System.Configuration.ConfigurationManager.AppSettings["dbFingerTable"];
            string dbFingerColumn = System.Configuration.ConfigurationManager.AppSettings["dbFingerColumn"];
            string dbIdColumn = System.Configuration.ConfigurationManager.AppSettings["dbIdColumn"];


            //return;

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            //byte[] buffer = new byte[0];
            byte[][] buffer = new byte[10][];
            int id = 0;
            int rowNumber = 0;

            Stopwatch sw = new Stopwatch();
            //Stopwatch stwd = new Stopwatch();
            //Stopwatch stws = new Stopwatch();
            //stw.Start();
            //stwd.Start();
            //stws.Start();

            //foreach (string region in _cache.GetSystemRegions())
            //{
            //    foreach (var kvp in _cache.GetObjectsInRegion(region))
            //    {
            //        Console.WriteLine("data item ('{0}','{1}') in region {2} of cache {3}", kvp.Key, kvp.Value.ToString(), region, "default");
            //    }
            //}
            //return;            

            string regionName = from.ToString();
            //_cache.RemoveRegion(regionName);
            //_cache.CreateRegion(regionName);

            try
            {
                //conn = buildConnectionString();
                var connStr = getConnectionString();
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;

                //cmd.CommandText = "SELECT " + dbIdColumn + "," + dbFingerColumn + " FROM " + dbFingerTable + " WHERE AppID = 20095420";

                //cmd.CommandText = "SELECT " + dbIdColumn + "," + dbFingerColumn + " FROM " + dbFingerTable + " WHERE datalength(" + dbFingerColumn + ") IS NOT NULL";
                //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM (SELECT ROW_NUMBER() OVER(ORDER BY AppID) AS row, AppID, AppWsq FROM Egy_T_FingerPrint WHERE datalength(AppWsq) IS NOT NULL) r WHERE row > {0} and row <= {1}", from, to);
                //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                //cmd.CommandText = String.Format("SELECT AppID," + fingerFields + " FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                cmd.CommandText = String.Format("SELECT AppID, " + fingerFields + " FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                //cmd.CommandText = String.Format("SELECT " + fingerFields + " FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                //cmd.CommandText = "SELECT AppID, AppWsq FROM Egy_T_FingerPrint WHERE AppID = 20095423";

                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = (int)reader[dbIdColumn];

                    rowNumber++;
                    //if (rowNumber % 1000 == 0)
                    //    Console.WriteLine("{0}", rowNumber + from);
                    
                    //Console.WriteLine("ID = {0}", id);
                    //if (id == 20000007)
                    //    id = id;


                    //if (!(reader.IsDBNull(1) && reader.IsDBNull(2) && reader.IsDBNull(3) && reader.IsDBNull(4) && reader.IsDBNull(5)
                    //      && reader.IsDBNull(6) && reader.IsDBNull(7) && reader.IsDBNull(8) && reader.IsDBNull(9) && reader.IsDBNull(10)
                    //     )
                    //   )
                    //if (!reader.IsDBNull(1))
//                    {
                        //                        id = (int)reader[dbIdColumn];
                        bool approved = false, confirmed = false;
                        int i = 0;
                        foreach (string s in fingerFieldsArray)
                        {
                            if (!reader.IsDBNull(i + 1) && ((byte[])reader[s]).Length > 1)
                            {
                                if (!approved)
                                    approved = true;
                                else if (approved && !confirmed)
                                    confirmed = true;

                                buffer[i++] = (byte[])reader[s];
                            }
                            else
                                buffer[i++] = new byte[0];
                        }

                        if (id == 123)
                        {
                            Console.WriteLine("ID: {0}, confirmed: {1}", id, confirmed);
                        }

                        //if (confirmed)
                        //    _cache.Add(id.ToString(), buffer, regionName);
//                    }
                    //else
                    //{
                    //    Console.WriteLine("NULL {0}", id);
                    //}
                }


                //int k = 0;
                //foreach (var cacheItem in _cache.GetObjectsInRegion(regionName))
                //{
                //    k++;
                //    //Console.WriteLine("Key={0} -- Value ={1}", cacheItem.Key, cacheItem.Value);
                //}

                //Console.WriteLine("{0}", rowNumber + from);
                //Console.WriteLine("Objects in Cache = {0}", k);
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    if (reader != null)
                        reader.Close();

                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn = null;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        enum FingerListEnum {li, lm, lr, ll, ri, rm, rr, rl, lt, rt}

        public int iterateCache(String regionName)
        {
            Stopwatch st = new Stopwatch();
            st.Start();

            var matcher = new BioProcessor.BioProcessor();
            matcher.enrollProbeTemplate(_probeTemplate);

            byte[][] buffer = new byte[10][];
            int rowNumber = 0;
            int retcode = 0;
            foreach (KeyValuePair<string, object> item in _cache.GetObjectsInRegion(regionName))
            {
                short numOfMatches = 0;
                bool matched = false;
                rowNumber++;
                //continue;
                //if (rowNumber % 1000 == 0)
                //    Console.WriteLine("Region name: {0}, row number: {1}", regionName, rowNumber);

                if (_ct.IsCancellationRequested)
                {
                    _ct.ThrowIfCancellationRequested();
                }
               
                //if (item.Key == "123")
                //if( false)
                {
                    buffer = item.Value as byte[][];
                    //for (int i = 0; i < buffer.Length; i++)
                    foreach(string finger in _fingerList)
                    {
                        FingerListEnum f = (FingerListEnum)Enum.Parse(typeof(FingerListEnum), finger);
                        if (buffer[(int)f] != null && (buffer[(int)f]).Length != 0)
                        {
                            matched = matcher.match(buffer[(int)f]);
                            if (matched)
                            {
                                numOfMatches++;
                            }
                        }
                    }

                    if (_fingerList.Count == numOfMatches)
                    {
                        retcode = int.Parse(item.Key);
                        break;
                    }
                }
            }
            
            //Console.WriteLine("====== Region name: {0}, row number: {1}", regionName, rowNumber);

            st.Stop();
            Console.WriteLine(" ----- Region name \"{0}\",  time elapsed: {1}", regionName, st.Elapsed);

            return retcode;
        }

        static private String getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
    }
}
