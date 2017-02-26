using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Images;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiometricsTest
{
    public partial class Form1 : Form
    {
        private static MemoryCache _cache = null;
        private NBiometricClient _biometricClient;

        public Form1()
        {
            InitializeComponent();

            try
            {
                if (_cache == null)
                {
                    _cache = MemoryCache.Default;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            //insertWSQ();

            await Task.Run(() => splitWSQ());
            //splitWSQ();

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

        }

        String connectionString2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\psc\PSCBioOffice\BiometricsTest\Database1.mdf;Integrated Security=True";
        enum FingerListEnum { li, lm, lr, ll, ri, rm, rr, rl, lt, rt }

        private void insertWSQ()
        {

            String connectionString = @"Server = (local); Database = MCCS_FP; Trusted_Connection = no; User ID = sa; Password = psc; Connection Timeout = 0; Pooling = true; Min Pool Size = 1;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlConnection conn2 = new SqlConnection(connectionString2))
            {
                conn.Open();
                conn2.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn2;

                int from = 0;
                int count = 10;
                byte[] wsq = new byte[0];
                //int id = 0;

                cmd2.CommandText = "DELETE FROM Egy_T_FingerPrint";
                cmd2.ExecuteNonQuery();

                cmd.CommandText = String.Format("SELECT AppWsq FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
                cmd2.CommandText = "INSERT INTO Egy_T_FingerPrint(AppWsq) VALUES (@wsq)";

                //cmd2.Parameters.Add("@id", SqlDbType.Int);
                cmd2.Parameters.Add("@wsq", SqlDbType.Image);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            wsq = (byte[])reader["AppWsq"];

                            cmd2.Parameters["@wsq"].Value = wsq;

                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private void splitWSQ()
        {
            int chunk = 0;
            if (!Int32.TryParse(textBoxChunk.Text, out chunk))
            {
                log("Number of chunks is invalid");
                return;
            }

            clearLog();

            Dictionary<int, byte[]> dict = new Dictionary<int, byte[]>();

            using (SqlConnection conn = new SqlConnection(connectionString2))
            {
                conn.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.Connection = conn;

                byte[] wsq = new byte[0];
                int id = 0;

                cmd2.CommandText = "SELECT AppWsq FROM Egy_T_FingerPrint";

                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            wsq = (byte[])reader["AppWsq"];

                            dict.Add(id++, wsq);
                        }
                    }
                }
            }

            NSubject subject;

            ArrayList fingersCollection = null;
            MemoryStream ms = null;

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new WsqSerializationBinder.MyBinder<WsqImage>();

            _biometricClient = new NBiometricClient { BiometricTypes = NBiometricType.Finger };
            _biometricClient.FingersFastExtraction = false;
            _biometricClient.FingersTemplateSize = NTemplateSize.Small;
            _biometricClient.FingersQualityThreshold = 48;
            _biometricClient.Initialize();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            int ii = 0;
            for (int k = 0; k < chunk; k++)
            {
                foreach (KeyValuePair<int, byte[]> entry in dict)
                {
                    ms = new MemoryStream(entry.Value);

                    try
                    {
                        fingersCollection = formatter.Deserialize(ms) as ArrayList;
                    }
                    catch (Exception) { continue; }
                    finally { ms.Close(); }

                    subject = new NSubject();

                    NImage nImage = null;

                    for (int i = 0; i < fingersCollection.Count; i++)
                    {
                        if (fingersCollection[i] != null)
                        {
                            try
                            {
                                nImage = NImage.FromMemory((fingersCollection[i] as WsqImage).Content, NImageFormat.Wsq);

                                var finger = new NFinger { Image = nImage };
                                subject.Fingers.Add(finger);

                                switch (i)
                                {
                                    case 0:
                                        finger.Position = NFPosition.LeftIndex;
                                        break;
                                    case 1:
                                        finger.Position = NFPosition.LeftMiddle;
                                        break;
                                    case 2:
                                        finger.Position = NFPosition.LeftRing;
                                        break;
                                    case 3:
                                        finger.Position = NFPosition.LeftLittle;
                                        break;
                                    case 4:
                                        finger.Position = NFPosition.RightIndex;
                                        break;
                                    case 5:
                                        finger.Position = NFPosition.RightMiddle;
                                        break;
                                    case 6:
                                        finger.Position = NFPosition.RightRing;
                                        break;
                                    case 7:
                                        finger.Position = NFPosition.RightLittle;
                                        break;
                                    case 8:
                                        finger.Position = NFPosition.LeftThumb;
                                        break;
                                    case 9:
                                        finger.Position = NFPosition.RightThumb;
                                        break;
                                }

                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Equals("Operation is not activated"))
                                    throw new Exception(ex.Message);

                                continue;
                                //throw new Exception(string.Format("Error creating image retrieved from database {0}", ex.Message));
                            }
                            finally
                            {
                                if (nImage != null)
                                {
                                    nImage.Dispose();
                                    nImage = null;
                                }
                            }
                        }
                    }

                    try
                    {
                        _biometricClient.CreateTemplate(subject);
                    }
                    catch (Exception ex)
                    {
                        while (ex.InnerException != null)
                            ex = ex.InnerException;
                        throw new Exception(ex.Message);
                    }

                    byte[][] buffer = new byte[10][];
                    bool valid; NFPosition pos = NFPosition.Unknown; NFRecord record = null;
                    for (int i = 0; i < fingersCollection.Count; i++)
                    {
                        if (fingersCollection[i] != null)
                        {
                            switch (i)
                            {
                                case 0:
                                    pos = NFPosition.LeftIndex;
                                    break;
                                case 1:
                                    pos = NFPosition.LeftMiddle;
                                    break;
                                case 2:
                                    pos = NFPosition.LeftRing;
                                    break;
                                case 3:
                                    pos = NFPosition.LeftLittle;
                                    break;
                                case 4:
                                    pos = NFPosition.RightIndex;
                                    break;
                                case 5:
                                    pos = NFPosition.RightMiddle;
                                    break;
                                case 6:
                                    pos = NFPosition.RightRing;
                                    break;
                                case 7:
                                    pos = NFPosition.RightLittle;
                                    break;
                                case 8:
                                    pos = NFPosition.LeftThumb;
                                    break;
                                case 9:
                                    pos = NFPosition.RightThumb;
                                    break;
                            }

                            valid = false;
                            int j = 0;
                            for (j = 0; j < subject.Fingers.Count; j++)
                            {
                                if (subject.Fingers[j].Position == pos)
                                {
                                    if (subject.Fingers[j].Objects.First().Status == NBiometricStatus.Ok)
                                    {
                                        if (subject.Fingers[j].Objects.First().Quality != 254)
                                        {
                                            valid = true;
                                            //Console.WriteLine(" ----- Size: {0}", subject.Fingers[k].Objects.First().Template.GetSize());

                                        }
                                    }

                                    break;
                                }
                            }

                            if (!valid)
                                buffer[i] = new byte[0];
                            else
                            {
                                record = subject.Fingers[j].Objects.First().Template;
                                buffer[i] = record.Save().ToArray();
                            }
                        }
                        else
                        {
                            buffer[i] = new byte[0];
                        }

                    }
                    _cache.Set((entry.Key + ii).ToString(), buffer, new DateTimeOffset(DateTime.Now).AddDays(1));
                }

                ii += dict.Count;
                log(ii.ToString());
            }

            log(string.Format(" ----- Time elapsed: {0} sec", sw.Elapsed));
        }


        private delegate void LogHandler(string text);

        private void log(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LogHandler(log), new object[] { text });
            }
            else
            {
                richTextBox.AppendText(text);
                richTextBox.AppendText(Environment.NewLine);
                richTextBox.ScrollToCaret();
            }
        }

        private void clearLog()
        {
            this.BeginInvoke(new Action(delegate ()
            {
                richTextBox.Clear();
            }));
        }

    }
}
