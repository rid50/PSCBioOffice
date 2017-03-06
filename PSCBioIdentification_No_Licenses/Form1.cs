using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Resources;

using PSCBioIdentification.Properties;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace PSCBioIdentification
{
    public partial class Form1 : Form
    {

        //[DllImport("Lookup.dll", CharSet = CharSet.Auto)]
        //public static extern bool initFingerMatcher();

        //[DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern UInt32 fillCache(
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
        //    string[] fingerList, int fingerListSize,
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
        //    string[] appSettings);

        //[DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern UInt32 match(
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
        //    string[] fingerList, int fingerListSize,
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
        //    byte[] template,
        //    UInt32 size,
        //    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
        //    string[] appSettings,
        //    System.Text.StringBuilder errorMessage, int messageSize);

        [DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void terminateMatchingService();

        //public static extern UInt32 match(byte[] template, UInt32 size, String errorMessage);
        //public static extern UInt32 match(byte[] template, UInt32 size, 
          //  [MarshalAs(UnmanagedType.LPArray, ArraySubType=UnmanagedType.LPStr)] String[] errorMessage);
        //public static extern UInt32 match([MarshalAs(UnmanagedType.LPStruct)] NFRecord template);
        //public static extern UInt32 match(System.Runtime.Remoting.ObjRef objRef);

        //[DllImport("GrabImage.dll", CharSet = CharSet.Ansi)]
        //public static extern int TakeSnap(String fileName);
        ////        public static extern void TakeSnap(System.Text.StringBuilder fileName);
        ////        public static extern void TakeSnap(char[] fileName);

        //[DllImport("GrabImage.dll", CharSet = CharSet.Auto)]
        //public static extern void DestroyGraph();


        private BackgroundWorker backgroundWorkerProgressBar;

        //private bool enrollMode = true;
        //private NGrayscaleImage[] images = new NGrayscaleImage[0];
        //private NGrayscaleImage[] resultImages = new NGrayscaleImage[0];
        //private Bitmap[] bitmaps = new Bitmap[0];
        //private Bitmap[] resultBitmaps = new Bitmap[0];
        //private NFRecord[] templates = new NFRecord[0];
        //private int count = 0;
        //private int availableCount = 0;
        //private NGrayscaleImage image;
        //private NGrayscaleImage resultImage;
        //private NFRecord template;
        //private NFRecord enrolledTemplate;
        //private FPScannerMan scannerMan;

        dynamic _serviceClient = null;

        CallbackFromDualHttpBindingService _callbackFromDualHttpBindingService = null;
        InstanceContext _instanceContext = null;

        //CancellationTokenSource _tokenSource = null;
        //CancellationToken _ct;

        private ManualResetEvent _mre;
        private UInt32 _matchingResult = 0;

 
        //private NBiometricClient[] _biometricClients = new NBiometricClient[10];        
        //private NSubject[] _subjects = new NSubject[10];

        //private NFinger _subjectFinger;
        //private NDevice _device;
        
        private bool _isCapturing = false;
        //private bool _isPopulatingChache = false;

        //System.Diagnostics.Stopwatch _sw;

        //private NDeviceManager scannerMan;
//        private string selectedScannerModules = string.Empty;
        private int userId = 0;

        //private WsqImage _wsqImage = null;
        private ArrayList _fingersCollection = null;
        private enum ProgramMode { PreEnrolled = 1, Verification = 2, Identification = 3 };
        private ProgramMode mode;
        private ProgramMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                //                OnEnrollModeChanged();
            }
        }

        private const string appName = "Public Services Company (Kuwait) - ";

        public Form1()
        {
            setCulture();
            InitializeComponent();
        }

        System.Windows.Forms.ToolTip _toolTip;

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorkerProgressBar = new BackgroundWorker();
            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            backgroundWorkerProgressBar.DoWork += new DoWorkEventHandler(backgroundWorkerProgressBar_DoWork);
            backgroundWorkerProgressBar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProgressBar_RunWorkerCompleted);
            backgroundWorkerProgressBar.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgressBar_ProgressChanged);
            backgroundWorkerProgressBar.WorkerSupportsCancellation = true;

 
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            trackBarLabel.Text = "" + trackBar1.Value;

            ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);

            _callbackFromDualHttpBindingService = new CallbackFromDualHttpBindingService { TotalRecords = 0, RunningSum = 0 };
            _callbackFromDualHttpBindingService.MyEvent += MyEvent;
            _instanceContext = new InstanceContext(_callbackFromDualHttpBindingService);

            MyConfigurationSettings conf;

            try
            {
                conf = new MyConfigurationSettings();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;

                ShowErrorMessage("Error connecting to endpoint configuration server: " + ex.Message);
                ShowRadioHideCheckButtons(true);
                EnableControls(false);
                manageCacheButton.Enabled = false;
                return;
            }


            //MemoryCachePopulateService.PopulateCacheServiceClient client = null;
            dynamic client = null;
            string errorMessage;

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            {
                client = new MemoryCachePopulateService.PopulateCacheServiceClient(_instanceContext);

                if (!IsServiceAvailable(client, out errorMessage))
                {
                    ShowErrorMessage(errorMessage);
                    if (client != null)
                        client.Close();

                    return;
                }
            }
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                client = new AppFabricCachePopulateService.PopulateCacheServiceClient(_instanceContext);


            //if (client == null  || (client != null && client.InnerChannel.State == CommunicationState.Faulted))
            //{
            //    ShowErrorMessage("PopulateCacheService communication faulted");
            //    if (client != null)
            //        client.Close();

            //    return;
            //}

            ArrayList fingerList = null;

            if (client != null)
            {
                try
                {
                    //DateTime dt = client.getExpirationTime();

                    fingerList = client.getFingerList();
                    if (fingerList != null && fingerList.Count > 0)
                        labelCacheValidationTime.Text += string.Format("Valid until: {0:MMM dd} {0:t}", client.getExpirationTime());
                }
                catch (FaultException ex)
                {

                    ShowErrorMessage(ex.Message);

                    //labelCacheUnavailable.Text = fault.Detail;
                    fingerList = null;
                    //labelCacheUnavailable.Text = string.Format("Cache is unavailable ({0}), Identification mode is disabled", fault.Detail);
                    //labelCacheUnavailable.Text = "AppFabric caching service is not available. Launch PowerShell command \"get-cachehost\" to see if it is down";
                    //MessageBox.Show(string.Format("{0}", fault.Detail));
                    //EnableControls(false);
                    //manageCacheButton.Tag = "off";
                    //manageCacheButton.Enabled = false;
                    //radioButtonIdentify.Tag = "off";
                    //radioButtonIdentify.Enabled = false;
                    //buttonScan.Enabled = false;
                }
                finally
                {
                    client.Close();
                }
            }
            //return;

            radioButtonIdentify.Tag = "on";

            if (fingerList == null)
            {
                fingerList = new ArrayList();

                radioButtonIdentify.Enabled = false;
                buttonScan.Enabled = false;
                radioButtonIdentify.Tag = "off";
            }

            Label lab; CheckBox cb;
            for (int i = 1; i < 11; i++)
            {
                lab = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                if (fingerList.IndexOf(lab.Text) != -1)
                {
                    cb.Checked = true;
                    lab.BackColor = Color.Cyan;
                }
                else
                {
                    if (fingerList.Count == 0)
                        cb.Checked = true;
                    else
                        cb.Checked = false;
                }

                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (fingerList.IndexOf(cb.Tag) != -1)
                    cb.Enabled = true;
                else
                    cb.Enabled = false;
            }

            _toolTip = new System.Windows.Forms.ToolTip();
            _toolTip.AutoPopDelay = 5000;
            _toolTip.InitialDelay = 100;
            _toolTip.OwnerDraw = true;
            _toolTip.ReshowDelay = 10;
            _toolTip.Draw += new DrawToolTipEventHandler(this.toolTip_Draw);
            _toolTip.Popup += new PopupEventHandler(toolTip_Popup);

            PictureBox pb; ButtonBase bb, bb2; Label lb;
            for (int i = 0; i < 10; i++)
            {
                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as PictureBox;
                pb.MouseClick += new MouseEventHandler(fpPictureBox_MouseClick);

                if (pb.Tag != null)
                    _toolTip.SetToolTip(pb, rm.GetString(pb.Tag as string));

                bb = this.Controls.Find("radioButton" + (i + 1).ToString(), true)[0] as ButtonBase;
                bb.Click += new EventHandler(buttonBase_Click);

                bb2 = this.Controls.Find("checkBox" + (i + 1).ToString(), true)[0] as ButtonBase;
                bb2.Click += new EventHandler(buttonBase_Click);
                bb2.Location = bb.Location;

                lb = this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label;
                lb.Parent = pb;
                lb.Font = new Font("Areal", 10.0f, FontStyle.Bold);
                lb.BringToFront();
                lb.Location = new Point(0, pb.Height - 20);
            
            }

            AcceptButton = buttonRequest;
 
            radioButtonVerify.Checked = true;
            EnableSemaphorControls(false);
            EnableControls(true);

            personId.Text = "210067490";
            personId.Text = "20005140";
            personId.Text = "20000004";
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the label box.
            trackBarLabel.Text = "" + trackBar1.Value;
        }

        public static bool IsServiceAvailable(dynamic client, out string errorMessage)
        {
            errorMessage = string.Empty;

 //           if (client != null)
            {
                String endPointHost = ConfigurationManager.AppSettings["endPointHost"];
                ServiceEndpoint serviceEndpoint = client.Endpoint;
                Uri uri = new Uri(serviceEndpoint.Address.Uri.Scheme + "://" + endPointHost + ":" + serviceEndpoint.Address.Uri.Port + serviceEndpoint.Address.Uri.PathAndQuery);
                client.Endpoint.Address = new EndpointAddress(uri.ToString());

                //string absoluteUri = client.Endpoint.Address.Uri.AbsoluteUri;

                return IsServiceAvailable(client.Endpoint.Address, out errorMessage);
            }
        }
        public static bool IsServiceAvailable(EndpointAddress endpointAddress, out string errorMessage)
        {
            errorMessage = string.Empty;
            Uri uri = endpointAddress.Uri;

            try
            {
                string address = uri.AbsoluteUri + "?wsdl";
                MetadataExchangeClient mexClient = null;
                if (uri.Scheme == "http")
                    mexClient = new MetadataExchangeClient(new Uri(address), MetadataExchangeClientMode.HttpGet);
                else if (uri.Scheme == "net.tcp")
                {
                    return true;
                    //mexClient = new MetadataExchangeClient(new EndpointAddress("net.tcp://localhost/MemoryCacheService/PopulateCacheService/mex"));
                    //mexClient.ResolveMetadataReferences = true;
                    //mexClient.OperationTimeout = new TimeSpan(0, 10, 0);
                }
                MetadataSet metadata = mexClient.GetMetadata();
                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    ex = ex.InnerException;

                errorMessage = ex.Message + " : " + uri.AbsoluteUri;
            }

            return false;
        }
        private void EnableSemaphorControls(bool capturing)
        {
            this.BeginInvoke(new Action(delegate ()
            {
                buttonScan.Enabled = !capturing;
                pictureBoxGreen.Active = capturing;
                pictureBoxGreen.Invalidate();
                pictureBoxRed.Active = !capturing;
                pictureBoxRed.Invalidate();
                //groupBoxMode.Enabled = capturing || Mode == ProgramMode.PreEnrolled;
                //radioButtonVerify.Enabled = capturing || Mode == ProgramMode.PreEnrolled;
                //radioButtonIdentify.Enabled = capturing || Mode == ProgramMode.PreEnrolled;

                //if (fingerView1.Finger == null)
                //    buttonScan.Enabled = !capturing;

                if (capturing)
                    WaitingForImageToScan();
            }));
        }

        private void EnableControls(bool enable)
        {
            this.BeginInvoke(new Action(delegate ()
            {
                //fillAppFabricCache.Enabled = enable;
                //if (enable && (string)manageCacheButton.Tag != "off")
                //if ((string)manageCacheButton.Tag != "off")
                    manageCacheButton.Enabled = enable;

                //buttonRequest.Enabled = enable;
                if (!radioButtonIdentify.Checked)
                {

                }

                //buttonScan.Enabled = enable;
                groupBoxMode.Enabled = enable;
                if (radioButtonIdentify.Checked)
                {
                    if ((string)radioButtonIdentify.Tag == "off")
                    {
                        radioButtonIdentify.Enabled = false;
                        buttonScan.Enabled = false;
                    }
                    else if ((string)radioButtonIdentify.Tag == "on")
                    {
                        radioButtonIdentify.Enabled = true;
                        buttonScan.Enabled = true;
                    }
                }

                panel2.Enabled = enable;
                //manageCacheButton.Text = IsCachingServiceRunning ? "Stop Cache Refreshing" : "Refresh Cache";
                buttonScan.Text = _isCapturing ? "Cancel" : "Scan";
                //radioButtonVerify.Enabled = enable;
                //radioButtonIdentify.Enabled = enable;

                //Thread.Sleep(0);
                //Application.DoEvents();

                //pictureBoxCheckMark.Image = null;
                //pictureBoxPhoto.Image = null;
            }));
        }




        private void OnImage(IAsyncResult iar)
        {

            EnableSemaphorControls(false);

            //NBiometricTask task = _biometricClient.CreateTask(NBiometricOperations.Segment | NBiometricOperations.CreateTemplate, subject);

            //// Perform task
            //_biometricClient.PerformTask(task);

            //int segmentCount = subject.Fingers.Count;
            //Console.WriteLine("Found {0} segments", segmentCount - 1);

            //for (int i = 1; i < segmentCount; i++)
            //{
            //    if (subject.Fingers[i].Status == NBiometricStatus.Ok)
            //    {
            //        Console.Write("\t {0}: ", subject.Fingers[i].Position);
            //        subject.Fingers[i].Image.Save(subject.Fingers[i].Position + ".png");
            //        Console.WriteLine("Saving image...");
            //    }
            //    else
            //    {
            //        Console.WriteLine("\t {0}: {1}", subject.Fingers[i].Position, subject.Fingers[i].Status);
            //    }
            //}

            //return;

            // Get task status
            //NBiometricStatus status = task.Status;
            // Check if extraction was canceled
            //if (status == NBiometricStatus.Canceled) return;


            //return;
            //foreach (var obj in fingers.Objects)
        }

        //NSubject probeSubject = null;

        private void OnSegmentCompleted(IAsyncResult r)
        {
        }

        private void ProcessTemplate(bool scannedTemplate = true)      // it can be a template from a file 
        {

        }


        private void OnEnrollFromDataServiceCompleted(byte[] buff)
        {
            try
            {
                if (Mode == ProgramMode.PreEnrolled)
                {
                    //if (processEnrolledData(buff))
                    //{
                    //    if (radioButtonIdentify.Checked)
                    //    {
                    //        Mode = ProgramMode.Identification;
                    //        startDataServiceProcess();          // go for a photo
                    //    }
                    //}
                }
                else
                {
                    using (var ms = new MemoryStream(buff))
                    {
                        if (ms.Length != 0)
                            pictureBoxPhoto.Image = Image.FromStream(ms);
                        else
                            pictureBoxPhoto.Image = null;
                    }

                    //BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));

                }
            }
            catch (Exception ex)
            {
                LogLine(ex.ToString(), true);
                ShowErrorMessage(ex.ToString());
            }


//            buttonRequest.Enabled = true;
//            buttonScan.Focus();

        }

      //private void scanner_ImageScanned(object sender, FPScannerImageScannedEventArgs ea)
        //{
        //    OnImage((NGrayscaleImage)ea.Image.Clone());
        //}


        //private void clearView()
        //{
        //    if (mode == ProgramMode.PreEnrolled || mode == ProgramMode.Identification)
        //    {
        //        nfView1.Image = null;
        //        if (mode == ProgramMode.Identification)
        //            clearFingerBoxes();
        //    }

        //    //nfView1.ResultImage = null;
        //    //nfView1.Template = null;
        //    //nfView1.Tree = null;
        //    nfView2.Image = null;
        //    nfView2.ResultImage = null;
        //    nfView2.Template = null;
        //    //nfView2.Tree = null;
        //    //image = null;
        //    //resultImage = null;
        //    //dgvFields.Rows.Clear();

        //    pictureBox1.Image = null;
        //    pictureBox2.Image = null;

        //}

        private void clearFingerBoxes()
        {
            MyPictureBox pb;
            for (int i = 0; i < 10; i++)
            {
                this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0].Text = "";
                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as MyPictureBox;
                pb.Active = false;
                //pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                pb.Image = null;
            }
        }

        private void clear()
        {
            ShowStatusMessage("");
//!!!!!            clearView();
            clearLog();

            pictureBoxCheckMark.Image = null;
            pictureBoxPhoto.Image = null;

            if (_fingersCollection != null)
                _fingersCollection.Clear();
        }

        private void WaitingForImageToScan()
        {
            //System.Threading.Thread.Sleep(50);
            //Application.DoEvents();

            ResourceManager rm2 = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
            string text2 = rm2.GetString("msgWaitingForImage"); // "Waiting for image..."

            LogLine(text2, true);
            ShowStatusMessage(text2);
        }

        //        private void doEnroll(int id, NImage image)
        private void doPreEnroll()
        {
            //this.enrolledTemplate = this.template;

            //ProgramMode mode = ProgramMode.Verify;

            if (radioButtonVerify.Checked)
                Mode = ProgramMode.Verification;
            else
                Mode = ProgramMode.Identification;

            //setModeRadioButtons(mode);


            /*
                        Settings settings = Settings.Default;

                        if (settings.SearchForDuplicates)
                        {
                            List<MatchingResult> results;
                            TimeSpan matchTime, seTime;
                            int matchedCount, countThreshold;
                            bool canceled;

                            MatchingResult? result = Identify(Template.Save(), false, false,
                                out matchedCount, out countThreshold, out results,
                                out matchTime, out seTime, out canceled);

                            if (canceled)
                            {
                                LogLine("Enrollment canceled", true);
                                return;
                            }

                            if (result != null)
                            {
                                MessageBox.Show(string.Format("Duplicate found, record Id: \"{0}\"", result.Value.RecordId), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        Data.Database.Records.Add(id.ToString(), Template.Save(),
                            Settings.Default.SaveImagesToDatabase ? image : null);
            */
        }

        //        private void doVerify(int id)
        private void doVerify()
        {
            //this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            //int score = 0;

        }

        private void doIdentify(byte[] probeTemplate)
        {
            startProgressBar();

        }

        private delegate void LogHandler(string text, bool scroll, bool mainLog);

        private void Log(string text, bool scroll, bool mainLog)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LogHandler(Log), new object[] { text, scroll, mainLog });
            }
            else
            {
                if (mainLog)
                {
                    rtbMain.AppendText(text);
                    if (scroll) rtbMain.ScrollToCaret();
                }
                else
                {
                    //rtbScanner.AppendText(text);
                    //if (scroll) rtbScanner.ScrollToCaret();
                }
            }
        }

        private void clearLog()
        {
            this.BeginInvoke(new Action(delegate ()
            {
                rtbMain.Clear();
            }));
        }

        private void LogLine(bool mainLog)
        {
            Log(Environment.NewLine, true, mainLog);
        }

        private void Log(string text, bool mainLog)
        {
            Log(text, true, mainLog);
        }

        private void LogLine(string text, bool mainLog)
        {
            //if (text == "
            Log(text, false, mainLog);
            LogLine(mainLog);
        }

        private void Log(string format, bool mainLog, params object[] args)
        {
            Log(string.Format(format, args), mainLog);
        }

        private void LogLine(string format, bool mainLog, params object[] args)
        {
            LogLine(string.Format(format, args), mainLog);
        }
 
        //private void setMode(ProgramMode mode)
        //{
        //    //setStatus(mode.ToString());

        //    //switch (mode)
        //    //{
        //    //    case ProgramMode.PreEnrolled:
        //    //        EnrollMode = true;
        //    //        break;
        //    //    case ProgramMode.Identification:
        //    //        EnrollMode = false;
        //    //        break;
        //    //    case ProgramMode.Verification:
        //    //        EnrollMode = false;
        //    //        break;
        //    //}

        //    this.mode = mode;
        //    //dgvFields.Rows.Clear();
        //}


        private void radioButtonGroup_CheckedChanged(object sender, EventArgs e)
        {
            var button = sender as RadioButton;
            if (button.Checked) {
                clearLog();

                switch (button.Text)
                {
                    //case "Enroll":
                    //    mode = ProgramMode.PreEnrolled;
                    //    break;
                    case "Verify":
                        mode = ProgramMode.PreEnrolled;
                        //nfView1.Visible = true;

                        personId.ReadOnly = false;
                        buttonRequest.Show();
                        ShowRadioHideCheckButtons(true);

                        //LogLine("Enter Person ID:", true);
                        //ShowStatusMessage("Enter Person ID:");


                        //System.Threading.Thread.Sleep(100);
                        //personId.Focus();

                        break;
                    case "Identify":
                        mode = ProgramMode.Identification;
                        //nfView1.Visible = false;

                        //checkBox1.Checked = true;
/*
                        var client = new CacheMatchingService.MatchingServiceClient();
                        ArrayList fingerList = client.getFingerList();

                        if (fingerList == null)
                            fingerList = new ArrayList();

                        CheckBox cb; //bool donotcheck = false;
                        for (int i = 1; i < 11; i++)
                        {
                            cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                            if (fingerList.IndexOf(cb.Tag) != -1)
                                cb.Enabled = true;
                            else
                                cb.Enabled = false;

                            //if (cb.Checked)
                            //{
                            //    donotcheck = true;
                            //    break;
                            //}
                        }
*/
                        //if (!donotcheck)
                        //{
                        //    checkBox5.Checked = true;
                        //    checkBox6.Checked = true;
                        //}
                        //checkBox7.Checked = true;

                        personId.ReadOnly = true;
                        buttonRequest.Hide();
                        buttonScan.Enabled = true;
                        ShowRadioHideCheckButtons(false);

                        //this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));

                        break;
                }
            }
        }

        //private void setModeRadioButtons(ProgramMode mode)
        //{
        //    switch (mode)
        //    {
        //        case ProgramMode.Enroll:
        //            //radioButtonEnroll.Enabled = true;
        //            //radioButtonEnroll.Checked = true;
        //            //radioButtonVerify.Enabled = false;
        //            break;
        //        case ProgramMode.Verify:
        //            //radioButtonVerify.Enabled = true;
        //            radioButtonVerify.Checked = true;
        //            //ShowRadioHideCheckButtons(true);
        //            //radioButtonEnroll.Enabled = false;
        //            break;
        //        case ProgramMode.Identify:
        //            //radioButtonIdentify.Enabled = true;
        //            radioButtonIdentify.Checked = true;
        //            //ShowRadioHideCheckButtons(false);
        //            break;
        //    }
        //}

        private void buttonRequest_Click(object sender, EventArgs e)
        {
            stopProgressBar();
            //System.Threading.Thread.Sleep(100);



            Mode = ProgramMode.PreEnrolled;
            //setMode(mode);
//            setModeRadioButtons(mode);

            clear();
            clearFingerBoxes();

            if (!isUserIdValid())
                return;

            EnableControls(false);

            //startProgressBar();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            //if ((sender as ButtonBase).Enabled == false)
            //    return;

        }

        //private void buttonRequest_Click(object sender, EventArgs e)

        class StateObject
        {
            public int LoopCounter;
        }



        // Determines the correct size for the button2 ToolTip.
        private void toolTip_Popup(object sender, PopupEventArgs e)
        {
            if (e.AssociatedControl == fpPictureBox2)
            {
                using (Font f = new Font("Tahoma", 9))
                {
                    e.ToolTipSize = TextRenderer.MeasureText(_toolTip.GetToolTip(e.AssociatedControl), f);
                }
            }
        }

        // Handles drawing the ToolTip.
        private void toolTip_Draw(System.Object sender,
            System.Windows.Forms.DrawToolTipEventArgs e)
        {

            // Draw the ToolTip differently depending on which 
            // control this ToolTip is for.
            // Draw a custom 3D border if the ToolTip is for button1.
            //if (e.AssociatedControl == fpPictureBox1)
            {
                // Draw the standard background.
                e.DrawBackground();

                // Draw the custom border to appear 3-dimensional.
                e.Graphics.DrawLines(SystemPens.ControlLightLight, new Point[] {
                    new Point (0, e.Bounds.Height - 1), 
                    new Point (0, 0), 
                    new Point (e.Bounds.Width - 1, 0)
                });
                e.Graphics.DrawLines(SystemPens.ControlDarkDark, new Point[] {
                    new Point (0, e.Bounds.Height - 1), 
                    new Point (e.Bounds.Width - 1, e.Bounds.Height - 1), 
                    new Point (e.Bounds.Width - 1, 0)
                });

                // Specify custom text formatting flags.
                TextFormatFlags sf = TextFormatFlags.VerticalCenter |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.NoFullWidthCharacterBreak;

                // Draw the standard text with customized formatting options.
                e.DrawText(sf);
            }
/*
            // Draw a custom background and text if the ToolTip is for button2.
            else if (e.AssociatedControl == fpPictureBox2)
            {
                // Draw the custom background.
                e.Graphics.FillRectangle(SystemBrushes.ActiveCaption, e.Bounds);

                // Draw the standard border.
                e.DrawBorder();

                // Draw the custom text.
                // The using block will dispose the StringFormat automatically.
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    using (Font f = new Font("Tahoma", 9))
                    {
                        e.Graphics.DrawString(e.ToolTipText, f,
                            SystemBrushes.ActiveCaptionText, e.Bounds, sf);
                    }
                }
            }
            // Draw the ToolTip using default values if the ToolTip is for button3.
            else if (e.AssociatedControl == fpPictureBox3)

            {
                e.DrawBackground();
                e.DrawBorder();
                e.DrawText();
            }
*/
        }

        private void checkRadioButton(string rbName)
        {
            RadioButton rb = this.Controls.Find(rbName, true)[0] as RadioButton;
            //rb.Checked = true;
            this.InvokeOnClick(rb, new EventArgs());
        }

        private bool isUserIdValid()
        {
            switch (mode)
            {
                case ProgramMode.PreEnrolled:
                case ProgramMode.Verification:
                    if (!Int32.TryParse(personId.Text, out this.userId))
                    {
                        ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                        string text = rm.GetString("msgEnterPersonId"); // "Enter Person Id"
                        ShowErrorMessage(text);
                        LogLine(text, true);
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void fingerChanged(int fingerNumber, bool radiobuttons)
        {
            MyPictureBox pb;
            for (int i = 0; i < _fingersCollection.Count; i++)
            {
                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as MyPictureBox;

                //if (pb.Name == "fpPictureBox" + (fingerNumber + 1).ToString())
                if (i == fingerNumber)
                {
                    pb.Active = true;
                    pb.Invalidate();
                } 
                else if (radiobuttons)
                {
                    if (pb.Active)
                    {
                        pb.Active = false;
                        pb.Invalidate();
                    }
                }
            }
        }

        private void fpPictureBox_MouseClick(object sender, EventArgs e)
        {
            PictureBox rb = sender as PictureBox;
            if (rb.Image == null)
                return;

            int rbNumber = "fpPictureBox".Length;

            this.BeginInvoke(new MethodInvoker(delegate() { checkRadioButton("radioButton" + rb.Name.Substring(rbNumber)); }));
        }

        //private void radioButton1_Click(object sender, EventArgs e)
        //{
        //    if (_fingersCollection == null || _fingersCollection.Count == 0)
        //        return;

        //    this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

        //    ProgramMode mode = ProgramMode.Enroll;
        //    setMode(mode);
        //    setModeRadioButtons(mode);

        //    clearLog();

        //    RadioButton rb = sender as RadioButton;
        //    int rbNumber = "radioButton".Length;
        //    rbNumber = Int32.Parse(rb.Name.Substring(rbNumber));
        //    WsqImage wsqImage = _fingersCollection[rbNumber - 1] as WsqImage;
        //    fingerChanged(rbNumber - 1);

        //    enrollFromWSQ(wsqImage);
        //}

        private void buttonBase_Click(object sender, EventArgs e)
        {
            if (sender.GetType().Name == "CheckBox")
            {
                CheckBox bb; int count = 0;
                for (int i = 0; i < 10; i++)
                {
                    bb = this.Controls.Find("checkBox" + (i + 1).ToString(), true)[0] as CheckBox;
                    if (bb.Checked)
                        count++;
                }



                return;
            }

            //if ((sender as ButtonBase).Text == "")
            //    return;

            if (_fingersCollection == null || _fingersCollection.Count == 0)
                return;

            //this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            Mode = ProgramMode.PreEnrolled;
            //setMode(mode);
            //setModeRadioButtons(mode);

            //clearLog();

            //RadioButton rb = sender as RadioButton;
            var rb = sender as ButtonBase;
            //var regex = new System.Text.RegularExpressions.Regex(@"\d+$");
            //string[] number = System.Text.RegularExpressions.Regex.Split(rb.Name, @"\D+");

            //var mc = System.Text.RegularExpressions.Regex.Matches(rb.Name, @"\d+$");
            //int i = Int32.Parse(mc[0].Value);
            var m = System.Text.RegularExpressions.Regex.Match(rb.Name, @"\d+$");
            //int i = Int32.Parse(m.Value);

            //int rbNumber = "radioButton".Length;
            //rbNumber = Int32.Parse(rb.Name.Substring(rbNumber));
            //WsqImage wsqImage = _fingersCollection[Int32.Parse(m.Value) - 1] as WsqImage;
            //WsqImage wsqImage = _fingersCollection[rbNumber - 1] as WsqImage;

            fingerChanged(Int32.Parse(m.Value) - 1, true);

            WsqImage wsqImage = _fingersCollection[Int32.Parse(m.Value) - 1] as WsqImage;
        }

        public void ShowStatusMessage(string message)
        {
            this.BeginInvoke(new Action(delegate ()
            {
                toolStripStatusLabelError.ForeColor = Color.Black;
                toolStripStatusLabelError.Text = System.Text.RegularExpressions.Regex.Replace(message, @"\r\n?|\n", "");
            }));

            //toolStripProgressBar.Visible = false;
            //this.Controls.SetChildIndex(statusStrip1, 0);

            Application.DoEvents();
        }

        void ShowErrorMessage(string message)
        {
            stopProgressBar();
            Application.DoEvents();

            this.BeginInvoke(new Action(delegate ()
            {
                toolStripStatusLabelError.ForeColor = Color.Red;
                toolStripStatusLabelError.Text = System.Text.RegularExpressions.Regex.Replace(message, @"\r\n?|\n", "");
            }));
        }

        void ShowRadioHideCheckButtons(bool show)
        {
            ButtonBase bb;
            for (int i = 1; i < 11; i++)
            {
                bb = this.Controls.Find("radioButton" + i.ToString(), true)[0] as ButtonBase;
                bb.Visible = show;
                bb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as ButtonBase;
                bb.Visible = !show;
            }

            if (ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache")
                show = true;

            radioButtonMan.Visible = !show;
            radioButtonWoman.Visible = !show;
            radioButtonManAndWoman.Visible = !show;

            radioButtonFirstMatch.Visible = !show;
            radioButtonAllOccurrences.Visible = !show;

        }

        private void setCulture()
        {
            //System.Resources.ResourceManager LocRM = new System.Resources.ResourceManager("PSCBioIdentification.Form1", typeof(Form1).Assembly);
            // Assign the string for the "strMessage" key to a message box.
            //MessageBox.Show(LocRM.GetString("strMessage"));

            String culture = null;

            //if (Request.UserLanguages != null)
            //    culture = Request.UserLanguages[0];
            if (System.Configuration.ConfigurationManager.AppSettings["Culture"] != null)
                culture = System.Configuration.ConfigurationManager.AppSettings["Culture"].ToString();

            if (culture != null)
            {
                setCulture(culture);
            }
        }

        private void setCulture(string culture)
        {
            if (culture != null)
            {
                try
                {
                    System.Globalization.CultureInfo info = new System.Globalization.CultureInfo(culture);
                    Thread.CurrentThread.CurrentCulture = info;
                    Thread.CurrentThread.CurrentUICulture = info;
                }
                catch (ArgumentException) { }
            }
        }

        private void changeCulture(string culture)
        {
            setCulture(culture);

            foreach (Control c in this.Controls)
            {
                ComponentResourceManager resources = new ComponentResourceManager(this.GetType());
                resources.ApplyResources(c, c.Name, new CultureInfo(culture));
            }
        }

        private void ScannersListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            //_device = scannersListBox.SelectedItem as NFScanner;

            CheckBox bb;
            for (int i = 0; i < 10; i++)
            {
                bb = this.Controls.Find("checkBox" + (i + 1).ToString(), true)[0] as CheckBox;
                bb.Checked = false;
            }
        }

        private void FingerViewMouseClick(object sender, MouseEventArgs e)
        {
         }


        private void terminateService()
        {
            if (ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache")
            {
                if (ConfigurationManager.AppSettings["cachingService"] == "local")
                {
                    terminateMatchingService();
                }
                else
                {
                    if (_serviceClient != null)
                        _serviceClient.terminateMatchingService();
                }
            } else
            {

                //CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
                //InstanceContext context = new InstanceContext(callback);
                //dynamic client = null;
                ////if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
                //    client = new MemoryCachePopulateService.PopulateCacheServiceClient(context);
                ////else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                ////    client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);

                //if (_tokenSource != null && !_tokenSource.IsCancellationRequested)
                //    _tokenSource.Cancel();

                //if (client != null)
                //    client.Terminate();

                if (_serviceClient != null)
                {
                    if (_serviceClient is MemoryCachePopulateService.PopulateCacheServiceClient)
                    {

                        //_serviceClient.Terminate();
                    } else if (_serviceClient is MemoryCacheMatchingService.MatchingServiceClient)
                    {

                    }

                }


                //_mre.Set();
            }

            //_serviceClient = null;
        }

        //delegate void d();

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            new Action(delegate ()
            {
                EnableControls(false);

                manageCacheButton.Enabled = false;
                ShowStatusMessage("The application is terminating, wait ...");
            }).Invoke();

            //this.Invoke((Action)(() =>
            //{
            //    EnableControls(false);
            //}));

            //act();

            //d d = delegate
            //{
            //    EnableControls(false);
            //    startProgressBar();
            //};
            //d();

            //if (backgroundWorkerCachingService.IsBusy)
            //{
            //    backgroundWorkerCachingService.CancelAsync();

                terminateService();

            //    //CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
            //    //InstanceContext context = new InstanceContext(callback);

            //    //if (_serviceClient != null)
            //    //{
            //    //    _serviceClient.Terminate();
            //    //}
            //    //else if (ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache")
            //    //{

            //    //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
            //    //    {
            //    //        terminateMatchingService();
            //    //    }
            //    //    else
            //    //    {
            //    //        //context = new InstanceContext(callback);
            //    //        dynamic client = new UnmanagedMatchingService.MatchingServiceClient(context);
            //    //        client.terminateMatchingService();
            //    //    }

            //    //}

            //    //dynamic client = null;
            //    //if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            //    //    client = new MemoryCachePopulateService.PopulateCacheServiceClient(context);
            //    //else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
            //    //    client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);


            //    //if (client != null)
            //    //    client.Terminate();
            //    //else
            //    //{
            //    //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
            //    //    {
            //    //        terminateMatchingService();
            //    //    }
            //    //    else
            //    //    {
            //    //        //context = new InstanceContext(callback);
            //    //        client = new UnmanagedMatchingService.MatchingServiceClient(context);
            //    //        client.terminateMatchingService();
            //    //    }
            //    //}

            //    //if (ConfigurationManager.AppSettings["cachingProvider"] != "ODBCCache")
            //    //{
            //    //    //context = new InstanceContext(callback);
            //    //    var client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);
            //    //    client.Terminate();
            //    //}
            //    //else
            //    //{
            //    //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
            //    //    {
            //    //        terminateMatchingService();
            //    //    }
            //    //    else
            //    //    {
            //    //        //context = new InstanceContext(callback);
            //    //        var client = new UnmanagedMatchingService.MatchingServiceClient(context);
            //    //        client.terminateMatchingService();
            //    //    }
            //    //}

            //    manageCacheButton.Enabled = false;

            //    ShowStatusMessage("The application is terminating, wait ...");

            //    while (backgroundWorkerCachingService.IsBusy)
            //    {
            //        Thread.Sleep(0);
            //        Application.DoEvents();
            //    }
            //}


        }


        void MyEvent(object sender, MyEventArgs e)
        {
            if (e == null)
            {
                //this.BeginInvoke((Action)(() =>
                //{
                //    ShowStatusMessage(string.Format(" --- {0:0.00}%", 100));
                //}));

                _mre.Set();                 //cache service finished cache populating or matching service completed a search 
            }
            else if (e.Error.Length == 0)   // Show message
            {
                if (_serviceClient != null && _serviceClient is MemoryCacheMatchingService.MatchingServiceClient)
                {

                    _matchingResult = Convert.ToUInt32(e.Message, 10);
                }
                else
                {
                    this.BeginInvoke((Action<string>)((Message) =>
                    {
                        ShowStatusMessage(Message);
                    }), e.Message);
                }
            }
            else                           // Error
            {
                if (_serviceClient != null)
                {
                }

                _mre.Set();                     //terminate cache service or matching service 

                //var act = new Action(delegate()
                this.BeginInvoke(new Action(delegate()
                {
                    Label lb;
                    for (int i = 1; i < 11; i++)
                    {
                        lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                        lb.BackColor = Color.Transparent;
                    }

                    stopProgressBar();
                    ShowStatusMessage("");
                    ShowErrorMessage(e.Error);
                    //MessageBox.Show(e.Error);
                    EnableControls(true);
                }));

                //stopProgressBar();
                //MessageBox.Show(e.Error);

                //act();

                //EnableControls(true);
            }
                //ShowErrorMessage(e.Error);
        }

//        private void fillAppFabricCache_Click(object sender, EventArgs e)
        private void manageCacheButton_Click(object sender, EventArgs e)
        {

                manageCacheButton.Enabled = false;

                Application.DoEvents();
                //ShowStatusMessage("Terminating the request...");

            //            CallbackFromAppFabricCacheService callback = new CallbackFromAppFabricCacheService();
            //            callback.MyEvent += MyEvent;
            //            InstanceContext context = new InstanceContext(callback);

            //            var client = new CachePopulateService.PopulateCacheServiceClient(context); 
            //            var fingerList = new System.Collections.ArrayList();

            //            CheckBox cb; Label lb;
            //            for (int i = 1; i < 11; i++)
            //            {
            //                lb = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
            //                lb.BackColor = Color.Transparent;

            //                cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
            //                if (cb.Checked)
            //                    fingerList.Add(cb.Tag);
            //            }


            //            client.Run(fingerList);
            //            //client.Run(new string[] { });
            //            //client.Run(new string[] { "0" });
            ////            _mre.WaitOne();
        }

        private void buttonRefreshScannerListBox_Click(object sender, EventArgs e)
        {
        }


        //class CallbackFromAppFabricCacheService : AppFabricCacheService.IPopulateCacheServiceCallback
        //{
        //    public void Respond(String str)
        //    {

        //        toolStripStatusLabelError.ForeColor = Color.Black;
        //        toolStripStatusLabelError.Text = message;
        //        ////this.Controls.SetChildIndex(statusStrip1, 0);

        //        //Application.DoEvents();

        //        Form1.ShowStatusMessage(str);
        //        //MessageBox.Show(str);
        //    }
        //}    

        //int lastLine = 0;
        private void HighlightCurrentLine()
        {
            // Save current selection
            int selectionStart = rtbMain.SelectionStart;
            int selectionLength = rtbMain.SelectionLength;

            // Get character positions for the current line
            int firstCharPosition = rtbMain.GetFirstCharIndexOfCurrentLine();
            int lineNumber = rtbMain.GetLineFromCharIndex(firstCharPosition);
            int lastCharPosition = rtbMain.GetFirstCharIndexFromLine(lineNumber + 1);
            if (lastCharPosition == -1)
                lastCharPosition = rtbMain.TextLength;

            //// Clear any previous color
            //if (lineNumber != lastLine)
            //{
            //    int previousFirstCharPosition = rtbMain.GetFirstCharIndexFromLine(lastLine);
            //    int previousLastCharPosition = rtbMain.GetFirstCharIndexFromLine(lastLine + 1);
            //    if (previousLastCharPosition == -1)
            //        previousLastCharPosition = rtbMain.TextLength;

            //    rtbMain.SelectionStart = previousFirstCharPosition;
            //    rtbMain.SelectionLength = previousLastCharPosition - previousFirstCharPosition;
            //    rtbMain.SelectionBackColor = SystemColors.Window;
            //    lastLine = lineNumber;
            //}

            // Set new color
            rtbMain.SelectionStart = firstCharPosition;
            rtbMain.SelectionLength = lastCharPosition - firstCharPosition;
            if (rtbMain.SelectionLength > 0)
                rtbMain.SelectionBackColor = Color.PaleTurquoise;

            // Reset selection
            rtbMain.SelectionStart = selectionStart;
            rtbMain.SelectionLength = selectionLength;
        }

        private void rtbMain_Click(object sender, EventArgs e)
        {
            if (rtbMain.SelectionBackColor == Color.PaleTurquoise)
            {
                int i = rtbMain.Text.IndexOf(':', rtbMain.GetFirstCharIndexOfCurrentLine());
                if (i != -1)
                {
                    i++;i++;
                    int j = rtbMain.Text.IndexOf(',', i);
                    personId.Text = rtbMain.Text.Substring(i, j - i);
                    this.userId = Int32.Parse(personId.Text);
                    pictureBoxCheckMark.Image = Properties.Resources.checkmark;
                    Mode = ProgramMode.PreEnrolled;
                 }
            }
        }
    }

    [Serializable]
    public class WsqImage
    {
        public int XSize { get; set; }
        public int YSize { get; set; }
        public int XRes { get; set; }
        public int YRes { get; set; }
        public int PixelFormat { get; set; }
        public byte[] Content { get; set; }
    }

    public class GenericBinder<T> : System.Runtime.Serialization.SerializationBinder
    {
        /// <summary>
        /// Resolve type
        /// </summary>
        /// <param name="assemblyName">eg. App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</param>
        /// <param name="typeName">eg. String</param>
        /// <returns>Type for the deserializer to use</returns>
        public override Type BindToType(string assemblyName, string typeName)
        {
            // We're going to ignore the assembly name, and assume it's in the same assembly 
            // that <T> is defined (it's either T or a field/return type within T anyway)

            string[] typeInfo = typeName.Split('.');
            bool isSystem = (typeInfo[0].ToString() == "System");
            string className = typeInfo[typeInfo.Length - 1];

            // noop is the default, returns what was passed in
            //Type toReturn = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            Type toReturn = null;
            try
            {
                toReturn = Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
            }
            catch (System.IO.FileLoadException) { }

            if (!isSystem && (toReturn == null))
            {   // don't bother if system, or if the GetType worked already (must be OK, surely?)
                System.Reflection.Assembly a = System.Reflection.Assembly.GetAssembly(typeof(T));
                string assembly = a.FullName.Split(',')[0];   //FullName example: "App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
                if (a == null)
                {
                    throw new ArgumentException("Assembly for type '" + typeof(T).Name.ToString() + "' could not be loaded.");
                }
                else
                {
                    Type newtype = a.GetType(assembly + "." + className);
                    if (newtype == null)
                    {
                        throw new ArgumentException("Type '" + typeName + "' could not be loaded from assembly '" + assembly + "'.");
                    }
                    else
                    {
                        toReturn = newtype;
                    }
                }
            }
            return toReturn;
        }
    }
}
