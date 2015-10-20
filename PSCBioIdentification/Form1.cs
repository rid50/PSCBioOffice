using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Neurotec.Biometrics;
using Neurotec.Images;
using Neurotec.DeviceManager;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Resources;

using PSCBioIdentification.Properties;
using System.Runtime.InteropServices;

namespace PSCBioIdentification
{
    public partial class Form1 : Form
    {

        //[DllImport("Lookup.dll", CharSet = CharSet.Auto)]
        //public static extern bool initFingerMatcher();

        [DllImport("Lookup.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern UInt32 match(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)]
            string[] arrOfFingers, int arrOffingersSize,
            byte[] template, UInt32 size, System.Text.StringBuilder errorMessage, int messageSize);
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
        private NFRecord template;
        private NFRecord enrolledTemplate;
        private FPScannerMan scannerMan;
        private string selectedScannerModules = string.Empty;
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

        public NFRecord Template
        {
            get
            {
                return template;
            }
        }

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
            backgroundWorkerProgressBar = new BackgroundWorker();
            backgroundWorkerProgressBar.WorkerSupportsCancellation = true;
            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            backgroundWorkerProgressBar.DoWork += new DoWorkEventHandler(backgroundWorkerProgressBar_DoWork);
            backgroundWorkerProgressBar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProgressBar_RunWorkerCompleted);
            backgroundWorkerProgressBar.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgressBar_ProgressChanged);

            backgroundWorkerDataService = new BackgroundWorker();
            backgroundWorkerDataService.DoWork += new DoWorkEventHandler(backgroundWorkerDataService_DoWork);
            backgroundWorkerDataService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerDataService_RunWorkerCompleted);

            backgroundWorkerMatchingService = new BackgroundWorker();
            backgroundWorkerMatchingService.DoWork += new DoWorkEventHandler(backgroundWorkerMatchingService_DoWork);
            backgroundWorkerMatchingService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerMatchingService_RunWorkerCompleted);

            Data.NFExtractor = new NFExtractor();
            Data.UpdateNfe();
            Data.UpdateNfeSettings();

            Data.NMatcher = new NMatcher();
            Data.UpdateNM();
            Data.UpdateNMSettings();

            //Data.Database = new Database();

            ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
            string selectedScannerModules = "Futronic";
            scannerMan = new FPScannerMan(selectedScannerModules, this);
            FPScanner scanner;
            if (scannerMan.Scanners.Count != 0)
            {
                scanner = scannerMan.Scanners[0];
                scanner.FingerPlaced += new EventHandler(scanner_FingerPlaced);
                scanner.FingerRemoved += new EventHandler(scanner_FingerRemoved);
                scanner.ImageScanned += new FPScannerImageScannedEventHandler(scanner_ImageScanned);
            }
            else
            {
                //ResourceManager rm = new ResourceManager("rmc", System.Reflection.Assembly.GetExecutingAssembly());
                string text = rm.GetString("msgNoScannersAttached"); // "No scanners attached"
                //string text = Resources.ResourceManager.GetString("msgNoScannersAttached"); // "No scanners attached"
                LogLine(text, true);
                ShowErrorMessage(text);
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

            //ProgramMode mode = (ProgramMode)Settings.Default.ProgramMode;
            //ProgramMode mode = ProgramMode.Identify;
            radioButtonIdentify.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;

            if (radioButtonIdentify.Checked)
            {
                this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
            }
            else
            {
                pictureBoxGreen.Active = false;
                pictureBoxGreen.Invalidate();
                pictureBoxRed.Active = true;
                pictureBoxRed.Invalidate();
            }

            //setMode(mode);
            //setModeRadioButtons(mode);

            personId.Focus();

            //personId.Text = "123";
            personId.Text = "20095423";
            //buttonRequest.Focus();
            //buttonScan.Enabled = false;
            //if (!initFingerMatcher())
            //    ShowErrorMessage("Lookup service: Error connecting to database");
        }

        private void startCapturing()
        {
            FPScanner scanner;
            if (scannerMan.Scanners.Count != 0)
                scanner = scannerMan.Scanners[0];
            else
            {
                ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                string text = rm.GetString("msgNoScannersAttached"); // "No scanners attached"
                ShowErrorMessage(text);
                LogLine(text, true);
                return;
            }

            try
            {
                if (!scanner.IsCapturing)
                {
                    scanner.StartCapturing();
                    WaitingForImageToScan();
                    pictureBoxGreen.Active = true;
                    pictureBoxGreen.Invalidate();
                    pictureBoxRed.Active = false;
                    pictureBoxRed.Invalidate();
                }
            }
            catch (Exception ex)
            {
                string text = string.Format("Error starting capturing on scanner {0}: {1}", scanner.Id, ex.Message);
                ShowErrorMessage(text);
                //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void stopCapturing()
        {
            FPScanner scanner;
            if (scannerMan.Scanners.Count != 0)
                scanner = scannerMan.Scanners[0];
            else
                return;

            try
            {
                if (scanner.IsCapturing)
                {
                    scanner.StopCapturing();
                    pictureBoxGreen.Active = false;
                    pictureBoxGreen.Invalidate();
                    pictureBoxRed.Active = true;
                    pictureBoxRed.Invalidate();
                }
            }
            catch (Exception ex)
            {
                string text = string.Format("Error stoppping capturing on scanner {0}: {1}", scanner.Id, ex.Message);
                ShowErrorMessage(text);
                //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /*
                private void buttonScan_Click(object sender, EventArgs e)
                {
                    _fingersCollection.Clear();

                    if ((bool)buttonScan.Tag)
                    //if (!(e is MyEventArgs))
                    {
                        //                if (!openFile())
                        //                    return;

                        ProgramMode mode = ProgramMode.Verify;
                        setMode(mode);
                        setRadioButtons(mode);
                        buttonRequest.Enabled = false;
                    }
                    else
                    {
                        ProgramMode mode = ProgramMode.Enroll;
                        setMode(mode);
                        setRadioButtons(mode);
                        buttonRequest.Enabled = true;
                    }

                    FPScanner scanner = scannerMan.Scanners[0];
                    try
                    {
                        if ((bool)buttonScan.Tag)
                        {
                            scanner.StartCapturingForOneImage();
                            //LogLine("Waiting for image...", true);
                        }
                        else
                        {
                            if (scanner.IsCapturing)
                                scanner.StopCapturing();

                            //System.Threading.Thread.Sleep(5000);

                            //                    dgvFields.Rows.Clear();
                            ///                  ClearLog();
                            Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        string text = string.Format("Error starting capturing on scanner {0}: {1}", scanner.Id, ex.Message);
                        MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if ((bool)buttonScan.Tag)
                        buttonScan.Text = "Stop scan";
                    else
                        buttonScan.Text = "Start scan";

                    buttonScan.Tag = !(bool)buttonScan.Tag;


                }
        */
        //Cursor _previousCursor;
        private void scanner_FingerPlaced(object sender, EventArgs e)
        {
            //startProgressBar();     // START
            //_previousCursor = Cursor.Current;
            //Cursor.Current = Cursors.WaitCursor;
        }

        private void scanner_FingerRemoved(object sender, EventArgs e)
        {
            //stopProgressBar();                     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //Cursor.Current = _previousCursor;
        }

        private void scanner_ImageScanned(object sender, FPScannerImageScannedEventArgs ea)
        {
            //FPScanner scanner = (FPScanner)sender;

            //if (!isUserIdValid())
            //  return;

            //            OnImage((NGrayscaleImage)ea.Image.Clone(), string.Format("Image scanned from scanner {0}", scanner.Id), false);
            OnImage((NGrayscaleImage)ea.Image.Clone());

            //setMode(ProgramMode.Enroll);
            //setRadioButtons(ProgramMode.Enroll);

            //this.InvokeOnClick(buttonScan, new MyEventArgs());

            //System.Threading.Thread.Sleep(2000);

            //            Application.DoEvents
            //scanner.StopCapturing();
            //dgvFields.Rows.Clear();
            //ClearLog();
            //Clear();

            /*
                        if ((bool)buttonScan.Tag)
                            buttonScan.Text = "Stop scan";
                        else
                            buttonScan.Text = "Start scan";

                        buttonScan.Tag = !(bool)buttonScan.Tag;
            */
        }

        private bool enrollFromWSQ(WsqImage wsqImage)
        {
            //string fileName = "lindex.wsq";

            if (!isUserIdValid())
                return false;
            /*
                        DBHelper.DBUtil db = new DBHelper.DBUtil();
                        byte[] serializedArrayOfWSQ = db.GetImage(this.userId, true);

                        MemoryStream ms = new MemoryStream(serializedArrayOfWSQ);

                        //Assembly.Load(string assemblyString)
                        // Construct a BinaryFormatter and use it to deserialize the data to the stream.
                        BinaryFormatter formatter = new BinaryFormatter();
                        ArrayList fingersCollection = null;
                        try
                        {
                            formatter.Binder = new GenericBinder<WsqImage>();
                            fingersCollection = formatter.Deserialize(ms) as ArrayList;
                        }
                        catch (SerializationException ex)
                        {
                            LogLine(ex.Message, true);
                            return false;
                        }
                        finally
                        {
                            ms.Close();
                        }

                        WsqImage im = fingersCollection[0] as WsqImage;
 
            */
            /*
                        im.Content = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_WSQ);
                        im.XRes = img.GetXRes();
                        im.YRes = img.GetYRes();
                        im.XSize = img.GetXSize();
                        im.YSize = img.GetYSize();
                        PixelFormat == 1 GRAY
                        PixelFormat == 6 RGB
            */

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

            MemoryStream ms = null;
            NImage nImage;
            try
            {
                //Bitmap bm = new Bitmap(ms);
                //nImage = NImage.FromBitmap(bm);

                //string fileName = @"c:\psc\wsq\lindex.wsq";
                //nImage = NImage.FromFile(fileName, NImageFormat.Wsq);

                ms = new MemoryStream(wsqImage.Content);
                nImage = NImageFormat.Wsq.LoadImage(ms);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("Error creating image retrieved from database {0}", ex.Message),
                //  Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                string text = string.Format("Error creating image retrieved from database {0}", ex.Message);
                ShowErrorMessage(text);

                return false;
            }
            finally
            {
                if (ms != null)
                    ms.Dispose();
            }

            float horzResolution = nImage.HorzResolution;
            float vertResolution = nImage.VertResolution;
            if (horzResolution < 250) horzResolution = 500;
            if (vertResolution < 250) vertResolution = 500;

            NGrayscaleImage grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, horzResolution, vertResolution, nImage);
            OnImage(grayImage);

            return true;
        }
        /*
                class MyNImageFormat : NImageFormat
                {
                    MyNImageFormat() {}


                }
        */
        private void clearView()
        {
            if (mode == ProgramMode.PreEnrolled || mode == ProgramMode.Identification)
                nfView1.Image = null;

            //nfView1.ResultImage = null;
            //nfView1.Template = null;
            //nfView1.Tree = null;
            nfView2.Image = null;
            nfView2.ResultImage = null;
            nfView2.Template = null;
            //nfView2.Tree = null;
            //image = null;
            //resultImage = null;
            //dgvFields.Rows.Clear();

            pictureBox1.Image = null;
            pictureBox2.Image = null;

        }

        private void clearFingerBoxes()
        {
            PictureBox pb;
            for (int i = 0; i <= 9; i++)
            {
                this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0].Text = "";
                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as PictureBox;
                //pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                pb.Image = null;
            }
        }

        private void clear()
        {
            ShowStatusMessage("");
            clearView();
            clearLog();

            /*
                        Settings settings = Settings.Default;
                        count = enrollMode && settings.FPUseGeneralization ? settings.FPGeneralizationTemplates : 1;
                        foreach (NGrayscaleImage image in images)
                        {
                            if (image != null) image.Dispose();
                        }
                        foreach (NGrayscaleImage resultImage in resultImages)
                        {
                            if (resultImage != null) resultImage.Dispose();
                        }
                        foreach (Bitmap bitmap in bitmaps)
                        {
                            if (bitmap != null) bitmap.Dispose();
                        }
                        foreach (Bitmap resultBitmap in resultBitmaps)
                        {
                            if (resultBitmap != null) resultBitmap.Dispose();
                        }
                        images = new NGrayscaleImage[count];
                        resultImages = new NGrayscaleImage[count];
                        bitmaps = new Bitmap[count];
                        resultBitmaps = new Bitmap[count];
                        templates = new NFRecord[count];
                        availableCount = 0;
            */
            this.template = null;
            this.enrolledTemplate = null;
            if (_fingersCollection != null)
                _fingersCollection.Clear();
            //LogWait();
        }
        /*
                private void OnEnrollModeChanged()
                {
                    //Clear();
                }
        */
        //        private void OnImage(NGrayscaleImage image, string logText, bool fromFile)
        private void OnImage(NGrayscaleImage image)
        {
            //            if (availableCount == count)
            //          {
            //            Clear();
            //      }
            //    else
            //  {
            clearView();
            //}
            //LogLine(logText, true);

            //if (!fromFile)
            //  LogLine(logText, false);

            //            LogLine("Size: {0}x{1}. Resolution: {2}x{3} dpi", true, image.Width, image.Height, image.HorzResolution, image.VertResolution);
            //if (images[availableCount] != null) images[availableCount].Dispose();
            //images[availableCount] = image;
            //this.image = image;

            //if (bitmaps[availableCount] != null) bitmaps[availableCount].Dispose();
            //bitmaps[availableCount] = image.ToBitmap();
            //nfView1.Image = bitmaps[availableCount];
            //nfView2.Image = bitmaps[availableCount];
            if (Mode == ProgramMode.PreEnrolled)
                nfView1.Image = image.ToBitmap();
            //nfView2.Image = image.ToBitmap();

            //startProgressBar(); //START

            //UseWaitCursor = true;
            NGrayscaleImage resultImage = (NGrayscaleImage)image.Clone();
            //Stopwatch sw = Stopwatch.StartNew();
            try
            {
                NfeExtractionStatus extractionStatus;
                template = Data.NFExtractor.Extract(resultImage, NFPosition.Unknown, NFImpressionType.LiveScanPlain, out extractionStatus);
                if (extractionStatus != NfeExtractionStatus.TemplateCreated)
                {
                    string text = string.Format("Extraction failed: {0}", extractionStatus.ToString());
                    ShowErrorMessage(text);

                    LogLine(text, true);
                    //LogLine("Waiting for image...", true);

                    pictureBox2.Image = Properties.Resources.redcross;

                    //      MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    sw.Stop();
                    //stopProgressBar();
                    //UseWaitCursor = false;

                    WaitingForImageToScan();

                    return;
                }
            }
            catch (Exception e)
            {
                string text = string.Format("Extraction error: {0}", e.Message);
                ShowErrorMessage(text);

                LogLine(text, true);

                //LogLine("Waiting for image...", true);

                pictureBox2.Image = Properties.Resources.redcross;

                //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //sw.Stop();
                //stopProgressBar();
                //UseWaitCursor = false;

                WaitingForImageToScan();

                return;
            }
            finally
            {
                //WaitingForImageToScan();
                //stopProgressBar();                !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }

            //sw.Stop();
            //stopProgressBar();
            //UseWaitCursor = false;
            //if (resultImages[availableCount] != null) resultImages[availableCount].Dispose();
            //resultImages[availableCount] = resultImage;
            //this.resultImage = resultImage;
            Bitmap bitmap;
            using (NImage ri = NImages.GetGrayscaleColorWrapper(resultImage, resultImageMinColor, resultImageMaxColor))
            {
                //if (resultBitmaps[availableCount] != null) resultBitmaps[availableCount].Dispose();
                //resultBitmaps[availableCount] = ri.ToBitmap();
                bitmap = ri.ToBitmap();
            }
            //templates[availableCount] = (NFRecord)template.Clone();
            this.template = (NFRecord)template.Clone();
            //nfView2.ResultImage = resultBitmaps[availableCount];
            nfView2.ResultImage = bitmap;
            if (nfView2.Template != null) nfView2.Template.Dispose();
            nfView2.Template = this.template;

            if (template == null)
            {
                ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                string text = rm.GetString("msgFingerprintImageIsOfLowQuality"); // "Fingerprint image is of low quality"
                ShowErrorMessage(text);
                LogLine(text, true);

                pictureBox2.Image = Properties.Resources.redcross;

                //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                WaitingForImageToScan();

                return;
            }
            //Settings settings = Settings.Default;
            //            LogLine("Template extracted{0}. G: {1}. Size: {2}. Time: {3}", true,
            //              Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Utils.QualityToPercent(template.Quality) / 100.0) : null,
            //           template.G, Data.SizeToString(template.Save().Length), Data.TimeToString(sw.Elapsed));
            
            LogLine("Template extracted{0}. G: {1}. Size: {2}", true,
                Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Helpers.QualityToPercent(template.Quality) / 100.0) : null,
                template.G, Data.SizeToString(template.Save().Length));

            //ShowStatusMessage(String.Format("Template extracted{0}. G: {1}. Size: {2}", true,
            //    Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Helpers.QualityToPercent(template.Quality) / 100.0) : null,
            //    template.G, Data.SizeToString(template.Save().Length)));

            //bool retcode = true;
            switch (Mode)
            {
                case ProgramMode.PreEnrolled:
                    doPreEnroll();
                    nfView2.Zoom = 1F;
                    //WaitingForImageToScan();
                    break;
                case ProgramMode.Verification:
                    this.BeginInvoke(new MethodInvoker(delegate() { doVerify(); }));
                    //doVerify();
                    nfView2.Zoom = 0.5F;
                    break;
                case ProgramMode.Identification:
                    this.BeginInvoke(new MethodInvoker(delegate() { doIdentify(template); }));
                    //doIdentify(template);
                    nfView2.Zoom = 0.5F;
                    break;
            }

            //if (retcode)
                //WaitingForImageToScan();

            //ResourceManager rm2 = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
            //string text2 = rm2.GetString("msgWaitingForImage"); // "Waiting for image..."

            //LogLine(text2, true);
            //ShowStatusMessage(text2);
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
            this.enrolledTemplate = this.template;

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
            //stopCapturing();

            this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            //Record record = Data.Database.Records[id.ToString()];
            /*
                        if (record == null)
                        {
                            MessageBox.Show(string.Format("No template is enrolled with id \"{0}\" into the database", id),
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                            return;
                        }

                        Stopwatch sw = Stopwatch.StartNew();
            */
            int score;

            NMMatchDetails matchDetails;
            try
            {
                //score = Data.NMatcher.Verify(Template.Save(), record.Template, out matchDetails);
                score = Data.NMatcher.Verify(Template.Save(), this.enrolledTemplate.Save(), out matchDetails);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("Error verifying templates: {0}", ex.Message),
                //  Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

                string text = string.Format("Error verifying templates: {0}", ex.Message);
                ShowErrorMessage(text);

                LogLine(string.Format("Error verifying templates: {0}", ex.Message), true);

                pictureBox2.Image = Properties.Resources.redcross;

                return;
            }

            //sw.Stop();

            //            string str = string.Format("Verification {0}. Time: {1}",
            //              score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score), Data.TimeToString(sw.Elapsed));
            string str = string.Format("Verification {0}", score == 0 ? "failed" : string.Format("succeeded. Score: {0}", score));

            LogLine(str, true);

            ShowStatusMessage(str);

            if (score > 0)
                pictureBox2.Image = Properties.Resources.checkmark;
            else
                pictureBox2.Image = Properties.Resources.redcross;

            if (score > 0)
            {
                startProgressBar();
                startDataServiceProcess();  // to get a picture
            }

            Thread.Sleep(1000);
            this.BeginInvoke(new MethodInvoker(delegate() { startCapturing(); }));
        }

        //class Record
        //{
        //    public UInt32 size;
        //    public byte[] template;
        //    //public String errorMessage;
        //    //public String[] errorMessage = new String[1];
        //    public System.Text.StringBuilder errorMessage;

        //}

        private void doIdentify(NFRecord template)
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
            this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            startProgressBar();
            startMatchingServiceProcess(template);

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
        /*
                private void LogWait()
                {
                    //if (count == 1)
                    //{
                    lblWaitingForImg.Text = "Waiting for image...";
                    LogLine(lblWaitingForImg.Text, true);
                    //}
                    //else
                    //{
                    //  lblWaitingForImg.Text = string.Format("Waiting for image {0} of {1}", availableCount + 1, count);
                    //LogLine(lblWaitingForImg.Text, true);
                    //}
                }
        */

        //private void setStatus(string status)
        //{
        //  Text = appName + status;
        //}

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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //            setStatus("Closing");

            if (scannerMan != null)
            {
                foreach (FPScanner scanner in scannerMan.Scanners)
                {
                    if (scanner.IsCapturing)
                        scanner.StopCapturing();
                }
                scannerMan.Dispose();
            }

            Data.NFExtractor.Dispose();
            Data.NMatcher.Dispose();
            //Data.Database.Dispose();

            Settings settings = Settings.Default;
            settings.NFRecordFromSize = WindowState != FormWindowState.Normal ? RestoreBounds.Size : Size;
            settings.NFRecordFormMaximized = WindowState == FormWindowState.Maximized;
            Settings.Default.Save();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (Settings.Default.NFRecordFormMaximized) WindowState = FormWindowState.Maximized;
        }

        private void radioButtonGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is ButtonBase)
            {
                //radioButton10.
                ButtonBase radiobutton = sender as ButtonBase;
                switch (radiobutton.Text)
                {
                    case "Enroll":
                        mode = ProgramMode.PreEnrolled;
                        break;
                    case "Verify":
                        mode = ProgramMode.Verification;
                        lblPersonId.Show();
                        personId.Show();
                        buttonRequest.Show();
                        ShowRadioHideCheckButtons(true);
                        break;
                    case "Identify":
                        mode = ProgramMode.Identification;
                        lblPersonId.Hide();
                        personId.Hide();
                        buttonRequest.Hide();
                        ShowRadioHideCheckButtons(false);
                        break;
                }

                //setStatus(mode.ToString());
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

            startProgressBar();
            startDataServiceProcess();
        }

        //private void buttonRequest_Click(object sender, EventArgs e)
        private void processEnrolledData(byte[] serializedArrayOfWSQ)
        {

            //byte[] serializedArrayOfWSQ = null;

/*
            this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            ProgramMode mode = ProgramMode.Enroll;
            setMode(mode);
            setModeRadioButtons(mode);

            Clear();

            if (!isUserIdValid())
                return;

            startProgressBar();
            startDbProcess();

            Application.DoEvents();

            DBHelper.DBUtil db = new DBHelper.DBUtil();
            byte[] serializedArrayOfWSQ = null;

            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
                    serializedArrayOfWSQ = db.GetImageFromWebService(IMAGE_TYPE.wsq, this.userId);
                else
                    serializedArrayOfWSQ = db.GetImage(IMAGE_TYPE.wsq, this.userId);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
                return;
            }

*/
            PictureBox pb;

            ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
            if (serializedArrayOfWSQ == null)
            {
                clearFingerBoxes();
/*
                for (int i = 0; i <= 9; i++)
                {
                    this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0].Text = "";
                    pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as PictureBox;
                    //pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                    pb.Image = null;
                }
*/
                string text = rm.GetString("msgThePersonHasNotYetBeenEnrolled"); // "The person has not yet been enrolled"
                
                LogLine(text, true);
                ShowErrorMessage(text);
                //stopProgressBar();
                return;
            }

            MemoryStream ms = new MemoryStream(serializedArrayOfWSQ);

            //Assembly.Load(string assemblyString)
            // Construct a BinaryFormatter and use it to deserialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Binder = new GenericBinder<WsqImage>();
                _fingersCollection = formatter.Deserialize(ms) as ArrayList;
            }
            catch (SerializationException ex)
            {
                LogLine(ex.Message, true);
                ShowErrorMessage(ex.Message);
                //stopProgressBar();
                return;
            }
            finally
            {
                ms.Close();
            }

            //byte[] buff = null;
            //System.Object theLock = new System.Object();

            int bestQuality = 0;
            int bestQualityRadioButton = 0;
            RadioButton rb = null; Label lab = null; WsqImage wsqImage = null;
            //bool rbChecked = false;
                //, pbChecked = false;
            for (int i = 0; i < _fingersCollection.Count; i++)
            {
                Control[] control = this.Controls.Find("radioButton" + (i + 1).ToString(), true);
                Control[] controlLab = this.Controls.Find("label" + (i + 1).ToString(), true);
                if (control.Length == 0)
                    continue;

                rb = control[0] as RadioButton;
                lab = controlLab[0] as Label;

                wsqImage = _fingersCollection[i] as WsqImage;
                if (wsqImage == null)
                {
                    rb.Enabled = false;
                    lab.Enabled = false;
                }
                else
                {
/*
                    if (!rbChecked)
                    {
                        //_wsqImage = im;
                        //this.BeginInvoke(new MethodInvoker(delegate() { checkRadioButton(rb.Name); }), new String[] { rb.Name });
                        this.BeginInvoke(new MethodInvoker(delegate() { checkRadioButton(rb.Name); }));
                        //rb.Checked = true;

                        rbChecked = true;
                    }
*/
                    rb.Enabled = true;
                    lab.Enabled = true;
                }

                pb = this.Controls.Find("fpPictureBox" + (i + 1).ToString(), true)[0] as PictureBox;
                //pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                //lab = this.Controls.Find("imageLabel" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as Label;
                //lab = this.Controls.Find("imageLabel3", true)[0] as Label;
                
                if (_fingersCollection[i] != null)
                {
                    //WsqImage wsq = _fingersCollection[i] as WsqImage;

                    //MemoryStream ms = null;
                    NImage nImage;
                    try
                    {
                        //Bitmap bm = new Bitmap(ms);
                        //nImage = NImage.FromBitmap(bm);

                        //string fileName = @"c:\psc\wsq\lindex.wsq";
                        //nImage = NImage.FromFile(fileName, NImageFormat.Wsq);

                        ms = new MemoryStream(wsqImage.Content);
                        nImage = NImageFormat.Wsq.LoadImage(ms);

                        float horzResolution = nImage.HorzResolution;
                        float vertResolution = nImage.VertResolution;
                        if (horzResolution < 250) horzResolution = 500;
                        if (vertResolution < 250) vertResolution = 500;

                        NGrayscaleImage grayImage = (NGrayscaleImage)NImage.FromImage(NPixelFormat.Grayscale, 0, horzResolution, vertResolution, nImage);
                        int q = GetImageQuality(grayImage, this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0] as Label);

                        if (bestQuality < q)
                        {
                            bestQuality = q;
                            bestQualityRadioButton = i;
                        }

                        pb.Image = nImage.ToBitmap();
                        //image = NImage.FromBitmap(bm);
                        pb.SizeMode = PictureBoxSizeMode.Zoom;

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
                    catch (Exception)
                    {
                        //MessageBox.Show(string.Format("Error creating image retrieved from database {0}", ex.Message),
                        //  Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //string text = string.Format("Error creating image retrieved from database {0}", ex.Message);
                        //ShowErrorMessage(text);

                        continue;
                    }
                    finally
                    {
                        ms.Close();
                    }

                    //if (!pb.Enabled)
                    //    pb.Enabled = true;

                    //lock (theLock)
                    //{
                    //    buff = ARHScanner.ConvertWSQToBmp(wsq);
                    //    ARHScanner.DisposeWSQImage();
                    //}
/*
                    if (buff != null)
                        if (buff.Length == 1)
                            continue;

                    if (buff == null)
                        pb.Image = null;
                    else
                    {
                        ms = new MemoryStream(buff);
                        pb.Image = Image.FromStream(ms);
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                    }
*/
                }
                else
                {
                    pb.Image = null;
                    this.Controls.Find("lbFinger" + (i + 1).ToString(), true)[0].Text = "";

                    //if (pb.Enabled)
                    //    pb.Enabled = false;
                }
            }

            //stopProgressBar();
            if (Mode == ProgramMode.PreEnrolled)
            {
                rb = this.Controls.Find("radioButton" + (bestQualityRadioButton + 1).ToString(), true)[0] as RadioButton;
                this.BeginInvoke(new MethodInvoker(delegate() { checkRadioButton(rb.Name); }));
            }

            //System.Threading.Thread.Sleep(5000);
            //stopProgressBar();                           //     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //buttonScan.Enabled = true;

            //pictureBox2.Image = Properties.Resources.redcross;
        }

        private int GetImageQuality(NGrayscaleImage image, Label lb)
        {
            NGrayscaleImage resultImage = (NGrayscaleImage)image.Clone();
            try
            {
                NfeExtractionStatus extractionStatus;
                template = Data.NFExtractor.Extract(resultImage, NFPosition.Unknown, NFImpressionType.LiveScanPlain, out extractionStatus);
                if (extractionStatus != NfeExtractionStatus.TemplateCreated)
                {
                    lb.Text = string.Format("Q: {0:P0}", 0);
                    lb.ForeColor = Color.Red;
                    return 0;
                }
            }
            catch (Exception)
            {
                lb.Text = string.Format("Q: {0:P0}", 0);
                lb.ForeColor = Color.Red;
                return 0;
            }

//            Bitmap bitmap;
//            using (NImage ri = NImages.GetGrayscaleColorWrapper(resultImage, resultImageMinColor, resultImageMaxColor))
//            {
//                bitmap = ri.ToBitmap();
//            }
            //templates[availableCount] = (NFRecord)template.Clone();
            this.template = (NFRecord)template.Clone();
/*
            //nfView2.ResultImage = resultBitmaps[availableCount];
            nfView2.ResultImage = bitmap;
            if (nfView2.Template != null) nfView2.Template.Dispose();
            nfView2.Template = this.template;

            if (template == null)
            {
                ResourceManager rm = new ResourceManager("PSCBioIdentification.Form1", this.GetType().Assembly);
                string text = rm.GetString("msgFingerprintImageIsOfLowQuality"); // "Fingerprint image is of low quality"
                ShowErrorMessage(text);
                LogLine(text, true);

                pictureBox2.Image = Properties.Resources.redcross;

                //MessageBox.Show(text, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
*/
            int i = 0;
            if (template != null)
            {
                i = Helpers.QualityToPercent(template.Quality);
                lb.Text = string.Format("Q: {0:P0}", i / 100.0);
                if (i > 80)
                    lb.ForeColor = Color.GreenYellow;
                else if (i > 50)
                    lb.ForeColor = Color.Orange;
                else
                    lb.ForeColor = Color.Red;
            } 
            else
            {
                lb.Text = string.Format("Q: {0:P0}", 0);
                lb.ForeColor = Color.Red;
            }

            return i;

            //LogLine("Template extracted{0}. G: {1}. Size: {2}", true,
            //    Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Utils.QualityToPercent(template.Quality) / 100.0) : null,
            //    template.G, Data.SizeToString(template.Save().Length));

//            ShowStatusMessage(String.Format("Template extracted{0}. G: {1}. Size: {2}", true,
//                Data.NFExtractor.UseQuality ? string.Format(". Quality: {0:P0}", Utils.QualityToPercent(template.Quality) / 100.0) : null,
//                template.G, Data.SizeToString(template.Save().Length)));
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

        private void fingerChanged(int fingerNumber)
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
                else
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
            if (_fingersCollection == null || _fingersCollection.Count == 0)
                return;

            this.BeginInvoke(new MethodInvoker(delegate() { stopCapturing(); }));

            Mode = ProgramMode.PreEnrolled;
            //setMode(mode);
            //setModeRadioButtons(mode);

            clearLog();

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
            WsqImage wsqImage = _fingersCollection[Int32.Parse(m.Value) - 1] as WsqImage;
            //WsqImage wsqImage = _fingersCollection[rbNumber - 1] as WsqImage;
            fingerChanged(Int32.Parse(m.Value) - 1);

            enrollFromWSQ(wsqImage);
        }

        void ShowStatusMessage(string message)
        {
            toolStripStatusLabelError.ForeColor = Color.Black;
            toolStripStatusLabelError.Text = message;
            Application.DoEvents();
        }

        void ShowErrorMessage(string message)
        {
            stopProgressBar();
            Application.DoEvents();

            toolStripStatusLabelError.ForeColor = Color.Red;
            toolStripStatusLabelError.Text = message;
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
