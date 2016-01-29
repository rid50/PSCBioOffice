using System;
using System.Text;
//using System.Configuration;
//using System.Data.SqlClient;
using System.Data;
//using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Security.Cryptography;
using PSCBioIdentification;

namespace DataSourceServices
{
    class DataBaseService : DataSource
    {
        enum FingerListEnum { li = 1, lm, lr, ll, ri, rm, rr, rl, lt, rt }

        string dbPictureTable = MyConfigurationSettings.AppSettings["dbPictureTable"];
        string dbFingerTable = MyConfigurationSettings.AppSettings["dbFingerTable"];
        string dbIdColumn = MyConfigurationSettings.AppSettings["dbIdColumn"];
        string dbPictureColumn = MyConfigurationSettings.AppSettings["dbPictureColumn"];
        string dbFingerColumn = MyConfigurationSettings.AppSettings["dbFingerColumn"];
        string fingerFields = "li,lm,lr,ll,ri,rm,rr,rl,lt,rt";

        public override byte[][] GetImage(IMAGE_TYPE imageType, System.Int32 id)
        {
            //throw new Exception(getConnectionString());
            //char[] sep = new char[] {','};
            //string[] sep = new string[] { "," };

            //string[] fingerFields = " li,lm,lr,ll,ri,rm,rr,rl,lt,rt ".Split(sep, StringSplitOptions.RemoveEmptyEntries);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            byte[][] buffer = new byte[11][];
//            byte[][] buffer2 = new byte[10][];

            try
            {
                //conn = buildConnectionString();
                conn = new SqlConnection(getConnectionString());
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                //if (id != 20031448)
                //{
                //    if (IMAGE_TYPE.wsq == imageType)
                //        cmd.CommandText = "SELECT wsq FROM visitors WHERE id = @id";
                //    else
                //        cmd.CommandText = "SELECT picture FROM visitors WHERE id = @id";
                //}
                //else
                /*
                                {
                                    if (IMAGE_TYPE.wsq == imageType)
                                        //cmd.CommandText = "SELECT AppId, AppWsq FROM v_fingerprintverify WHERE ChkId = @id";
                                        cmd.CommandText = "SELECT AppWsq FROM T_AppPers WHERE AppId = @id";
                                    else
                                        //cmd.CommandText = "SELECT AppId, AppImage FROM v_fingerprintverify WHERE ChkId = @id";
                                        cmd.CommandText = "SELECT AppImage FROM T_AppPers WHERE AppId = @id";
                                }
                */
                if (IMAGE_TYPE.wsq == imageType)
                    cmd.CommandText = "SELECT " + dbFingerColumn + "," + fingerFields + " FROM " + dbFingerTable + " WHERE " + dbIdColumn + " = @id";
                else
                    cmd.CommandText = "SELECT " + dbPictureColumn + " FROM " + dbPictureTable + " WHERE " + dbIdColumn + " = @id";

                //throw new Exception(cmd.CommandText);


                //cmd.Parameters.Add(new SqlCeParameter("@id", SqlDbType.Int));   // doesn't work
                cmd.Parameters.AddWithValue("@id", id);

                //cmd.Parameters.Add("@id", SqlDbType.Int);
                //cmd.Parameters[0].Value = id;

                reader = cmd.ExecuteReader();
                //reader.Read();

                //SqlBinary binary;
                //SqlBytes bytes;

                //                if (reader.HasRows)   //Does not work for CE
                if (reader.Read())
                {
                    //if (!reader.IsDBNull(0))
                    //    id = reader.GetInt32(0);
                    if (!reader.IsDBNull(0) && ((byte[])reader[0]).Length != 1)
                    {
                        //binary = reader.GetSqlBinary(1);
                        //if (id != 20031448)
                        //{
                        //    if (IMAGE_TYPE.wsq == imageType)
                        //        buffer = (byte[])reader["wsq"];
                        //    //buffer = (byte[])reader["AppWsq"];
                        //    else
                        //        buffer = (byte[])reader["picture"];
                        //}
                        //else
                        /*
                                                {
                                                    if (IMAGE_TYPE.wsq == imageType)
                                                        buffer = (byte[])reader["AppWsq"];
                                                    else
                                                        buffer = (byte[])reader["AppImage"];
                                                }
                        */
                        if (IMAGE_TYPE.wsq == imageType)
                        {
                            buffer[0] = (byte[])reader[dbFingerColumn];  //(byte[])reader["AppWsq"];

                            string[] fingerFieldsArray = fingerFields.Split(new char[] { ',' });

                            int i = 1;
                            foreach (string finger in fingerFieldsArray)
                            {
                                FingerListEnum f = (FingerListEnum)Enum.Parse(typeof(FingerListEnum), finger);
                                if (!reader.IsDBNull(i) && ((byte[])reader[finger]).Length > 1)
                                    buffer[(int)f] = (byte[])reader[finger];  //(byte[])reader["li"];
                                else
                                    buffer[(int)f] = new byte[0];

                                i++;
                            }
                        }
                        else
                            buffer[0] = (byte[])reader[dbPictureColumn]; //(byte[])reader["AppImage"];

                        //buffer = (byte[])reader["AppImage"];
                        //int maxSize = 200000;
                        //buffer = new byte[maxSize];
                        //reader.GetBytes(1, 0L, buffer, 0, maxSize);
                    }
                    //else
                    //{
                    //    buffer = new byte[1];
                    //}
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

/*
        public override byte[] GetImage(IMAGE_TYPE imageType, int id)
        {
            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataReader reader = null;

            byte[] buffer = null;

            try
            {
                //conn = buildConnectionString();
                conn = new SqlConnection(getConnectionString());
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;

                //if (id != 20031448)
                //{
                //    if (IMAGE_TYPE.wsq == imageType)
                //        cmd.CommandText = "SELECT wsq FROM visitors WHERE id = @id";
                //    else
                //        cmd.CommandText = "SELECT picture FROM visitors WHERE id = @id";
                //}
                //else
                {
                    if (IMAGE_TYPE.wsq == imageType)
                        //cmd.CommandText = "SELECT AppId, AppWsq FROM v_fingerprintverify WHERE ChkId = @id";
                        cmd.CommandText = "SELECT AppWsq FROM T_AppPers WHERE AppId = @id";
                    else
                        //cmd.CommandText = "SELECT AppId, AppImage FROM v_fingerprintverify WHERE ChkId = @id";
                        cmd.CommandText = "SELECT AppImage FROM T_AppPers WHERE AppId = @id";
                }

                //cmd.Parameters.Add(new SqlCeParameter("@id", SqlDbType.Int));   // doesn't work
                cmd.Parameters.AddWithValue("@id", id);

                //cmd.Parameters.Add("@id", SqlDbType.Int);
                //cmd.Parameters[0].Value = id;

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
                        //if (id != 20031448)
                        //{
                        //    if (IMAGE_TYPE.wsq == imageType)
                        //        buffer = (byte[])reader["wsq"];
                        //    //buffer = (byte[])reader["AppWsq"];
                        //    else
                        //        buffer = (byte[])reader["picture"];
                        //}
                        //else
                        {
                            if (IMAGE_TYPE.wsq == imageType)
                                buffer = (byte[])reader["AppWsq"];
                            else
                                buffer = (byte[])reader["AppImage"];
                        }
                        //buffer = (byte[])reader["AppImage"];
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

                    if (conn.State == ConnectionState.Open)
                        conn.Close();

                    if (conn != null)
                        conn = null;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return buffer;

        }
*/
        private SqlConnection buildConnectionString()
        {
            String serverName = MyConfigurationSettings.AppSettings["ServerName"];
            if (serverName.Length == 0)
                serverName = "Data Source=" + decrypt(PSCBioIdentification.Credentials.Default.ServerName, "PSC");
            else
                serverName = "Data Source=" + serverName;

            if (PSCBioIdentification.Credentials.Default.IntegratedSecurity == true) {
                return new SqlConnection(
                    serverName +
                    //"Data Source=." +
                    //"Data Source=" + decrypt(PSCBioIdentification.Credentials.Default.ServerName, "PSC") +
                    ";Database=" + decrypt(PSCBioIdentification.Credentials.Default.DataBaseName, "PSC") +
                    ";Integrated Security=True");
            } else {
                return new SqlConnection(
                    serverName +
                    //"Data Source=" + decrypt(PSCBioIdentification.Credentials.Default.ServerName, "PSC") +
                    ";Database=" + decrypt(PSCBioIdentification.Credentials.Default.DataBaseName, "PSC") +
                    ";User ID=" + decrypt(PSCBioIdentification.Credentials.Default.DBUser, "PSC") +
                    ";Password=" + decrypt(PSCBioIdentification.Credentials.Default.DBPass, "PSC"));
            }
        }

        private String getConnectionString()
        {
            string conn = null;
            try
            {
                conn = MyConfigurationSettings.ConnectionStrings["ConnectionString"].ToString();
            } catch (Exception) {
                throw new Exception("Connection to a database is not available");
            }

            return conn;
        }

        private static String decrypt(String strEncrypted, String strKey)
        {
            try
            {
                var objDESCrypto = new TripleDESCryptoServiceProvider();
                var objHashMD5 = new MD5CryptoServiceProvider();

                byte[] byteHash, byteBuff;
                String strTempKey = strKey;

                byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
                objHashMD5 = null;
                objDESCrypto.Key = byteHash;
                objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB

                byteBuff = Convert.FromBase64String(strEncrypted);
                String strDecrypted = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
                objDESCrypto = null;

                return strDecrypted;
            }
            catch (Exception ex)
            {
                return "Wrong Input. " + ex.Message;
            }
        }
    }
}
