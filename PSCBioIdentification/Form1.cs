using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

//using Neurotec.DeviceManager;
using Neurotec.Biometrics;
using Neurotec.Biometrics.Client;
using Neurotec.Biometrics.Gui;
using Neurotec.Devices;
using Neurotec.Images;

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

        private static readonly NRgb resultImageMinColor = new NRgb(0, 230, 0);
        private static readonly NRgb resultImageMaxColor = new NRgb(255, 255, 255);

        private BackgroundWorker backgroundWorkerProgressBar;
        private BackgroundWorker backgroundWorkerDataService;
        private BackgroundWorker backgroundWorkerMatchingService;
        private BackgroundWorker backgroundWorkerCachingService;

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

        //private NImage _image;
        private NDeviceManager _deviceManager;
        private NBiometricClient _biometricClient;
        private NSubject _subject;
        private NSubject _subject2;

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

        //public NFRecord Template
        //{
        //    get
        //    {
        //        return template;
        //    }
        //}

 //       public NDevice Device
 //       {
 //           get
 //           {
 //               return _device;
 //           }
 //           set
 //           {
 //               if (_device != value)
 //               {
 ////                   if (_device != null && !IsValidDeviceType(_device.GetType())) throw new ArgumentException("Invalid NDevice type");
 ////                   CheckIsBusy();
 //                   _device = value;
 ////                   OnDeviceChanged();
 //               }
 //           }
 //       }
        //public bool EnrollMode
        //{
        //    get
        //    {
        //        return enrollMode;
        //    }
        //    set
        //    {
        //        enrollMode = value;
        //        //                OnEnrollModeChanged();
        //    }
        //}


        public Form1()
        {
            setCulture();
            InitializeComponent();
        }

        System.Windows.Forms.ToolTip _toolTip;

        private void Form1_Load(object sender, EventArgs e)
        {
            ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);

            backgroundWorkerProgressBar = new BackgroundWorker();
            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            backgroundWorkerProgressBar.DoWork += new DoWorkEventHandler(backgroundWorkerProgressBar_DoWork);
            backgroundWorkerProgressBar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProgressBar_RunWorkerCompleted);
            backgroundWorkerProgressBar.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgressBar_ProgressChanged);
            backgroundWorkerProgressBar.WorkerSupportsCancellation = true;

            backgroundWorkerDataService = new BackgroundWorker();
            backgroundWorkerDataService.DoWork += new DoWorkEventHandler(backgroundWorkerDataService_DoWork);
            backgroundWorkerDataService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerDataService_RunWorkerCompleted);
            backgroundWorkerDataService.WorkerSupportsCancellation = true;

            backgroundWorkerMatchingService = new BackgroundWorker();
            backgroundWorkerMatchingService.DoWork += new DoWorkEventHandler(backgroundWorkerMatchingService_DoWork);
            backgroundWorkerMatchingService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerMatchingService_RunWorkerCompleted);
            backgroundWorkerMatchingService.WorkerSupportsCancellation = true;

            backgroundWorkerCachingService = new BackgroundWorker();
            backgroundWorkerCachingService.DoWork += new DoWorkEventHandler(backgroundWorkerCachingService_DoWork);
            backgroundWorkerCachingService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerCachingService_RunWorkerCompleted);
            backgroundWorkerCachingService.WorkerSupportsCancellation = true;

            //Data.NFExtractor = new NFExtractor();
            //Data.UpdateNfe();
            //Data.UpdateNfeSettings();

            //Data.NMatcher = new NMatcher();
            //Data.UpdateNM();
            //Data.UpdateNMSettings();

            //Data.Database = new Database();

            _biometricClient = new NBiometricClient { UseDeviceManager = true, BiometricTypes = NBiometricType.Finger };
            _biometricClient.Initialize();
            _biometricClient.FingersQualityThreshold = 48;
            //_biometricClient.FingerScanner.CapturePreview += OnFingerPropertyChanged;

            //_biometricClient.FingersReturnProcessedImage = true;

            _deviceManager = new NDeviceManager { DeviceTypes = NDeviceType.FingerScanner, AutoPlug = true };
            //_deviceManager = _biometricClient.DeviceManager;
            _deviceManager.Initialize();
            _deviceManager.Devices.CollectionChanged += deviceManager_CollectionChanged;

            //set type of the device used
            //_deviceManager.DeviceTypes = NDeviceType.FingerScanner;

            UpdateScannerList(false);

            //var client = new CacheMatchingService.MatchingServiceClient();

            //var conf = new MyConfigurationSettings();
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
                //MessageBox.Show("Error connecting to endpoint server");
                //Application.Exit();
            }

            _callbackFromDualHttpBindingService = new CallbackFromDualHttpBindingService { TotalRecords = 0, RunningSum = 0 };
            _callbackFromDualHttpBindingService.MyEvent += MyEvent;
            _instanceContext = new InstanceContext(_callbackFromDualHttpBindingService);

            //CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
            //InstanceContext context = new InstanceContext(callback);

            dynamic client = null;
            string errorMessage;

            if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
            {
                client = new MemoryCachePopulateService.PopulateCacheServiceClient(_instanceContext);

                //if (!IsServiceAvailable(client.Endpoint.Address.Uri.AbsoluteUri, out errorMessage))
                if (!IsServiceAvailable(client, out errorMessage))
                {
                    //ShowErrorMessage(errorMessage + " : " + client.Endpoint.Address.Uri.AbsoluteUri);
                    ShowErrorMessage(errorMessage);
                    client.Close();
                    return;
                }
            }
            else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                client = new AppFabricCachePopulateService.PopulateCacheServiceClient(_instanceContext);

            ArrayList fingerList = null;

            if (client != null)
            {
                try
                {
                    fingerList = client.getFingerList();
                    if (fingerList != null && fingerList.Count > 0)
                        labelCacheValidationTime.Text += string.Format("Valid until: {0:MMM dd} {0:t}", client.getExpirationTime());
                }
                catch (FaultException<System.ComponentModel.DataAnnotations.ValidationException>)
                {
                    //labelCacheUnavailable.Text = fault.Detail;
                    fingerList = null;
                    //labelCacheUnavailable.Text = string.Format("Cache is unavailable ({0}), Identification mode is disabled", fault.Detail);
                    labelCacheUnavailable.Text = "AppFabric caching service is not available. Launch PowerShell command \"get-cachehost\" to see if it is down";
                    //MessageBox.Show(string.Format("{0}", fault.Detail));
                    //EnableControls(false);
                    manageCacheButton.Tag = "off";
                    manageCacheButton.Enabled = false;
                    radioButtonIdentify.Tag = "off";
                    radioButtonIdentify.Enabled = false;
                    buttonScan.Enabled = false;
                } finally
                {
                    client.Close();
                }
            }


/*
            if (ConfigurationManager.AppSettings["cachingProvider"] != "ODBCCache")
            {
                CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
                InstanceContext context = new InstanceContext(callback);
                var client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);

                try
                {
                    fingerList = client.getFingerList();
                    if (fingerList != null)
                        labelCacheValidationTime.Text += string.Format("Valid until: {0:MMM dd} {0:t}", client.getExpirationTime());
                }
                catch (FaultException<System.ComponentModel.DataAnnotations.ValidationException>)
                {
                    //labelCacheUnavailable.Text = fault.Detail;
                    fingerList = null;
                    //labelCacheUnavailable.Text = string.Format("Cache is unavailable ({0}), Identification mode is disabled", fault.Detail);
                    labelCacheUnavailable.Text = "AppFabric caching service is not available. Launch PowerShell command \"get-cachehost\" to see if it is down";
                    //MessageBox.Show(string.Format("{0}", fault.Detail));
                    //EnableControls(false);
                    manageCacheButton.Tag = "off";
                    manageCacheButton.Enabled = false;
                    radioButtonIdentify.Tag = "off";
                    radioButtonIdentify.Enabled = false;
                    buttonScan.Enabled = false;
                    //               return;
                }
            }
*/
            if (fingerList == null)
                fingerList = new ArrayList();
            //else
              //  labelCacheValidationTime.Text += string.Format(" {0:MMM dd} {0:t}", client.getExpirationTime());

            Label lab; CheckBox cb;
            for (int i = 1; i < 11; i++)
            {
                lab = this.Controls.Find("labCache" + i.ToString(), true)[0] as Label;
                if (fingerList.IndexOf(lab.Text) != -1)
                {
                    cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                    cb.Checked = true;
                    lab.BackColor = Color.Cyan;
                }

                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (fingerList.IndexOf(cb.Tag) != -1)
                    cb.Enabled = true;
                else
                    cb.Enabled = false;
            }

            //string selectedScannerModules = "Futronic";
            //scannerMan = new FPScannerMan(selectedScannerModules, this);
            ////scannerMan = new NDeviceManager(selectedScannerModules, this);
            //FPScanner scanner;
            //if (scannerMan.Scanners.Count != 0)
            //{
            //    scanner = scannerMan.Scanners[0];
            //    scanner.FingerPlaced += new EventHandler(scanner_FingerPlaced);
            //    scanner.FingerRemoved += new EventHandler(scanner_FingerRemoved);
            //    scanner.ImageScanned += new FPScannerImageScannedEventHandler(scanner_ImageScanned);
            //}
            //else
            //{
            //    //ResourceManager rm = new ResourceManager("rmc", System.Reflection.Assembly.GetExecutingAssembly());
            //    string text = rm.GetString("msgNoScannersAttached"); // "No scanners attached"
            //    //string text = Resources.ResourceManager.GetString("msgNoScannersAttached"); // "No scanners attached"
            //    LogLine(text, true);
            //    ShowErrorMessage(text);
            //}

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
 
            //ProgramMode mode = (ProgramMode)Settings.Default.ProgramMode;
            //ProgramMode mode = ProgramMode.Identify;

            radioButtonVerify.Checked = true;
            //radioButtonIdentify.Checked = true;
            EnableSemaphorControls(false);
            EnableControls(true);

            //setMode(mode);
            //setModeRadioButtons(mode);

            //personId.Focus();

            //personId.Text = "123"; 20010235
            personId.Text = "210067490";
            //personId.Text = "20005140";

            //personId.Text = "20005232";
            //personId.Text = "20002346";            

            //buttonRequest.Focus();
            //buttonScan.Enabled = false;
            //if (!initFingerMatcher())
            //    ShowErrorMessage("Lookup service: Error connecting to database");


            //var cl = new MemoryCache.MemoryCacheServiceClient();
            //var fingersCollection = cl.GetQualityFingerCollection("210067490");
            //var fingersCollection = cl.GetRawFingerCollection("210067490");
        }


        public static bool IsServiceAvailable(dynamic client, out string errorMessage)
        {
            String endPointHost = ConfigurationManager.AppSettings["endPointHost"];
            ServiceEndpoint serviceEndpoint = client.Endpoint;
            Uri uri = new Uri(serviceEndpoint.Address.Uri.Scheme + "://" + endPointHost + ":" + serviceEndpoint.Address.Uri.Port + serviceEndpoint.Address.Uri.PathAndQuery);
            client.Endpoint.Address = new EndpointAddress(uri.ToString());

            //string absoluteUri = client.Endpoint.Address.Uri.AbsoluteUri;

            return IsServiceAvailable(client.Endpoint.Address, out errorMessage);
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
        }

        private void EnableControls(bool enable)
        {
            //fillAppFabricCache.Enabled = enable;
            if (enable && (string)manageCacheButton.Tag != "off")
                manageCacheButton.Enabled = enable;
            
            buttonRequest.Enabled = enable;
            buttonScan.Enabled = fingerView1.Finger != null || radioButtonIdentify.Checked;
            groupBoxMode.Enabled = enable;
            if ((string)radioButtonIdentify.Tag == "off")
            {
                radioButtonIdentify.Enabled = false;
                buttonScan.Enabled = false;
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
        }

        private void deviceManager_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BeginInvoke(new Action<NotifyCollectionChangedEventArgs>(ea =>
            {
                scannersListBox.BeginUpdate();
                switch (ea.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        bool add = true;
                        foreach (var item in scannersListBox.Items)
                        {
                            if (item == (NDevice)ea.NewItems[0])
                            {
                                add = false;
                                break;
                            }
                        }

                        if (add)
                        {
                            scannersListBox.Items.Add((NDevice)ea.NewItems[0]);
                            if (scannersListBox.SelectedItem == null) scannersListBox.SelectedIndex = 0;
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        for (int i = 0; i < scannersListBox.Items.Count; i++) {
                            if (scannersListBox.Items[i] == (NDevice)ea.OldItems[0])
                            {
                                scannersListBox.Items.RemoveAt(i);
                                scannersListBox.SelectedIndex = i == 0 ? (scannersListBox.Items.Count == 0 ? -1 : 0) : i - 1;
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        UpdateScannerList(false);
                        break;
                }
                scannersListBox.EndUpdate();
            }), e);
        }

        private NDevice GetSelectedDevice()
        {
            return (NDevice)this.Invoke((Func<NDevice>)(() =>
            {
                return scannersListBox.SelectedItem == null ? null : (NDevice)scannersListBox.SelectedItem;
            }));

//            return scannersListBox.SelectedItem == null ? null : (NDevice)scannersListBox.SelectedItem;
        }

        private void UpdateScannerList(bool refresh)
        {
            scannersListBox.BeginUpdate();
            try
            {
                scannersListBox.Items.Clear();

                 //if (refresh)
                //{
                //    _deviceManager.Devices.CollectionChanged -= deviceManager_CollectionChanged;
                //    _deviceManager.Dispose();
                //    //_deviceManager = _biometricClient.DeviceManager;
                //    _deviceManager = new NDeviceManager { DeviceTypes = NDeviceType.FingerScanner, AutoPlug = true };
                //    _deviceManager.Initialize();
                //    _deviceManager.Devices.CollectionChanged += deviceManager_CollectionChanged;
                //}

                //if (_biometricClient.DeviceManager != null)
                if (_deviceManager != null)
                {
                    //_deviceManager.Reset();
                    //_deviceManager.Initialize();
                    //foreach (NDevice item in _biometricClient.DeviceManager.Devices)
                    foreach (NDevice item in _deviceManager.Devices)
                    {
                        scannersListBox.Items.Add(item);
                    }
                    if (scannersListBox.Items.Count != 0)
                        scannersListBox.SelectedIndex = 0;
                }
            }
            finally
            {
                scannersListBox.EndUpdate();
                //_deviceManager.Dispose();
            }
        }

        private delegate NFinger CaptureFingersDelegate(NFinger finger, short numberOfFingers);
        private NFinger CaptureFingers(NFinger finger, short numberOfFingers)
        {
            finger.ImpressionType = NFImpressionType.LiveScanPlain;

            switch (numberOfFingers)
            {
                //case 1:
                //    finger.Position = NFPosition.Unknown;
                //    break;
                case 2:
                    finger.Position = NFPosition.UnknownTwoFingers;
                    break;
                //case 3:
                    //finger.Position = NFPosition.LeftIndex & NFPosition.LeftMiddle & NFPosition.LeftRing;
                    //finger.Position = NFPosition.UnknownThreeFingers;
                    //break;
                case 4:
                    finger.Position = NFPosition.PlainLeftFourFingers;
                    break;
            }

            var fScanner = (NFScanner)GetSelectedDevice();
            fScanner.CapturePreview += DeviceCapturePreview;
            fScanner.Capture(finger);
            fScanner.CapturePreview -= DeviceCapturePreview;
            return finger;
        }

        private void DeviceCapturePreview(object sender, NBiometricDeviceCapturePreviewEventArgs e)
        {
            this.BeginInvoke((Action)(() =>
            {
                clearLog();
            }));


            foreach (var obj in ((NFrictionRidge)e.Biometric).Objects)
            {
                this.BeginInvoke((Action<string>)((txt) =>
                {
                    LogLine(txt, true);
                }), string.Format("{0}: {1}", obj.Position, obj.Status));
            }
        }

        private void startCapturing()
        {
            Boolean fileTemplate = false;

            if (ConfigurationManager.AppSettings["fingerFrom"] == "file")
                fileTemplate = true;

            if (_isCapturing)
                return;

            if (_biometricClient.FingerScanner == null && !fileTemplate)
            {
                ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                string text = rm.GetString("msgNoScannersAttached"); // "No scanners attached"
                LogLine(text, true);
                ShowErrorMessage(text);
                return;
            }
            else
            {
                short count = 0;
                //if (radioButtonIdentify.Checked)
                //{
                //    CheckBox bb;
                //    for (int i = 0; i < 10; i++)
                //    {
                //        bb = this.Controls.Find("checkBox" + (i + 1).ToString(), true)[0] as CheckBox;
                //        if (bb.Checked)
                //            count++;
                //    }

                //    if (count == 0)
                //        return;
                //}

                if (radioButtonIdentify.Checked && !fileTemplate)
                {
                    CheckBox bb;
                    for (int i = 0; i < 10; i++)
                    {
                        bb = this.Controls.Find("checkBox" + (i + 1).ToString(), true)[0] as CheckBox;
                        if (bb.Checked)
                            count++;
                    }

                    if (count == 0)
                        return;

                    var fScanner = ((NFingerScanner)GetSelectedDevice());
                    if (fScanner == null)
                    {
                        ShowErrorMessage("No scanner selected");
                        return;
                    }

                    var supportedPositions = fScanner.GetSupportedPositions();
                    if (supportedPositions.Contains<NFPosition>(NFPosition.PlainLeftFourFingers))
                    {
                        //if (count < 2)
                        //{
                        //    ShowErrorMessage("At least 2 fingers should be selected");
                        //    return;
                        //}
                        if (count != 2 && count != 4)
                        {
                            ShowErrorMessage("2 or 4 fingers might be selected");
                            return;
                        }
                    }
                    else if (count != 2)
                    {
                        ShowErrorMessage("2 fingers might be selected");
                        return;
                    }

                    clear();
                    clearFingerBoxes();
                }

                _isCapturing = true;

                //if (radioButtonIdentify.Checked)
                //{
                //    clear();
                //    clearFingerBoxes();
                //}

                EnableSemaphorControls(true);
                EnableControls(false);
                pictureBoxCheckMark.Image = null;
                pictureBoxPhoto.Image = null;

//                buttonScan.Text = "Cancel";

                startProgressBar();

#pragma warning disable 0162
                if (fileTemplate && radioButtonIdentify.Checked)  // if true, proceed without scanning, file template will be used
                {
                    ProcessTemplate(false); //only false
                    _isCapturing = false;
                    //_subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");

                    //Mode = ProgramMode.Identification;
                    //BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.GetTemplateBuffer().ToArray()); }));
                    return;
                }

                //using (var subject = new NSubject())
                //using (var finger = new NFinger())

                //_subject2 = null;

                // Create a finger
                // Add finger to the subject and fingerView
                if (_subject2 == null)
                {
                    _subject2 = new NSubject();
                    //var subjectFinger = new NFinger();
                    //_subject2.Fingers.Add(subjectFinger);
                    //_subjectFinger.PropertyChanged += OnAttributesPropertyChanged;
                    //fingerView2.Finger = subjectFinger;
                    //fingerView2.ZoomToFit = true;
                    fingerView2.ShownImage = ShownImage.Result;
                    //_biometricClient.FingersReturnProcessedImage = true;
                    _biometricClient.FingersReturnBinarizedImage = true;
                }

                if (_subject2.Fingers.Count > 0)
                    _subject2.Fingers.RemoveAt(0);

                var finger = new NFinger();
                //finger.ImpressionType = NFImpressionType.LiveScanPlain;
                //finger.CaptureOptions = NBiometricCaptureOptions.None;
                //finger.PropertyChanged += OnAttributesPropertyChanged;

                _subject2.Fingers.Add(finger);
                fingerView2.Finger = finger;
                fingerView2.ShownImage = ShownImage.Original;

                //if (_subject2.Fingers.Count > 0)
                //    _subject2.Fingers.RemoveAt(0);
                //else
                //    fingerView2.Finger = subjectFinger;

                //subjectFinger.PropertyChanged += OnAttributesPropertyChanged;
                //fingerView2.Finger = subjectFinger;

                _biometricClient.FingersTemplateSize = NTemplateSize.Small;
                //_biometricClient.FingersReturnProcessedImage = true;

                AsyncCallback callback = new AsyncCallback(OnImage);

                // Begin capturing
                if (radioButtonIdentify.Checked)
                {
                    var del = new CaptureFingersDelegate(CaptureFingers);
                    IAsyncResult result = del.BeginInvoke(finger, count, OnImage, null);

                    //IAsyncResult result = BeginInvoke(new Action(delegate() {
                    ////BeginInvoke((Action)(() =>
                    //    finger.Position = NFPosition.UnknownTwoFingers;
                    //    var fScanner = (NFScanner)_device;
                    //    fScanner.Capture(finger);
                    //}), OnImage, null);
                }
                else if (radioButtonVerify.Checked)
                {
                    finger.Position = NFPosition.Unknown;
                    NBiometricTask task = _biometricClient.CreateTask(NBiometricOperations.Capture | NBiometricOperations.CreateTemplate, _subject2);
                    _biometricClient.BeginPerformTask(task, OnEnrollFromScannerCompleted, null);
                }
            }
        }

        private void stopCapturing(bool mouseclick = false)
        {
            if (_isCapturing)
            {
                if (radioButtonIdentify.Checked && mouseclick)
                    ((NFScanner)GetSelectedDevice()).Cancel();
                else
                    _biometricClient.Cancel();
                
                _isCapturing = false;

                EnableSemaphorControls(false);
            }
            else if (radioButtonIdentify.Checked)
            {
                if (IsMatchingServiceRunning)
                {
                    terminateService();

                    //dynamic client = null;
                    //if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
                    //    client = new MemoryCacheMatchingService.MatchingServiceClient();
                    //else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                    //    client = new AppFabricCacheMatchingService.MatchingServiceClient();


                    //if (client != null)
                    //    client.Terminate();
                    //else
                    //    terminateMatchingService();

                    //if (ConfigurationManager.AppSettings["cachingProvider"] != "ODBCCache")
                    //{
                    //    var matchingServiceClient = new AppFabricCacheMatchingService.MatchingServiceClient();
                    //    matchingServiceClient.Terminate();
                    //}
                    //else
                    //    terminateMatchingService();
                }
            }

            if (radioButtonVerify.Checked || (mouseclick && !IsMatchingServiceRunning))
            {
                EnableControls(true);
//                buttonScan.Text = "Scan";
                stopProgressBar();
            }

            //FPScanner scanner;
            //if (scannerMan.Scanners.Count != 0)
            //    scanner = scannerMan.Scanners[0];
            //else
            //    return;

            //try
            //{
            //    if (scanner.IsCapturing)
            //    {
            //        scanner.StopCapturing();
            //        pictureBoxGreen.Active = false;
            //        pictureBoxGreen.Invalidate();
            //        pictureBoxRed.Active = true;
            //        pictureBoxRed.Invalidate();
            //        if (Mode != ProgramMode.PreEnrolled)
            //        {
            //            radioButtonVerify.Enabled = false;
            //            radioButtonIdentify.Enabled = false;
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    string text = string.Format("Error stoppping capturing on scanner {0}: {1}", scanner.Id, ex.Message);
            //    ShowErrorMessage(text);
            //    //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
        }
        
        //private void OnImage(IAsyncResult r)
        //{
        //    if (InvokeRequired)
        //    {
        //        BeginInvoke(new AsyncCallback(OnImage), r);
        //    }
        //    else
        //    {

        private void OnImage(IAsyncResult iar)
        {

            EnableSemaphorControls(false);
            stopCapturing();

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
            NFinger fingers = null;
            try {
                AsyncResult result = (AsyncResult)iar;
                CaptureFingersDelegate caller = (CaptureFingersDelegate)result.AsyncDelegate;
                fingers = (NFinger)caller.EndInvoke(result);
            }
            catch (Exception ex)
            {
                this.Invoke((Action<string>)((txt) =>
                {
                    ShowErrorMessage(txt);
                    EnableControls(true);
                }), ex.Message);
                return;
            }

            //fingers.Save()

            var obj = fingers.Objects[0];
            if (obj.Status == NBiometricStatus.Ok)
            {
                if (_subject2.Fingers.Count > 0)
                    _subject2.Fingers.RemoveAt(0);

                //var subject = new NSubject();
                var finger = new NFinger { Image = fingers.Image };
                //finger = new NFinger { I}
                finger.Position = fingers.Position;
                _subject2.Fingers.Add(finger);

                //NSubject probeSubject = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());

                //finger.Position = NFPosition.UnknownTwoFingers;
                //_subject.MissingFingers.Clear();
                //foreach (Object o in chlbMissing.CheckedItems)
                //{
                //    _subject.MissingFingers.Add((NFPosition)o);
                //}
                //_biometricClient.FingersDeterminePatternClass = true;
                //_biometricClient.FingersCalculateNfiq = true;
                //NBiometricTask task = _biometricClient.CreateTask(NBiometricOperations.Segment | NBiometricOperations.CreateTemplate | NBiometricOperations.AssessQuality, _subject2);
                //bool segmentationOnServer = true;
                //if (segmentationOnServer)
                {
                    NBiometricTask task = _biometricClient.CreateTask(NBiometricOperations.Segment | NBiometricOperations.CreateTemplate, _subject2);
                    _biometricClient.BeginPerformTask(task, OnSegmentCompleted, null);
                }
                //else
                //{
                //    _biometricClient.CreateTemplate(_subject2);
                //    Mode = ProgramMode.Identification;
                //    BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.GetTemplateBuffer().ToArray()); }));
                //}

                //if (_subject2.Fingers[0].Objects[0].Template != null)
                //{
                //    string text = string.Format("Template extracted {0}. Size: {1}",
                //        string.Format(". Quality: {0}", _subject2.Fingers[0].Objects[0].Quality),
                //        _subject2.Fingers[0].Objects[0].Template.GetSize());

                //    this.Invoke((Action<string>)((txt) =>
                //    {
                //        LogLine(txt, true);
                //    }), text);

                //    //File.WriteAllBytes("TwoFingersTemplate.temp", _subject2.Fingers[0].Objects[0].Template.Save().ToArray());

                //    Mode = ProgramMode.Identification;
                //    BeginInvoke(new MethodInvoker(delegate() { doIdentify(_subject2.GetTemplateBuffer().ToArray()); }));
                //    //BeginInvoke(new MethodInvoker(delegate() { doIdentify(_subject2.Fingers[0].Objects[0].Template); }));
                //}
                //else
                //{
                //    obj.Status = NBiometricStatus.BadObject;
                //}


            }

            if (obj.Status != NBiometricStatus.Ok)
            {
                _subject2 = null;

                string text = string.Format("Extraction failed: {0}", obj.Status);

                this.Invoke((Action<string>)((txt) =>
                {
                    ShowErrorMessage(txt);
                    LogLine(txt, true);
                    EnableControls(true);
                }), text);
            }
        }

        //NSubject probeSubject = null;

        private void OnSegmentCompleted(IAsyncResult r)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnSegmentCompleted), r);
            }
            else
            {
                NBiometricTask task = _biometricClient.EndPerformTask(r);
                //NBiometricStatus status = task.Status;
                //if (task.Error != null) Utils.ShowException(task.Error);

                //lblStatus.Text = string.Format("Segmentation status: {0}", status);
                //lblStatus.ForeColor = status == NBiometricStatus.Ok ? Color.Green : Color.Red;
                //ShowSegments();
                //btnSaveImages.Enabled = status == NBiometricStatus.Ok;

                if (task.Error == null && task.Status == NBiometricStatus.Ok) {

                    ProcessTemplate(true);
/*
                    var fingerList = new List<NFPosition>();
                    CheckBox cb;
                    int i;
                    for (i = 4; i > 0; i--)
                    {
                        cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                        if (cb.Checked)
                            fingerList.Add(getFingerPositionByTag(cb.Tag as string));
                    }

                    for (i = 5; i < 11; i++)
                    {
                        cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                        if (cb.Checked)
                            fingerList.Add(getFingerPositionByTag(cb.Tag as string));
                    }

                    //NSubject _probeSubject = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
                    //_probeSubject.Fingers[0].Objects[0].Template.Position = fingerList[1];
                    //_probeSubject.Fingers[1].Objects[0].Template.Position = fingerList[0];

                    var sb = new StringBuilder();

                    int segmentsCount = _subject2.Fingers.Count - 1;
                    i = 1;

#pragma warning disable 0162
                    if (false)
                    {
                        _subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");
                        segmentsCount = _subject2.Fingers.Count;
                        i = 0;
                    }
#pragma warning restore 0162

                    if (segmentsCount > 0 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
                    {
                        sb.Append(string.Format("Templates extracted: \n"));
                        if (_subject2.Fingers[i].Objects[0].Template != null)
                        {
                            _subject2.Fingers[i].Position = fingerList[0];
                            sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[0].ToString(),
                                                string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                        }
                        //finger = _subject2.Fingers[1];
                    }
                    i++;
                    if (segmentsCount > 1 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
                    {
                        if (_subject2.Fingers[i].Objects[0].Template != null)
                        {
                            _subject2.Fingers[i].Position = fingerList[1];
                            sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[1].ToString(),
                                                string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                        }
                    }
                    i++;
                    if (segmentsCount > 2 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
                    {
                        if (_subject2.Fingers[i].Objects[0].Template != null)
                        {
                            _subject2.Fingers[i].Position = fingerList[2];
                            sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[2].ToString(),
                                                string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                        }
                    }
                    i++;
                    if (segmentsCount > 3 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
                    {
                        if (_subject2.Fingers[i].Objects[0].Template != null)
                        {
                            _subject2.Fingers[i].Position = fingerList[3];
                            sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[3].ToString(),
                                                string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                        }
                    }

                    this.Invoke((Action<string>)((txt) =>
                    {
                        LogLine(txt, true);
                    }), sb.ToString());

                    //_subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");


                    //_biometricClient.MatchingWithDetails = true;
                    //if (probeSubject == null)
                    //{
                    //    //File.WriteAllBytes(@"C:\roman\psc\wsq\LeftIndexMiddle.template", _subject2.GetTemplateBuffer().ToArray());
                    //    probeSubject = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
                    //}
                    //else {
                    //    _subject2 = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
                    //    //File.WriteAllBytes(@"C:\roman\psc\wsq\LeftIndexRing.template", _subject2.GetTemplateBuffer().ToArray());
                    //}

                    //var stw = new System.Diagnostics.Stopwatch();
                    //stw.Start();

                    //bool matched = true;
                    //if (false)
                    //{
                    //    int threshold = _biometricClient.FingersQualityThreshold;
                    //    var status = _biometricClient.Verify(probeSubject, _subject2);
                    //    if (status == NBiometricStatus.Ok) {
                    //        foreach (var matchingResult in probeSubject.MatchingResults)
                    //        {
                    //            //int fsc = matchingResult.MatchingDetails.FingersScore;
                    //            foreach (var finger in matchingResult.MatchingDetails.Fingers)
                    //            {
                    //                if (threshold > finger.Score)
                    //                {
                    //                    matched = false;
                    //                    break;
                    //                }
                    //            }

                    //            if (!matched)
                    //                break;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (var finger in _subject2.Fingers)
                    //    {
                    //        if (finger.Objects[0].Template == null)
                    //            continue;

                    //        var template = new NFTemplate();
                    //        template.Records.Add(finger.Objects[0].Template);
                    //        var subject = NSubject.FromMemory(template.Save().ToArray());
                    //        var status = _biometricClient.Verify(probeSubject, subject);
                    //        if (status != NBiometricStatus.Ok)
                    //        {
                    //            matched = false;
                    //            break;
                    //        }
                    //    }
                    //}

                    //stw.Stop();
                    //string str = string.Format("{0}, Time elapsed: {1:ss\\.fffff}", !matched ? "Failure" : "Succeess", stw.Elapsed);
                    //LogLine(str, true);
                    ////}

                    //EnableControls(true);
                    //return;
                    //var sp = _biometricClient.FingersMatchingSpeed;


                    Mode = ProgramMode.Identification;
                    BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.GetTemplateBuffer().ToArray()); }));
                    //BeginInvoke(new MethodInvoker(delegate() { doIdentify(_subject2.Fingers); }));
                    //BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.Fingers[0].Objects[0].Template); }));
*/
                }
                else
                {
                    string error = "";
                    if (task.Error != null)
                        error = task.Error.Message;
                    else
                        error = task.Status.ToString();

                    _subject2 = null;

                    //string text = string.Format("Segmentation failed: {0}", error);

                    this.Invoke((Action<string>)((txt) =>
                    {
                        ShowErrorMessage(txt);
                        LogLine(txt, true);
                        EnableControls(true);
                    }), string.Format("Segmentation failed: {0}", error));

                    System.Timers.Timer timer = new System.Timers.Timer(1000);
                    //timer.Elapsed += async (sender, e) => await HandleTimer();
                    timer.Elapsed += OnTimedEvent;
                    timer.AutoReset = false;
                    timer.Start();

                    //startCapturing();

                }

                //NFAttributes attributes = finger.Objects.FirstOrDefault();
                //string p = @"Position: " + finger.Position;
                //if (attributes != null)
                //{
                //    string q = @"Quality: " + attributes.NfiqQuality;
                //    string c = @"Class: " + attributes.PatternClass;
                //}

            }
        }

        private void ProcessTemplate(bool scannedTemplate = true)      // it can be a template from a file 
        {
            var fingerList = new List<NFPosition>();
            CheckBox cb;
            int i;
            for (i = 4; i > 0; i--)
            {
                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    fingerList.Add(getFingerPositionByTag(cb.Tag as string));
            }

            for (i = 5; i < 11; i++)
            {
                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                    fingerList.Add(getFingerPositionByTag(cb.Tag as string));
            }

            //NSubject _probeSubject = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
            //_probeSubject.Fingers[0].Objects[0].Template.Position = fingerList[1];
            //_probeSubject.Fingers[1].Objects[0].Template.Position = fingerList[0];

            var sb = new StringBuilder();

            int segmentsCount;

#pragma warning disable 0162
            if (scannedTemplate)
            {
                segmentsCount = _subject2.Fingers.Count - 1;
                i = 1;
            }
            else
            {
                //_subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexRing.template");
                //_subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");
                _subject2 = NSubject.FromFile(@"LeftIndexMiddle.template");
                segmentsCount = _subject2.Fingers.Count;
                i = 0;
            }
#pragma warning restore 0162

            if (segmentsCount > 0 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
            {
                sb.Append(string.Format("Templates extracted: \n"));
                if (_subject2.Fingers[i].Objects[0].Template != null)
                {
                    _subject2.Fingers[i].Position = fingerList[0];
                    sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[0].ToString(),
                                        string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                }
                //finger = _subject2.Fingers[1];
            }
            i++;
            if (segmentsCount > 1 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
            {
                if (_subject2.Fingers[i].Objects[0].Template != null)
                {
                    _subject2.Fingers[i].Position = fingerList[1];
                    sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[1].ToString(),
                                        string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                }
            }
            i++;
            if (segmentsCount > 2 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
            {
                if (_subject2.Fingers[i].Objects[0].Template != null)
                {
                    _subject2.Fingers[i].Position = fingerList[2];
                    sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[2].ToString(),
                                        string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                }
            }
            i++;
            if (segmentsCount > 3 && _subject2.Fingers[i].Status == NBiometricStatus.Ok)
            {
                if (_subject2.Fingers[i].Objects[0].Template != null)
                {
                    _subject2.Fingers[i].Position = fingerList[3];
                    sb.Append(string.Format("{0}: {1}. Size: {2}\n", fingerList[3].ToString(),
                                        string.Format("Quality: {0}", _subject2.Fingers[i].Objects[0].Quality), _subject2.Fingers[i].Objects[0].Template.GetSize()));
                }
            }

            this.Invoke((Action<string>)((txt) =>
            {
                LogLine(txt, true);
            }), sb.ToString());

            //_subject2 = NSubject.FromFile(@"C:\roman\psc\wsq\LeftIndexMiddle.template");


            //_biometricClient.MatchingWithDetails = true;
            //if (probeSubject == null)
            //{
            //    //File.WriteAllBytes(@"C:\roman\psc\wsq\LeftIndexMiddle.template", _subject2.GetTemplateBuffer().ToArray());
            //    probeSubject = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
            //}
            //else {
            //    _subject2 = NSubject.FromMemory(_subject2.GetTemplateBuffer().ToArray());
            //    //File.WriteAllBytes(@"C:\roman\psc\wsq\LeftIndexRing.template", _subject2.GetTemplateBuffer().ToArray());
            //}

            //var stw = new System.Diagnostics.Stopwatch();
            //stw.Start();

            //bool matched = true;
            //if (false)
            //{
            //    int threshold = _biometricClient.FingersQualityThreshold;
            //    var status = _biometricClient.Verify(probeSubject, _subject2);
            //    if (status == NBiometricStatus.Ok) {
            //        foreach (var matchingResult in probeSubject.MatchingResults)
            //        {
            //            //int fsc = matchingResult.MatchingDetails.FingersScore;
            //            foreach (var finger in matchingResult.MatchingDetails.Fingers)
            //            {
            //                if (threshold > finger.Score)
            //                {
            //                    matched = false;
            //                    break;
            //                }
            //            }

            //            if (!matched)
            //                break;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var finger in _subject2.Fingers)
            //    {
            //        if (finger.Objects[0].Template == null)
            //            continue;

            //        var template = new NFTemplate();
            //        template.Records.Add(finger.Objects[0].Template);
            //        var subject = NSubject.FromMemory(template.Save().ToArray());
            //        var status = _biometricClient.Verify(probeSubject, subject);
            //        if (status != NBiometricStatus.Ok)
            //        {
            //            matched = false;
            //            break;
            //        }
            //    }
            //}

            //stw.Stop();
            //string str = string.Format("{0}, Time elapsed: {1:ss\\.fffff}", !matched ? "Failure" : "Succeess", stw.Elapsed);
            //LogLine(str, true);
            ////}

            //EnableControls(true);
            //return;
            //var sp = _biometricClient.FingersMatchingSpeed;


            Mode = ProgramMode.Identification;
            BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.GetTemplateBuffer().ToArray()); }));
            //BeginInvoke(new MethodInvoker(delegate() { doIdentify(_subject2.Fingers); }));
            //BeginInvoke(new MethodInvoker(delegate () { doIdentify(_subject2.Fingers[0].Objects[0].Template); }));


        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            startCapturing();
        }

        private Task HandleTimer()
        {
            startCapturing();
            return null;
        }

        private NFPosition getFingerPositionByTag(string tag)
        {
            switch (tag)
            {
                case "li":
                    return NFPosition.LeftIndex;
                case "lm":
                    return NFPosition.LeftMiddle;
                case "lr":
                    return NFPosition.LeftRing;
                case "ll":
                    return NFPosition.LeftLittle;
                case "ri":
                    return NFPosition.RightIndex;
                case "rm":
                    return NFPosition.RightMiddle;
                case "rr":
                    return NFPosition.RightRing;
                case "rl":
                    return NFPosition.RightLittle;
                case "lt":
                    return NFPosition.LeftThumb;
                case "rt":
                    return NFPosition.RightThumb;
                default:
                    return NFPosition.Unknown;
            }
        }

        private void OnFingerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                var finger = sender as NFinger;
                BeginInvoke(new Action<NBiometricStatus>(status => toolStripStatusLabelError.Text = status.ToString()), finger.Status);
            }
        }

        private void OnAttributesPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                var finger = sender as NFinger;
                BeginInvoke(new Action<NBiometricStatus>(status => toolStripStatusLabelError.Text = status.ToString()), finger.Status);
            }
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

        private void OnEnrollFromScannerCompleted(IAsyncResult r)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnEnrollFromScannerCompleted), r);
            }
            else
            {
                //fingerView2.Finger = _subject2.Fingers[0];

                NBiometricTask task = _biometricClient.EndPerformTask(r);
                //EnableControls(false);
                NBiometricStatus status = task.Status;

                stopCapturing();

                // Check if extraction was canceled
                if (status == NBiometricStatus.Canceled) return;

                if (status == NBiometricStatus.Ok)
                {
                    ////this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

                    //string text = string.Format("Template extracted {0}. Size: {1}",
                    //    string.Format(". Quality: {0}", _subject2.Fingers[0].Objects[0].Quality),
                    //    _subject2.Fingers[0].Objects[0].Template.GetSize());

                    ////string text = string.Format("Template extracted {0}",
                    ////    string.Format(". Quality: {0}", _subject2.Fingers[0].Objects[0].Quality));

                    ////fingerView2.ShownImage = ShownImage.Result;

                    ////ShowErrorMessage(text);
                    //LogLine(text, true);

                    //if (radioButtonIdentify.Checked)
                    //{
                    //    Mode = ProgramMode.Identification;
                    //    BeginInvoke(new MethodInvoker(delegate() { doIdentify(_subject2.Fingers[0].Objects[0].Template); }));
                    //    //startDataServiceProcess();          // go to get a photo
                    //}
                    //else if (radioButtonVerify.Checked)
                    //{
                        Mode = ProgramMode.Verification;
                        BeginInvoke(new MethodInvoker(delegate { doVerify(); }));
                    //}


                    //LogLine("Template extracted{0}. G: {1}. Size: {2}", true,
                    //    Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Helpers.QualityToPercent(template.Quality) / 100.0) : null,
                    //    template.G, Data.SizeToString(template.Save().Length));

                    //ShowStatusMessage(String.Format("Template extracted{0}. G: {1}. Size: {2}", true,
                    //    Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Helpers.QualityToPercent(template.Quality) / 100.0) : null,
                    //    template.G, Data.SizeToString(template.Save().Length)));

                    //lblQuality.Text = String.Format("Quality: {0}", _subjectFinger.Objects[0].Quality);
                }
                else
                {
                    string text = string.Format("Extraction failed: {0}", status);
                    ShowErrorMessage(text);
                    LogLine(text, true);

                    //pictureBoxCheckMark.Image = Properties.Resources.redcross;

                    //MessageBox.Show(string.Format("The template was not extracted: {0}.", status), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _subject2 = null;
                    //_subjectFinger = null;
                    EnableControls(true);
                }
            }
        }

        //private void scanner_ImageScanned(object sender, FPScannerImageScannedEventArgs ea)
        //{
        //    OnImage((NGrayscaleImage)ea.Image.Clone());
        //}

        private void enrollFromWSQ(WsqImage wsqImage)
        {
            //string fileName = "lindex.wsq";

            //if (!isUserIdValid())
            //    return;

            //DBHelper.DBUtil db = new DBHelper.DBUtil();
            //byte[] serializedArrayOfWSQ = db.GetImage(this.userId, true);

            //MemoryStream ms = new MemoryStream(serializedArrayOfWSQ);

            ////Assembly.Load(string assemblyString)
            //// Construct a BinaryFormatter and use it to deserialize the data to the stream.
            //BinaryFormatter formatter = new BinaryFormatter();
            //ArrayList fingersCollection = null;
            //try
            //{
            //    formatter.Binder = new GenericBinder<WsqImage>();
            //    fingersCollection = formatter.Deserialize(ms) as ArrayList;
            //}
            //catch (SerializationException ex)
            //{
            //    LogLine(ex.Message, true);
            //    return false;
            //}
            //finally
            //{
            //    ms.Close();
            //}

            //WsqImage im = fingersCollection[0] as WsqImage;
 

            //im.Content = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_WSQ);
            //im.XRes = img.GetXRes();
            //im.YRes = img.GetYRes();
            //im.XSize = img.GetXSize();
            //im.YSize = img.GetYSize();
            //PixelFormat == 1 GRAY
            //PixelFormat == 6 RGB

                        //byte[] buffer = null;
            //string fileName = "lindex.wsq";
            //FileStream fs = null;
            //try
            //{
            //    fs = new FileStream(fileName, FileMode.Create);
            //    BinaryWriter bw = new BinaryWriter(fs);
            //    bw.Write(wsqImage.Content);
            //    bw.Flush();
            //}
            //finally
            //{
            //    fs.Dispose();
            //}

            //MemoryStream ms = null;
            NImage image = null;
            NFinger finger = null;
            try
            {
                //Bitmap bm = new Bitmap(ms);
                //nImage = NImage.FromBitmap(bm);

                //string fileName = @"c:\psc\wsq\lindex.wsq";
                //nImage = NImage.FromFile(fileName, NImageFormat.Wsq);
                fingerView1.Finger = null;
                //fingerView2.Finger = null;

                image = NImage.FromMemory(wsqImage.Content);
                finger = new NFinger { Image = image };

                _subject = new NSubject();
                _subject.Fingers.Add(finger);
                fingerView1.Finger = finger;
                //fingerView1.ShownImage = ShownImage.Original;

                NBiometricTask task = _biometricClient.CreateTask(NBiometricOperations.Segment | NBiometricOperations.CreateTemplate, _subject);
                _biometricClient.PerformTask(task);
                if (!(task.Error == null && task.Status == NBiometricStatus.Ok))
                {
                    if (task.Error != null)
                        throw new Exception("Template creation error:" + task.Error);
                    else
                        throw new Exception("Template creation error:" + task.Status);
                }

                //_subject.Fingers[0].Image.Save("leftIndex.wsq");
                //_biometricClient.CreateTemplate(_subject);
                //File.WriteAllBytes("leftIndex.template", _subject.GetTemplateBuffer().ToArray());

                //_subjectFinger = new NFinger { Image = image };
                //_subject = new NSubject();
                //_subject.Fingers.Add(_subjectFinger);
                //fingerView2.Finger = _subjectFinger;
                //fingerView2.ShownImage = ShownImage.Original;
                //_biometricClient.CreateTemplate(_subject);


                //ms = new MemoryStream(wsqImage.Content);
                //!!!!!!!!!!!                nImage = NImageFormat.Wsq.LoadImage(ms);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("Error creating image retrieved from database {0}", ex.Message),
                //  Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                string text = string.Format("Error creating image retrieved from database: {0} ", ex.Message);
                ShowErrorMessage(text);

                //return false;
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }

                if (finger != null)
                    finger = null;

                //if (ms != null)
                  //  ms.Dispose();
            }


            EnableControls(true);
            stopProgressBar();

            //float horzResolution = nImage.HorzResolution;
            //float vertResolution = nImage.VertResolution;
            //if (horzResolution < 250) horzResolution = 500;
            //if (vertResolution < 250) vertResolution = 500;

//!!!!!!!!            NGrayscaleImage grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, horzResolution, vertResolution, nImage);
//!!!!!!!!!!!!!!            OnImage(grayImage);
            //BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));


            //return true;
        }

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
            _subject = null;
            _subject2 = null;
            fingerView1.Finger = null;
            fingerView2.Finger = null;
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

            this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));

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
            if (_subject != null && _subject2 != null)
            {
                _biometricClient.MatchingWithDetails = true;
                //fingerView1.MatedMinutiae = null;
                //fingerView2.MatedMinutiae = null;

                var sw = System.Diagnostics.Stopwatch.StartNew();
                sw.Start();

                NBiometricStatus status;
                if (ConfigurationManager.AppSettings["verificationService"] == "local")
                {
                    try
                    {
                        status = _biometricClient.Verify(_subject, _subject2);
                    } catch(Exception ex)
                    {
                        while(ex.InnerException != null)
                            ex = ex.InnerException;

                        ShowErrorMessage("Matcher: " + ex.Message);
                        status = NBiometricStatus.MatchNotFound;
                    }
                }
                else
                {
                    bool retcode = false;

                    //var matchingServiceClient = new MemoryCacheMatchingService.MatchingServiceClient(_instanceContext);
                    var matchingServiceClient = new MemoryCacheMatchingService.MatchingServiceClient();
                    //var b = _subject.GetTemplateBuffer().ToArray();
                    //var b2 = _subject2.GetTemplateBuffer().ToArray();
                    try {
                        retcode = matchingServiceClient.verify(_subject.GetTemplateBuffer().ToArray(), _subject2.GetTemplateBuffer().ToArray());
                        //retcode = matchingServiceClient.verify(b, b2);
                    }
                    catch (Exception ex)
                    {
                        while (ex.InnerException != null)
                            ex = ex.InnerException;

                        ShowErrorMessage("Matcher: " + ex.Message);
                    }

                    if (retcode)
                        status = NBiometricStatus.Ok;
                    else
                        status = NBiometricStatus.MatchNotFound;
                }

                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                LogLine("Matched in " + elapsedTime, true);

                //LogLine(string.Format("Verification status: {0}", status ? "Success" : "Failue"), true);
                LogLine(string.Format("Verification status: {0}", status == NBiometricStatus.Ok ? "Success" : "Failue"), true);

                fingerView1.Finger = _subject.Fingers[1];

                //ShowStatusMessage(str);

                if (status == NBiometricStatus.Ok)
                //if (status)
                {
                    // Get matching score
                    //int score = _subject.MatchingResults[0].Score;
                    //LogLine(string.Format("Score of matched templates: {0}", score), true);
                    //if (_subject.MatchingResults.Count > 0)
                    //{
                    //    //fingerView1.ShownImage = ShownImage.Original;

                    //    var matedMinutiae = _subject.MatchingResults[0].MatchingDetails.Fingers[0].GetMatedMinutiae();

                    //    fingerView1.MatedMinutiaIndex = 0;
                    //    fingerView1.MatedMinutiae = matedMinutiae;

                    //    fingerView1.MatedMinutiaIndex = 1;
                    //    fingerView1.MatedMinutiae = matedMinutiae;
                    //}

                    //fingerView1.PrepareTree();
                    //fingerView2.Tree = fingerView1.Tree;

                    pictureBoxCheckMark.Image = Properties.Resources.checkmark;

                    //startProgressBar();
                    startDataServiceProcess();  // to get a picture

                    //Thread.Sleep(1000);
                    //this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));

                }
                else
                {
                    //LogLine(string.Format("Verification status: {0}", status), true);
                    pictureBoxPhoto.Image = null;
                    pictureBoxCheckMark.Image = Properties.Resources.redcross;
                    buttonScan.Focus();
                }

                //string str = string.Format("Verification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));



            }

            //NMMatchDetails matchDetails;
            //try
            //{
            //    score = Data.NMatcher.Verify(Template.Save(), this.enrolledTemplate.Save(), out matchDetails);
            //}
            //catch (Exception ex)
            //{
            //    string text = string.Format("Error verifying templates: {0}", ex.Message);
            //    ShowErrorMessage(text);

            //    LogLine(string.Format("Error verifying templates: {0}", ex.Message), true);

            //    pictureBox2.Image = Properties.Resources.redcross;

            //    return;
            //}

            //sw.Stop();

            //string str = string.Format("Verification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));

            //LogLine(str, true);

            //ShowStatusMessage(str);

            //if (score > 0)
            //    pictureBoxCheckMark.Image = Properties.Resources.checkmark;
            //else
            //    pictureBoxCheckMark.Image = Properties.Resources.redcross;

            //if (score > 0)
            //{
            //    startProgressBar();
            //    startDataServiceProcess();  // to get a picture
            //}

            //Thread.Sleep(1000);
            //this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
        }

        //class Record
        //{
        //    public UInt32 size;
        //    public byte[] template;
        //    //public String errorMessage;
        //    //public String[] errorMessage = new String[1];
        //    public System.Text.StringBuilder errorMessage;

        //}

        //private void doIdentify(NFRecord template)
        //private void doIdentify(NSubject.FingerCollection probeFingerCollection)
        private void doIdentify(byte[] probeTemplate)
        {
            //var pos = template.Position;
            //var dc = template.DoubleCores;

            //bool retcode = true;
            //long retcode = match();
            //System.Runtime.Remoting.ObjRef objRef = template.CreateObjRef(template.GetType());
            //UInt32 score = match(objRef);
            //byte[] ar = template.Save();
            //int i = ar.Length;
            //UInt32 score = match(template.Save);
            //UInt32 score = (UInt32)template.GetSize();

           //IntPtr* ptr = template; }

            //UInt32 score = match(template);

            //Record record = new Record();
            //record.size = (UInt32)template.GetSize();
            //record.template = template.Save();
            //record.errorMessage = new System.Text.StringBuilder(512);
            
            
            //record.errorMessage.Append("kuku");
            //record.errorMessage[0] = "kuku";
            //record.errorMessage = "kuku";
            //String errorMessage = "kuku";
            //pt.x = 5;
            //pt.y = 6;

            //byte[] bt;
            //this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            startProgressBar();
            startMatchingServiceProcess(probeTemplate);

            //UInt32 score = 0;
            //unsafe
            //{
            //    fixed (UInt32* ptr = &record.size)
            //    {
            //        score = match(record.template, record.size, record.errorMessage, record.errorMessage.Capacity);
            //    }
            //}

            //stopProgressBar();

            ////NMMatchDetails matchDetails;
            ////try
            ////{
            ////    //score = Data.NMatcher.Verify(Template.Save(), record.Template, out matchDetails);
            ////    score = Data.NMatcher.Verify(Template.Save(), this.enrolledTemplate.Save(), out matchDetails);
            ////}
            ////catch (Exception ex)
            ////{
            ////    //MessageBox.Show(string.Format("Error verifying templates: {0}", ex.Message),
            ////    //  Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

            ////    string text = string.Format("Error verifying templates: {0}", ex.Message);
            ////    ShowErrorMessage(text);

            ////    LogLine(string.Format("Error verifying templates: {0}", ex.Message), true);

            ////    pictureBox2.Image = Properties.Resources.redcross;

            ////    return;
            ////}

            ////sw.Stop();

            ////            string str = string.Format("Verification {0}. Time: {1}",
            ////              score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score), Data.TimeToString(sw.Elapsed));
            //string str = string.Format("Identification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));

            //LogLine(str, true);

            ////ShowStatusMessage(str);

            //personId.Text = "";
            //if (score > 0)
            //{
            //    this.userId = (int)score;
            //    personId.Text = score.ToString();
            //    pictureBox2.Image = Properties.Resources.checkmark;
            //} 
            //else
            //    pictureBox2.Image = Properties.Resources.redcross;

            //if (score > 0)
            //{
            //    startProgressBar();
            //    startDataServiceProcess();
            //    /*
            //                    DBHelper.DBUtil db = new DBHelper.DBUtil();
            //                    byte[] buffer;

            //                    MemoryStream ms = null;
            //                    try
            //                    {
            //                        if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
            //                            buffer = db.GetImageFromWebService(IMAGE_TYPE.picture, this.userId);
            //                        else
            //                            buffer = db.GetImage(IMAGE_TYPE.picture, this.userId);

            //                        ms = new MemoryStream(buffer);
            //                        pictureBox1.Image = Image.FromStream(ms);
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        ShowErrorMessage(ex.Message);
            //                        return;
            //                    }
            //                    finally
            //                    {
            //                        ms.Dispose();
            //                    }
            //    */
            //}
            //else
            //{
            //    if (record.errorMessage.Length != 0)
            //    {
            //        //retcode = false;
            //        MessageBox.Show(record.errorMessage.ToString());
            //        //ShowErrorMessage(record.errorMessage.ToString());
            //    }

            //}

            //return retcode;

        }

        //private void backgroundWorkerDataService_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    stopProgressBar();
        //    Application.DoEvents();

        //    //MessageBox.Show(toolStripProgressBar.Value.ToString());

        //    if (e.Error != null)
        //    {
        //        LogLine(e.Error.Message, true); 
        //        ShowErrorMessage(e.Error.Message);
        //    }
        //    else
        //    {
        //        try
        //        {
        //            if (mode == ProgramMode.Enroll)
        //                processEnrolledData(e.Result as byte[]);
        //            else if (mode == ProgramMode.Verify || mode == ProgramMode.Identify)
        //            {
        //                using (var ms = new MemoryStream(e.Result as byte[]))
        //                {
        //                    if (ms.Length != 0)
        //                        pictureBox1.Image = Image.FromStream(ms);
        //                    else
        //                        pictureBox1.Image = null;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogLine(ex.ToString(), true);
        //            ShowErrorMessage(ex.ToString());
        //        }
        //    }
        //}

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
            rtbMain.Clear();
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

                        LogLine("Enter Person ID:", true);
                        ShowStatusMessage("Enter Person ID:");

                        this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

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

            this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));


            Mode = ProgramMode.PreEnrolled;
            //setMode(mode);
//            setModeRadioButtons(mode);

            clear();
            clearFingerBoxes();

            if (!isUserIdValid())
                return;

            EnableControls(false);

            //startProgressBar();
            startDataServiceProcess();
        }

        private void buttonScan_Click(object sender, EventArgs e)
        {
            //if ((sender as ButtonBase).Enabled == false)
            //    return;

            if (!_isCapturing && !IsMatchingServiceRunning)
                BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
            else
                BeginInvoke(new MethodInvoker(delegate() { stopCapturing(true); }));
        }

        //private void buttonRequest_Click(object sender, EventArgs e)
        private bool processEnrolledData(byte[][] serializedWSQArray)
        {
            bool createTemplate = true;

            PictureBox pb;
            //NSubject[] subjects = new NSubject[10];

            if (serializedWSQArray == null)
            {
                clearFingerBoxes();
                this.Invoke((Action)(() =>
                {
                    ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                    string text = rm.GetString("msgThePersonHasNotYetBeenEnrolled"); // "The person has not yet been enrolled"
                    LogLine(text, true);
                    ShowErrorMessage(text);
                    //stopProgressBar();
                }), null);

                return false;
            }

            MemoryStream ms = new MemoryStream(serializedWSQArray[0]);

            if (_fingersCollection != null)
                _fingersCollection.Clear();

            //Assembly.Load(string assemblyString)
            // Construct a BinaryFormatter and use it to deserialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Binder = new GenericBinder<WsqImage>();
                _fingersCollection = formatter.Deserialize(ms) as ArrayList;
            }
            catch (SerializationException)
            {
                this.Invoke((Action)(() =>
                {
                    stopProgressBar();
                    LogLine("No user with provided ID found", true);
                    ShowErrorMessage("No user with provided ID found");
                }), null);

                return false;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }

            //byte[] buff = null;
            //System.Object theLock = new System.Object();

			WsqImage wsqImage = null;
            int bestQuality = 0;
            int bestQualityImage = 0;
            RadioButton rb = null; Label lb = null;
            //Bitmap bm = null;
            //bool rbChecked = false;
            //, pbChecked = false;

            _biometricClient.FingersTemplateSize = NTemplateSize.Small;
            _biometricClient.FingersFastExtraction = false;
            _biometricClient.FingersQualityThreshold = 48;
            //TimeSpan ts;
            //string elapsedTime;
            //var sw = System.Diagnostics.Stopwatch.StartNew();
            //sw.Start();

            _subject = new NSubject();

            //for (int j = 0; j < 3; j++)
            int pct = 0;
            for (int i = 0; i < _fingersCollection.Count; i++)
            {
                rb = this.Controls.Find("radioButton" + (i + 1).ToString(), true)[0] as RadioButton;
                lb = this.Controls.Find("label" + (i + 1).ToString(), true)[0] as Label;

                wsqImage = _fingersCollection[i] as WsqImage;
                if (wsqImage == null)
                {
                    this.Invoke((Action<RadioButton, Label>)((rbutton, lab) =>
                    {
                        rbutton.Enabled = false;
                        lab.Enabled = false;
                    }), rb, lb);
                }
                else
                {
                    this.Invoke((Action<RadioButton, Label>)((rbutton, lab) =>
                    {
                        rbutton.Enabled = true;
                        lab.Enabled = true;
                    }), rb, lb);
                }

                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as PictureBox;

                if (_fingersCollection[i] != null)
                {
                    //WsqImage wsq = _fingersCollection[i] as WsqImage;

                    //MemoryStream ms = null;
                    NImage nImage = null;
                    try
                    {
                        //ms = new MemoryStream(wsqImage.Content);
                        //bm = new Bitmap(ms);
                        nImage = NImage.FromMemory(wsqImage.Content, NImageFormat.Wsq);

                        this.Invoke((Action<NImage>)((img) =>
                        {
                            //pb.Image = nImage.ToBitmap();
                            pb.Image = img.ToBitmap();
                            pb.SizeMode = PictureBoxSizeMode.Zoom;
                        }), nImage);

                        if (!createTemplate)
                        {
                            if (serializedWSQArray[i + 1].Length != 0)
                            {
                                NFRecord record = new NFRecord(serializedWSQArray[i + 1]);
                                pct = record.Quality;
                                if (pct == 254)
                                    pct = 0;
                            }
                            else
                                pct = 0;

                            if (bestQuality < pct)
                            {
                                bestQuality = pct;
                                bestQualityImage = i;
                            }

                            this.Invoke((Action<int, int>)((percent, indx) =>
                            {
                                var lbf = (this.Controls.Find("lbFinger" + (indx + 1).ToString(), true)[0] as Label);
                                lbf.Text = string.Format("Q: {0:P0}", percent / 100.0);
                                if (pct > 79)
                                    lbf.ForeColor = Color.GreenYellow;
                                else if (pct > 39)
                                    lbf.ForeColor = Color.Orange;
                                else
                                    lbf.ForeColor = Color.Red;
                            }), pct, i);
                        }
                        else
                        {
                            //File.WriteAllBytes(saveFileDialog.FileName, _subject.GetTemplateBuffer().ToArray());

                            //string fileName = @"c:\psc\wsq\lindex.wsq";
                            //nImage = NImage.FromFile(fileName, NImageFormat.Wsq);

                            var finger = new NFinger { Image = nImage };
                            //if (subject.Fingers.Count > 0)
                            //    subject.Fingers.RemoveAt(0);

                            //var subject = new NSubject();
                            //_subjects[i].Fingers.Add(finger);
                            _subject.Fingers.Add(finger);
                            switch (rb.Tag as string)
                            {
                                case "li":
                                    finger.Position = NFPosition.LeftIndex;
                                    break;
                                case "lm":
                                    finger.Position = NFPosition.LeftMiddle;
                                    break;
                                case "lr":
                                    finger.Position = NFPosition.LeftRing;
                                    break;
                                case "ll":
                                    finger.Position = NFPosition.LeftLittle;
                                    break;
                                case "ri":
                                    finger.Position = NFPosition.RightIndex;
                                    break;
                                case "rm":
                                    finger.Position = NFPosition.RightMiddle;
                                    break;
                                case "rr":
                                    finger.Position = NFPosition.RightRing;
                                    break;
                                case "rl":
                                    finger.Position = NFPosition.RightLittle;
                                    break;
                                case "lt":
                                    finger.Position = NFPosition.LeftThumb;
                                    break;
                                case "rt":
                                    finger.Position = NFPosition.RightThumb;
                                    break;
                            }

                            //int q = GetImageQuality(subject, this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label);

                            //if (bestQuality < q)
                            //{
                            //    bestQuality = q;
                            //    bestQualityImage = i;
                            //}

                            //this.Invoke((Action<NImage>)((img) =>
                            //{
                            //    //pb.Image = nImage.ToBitmap();
                            //    pb.Image = img.ToBitmap();
                            //    pb.SizeMode = PictureBoxSizeMode.Zoom;
                            //}), nImage);

                            //if (rbChecked && !pbChecked)
                            //{
                            //    pb.BorderStyle = BorderStyle.Fixed3D;
                            //    pbChecked = true;
                            //}
                            //else
                            //    pb.BorderStyle = BorderStyle.FixedSingle;
                            //lab.Visible = true;
                            //pb.BringToFront();

                            //pb.MouseClick += new MouseEventHandler(fpPictureBox_MouseClick);

                            //if (pb.Tag != null)
                            //    _toolTip.SetToolTip(pb, rm.GetString(pb.Tag as string));

                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(string.Format("Error creating image retrieved from database {0}", ex.Message),
                        //  Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //string text = string.Format("Error creating image retrieved from database {0}", ex.Message);
                        //ShowErrorMessage(text);
                        if (ex.Message == "Operation is not activated")
                        {

                            this.Invoke((Action<string>)((text) =>
                            {
                                ShowErrorMessage(text);
                            }), ex.Message);
                            return false;
                        }

                        continue;
                    }
                    finally
                    {
                        if (nImage != null)
                        {
                            nImage.Dispose();
                            nImage = null;
                        }

                        if (ms != null)
                        {
                            ms.Close();
                            ms = null;
                        }
                    }

                    //if (!pb.Enabled)
                    //    pb.Enabled = true;

                    //lock (theLock)
                    //{
                    //    buff = ARHScanner.ConvertWSQToBmp(wsq);
                    //    ARHScanner.DisposeWSQImage();
                    //}
                }
                else
                {
                    this.Invoke((Action<PictureBox, int>)((pbox, idx) =>
                    {
                        pbox.Image = null;
                        this.Controls.Find("lbFinger" + (idx + 1).ToString(), true)[0].Text = "";
                    }), pb, i);
                }
            }


            //serializedWSQArray.

            if (!createTemplate)
            {
                if (radioButtonVerify.Checked)
                {
                    this.BeginInvoke((Action<int>)((best) =>
                    {
                        var rb2 = this.Controls.Find("radioButton" + (best + 1).ToString(), true)[0] as RadioButton;
                        BeginInvoke(new MethodInvoker(delegate() { checkRadioButton(rb2.Name); }));
                    }), bestQualityImage);
                }
                else if (radioButtonIdentify.Checked)
                {
                    CheckBox cb;
                    for (int i = 1; i < 11; i++)
                    {
                        cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                        if (cb.Checked)
                        {
                            this.BeginInvoke((Action<CheckBox>)((cbox) =>
                            {
                                var m = System.Text.RegularExpressions.Regex.Match(cbox.Name, @"\d+$");
                                fingerChanged(Int32.Parse(m.Value) - 1, false);
                            }), cb);
                        }
                    }
                }

                if (_subject != null)
                {
                    _subject.Dispose();
                    _subject = null;
                }
            }
            else
            {
                //_sw = System.Diagnostics.Stopwatch.StartNew();
                _biometricClient.BeginCreateTemplate(_subject, OnExtractionCompleted, null);
            }

            return true;
        }

        class StateObject
        {
            public int LoopCounter;
        }

        private void MyBeginCreateTemplate(NSubject[] subjects, NBiometricClient[] biometricClients, AsyncCallback callback)
        {

            Task[] taskArray = new Task[10];
            //Task[] taskArray = new Task[1];

            System.Diagnostics.Stopwatch sw;

            for (int i = 0; i < taskArray.Length; i++)
            {
                taskArray[i] = Task.Factory.StartNew((Object obj) =>
                {
                    StateObject state = obj as StateObject;
                    sw = System.Diagnostics.Stopwatch.StartNew();
                    biometricClients[state.LoopCounter].CreateTemplate(subjects[state.LoopCounter]);
                    sw.Stop();
                    TimeSpan ts = sw.Elapsed;
                    string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                    LogLine("Template " + state.LoopCounter + " creation time " + elapsedTime, true);

                },
                new StateObject() { LoopCounter = i });
            }

            try
            {
                Task.WaitAll(taskArray);
            }
            catch (AggregateException ex)
            {
                throw ex.Flatten();

                //Console.WriteLine(ex.Message);
            }

            //_sw.Stop();
            //TimeSpan ts = _sw.Elapsed;
            //string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
            //LogLine("Template creation time " + elapsedTime, true);
            ////sConsole.WriteLine("RunTime " + elapsedTime);

            callback(null);

        }

        private void OnExtractionCompleted(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnExtractionCompleted), result);
            }
            else
            {
                //_sw.Stop();
                //TimeSpan ts = _sw.Elapsed;
                //string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                //LogLine("Total template creation time " + elapsedTime, true);

                var lbs = new List<Label>();
                for (int i = 0; i < _fingersCollection.Count; i++)
                {
                    //lbs.Add(this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label);
                    //var lb = (this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label);
                    this.Invoke((Action<List<Label>, int>)((list, indx) =>
                    {
                        var lb = (this.Controls.Find("lbFinger" + (indx + 1).ToString(), true)[0] as Label);
                        list.Add(lb);
                    }), lbs, i);

                    //lbs.Add(lb);
                }

                try
                {
                    NBiometricStatus status = _biometricClient.EndCreateTemplate(result);
                }
                catch (Exception)
                {
                    this.Invoke((Action<List<Label>>)((lbss) =>
                    {
                        foreach (Label lb in lbss)
                        {
                            lb.Text = string.Format("Q: {0:P0}", 0);
                            lb.ForeColor = Color.Red;
                        }
                    }), lbs);
                }
                finally
                {
                    ///this.Invoke((Action)(() =>
                    //{
                    NFPosition bestQualityImagePosition = GetImageQuality(_subject);

                    if (radioButtonVerify.Checked)
                    {
                        if (bestQualityImagePosition != NFPosition.Unknown)
                        {
                            int bestQualityImageIndex = 0;
                            switch (bestQualityImagePosition)
                            {
                                case NFPosition.LeftIndex:
                                    bestQualityImageIndex = 1;
                                    break;
                                case NFPosition.LeftMiddle:
                                    bestQualityImageIndex = 2;
                                    break;
                                case NFPosition.LeftRing:
                                    bestQualityImageIndex = 3;
                                    break;
                                case NFPosition.LeftLittle:
                                    bestQualityImageIndex = 4;
                                    break;
                                case NFPosition.RightIndex:
                                    bestQualityImageIndex = 5;
                                    break;
                                case NFPosition.RightMiddle:
                                    bestQualityImageIndex = 6;
                                    break;
                                case NFPosition.RightRing:
                                    bestQualityImageIndex = 7;
                                    break;
                                case NFPosition.RightLittle:
                                    bestQualityImageIndex = 8;
                                    break;
                                case NFPosition.LeftThumb:
                                    bestQualityImageIndex = 9;
                                    break;
                                case NFPosition.RightThumb:
                                    bestQualityImageIndex = 10;
                                    break;
                            }

                            this.BeginInvoke((Action<int>)((best) =>
                            {
                                var rb = this.Controls.Find("radioButton" + best.ToString(), true)[0] as RadioButton;
                                BeginInvoke(new MethodInvoker(delegate () { checkRadioButton(rb.Name); }));
                            }), bestQualityImageIndex);
                        } else
                        {
                            this.Invoke((Action)(() =>
                            {
                                stopProgressBar();
                                //LogLine("No prescanned fingers found", true);
                                ShowErrorMessage("No prescanned fingers found");
                                EnableControls(true);
                            }));
                        }
                    }
                    else if (radioButtonIdentify.Checked)
                    {
                        CheckBox cb;
                        for (int i = 1; i < 11; i++)
                        {
                            cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                            if (cb.Checked)
                            {
                                this.BeginInvoke((Action<CheckBox>)((cbox) =>
                                {
                                    var m = System.Text.RegularExpressions.Regex.Match(cbox.Name, @"\d+$");
                                    fingerChanged(Int32.Parse(m.Value) - 1, false);
                                }), cb);
                            }
                        }
                    }

                    if (_subject != null)
                    {
                        _subject.Dispose();
                        _subject = null;
                    }
                    //}), null);
                }
            }
        }

        private NFPosition GetImageQuality(NSubject subject)
        {
            NFPosition bestQualityImageIndex = NFPosition.Unknown;

            var lbs = new List<Label>();
            for (int i = 0; i < _fingersCollection.Count; i++)
            {
                //lbs.Add(this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label);
                this.Invoke((Action<List<Label>, int>)((list, indx) =>
                {
                    var lb = (this.Controls.Find("lbFinger" + (indx + 1).ToString(), true)[0] as Label);
                    lb.Text = "";
                    list.Add(lb);
                }), lbs, i);

            }

            try
            {
                int pct = 0, bestQuality = 0;
                Label lb = null;
                //foreach (Label lb in lbs)
                for (int i = 0; i < subject.Fingers.Count; i++)
                {
                    //i++;
                    switch (subject.Fingers[i].Position)
                    {
                        case NFPosition.LeftIndex:
                            lb = lbs[0];
                            break;
                        case NFPosition.LeftMiddle:
                            lb = lbs[1];
                            break;
                        case NFPosition.LeftRing:
                            lb = lbs[2];
                            break;
                        case NFPosition.LeftLittle:
                            lb = lbs[3];
                            break;
                        case NFPosition.RightIndex:
                            lb = lbs[4];
                            break;
                        case NFPosition.RightMiddle:
                            lb = lbs[5];
                            break;
                        case NFPosition.RightRing:
                            lb = lbs[6];
                            break;
                        case NFPosition.RightLittle:
                            lb = lbs[7];
                            break;
                        case NFPosition.LeftThumb:
                            lb = lbs[8];
                            break;
                        case NFPosition.RightThumb:
                            lb = lbs[9];
                            break;
                    }

                    if (subject.Fingers[i].Objects.First().Status == NBiometricStatus.Ok)
                    {
                        pct = subject.Fingers[i].Objects.First().Quality;
                        if (pct == 254)
                            pct = 0;

                        if (bestQuality < pct)
                        {
                            bestQuality = pct;
                            bestQualityImageIndex = subject.Fingers[i].Position;
                        }

                        this.Invoke((Action<int, Label>)((percent, lab) =>
                        {
                            lab.Text = string.Format("Q: {0:P0}", percent / 100.0);
                            if (pct > 79)
                                lab.ForeColor = Color.GreenYellow;
                            else if (pct > 39)
                                lab.ForeColor = Color.Orange;
                            else
                                lab.ForeColor = Color.Red;
                        }), pct, lb);
                    }
                    else
                    {
                        this.Invoke((Action<Label>)((lab) =>
                        {
                            lab.Text = string.Format("Q: {0:P0}", 0);
                            lab.ForeColor = Color.Red;
                        }), lb);
                    }
                }
                //var sw = System.Diagnostics.Stopwatch.StartNew();
                //var sw = new System.Diagnostics.Stopwatch();
                //sw.Start();
                //_biometricClient.CreateTemplate(subject);
                //sw.Stop();
                //TimeSpan ts = sw.Elapsed;
                //string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                //Console.WriteLine("RunTime " + elapsedTime);
            }
            catch (Exception)
            {
                this.Invoke((Action<List<Label>>)((list) =>
                {
                    foreach (Label lb in list)
                    {
                        lb.Text = string.Format("Q: {0:P0}", 0);
                        lb.ForeColor = Color.Red;
                    }
                }), lbs);

                return 0;
            }

            return bestQualityImageIndex;
        }

        private int GetImageQuality(NSubject subject, Label lb)
        {
            int pct = 0;
            NBiometricStatus status = NBiometricStatus.None;

            try
            {
                //var sw = System.Diagnostics.Stopwatch.StartNew();
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                status = _biometricClient.CreateTemplate(subject);
                sw.Stop();
                TimeSpan ts = sw.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine("RunTime " + elapsedTime);
                //sw.Start();

                if (status != NBiometricStatus.Ok)
                {
                    this.Invoke((Action<Label>)((lab) =>
                    {
                        lab.Text = string.Format("Q: {0:P0}", 0);
                        lab.ForeColor = Color.Red;
                    }), lb);

                    return 0;
                }
            }
            catch (Exception)
            {
                this.Invoke((Action<Label>)((lab) =>
                {
                    lab.Text = string.Format("Q: {0:P0}", 0);
                    lab.ForeColor = Color.Red;
                }), lb);

                return 0;
            }
            finally
            {
                if (status == NBiometricStatus.Ok)
                {
                    pct = subject.Fingers[0].Objects.First().Quality;

                    this.Invoke((Action<int, Label>)((percent, lab) =>
                    {
                        lab.Text = string.Format("Q: {0:P0}", percent / 100.0);
                        if (pct > 80)
                            lab.ForeColor = Color.GreenYellow;
                        else if (pct > 50)
                            lab.ForeColor = Color.Orange;
                        else
                            lab.ForeColor = Color.Red;
                    }), pct, lb);
                }
            }

            return pct;
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

                var fScanner = ((NFingerScanner)GetSelectedDevice());
                if (fScanner == null)
                    return;

                bool CancelEvent = false; bool cancel = false;
                foreach (NFPosition position in fScanner.GetSupportedPositions())
                {
                    CancelEvent = false;
                    switch (position) {
                        case NFPosition.PlainLeftFourFingers:
                        case NFPosition.UnknownFourFingers:
                            if (count > 4)
                                CancelEvent = true;
                            cancel = true;
                            break;
                        //case NFPosition.UnknownThreeFingers:
                        //    if (count > 3)
                        //        CancelEvent = true;
                        //    break;
                        case NFPosition.UnknownTwoFingers:
                            if (count > 2)
                                CancelEvent = true;
                            break;
                    }

                    if (cancel)
                        break;
                    //if (CancelEvent)
                    //    (sender as CheckBox).Checked = false;
                }

                if (CancelEvent)
                    (sender as CheckBox).Checked = false;

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
            enrollFromWSQ(wsqImage);
        }

        public void ShowStatusMessage(string message)
        {
            toolStripStatusLabelError.ForeColor = Color.Black;
            toolStripStatusLabelError.Text = System.Text.RegularExpressions.Regex.Replace(message, @"\r\n?|\n", "");
            //toolStripProgressBar.Visible = false;
            //this.Controls.SetChildIndex(statusStrip1, 0);

            Application.DoEvents();
        }

        void ShowErrorMessage(string message)
        {
            stopProgressBar();
            Application.DoEvents();

            toolStripStatusLabelError.ForeColor = Color.Red;
            toolStripStatusLabelError.Text = System.Text.RegularExpressions.Regex.Replace(message, @"\r\n?|\n", "");
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
            _biometricClient.FingerScanner = scannersListBox.SelectedItem as NFScanner;
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
            if (e.Button == MouseButtons.Left)
            {
                if (fingerView2.ShownImage == ShownImage.Original)
                    fingerView2.ShownImage = ShownImage.Result;
                else
                    fingerView2.ShownImage = ShownImage.Original;
            }
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
                        if (backgroundWorkerCachingService.IsBusy)
                        {
                            backgroundWorkerCachingService.CancelAsync();
                            //_mre.Set();                      //Terminate CashingService
                            //TerminateCaching(_serviceClient);

                            try
                            {
                                TerminateCaching(_serviceClient);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }

                        //_serviceClient.Terminate();
                    } else if (_serviceClient is MemoryCacheMatchingService.MatchingServiceClient)
                    {
                        if (backgroundWorkerMatchingService.IsBusy)
                        {
                            //backgroundWorkerMatchingService.CancelAsync();
                            //_mre.Set();                      //Terminate MatchingServiceService

                            try
                            {
                                TerminateMatching(_serviceClient);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                    }

                }

                if (backgroundWorkerDataService.IsBusy)
                {
                    backgroundWorkerDataService.CancelAsync();
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
                stopCapturing();
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

            if (backgroundWorkerCachingService.IsBusy)
            {
                backgroundWorkerCachingService.CancelAsync();
                while (backgroundWorkerCachingService.IsBusy)
                {
                    Thread.Sleep(0);
                    Application.DoEvents();
                }
            }

            if (backgroundWorkerMatchingService.IsBusy)
            {
                backgroundWorkerMatchingService.CancelAsync();
                while (backgroundWorkerMatchingService.IsBusy)
                {
                    Thread.Sleep(0);
                    Application.DoEvents();
                }
            }

            if (backgroundWorkerDataService.IsBusy)
            {
                backgroundWorkerDataService.CancelAsync();
                while (backgroundWorkerDataService.IsBusy)
                {
                    Thread.Sleep(0);
                    Application.DoEvents();
                }
            }

            _deviceManager.Devices.CollectionChanged -= deviceManager_CollectionChanged;

            //NDevice device = GetSelectedDevice();

            //if (device != null && device.IsAvailable)
            //{
            //_deviceManager.DisconnectFromDevice(_device);
            //Neurotec.Gui.NGui.InvokeAsync(((NBiometricDevice)device).Cancel);
            new Action(delegate ()
                //Neurotec.Gui.NGui.InvokeAsync(delegate ()
                {
                    NDevice device = GetSelectedDevice();

                    if (device != null && device.IsAvailable)
                    {
                        ((NBiometricDevice)device).Cancel();
                        //try
                        //{
                        //    _deviceManager.ConnectToDeviceDisconnectFromDevice(device);
                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show(ex.ToString(), @"Disconnect from Device Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                        //_deviceManager.DisconnectFromDevice(device);
                    }

                    if (_biometricClient != null)
                        _biometricClient.Cancel();
                }).Invoke();

                //    ((NBiometricDevice)device).Cancel);
                //_deviceManager.DisconnectFromDevice(device);
            //}

            //if (_biometricClient != null)
            //    _biometricClient.Cancel();
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
                    if (_serviceClient is MemoryCachePopulateService.PopulateCacheServiceClient)
                    {
                        if (backgroundWorkerCachingService.IsBusy)
                        {
                            backgroundWorkerCachingService.CancelAsync();
                        }

                        //_serviceClient.Terminate();
                    }
                    else if (_serviceClient is MemoryCacheMatchingService.MatchingServiceClient)
                    {
                        if (backgroundWorkerMatchingService.IsBusy)
                        {
                            backgroundWorkerMatchingService.CancelAsync();
                        }
                    }
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
            if (!IsCachingServiceRunning)
            {
                int count = 0;
                CheckBox cb;
                for (int i = 1; i < 11; i++)
                {
                    cb = this.Controls.Find("checkBoxCache" + i.ToString(), true)[0] as CheckBox;
                    if (cb.Checked)
                        if (++count > 1)
                            break;
                }

                if (count < 2)
                {
                    ShowErrorMessage("At least two fingers should be selected");
                    return;
                }

                labelCacheValidationTime.Text = "";

                startCachingServiceProcess();
            }
            else
            {
                //backgroundWorkerCachingService.CancelAsync();

                terminateService();

                //CallbackFromCacheFillingService callback = new CallbackFromCacheFillingService();
                //InstanceContext context = new InstanceContext(callback);
                ////InstanceContext context = null;

                //if (_serviceClient != null)
                //{
                //    _serviceClient.Terminate();
                //}
                //else if (ConfigurationManager.AppSettings["cachingProvider"] == "ODBCCache")
                //{

                //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
                //    {
                //        terminateMatchingService();
                //    }
                //    else
                //    {
                //        //context = new InstanceContext(callback);
                //        dynamic client = new UnmanagedMatchingService.MatchingServiceClient(context);
                //        client.terminateMatchingService();
                //    }

                //}

                //dynamic client = null;
                //if (ConfigurationManager.AppSettings["cachingProvider"] == "MemoryCache")
                //    client = new MemoryCachePopulateService.PopulateCacheServiceClient(context);
                //else if (ConfigurationManager.AppSettings["cachingProvider"] == "AppFabricCache")
                //    client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);


                //if (client != null)
                //    client.Terminate();
                //else
                //{
                //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
                //    {
                //        terminateMatchingService();
                //    }
                //    else
                //    {
                //        //context = new InstanceContext(callback);
                //        client = new UnmanagedMatchingService.MatchingServiceClient(context);
                //        client.terminateMatchingService();
                //    }
                //}

                //if (ConfigurationManager.AppSettings["cachingProvider"] != "ODBCCache")
                //{
                //    //context = new InstanceContext(callback);
                //    var client = new AppFabricCachePopulateService.PopulateCacheServiceClient(context);
                //    client.Terminate();
                //}
                //else
                //{
                //    if (ConfigurationManager.AppSettings["cachingService"] == "local")
                //    {
                //        terminateMatchingService();
                //    }
                //    else
                //    {
                //        //context = new InstanceContext(callback);
                //        var client = new UnmanagedMatchingService.MatchingServiceClient(context);
                //        client.terminateMatchingService();
                //    }
                //}

                manageCacheButton.Enabled = false;
                Application.DoEvents();
                //ShowStatusMessage("Terminating the request...");
            }

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
            UpdateScannerList(true);
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
