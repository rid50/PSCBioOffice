using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
//using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
//using System.Data.SqlServerCe;
using System.Data.Common;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web;

enum IMAGE_TYPE
{
    picture = 0,
    wsq = 1
}

namespace DBHelper
{
    class DBUtil
    {
        public byte[] GetImageFromWebService(IMAGE_TYPE imageType, int id)
        {
            String url;
            if (imageType == IMAGE_TYPE.picture)
                url = "http://nomad.host22.com/kuwaitindex/bio_picture.php?id=";
            else
                url = "http://nomad.host22.com/kuwaitindex/bio_wsq.php?id=";

            url += id.ToString();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            byte[] bytes = null;
            using (Stream sm = request.GetResponse().GetResponseStream())
            {
                try
                {
                    //List<JsonResult> result = jsonStr.FromJson<List<JsonResult>>(s);

                    //StreamReader sr = new StreamReader(sm);
                    //String str = sr.ReadToEnd();
                    //sr.Close();
                    DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(List<JsonResult>));
                    List<JsonResult> result = serialiser.ReadObject(sm) as List<JsonResult>;
                    if (result.Count != 0)
                    {
                        if (result[0].result != null && result[0].result != "success")
                            throw new Exception(result[0].result);
                        //MessageBox.Show(result[0].result);
                        else
                        {
                            try
                            {
                                if (imageType == IMAGE_TYPE.picture)
                                {
                                    if (result[0].picture != null)
                                            bytes = System.Convert.FromBase64String(result[0].picture);
                                }
                                else
                                {
                                    if (result[0].wsq != null)
                                        bytes = System.Convert.FromBase64String(result[0].wsq);
                                }
                            }
                            catch (Exception ex) { throw new Exception(ex.Message); }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return bytes;
        }

        internal void UploadImage(IMAGE_TYPE imageType, int id, ref byte[] buffer)
        {
            String url;
            if (imageType == IMAGE_TYPE.picture)
                url = "http://nomad.host22.com/kuwaitindex/bio_picture.php?id=";
            else
                url = "http://nomad.host22.com/kuwaitindex/bio_wsq.php?id=";

            url += id.ToString();

            List<string> postData = new List<string>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            postData.Add(HttpUtility.UrlEncode("id") + "=" + HttpUtility.UrlEncode(id.ToString()));

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String = System.Convert.ToBase64String(buffer, 0, buffer.Length);
            }
            catch (System.ArgumentNullException ex)
            {
                throw new Exception(ex.ToString());
            }

            string urlEncode;
            if (imageType == IMAGE_TYPE.picture)
                urlEncode = HttpUtility.UrlEncode("picture");
            else
                urlEncode = HttpUtility.UrlEncode("wsq");

            urlEncode += "=" + HttpUtility.UrlEncode(base64String.ToString());

            postData.Add(urlEncode);
            string queryString = String.Join("&", postData.ToArray());
            byte[] byteArray = Encoding.UTF8.GetBytes(queryString);
            //write to stream 
            request.ContentLength = byteArray.Length;
            Stream s = request.GetRequestStream();
            s.Write(byteArray, 0, byteArray.Length);
            s.Close();

            //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(JsonResult));

            using (Stream sm = request.GetResponse().GetResponseStream())
            {
                //StreamReader sr = new StreamReader(sm);
                //String jsonStr = sr.ReadToEnd(); 

                //string json = @"{""Name"" : ""My Product""}";
                //MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
                try
                {
                    //List<JsonResult> result = jsonStr.FromJson<List<JsonResult>>(s);

                    DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(List<JsonResult>));
                    List<JsonResult> result = serialiser.ReadObject(sm) as List<JsonResult>;
                    if (result[0].result != "success")
                        throw new Exception(result[0].result);

                    //List<JsonResult> result = JSONHelper.Deserialise<List<JsonResult>>(jsonStr);
                    //JsonResult result = ser.ReadObject(sm) as JsonResult;
                    //MessageBox.Show("Result: " + result.result[0]);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public byte[] GetImage(IMAGE_TYPE imageType, int id)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            byte[] buffer = null;

            try
            {
                conn = new SqlConnection(getConnectionString());
                
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                if (IMAGE_TYPE.wsq == imageType)
                    cmd.CommandText = "SELECT wsq FROM visitors WHERE id = @id";
                else
                    cmd.CommandText = "SELECT picture FROM visitors WHERE id = @id";

                //cmd.Parameters.Add(new SqlCeParameter("@id", SqlDbType.Int));   // doesn't work
                cmd.Parameters.AddWithValue("@id", id);

                reader = cmd.ExecuteReader();
                //reader.Read();

                //SqlBinary binary;
                //SqlBytes bytes;

                //                if (reader.HasRows)   //Does not work for CE
                if (reader.Read())
                {
                    //if (!reader.IsDBNull(0))
                    //    id = reader.GetInt32(0);
                    if (!reader.IsDBNull(0))
                    {
                        //binary = reader.GetSqlBinary(1);
                        if (IMAGE_TYPE.wsq == imageType)
                            buffer = (byte[])reader["wsq"];
                        else
                            buffer = (byte[])reader["picture"];

                        //int maxSize = 200000;
                        //buffer = new byte[maxSize];
                        //reader.GetBytes(1, 0L, buffer, 0, maxSize);
                    }
                }
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

            return buffer;

        }

        public void SaveTemplate(int id, byte[] buffer)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(getConnectionString());
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                /*
                            if (mode == (int)SAVE.UPDATE)
                                cmd.CommandText = "UPDATE visitors SET wsq = @wsq WHERE id = @id";
                            else
                                cmd.CommandText = "INSERT INTO visitors (id, wsq) VALUES (@id, @wsq)";
                */
                cmd.CommandText = @"
                            begin tran
                                update visitors with (serializable) SET wsq = @wsq where id = @id
                                if @@rowcount = 0 
                                begin
                                    insert into visitors (id, wsq) values (@id, @wsq) 
                                end
                            commit tran ";

                cmd.Parameters.Add("@wsq", SqlDbType.VarBinary);
                cmd.Parameters["@wsq"].Value = buffer;

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
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

        private String getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        }
    }


    [DataContract]
    public class JsonResult
    {
        [DataMember(Name = "result", IsRequired = false)]
        public string result;
        [DataMember(Name = "picture", IsRequired = false)]
        public string picture;
        [DataMember(Name = "wsq", IsRequired = false)]
        public string wsq;
    }
}
