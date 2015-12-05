using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
/*
enum SAVE
{
    INSERT = 0,
    UPDATE = 1
}
*/
public class DBUtil
{
    public byte[] GetImage(int id, bool wsq)
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
/*            
            if (wsq)
                cmd.CommandText = "SELECT wsq FROM visitors WHERE id = @id";
            else
                cmd.CommandText = "SELECT picture FROM visitors WHERE id = @id";
*/
            if (wsq)
                cmd.CommandText = "SELECT AppWsq FROM T_AppPers WHERE AppId = @id";
            else
                cmd.CommandText = "SELECT AppImage FROM T_AppPers WHERE AppId = @id";


            //if (wsq)
            //    cmd.CommandText = "SELECT AppId, AppWsq FROM v_fingerprintverify WHERE ChkId = @id";
            //else
            //    cmd.CommandText = "SELECT AppId, AppImage FROM v_fingerprintverify WHERE ChkId = @id";

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
/*
                    if (wsq)
                        buffer = (byte[])reader["wsq"];
                    else
                        buffer = (byte[])reader["picture"];
*/
                    if (wsq)
                        buffer = (byte[])reader["AppWsq"];
                    else
                        buffer = (byte[])reader["AppImage"];
                    
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
    
    public void SaveTemplate(int mode, int id, byte[] buffer)
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
                                update T_AppPers with (serializable) SET AppWsq = @wsq where AppId = @id
                                if @@rowcount = 0 
                                begin
                                    insert into T_AppPers (AppId, AppWsq) values (@id, @wsq) 
                                end
                            commit tran ";

            cmd.Parameters.Add("@wsq", SqlDbType.Image);
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
