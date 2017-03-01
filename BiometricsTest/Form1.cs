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
        private NBiometricTask _enrollTask = null;

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

        private async void button3_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = false;

            await Task.Run(() => identify());

            button1.Enabled = true;
            button3.Enabled = true;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button3.Enabled = false;

            //await Task.Run(() => fillCache());
            await Task.Run(() => fillCacheByTemplates());
            
            button1.Enabled = true;
            button3.Enabled = true;

        }

        //enum FingerListEnum { li, lm, lr, ll, ri, rm, rr, rl, lt, rt }

        private void identify()
        {

            //if (InvokeRequired)
            //{
            //    BeginInvoke(new AsyncCallback(identify), result);
            //}
            //else
//            {
            clearLog();

            _biometricClient = new NBiometricClient { BiometricTypes = NBiometricType.Finger };
            _biometricClient.FingersFastExtraction = false;
            _biometricClient.FingersTemplateSize = NTemplateSize.Small;
            _biometricClient.FingersQualityThreshold = 48;
            _biometricClient.MatchingThreshold = trackBar1.Value;
            _biometricClient.FingersMatchingSpeed = NMatchingSpeed.High;
            _biometricClient.MatchingFirstResultOnly = false;

            _biometricClient.Initialize();

            _enrollTask = _biometricClient.CreateTask(NBiometricOperations.Enroll, null);

            var fingerList = new List<NFPosition>();
            var sb = new StringBuilder();

            NSubject probeSubject = NSubject.FromFile(@"LeftIndexMiddle.template");

            for (int i = 0; i < probeSubject.Fingers.Count; i++)
            {
                if (probeSubject.Fingers[i].Status == NBiometricStatus.Ok)
                {
                    if (sb.Length == 0)
                        sb.Append(string.Format("Templates extracted: \n"));

                    if (probeSubject.Fingers[i].Objects[0].Template != null)
                    {
                        fingerList.Add(probeSubject.Fingers[i].Position);
                        sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[0].ToString(),
                                            string.Format("Quality: {0}", probeSubject.Fingers[i].Objects[0].Quality), probeSubject.Fingers[i].Objects[0].Template.GetSize()));
                    }
                }
            }

            this.Invoke((Action<string>)((txt) =>
            {
                log(txt);
            }), sb.ToString());


            foreach (KeyValuePair<string, object> item in _cache.Select(x => new KeyValuePair<string, object>(x.Key, x.Value)))
            {
                var template = new NFTemplate();

                foreach (byte[] bt in item.Value as byte[][])
                {
                    if (bt != null && bt.Length != 0)
                    {
                        foreach (NFPosition pos in fingerList)
                        {
                            var record = new NFRecord(bt);
                            if (record.Position == pos)
                            {
                                template.Records.Add((NFRecord)record.Clone());
                                break;
                            }
                        }
                    }
                }

                if (template == null)
                    throw new Exception("Gallery template is null");

                using (var gallerySubject = NSubject.FromMemory(template.Save().ToArray()))
                {
                    if (gallerySubject == null)
                        throw new Exception("Gallery template is null");

                    gallerySubject.Id = string.Format("GallerySubject_{0}", item.Key);
                    _enrollTask.Subjects.Add(gallerySubject);
                }

            }

            var list = new List<Tuple<string, int>>();
            string retcode = string.Empty;

            NBiometricStatus status = NBiometricStatus.None;

            _biometricClient.PerformTask(_enrollTask);
            status = _enrollTask.Status;
            if (status != NBiometricStatus.Ok)
            {
                log(string.Format("Enrollment was unsuccessful. Status: {0}.", status));
                if (_enrollTask.Error != null) throw _enrollTask.Error;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            status = _biometricClient.Identify(probeSubject);

            if (status == NBiometricStatus.Ok)
            {
                if (checkBox1.Checked)
                {
                    foreach (var matchingResult in probeSubject.MatchingResults)
                    {
                        int i = matchingResult.Id.IndexOf('_');
                        //list.Add(new Tuple<string, int>(matchingResult.Id, matchingResult.Score));
                        log(string.Format(" ----- Matching Id: {0}, Matching Score: {1}", matchingResult.Id.Substring(i + 1), matchingResult.Score));
                    }
                }
            }
            else
            {
                log(string.Format(" -- Matching has failed"));
            }

            log(string.Format(" ----- Time elapsed: {0} sec", sw.Elapsed));

            _enrollTask.Dispose();
            _enrollTask = null;
            _biometricClient.Cancel();
            //}
        }


        String connectionString2 = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\psc\PSCBioOffice\BiometricsTest\Database1.mdf;Integrated Security=True";

        private void fillCache()
        {
            int numOfChunks = 0;
            if (!Int32.TryParse(textBoxChunk.Text, out numOfChunks))
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
            for (int k = 0; k < numOfChunks; k++)
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
            _biometricClient.Cancel();
        }

        enum FingerListEnum { li, lm, lr, ll, ri, rm, rr, rl, lt, rt }

        private void fillCacheByTemplates()
        {
            string fingerFields = "li,lm,lr,ll,ri,rm,rr,rl,lt,rt";
            string[] fingerFieldsArray = fingerFields.Split(new char[] { ',' });

            String connectionString = @"Server = (local); Database = MCCS_FP; Trusted_Connection = no; User ID = sa; Password = psc; Connection Timeout = 0; Pooling = true; Min Pool Size = 1;";

            clearLog();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                int id = 0;

                cmd.CommandText = "SELECT " + fingerFields + " FROM Egy_T_FingerPrint";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    while (reader.Read())
                    {
                        byte[][] buffer = new byte[10][];

                        bool approved = false, confirmed = false;   // this two variables asure that no less than 2 finger templates will be saved in memry cache
                        int i = 0;
                        foreach (string finger in fingerFieldsArray)
                        {
                            FingerListEnum f = (FingerListEnum)Enum.Parse(typeof(FingerListEnum), finger);
                            if (!reader.IsDBNull(i) && ((byte[])reader[finger]).Length > 1)
                            {
                                if (!approved)
                                    approved = true;
                                else if (approved && !confirmed)
                                    confirmed = true;

                                buffer[(int)f] = (byte[])reader[finger];
                            }
                            else
                                buffer[(int)f] = new byte[0];

                            i++;
                        }

                        //if (id.ToString() == "20005140")
                        //{
                        //    int k = 0;

                        //}

                        if (confirmed)
                            _cache.Set((id++).ToString(), buffer, new DateTimeOffset(DateTime.Now).AddDays(1));

                        if (id % 10000 == 0)
                            log(id.ToString());
                    }

                    log(string.Format(" ----- Time elapsed: {0} sec", sw.Elapsed));
                }
            }
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            trackBarLabel.Text = "" + trackBar1.Value;
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the label box.
            trackBarLabel.Text = "" + trackBar1.Value;
        }
    }
}
