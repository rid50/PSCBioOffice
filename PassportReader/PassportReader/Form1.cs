using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Text;
using PassportReaderNS.GEPIRReference;
using System.Collections;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

//using DBHelper;
using System.Collections.Generic;
using System.Configuration;
using DBHelper;
using Neurotec.Images;
using Neurotec.Biometrics;
using System.Drawing.Drawing2D;
using WsqSerializationBinder;

namespace PassportReaderNS
{
    public partial class Form1 : Form
    {
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("User32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32")]
        private static extern bool HideCaret(IntPtr hWnd);

        private BackgroundWorker backgroundWorkerProgressBar;
        private BackgroundWorker backgroundWorkerDataService;
        private BackgroundWorker backgroundWorkerConnect;
        private BackgroundWorker backgroundWorkerListen;

        delegate int PrMethod();
        public delegate int GetImage(out byte[] buff);

        ARHScanner _sc;
        bool _noMove = false;
        bool _noOut = false;
        int _pictureBoxWidth = 0;
        string _gtin = "";
        string _barcodeType = "";

        ArrayList _fingersCollection = null;

//        byte _howManyTimemesToScan = 0;

//        byte _fingersQualityThreshold = 50;
        public int ErrorCode { get; set; }

        //private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    tabControl1.SelectedTab = tabPage3;
        //}

        public Form1()
        {
            InitializeComponent();
            //TextBoxID.Text = "210067490";
            //TextBoxID.Text = "20005140";
            TextBoxID.Text = "20000004";

            tabControl1.Controls.Remove(tabPage1);
            tabControl1.Controls.Remove(tabPage2);
            tabControl1.Controls.Remove(tabPage4);
            tabControl1.SelectedTab = tabPage3;
            //trackBar1.Value = 50;
        }

        void ShowError(ARHScanner sc)
        {
            toolStripStatusLabelError.ForeColor = Color.Red;

            if (ErrorCode != 1303 && ErrorCode != -1)
                toolStripStatusLabelError.Text = String.Format("{0} --- Error code: {1}", sc.ErrorMessage, ErrorCode);
            else
                toolStripStatusLabelError.Text = String.Format("{0}", sc.ErrorMessage);

            if (ErrorCode == 2 || ErrorCode == 4117 || ErrorCode == 19
                || ErrorCode == 16 || ErrorCode == 4100 || ErrorCode == -1)
            {
                button1.Enabled = true;
                button1.Focus();
            }

            stopProgressBar();
        }

        void ShowStatusMessage(string message)
        {
            toolStripStatusLabelError.ForeColor = Color.Black;
            toolStripStatusLabelError.Text = message;
        }

        private void backgroundWorkerConnect_Connect(object sender, DoWorkEventArgs e)
        {
            ARHScanner sc = e.Argument as ARHScanner;
            ErrorCode = sc.prConnect();

            e.Result = sc;
        }

        private void backgroundWorkerConnect_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowStatusMessage("Image processing...");

            ARHScanner sc = e.Result as ARHScanner;

            if (ErrorCode != 0)
                ShowError(sc);
            else
            {
                startListenProcess(sc);
            }
        }

        void startListenProcess(ARHScanner sc)
        {
            if (backgroundWorkerListen.IsBusy)
                return;

            backgroundWorkerListen.RunWorkerAsync(sc);
        }

        private void backgroundWorkerListen_Listen(object sender, DoWorkEventArgs e)
        {
            ARHScanner sc = e.Argument as ARHScanner;
            ErrorCode = sc.prListen();

            e.Result = sc;
        }

        private void backgroundWorkerListen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ARHScanner sc = e.Result as ARHScanner;

            if (sc != null && ErrorCode == 0)
            {
                //if (sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_OUT && !_noOut)
                if (sc.DeviceState == (int)Pr22.Util.PresenceState.Empty && !_noOut)
                {
                    _noOut = true;

                    if (toolStripStatusLabelError.Text == "Image processing...")
                        toolStripStatusLabelError.Text = "";

                    tabControl1.TabPages[0].Tag = "No barcode found";
                    tabControl1.TabPages[1].Tag = "No MRZ found";

                    button1.Enabled = true;
                    button1.Focus();
                }
                //else if (sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_NOMOVE && !_noMove)
                else if (sc.DeviceState == (int)Pr22.Util.PresenceState.NoMove && !_noMove)
                {
                    _noMove = true;
                    _noOut = false;

                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = false;

                    ShowStatusMessage("Image processing...");
                    startProgressBar();

                    textBox1.Text = "";
                    pictureBox1.Image = null;
                    pictureBox1.BorderStyle = BorderStyle.None;
                    textBox2.Text = "";
                    pictureBox2.Image = null;
                    pictureBox2.BorderStyle = BorderStyle.None;
                    textBox3.Text = "";

                    //System.Threading.Thread.Sleep(100);
                    Application.DoEvents();

                    PrMethod[] delegates = new PrMethod[] {
                                                    sc.prCaptureImage,
                                                    sc.prCaptureBarcode,
                                                    sc.prGetGTIN,
                                                    sc.prGetBarcodeData,
                                                    sc.prGetBarcodeImage,
                                                    sc.prSaveBarcodeImage,
                                                    sc.prReleaseDocument,
                                                    sc.prCaptureMRZ,
                                                    sc.prGetMRZData,
                                                    sc.prGetMRZImage,
                                                    sc.prSaveMRZImage,
                                                    sc.prReleaseDocument
                                                };

                    foreach (PrMethod prMethod in delegates)
                    {
                        if (prMethod.Method.Name == "prGetBarcodeData" ||
                            prMethod.Method.Name == "prGetMRZData")
                        {
                            IList list = new ArrayList();
                            if (prMethod.Method.Name == "prGetBarcodeData")
                                ErrorCode = sc.prGetBarcodeData(list);
                            else
                                ErrorCode = sc.prGetMRZData(list);

                            if (ErrorCode != 0)
                            {
                                break;
                            }

                            if (list.Count != 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int i = 0; i < list.Count; i++)
                                {
                                    sb.Append(list[i]);
                                    sb.Append(Environment.NewLine);
                                }
                                if (prMethod.Method.Name == "prGetBarcodeData")
                                    textBox1.Text = sb.ToString();
                                else
                                    textBox2.Text = sb.ToString();
                            }

                            if (prMethod.Method.Name == "prGetBarcodeData")
                                textBox1.Select(0, 0);
                            else
                                textBox2.Select(0, 0);
                        }
                        else if (prMethod.Method.Name == "prGetBarcodeImage" ||
                                prMethod.Method.Name == "prGetMRZImage")
                        {
                            GetImage method;
                            if (prMethod.Method.Name == "prGetBarcodeImage")
                            {
                                //IntPtr hDc = GetWindowDC(pictureBox1.Handle);
                                //sc.prGetBarcodeImage(hDc);
                                //ReleaseDC(pictureBox1.Handle, hDc);

                                method = new GetImage(sc.prGetBarcodeImage);
                                ErrorCode = ShowImage(method, pictureBox1);
                            }
                            else
                            {
                                method = new GetImage(sc.prGetMRZImage);
                                ErrorCode = ShowImage(method, pictureBox2);
                            }

                            if (ErrorCode != 0)
                            {
                                break;
                            }
                        }
                        else if (prMethod.Method.Name == "prGetGTIN")
                        {
                            if ((ErrorCode = sc.prGetGTIN(out _gtin, out _barcodeType)) != 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            ErrorCode = prMethod();

                            switch (prMethod.Method.Name)
                            {
                                case "prCaptureBarcode":
                                    if (ErrorCode == 1303)
                                        tabControl1.TabPages[0].Tag = sc.ErrorMessage;
                                    else
                                        tabControl1.TabPages[0].Tag = "";
                                    break;
                                case "prCaptureMRZ":
                                    if (ErrorCode == 1303)
                                        tabControl1.TabPages[1].Tag = sc.ErrorMessage;
                                    else
                                        tabControl1.TabPages[1].Tag = "";
                                    break;

                                //case "prSaveBarcodeImage":
                                //ShowImage(pictureBox1, "barcode.jpg");
                                //  break;
                                //case "prSaveMRZImage":
                                //ShowImage(pictureBox2, "mrz.jpg");
                                //  break;
                            }

                            if (ErrorCode == 1303)
                                ErrorCode = 0;

                            if (ErrorCode != 0)
                            {
                                break;
                            }
                        }

                        button1.Enabled = true;
                        button1.Focus();

                        Application.DoEvents();
                    }   // end of foreach

                    if (ErrorCode == 0)
                        ShowStatusMessage("");
                    else
                        ShowError(sc);

                    Application.DoEvents();

                    foreach (TabPage page in tabControl1.TabPages)
                    {
                        if (!String.IsNullOrEmpty(page.Tag as string))
                        {
                            if (page.Equals(tabControl1.SelectedTab))
                            {
                                toolStripStatusLabelError.Text = page.Tag as string;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(toolStripStatusLabelError.Text))
                                    ShowStatusMessage(page.Tag as string);
                            }

                            toolStripStatusLabelError.ForeColor = Color.Red;
                        }
                        /*
                                                else
                                                {
                                                    if (!page.Equals(tabControl1.SelectedTab))
                                                        tabControl1.SelectTab(page.Name);
                                                }
                        */
                    }
                }
                //else if (sc.DeviceState != (int)pr.PR_TESTDOC.PR_TD_NOMOVE)
                else if (sc.DeviceState != (int)Pr22.Util.PresenceState.NoMove)
                    _noMove = false;

                stopProgressBar();

                button1.Enabled = true;

                if (_barcodeType.StartsWith("EAN") || _barcodeType.StartsWith("UPC"))
                {
                    button2.Enabled = true;
                    button3.Enabled = true;
                }

                //button1.Focus();

                startListenProcess(sc);
            }
            else if (ErrorCode != 0)
                ShowError(sc);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;

            _pictureBoxWidth = pictureBox1.Width;

            backgroundWorkerProgressBar = new BackgroundWorker();
            backgroundWorkerProgressBar.WorkerSupportsCancellation = true;
            backgroundWorkerProgressBar.WorkerReportsProgress = true;
            backgroundWorkerProgressBar.DoWork += new DoWorkEventHandler(backgroundWorkerProgressBar_DoWork);
            backgroundWorkerProgressBar.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerProgressBar_RunWorkerCompleted);
            backgroundWorkerProgressBar.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerProgressBar_ProgressChanged);

            backgroundWorkerDataService = new BackgroundWorker();
            backgroundWorkerDataService.DoWork += new DoWorkEventHandler(backgroundWorkerDataService_DoWork);
            backgroundWorkerDataService.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerDataService_RunWorkerCompleted);
            backgroundWorkerDataService.WorkerSupportsCancellation = true;

            backgroundWorkerListen = new BackgroundWorker();
            backgroundWorkerListen.DoWork += new DoWorkEventHandler(backgroundWorkerListen_Listen);
            backgroundWorkerListen.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerListen_RunWorkerCompleted);

            backgroundWorkerConnect = new BackgroundWorker();
            backgroundWorkerConnect.DoWork += new DoWorkEventHandler(backgroundWorkerConnect_Connect);
            backgroundWorkerConnect.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerConnect_RunWorkerCompleted);

            _sc = new ARHScanner();

            button1.Tag = "read";

            PictureBox pb;
            CheckBox cb;
            for (int i = 1; i < 12; i++)
            {
                if (i == 9)
                    continue;

                pb = this.Controls.Find("fpPictureBox" + i.ToString(), true)[0] as PictureBox;
                pb.Image = null;

                pb.MouseHover += new EventHandler(fpPictureBox_MouseHover);
                pb.MouseLeave += new EventHandler(fpPictureBox_MouseLeave);

                cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                cb.Checked = true;
            }

            //TextBoxID.Focus();

            /*
                        if (backgroundWorkerConnect.IsBusy)
                            return;

                        startProgressBar();
                        ShowStatusMessage("Connecting to the device");

                        backgroundWorkerConnect.RunWorkerAsync(_sc);
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //TextBoxID.Enabled = false;

            Button btn = sender as Button;
            if ((btn.Tag as string) == "read")
            {
                button1.Enabled = false;

                if (backgroundWorkerConnect.IsBusy)
                    return;

                startProgressBar();
                ShowStatusMessage("Connecting to the device");

                backgroundWorkerConnect.RunWorkerAsync(_sc);

                btn.Tag = "refresh";

                return;
            }
            else
                _noMove = false;

            if (ErrorCode == 2 || ErrorCode == 4117 || ErrorCode == 19
                || ErrorCode == 16 || ErrorCode == 4100 || ErrorCode == -1)
            {
                if (backgroundWorkerConnect.IsBusy)
                    return;

                startProgressBar();
                toolStripStatusLabelError.Text = "Connecting to the device";
                toolStripStatusLabelError.ForeColor = Color.Black;

                backgroundWorkerConnect.RunWorkerAsync(_sc);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //tabControl1.SelectedTab = tabPage3;

            if (!String.IsNullOrEmpty(tabControl1.SelectedTab.Tag as string))
            {
                ShowStatusMessage(tabControl1.SelectedTab.Tag as string);
                toolStripStatusLabelError.ForeColor = Color.Red;
            }
        }

        public int ShowImage(GetImage method, PictureBox pb)
        {
            int retCode = 0;

            try
            {
                byte[] buff = null;
                retCode = method(out buff);

                if (buff != null)
                {
                    MemoryStream stream = new MemoryStream(buff);
                    pb.Image = Image.FromStream(stream);

                    //pb.Image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

                    if (pb.Image.Width > _pictureBoxWidth)
                        pb.Width = _pictureBoxWidth;
                    else
                        pb.Width = pb.Image.Width;

                    pb.Height = pb.Image.Height;

                    pb.BorderStyle = BorderStyle.FixedSingle;
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception e)
            {
                _sc.ErrorMessage = e.Message + " --- ShowImage()";
                return 1305;
            }

            return retCode;
        }

        public void ShowImage2(PictureBox pb, string fileName)
        {

            /*
            Size size = SystemInformation.PrimaryMonitorSize;
            IntPtr hDc = GetWindowDC(GetDesktopWindow());
            int xScreen = GetDeviceCaps(hDc, 4);              //HORZSIZE
            int yScreen = GetDeviceCaps(hDc, 6);              //VERTSIZE
            int ii = ReleaseDC(GetDesktopWindow(), hDc);

            double xDpi = size.Width / (xScreen / 25.4);
            double yDpi = size.Height / (yScreen / 25.4);
            */

            if (!File.Exists(fileName))
                return;

            IntPtr hWnd = pb.Handle;

            FileStream fs = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                byte[] buffer = br.ReadBytes((int)fs.Length);

                Image img = Image.FromStream(fs);
                /*
                    xDpi = img.HorizontalResolution / xDpi;
                    yDpi = img.VerticalResolution / yDpi;
                    //The maximum recommended size is 200% of the nominal size or 2.938" wide x 2.04" high
                    // 2.938" * 85 dpi ~ 250 pixel
                    // 2.04" * 85 dpi ~ 173 pixel
                    if (img.Width > 250)
                    {
                        pictureBox1.Width = (int)(2 * img.Width / xDpi);
                        pictureBox1.Height = (int)(2 * img.Height / yDpi);
                    }
                    else
                    {
                        pictureBox1.Width = img.Width;
                        pictureBox1.Height = img.Height;
                    }
                */
                if (img.Width > _pictureBoxWidth)
                    pb.Width = _pictureBoxWidth;
                else
                    pb.Width = img.Width;

                pb.Height = img.Height;

                pb.BorderStyle = BorderStyle.FixedSingle;
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Image = img;
            }
            catch (Exception)
            {
                pb.Image = null;
            }

            if (fs != null)
                fs.Close();

            try
            {
                File.Delete(fileName);
            }
            catch { }
        }

        private void service_GetItemByGTINCompleted(object sender, GetItemByGTINCompletedEventArgs e)
        {
            gepirItem item = e.Result;
            textBox1.Text = item.ToString();
        }

        private void service_GepirVersion2Completed(object sender, GepirVersion2CompletedEventArgs e)
        {
            System.Xml.XmlNode node = e.Result;
            textBox1.Text = node.InnerText;
        }


        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //            _gtin = "6271021090002";

            if (string.IsNullOrEmpty(_gtin))
            {
                textBox3.Text = "no barcode available";
                return;
            }

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox3.Text = "";

            Application.DoEvents();

            router service = new router();
            service.PreAuthenticate = false;
            gepirRequestHeader header = new gepirRequestHeader();
            header.requesterGln = "1000000000009";  //GS1 US Customer Service GLN
            //6270000000001	GS1 Kuwait
            header.cascade = 9;
            service.gepirRequestHeaderValue = header;

            //service.GetItemByGTINCompleted += new GetItemByGTINCompletedEventHandler(service_GetItemByGTINCompleted);
            //service.GepirVersion2Completed += new GepirVersion2CompletedEventHandler(service_GepirVersion2Completed);

            GetItemByGTIN itemParam = new GetItemByGTIN();
            itemParam.requestedGtin = _gtin; // "6271021090002";
            itemParam.requestedLanguages = new string[] { "en" };
            itemParam.version = 3;
            //param.version = 7600001001001;

            GetPartyByGTIN partyParam = new GetPartyByGTIN();

            partyParam.requestedGtin = new string[] { _gtin };  // "6271021090002" 
            partyParam.requestedLanguages = new string[] { "en" };

            service.UseDefaultCredentials = true;

            try
            {
                //gepirItem item = service.GetItemByGTIN(itemParam);

                gepirParty party = service.GetPartyByGTIN(partyParam);

                //string text = iterateThroughAllProperties(party);

                partyDataLineType dl = party.partyDataLine[0];

                StringBuilder sb = new StringBuilder();

                if (dl.partyName != null)
                {
                    sb.Append("Party Name: " + dl.partyName);
                    sb.Append(Environment.NewLine);
                }

                if (dl.streetAddress != null && dl.streetAddress[0] != null)
                {
                    sb.Append("Street Address: " + dl.streetAddress[0]);
                    sb.Append(Environment.NewLine);
                }

                if (dl.pOBoxNumber != null)
                {
                    sb.Append("PO Box Number: " + dl.pOBoxNumber);
                    sb.Append(Environment.NewLine);

                }

                if (dl.postalCode != null)
                {
                    sb.Append("Postal Code: " + dl.postalCode);
                    sb.Append(Environment.NewLine);
                }

                if (dl.city != null)
                {
                    sb.Append("City: " + dl.city);
                    sb.Append(Environment.NewLine);
                }

                if (dl.countryISOCode != null)
                    sb.Append("Country: " + dl.countryISOCode);

                if (sb.ToString() != "")
                {
                    textBox3.Text = sb.ToString();
                }
                else
                    textBox3.Text = "no information found";

            }
            catch (System.Web.Services.Protocols.SoapException sex)
            {
                String errorMessage = sex.Message;
                if (sex.Detail != null)
                {
                    System.Xml.XmlNode node = sex.Detail.SelectSingleNode("error");
                    if (node != null)
                    {
                        toolStripStatusLabelError.Text = node.Attributes["errorMessage"].Value;
                        //errorMessage = node.Attributes["errorMessage"].Value;
                    }
                }

                toolStripStatusLabelError.Text = errorMessage;
                toolStripStatusLabelError.ForeColor = Color.Black;
                //throw new Exception(errorMessage);
            }

            //textBox3.Select(0, 0);
            HideCaret(textBox3);
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;

            button1.Focus();
        }

        string iterateThroughAllProperties(gepirParty party)
        {
            StringBuilder sb = new StringBuilder();
            if (party != null)
            {
                foreach (partyDataLineType dl in party.partyDataLine)
                {
                    System.Reflection.PropertyInfo[] properties = dl.GetType().GetProperties();
                    foreach (System.Reflection.PropertyInfo prop in properties)
                    {
                        sb.Append(prop.Name + ": " + prop.GetValue(dl, null));
                        sb.Append(Environment.NewLine);
                    }
                    if (!string.IsNullOrEmpty(sb.ToString()))
                        textBox3.Text = sb.ToString();
                }
            }

            return sb.ToString();
        }

        public void HideCaret(TextBox textBox)
        {
            HideCaret(textBox.Handle);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //saveWsqInDatabase();

            int hand = 0;
            int count = 3;
            int offset = 0; // fingers collection offset

            //TextBoxID.Enabled = true;

            int id;

            if (TextBoxID.Text.Length == 0 || !Int32.TryParse(TextBoxID.Text, out id)) {
                toolStripStatusLabelError.Text = "Please enter a valid ID";
                toolStripStatusLabelError.ForeColor = Color.Red;
                return;
            } else
                toolStripStatusLabelError.Text = "";

            buttonLeftHand.Enabled = false;
            buttonRightHand.Enabled = false;
            buttonThumbs.Enabled = false;
            buttonReadFingers.Enabled = false;
            button4.Enabled = false;

            if (e is MyEventArgs)
            {
                hand = (e as MyEventArgs).hand;
                if (hand == 0)      // only left hand
                    count = 1;
                else if (hand == 1) // only right hand
                {
                    count = 2;
                    offset = 4;
                }
                else                // only thumbs hand = 2; count = 3
                    offset = 7;     // 7 !!! not 8 - this is because a pattern for thumbs is always 0110
            }
            else
            {
                PictureBox pb;
                CheckBox cb;
                for (int i = 1; i < 12; i++)
                {
                    if (i == 9)
                        continue;

                    pb = this.Controls.Find("fpPictureBox" + i.ToString(), true)[0] as PictureBox;
                    pb.Image = null;

                    cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                    cb.Checked = true;
                }
            }

            tabControl1.SelectTab("tabPage3");
            Application.DoEvents();

            _sc.ErrorMessage = "";

            button4.Enabled = false;

            ShowStatusMessage("Finger scan processing...");

            Application.DoEvents();

            startProgressBar();

            if ((ErrorCode = _sc.fpsConnect()) != 0)
            {
                ShowError(_sc);
            }
            else
            {
                if (!(e is MyEventArgs) || _fingersCollection == null)
                {
                    _fingersCollection = new ArrayList(10);
                    for (int i = 0; i < _fingersCollection.Capacity; i++)
                        _fingersCollection.Add(null);
                    //                    fingersCollection.Add(new Byte[] { new Byte() });
                }

                bool[] processAsTemplate = new bool[_fingersCollection.Count];
                //SoundPlayer simpleSound = null;
                for (int i = hand; i < count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            label1.Text = "Put the left hand on the glass. ";
                            //simpleSound = new SoundPlayer("left_hand.wav");
                            //simpleSound.Play();
                            break;
                        case 1:
                            label1.Text = "Put the right hand on the glass. ";
                            //simpleSound = new SoundPlayer("right_hand.wav");
                            //simpleSound.Play();
                            break;
                        case 2:
                            label1.Text = "Put both thumbs on the glass. ";
                            //simpleSound = new SoundPlayer("both_thumbs.wav");
                            //simpleSound.Play();
                            break;
                    }

                    Application.DoEvents();

                    System.Threading.Thread.Sleep(2000);

                    label1.Text += "Go ...";
                    Application.DoEvents();

                    if ((ErrorCode = scanFingers(i)) != 0)
                    {
                        ShowError(_sc);
                        break;
                    }

                    //byte[] fingersCollection = new byte[_fingersCollection.Count];

                    //fingersCollection = _fingersCollection.Clone() as ArrayList;

                    for (int k = 0; k < 4; k++)
                    {
                        //if (_sc.ArrayOfBMP[k] != null && ((byte[])_sc.ArrayOfBMP[k]).Length > 1)
                        //    _fingersCollection[k + offset] = _sc.ArrayOfWSQ[k];
                        //if (_sc.ArrayOfWSQ[k] != null && ((WsqImage)_sc.ArrayOfWSQ[k]).Content != null)
                        if (_sc.ArrayOfWSQ[k] != null)
                        {
                            _fingersCollection[k + offset] = _sc.ArrayOfWSQ[k];
                            processAsTemplate[k + offset] = true;
                        }
                    }

                    offset += 4;
                    if (offset == 8)
                        offset = 7;
                }

                if (ErrorCode == 0)
                {
                    ShowStatusMessage("");

                    byte[] buff = null;
                    MemoryStream ms = new MemoryStream();

                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(ms, _fingersCollection as ArrayList);
                        buff = ms.ToArray();
                        saveWsqInDatabase(id, buff, processAsTemplate);
                    }
                    catch (SerializationException ex)
                    {
                        toolStripStatusLabelError.ForeColor = Color.Red;
                        toolStripStatusLabelError.Text = ex.Message;
                    }
                    finally
                    {
                        ms.Close();
                    }

                    //byte[] buffer = (_sc.ArrayOfWSQ as ArrayList).ToArray(typeof(byte[])) as byte[];
//                    saveWsqInDatabase(buff);
                }
            }

            label1.Text = "";

            _sc.fpsDisconnect();

            stopProgressBar();

            buttonLeftHand.Enabled = true;
            buttonRightHand.Enabled = true;
            buttonThumbs.Enabled = true;
            buttonReadFingers.Enabled = true;
            button4.Enabled = true;

            button4.Focus();
        }

        private int scanFingers(int hand)  // 0 - left hand;  1 - right hand;  2 - thumbs
        {
            int errorCode = 0;

            //IList list = new ArrayList();

            // The finger list has the format 0hhh 0000 iiii mmmm rrrr llll tttt ssss
            //	h - scan object: 001 left hand, 010 right hand, 011 same fingers of both hands

            StringBuilder sb = new StringBuilder(10);
            int pictureBoxOffset = 1;
            int count = 4;

            switch (hand)
            {
                case 0:
                    sb.Append("10");        //0x10333300    left hand
                    //pictureBoxOffset = 1;
                    break;
                case 1:
                    sb.Append("20");        //0x20333300    right hand
                    pictureBoxOffset = 5;
                    break;
                case 2:
                    sb.Append("300000");    //0x30000033    thumbs
                    pictureBoxOffset = 9;
                    count = 3;
                    //pictureBoxOffset = 10;
                    //count = 2;
                    break;
            }

            for (int j = 0; j < count; j++)
            {
                if (j + pictureBoxOffset == 9)
                    continue;

                if (((CheckBox)this.Controls.Find("checkBox" + (j + pictureBoxOffset).ToString(), true)[0]).Checked)
                    sb.Append("3");
                else
                    sb.Append("0");
            }

            switch (hand)
            {
                case 0:
                case 1:
                    sb.Append("00");
                    break;
            }

            int handAndFingerMask = Int32.Parse(sb.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
            if ((errorCode = _sc.fpsGetFingersImages(handAndFingerMask, false)) == 0)
            {

                //saveWsqInDatabase();
                showFingers(pictureBoxOffset);
            }

            return errorCode;
        }

        enum SAVE
        {
            INSERT = 0,
            UPDATE = 1
        }

        private void saveWsqInDatabase(int id, byte[] buffer, bool[] processAsTemplate)
        {
            //FileStream fs = null;

            try
            {
                //  fs = new FileStream("lindex.wsq", FileMode.Open);
                //  BinaryReader br = new BinaryReader(fs);
                //  byte[] buffer = br.ReadBytes((int)fs.Length);

                if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
                {
                    DBUtil db = new DBUtil();
                    db.UploadImage(IMAGE_TYPE.wsq, id, ref buffer);
                }
                else
                {
                    var bioProcessor = new BioProcessor.BioProcessor(FingersQualityThreshold : (byte)trackBar1.Value);

                    Dictionary<string, byte[]> templates = bioProcessor.GetTemplatesFromWSQImage(id, buffer, processAsTemplate);
                    Dictionary<string, string> settings = new Dictionary<string, string>();
                    foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                    {
                        settings.Add(key, ConfigurationManager.AppSettings[key]);
                    }

                    foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                    {
                        settings.Add(cs.Name, cs.ConnectionString);
                    }

                    var db = new DAO.Database(settings);
                    db.SaveWSQTemplate(id, templates);
                    this.InvokeOnClick(buttonReadFingers, new MyEventArgs(0));
                }


                //db.SaveTemplate(id, buffer);
            }
            catch (Exception ex)
            {
                toolStripStatusLabelError.ForeColor = Color.Red;
                toolStripStatusLabelError.Text = ex.Message;
            }
            finally
            {
                //fs.Dispose();
            }
        }

        private void showFingers(int pictureBoxOffset)
        {
            MemoryStream stream;
            PictureBox pb;
            CheckBox cb;
            byte[] buff;

            int i = 0; int k = 4;
            if (pictureBoxOffset == 9)
            {
                i = 1; k = 3;
            }
            for (; i < k; i++)
            {
                buff = (byte[])_sc.ArrayOfBMP[i];
                if (buff != null && buff.Length == 1)
                    continue;

                pb = this.Controls.Find("fpPictureBox" + (i + pictureBoxOffset).ToString(), true)[0] as PictureBox;
                if (buff == null)
                    pb.Image = null;
                else
                {
                    stream = new MemoryStream(buff);
                    pb.Image = Image.FromStream(stream);
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    cb = this.Controls.Find("checkBox" + (i + pictureBoxOffset).ToString(), true)[0] as CheckBox;
                    cb.Checked = false;
                }
            }
        }

        private class MyEventArgs : EventArgs
        {
            public int hand;
            public MyEventArgs(int hand)
            {
                this.hand = hand;
            }
        }

        private void buttonLeftHand_Click(object sender, EventArgs e)
        {
            bool ch = false;

            for (int i = 1; i < 5; i++)
            {
                CheckBox cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                {
                    ch = true;
                    break;
                }
            }

            if (ch)
                this.InvokeOnClick(button4, new MyEventArgs(0));
        }

        private void buttonRightHand_Click(object sender, EventArgs e)
        {
            bool ch = false;

            for (int i = 5; i < 9; i++)
            {
                CheckBox cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                {
                    ch = true;
                    break;
                }
            }

            if (ch)
                this.InvokeOnClick(button4, new MyEventArgs(1));
        }

        private void buttonThumbs_Click(object sender, EventArgs e)
        {
            bool ch = false;

            for (int i = 10; i < 12; i++)
            {
                CheckBox cb = this.Controls.Find("checkBox" + i.ToString(), true)[0] as CheckBox;
                if (cb.Checked)
                {
                    ch = true;
                    break;
                }
            }

            if (ch)
                this.InvokeOnClick(button4, new MyEventArgs(2));
        }

        Helper _helper = null;
        FontFamily _fontFamily = null;

        private void generateBarcodeImage()
        {
            if (_helper == null)
                _helper = new Helper();

            if (_fontFamily == null)
            {
                // Create a private font collection
                PrivateFontCollection pfc = new PrivateFontCollection();
                // Load in the temporary barcode font
                pfc.AddFontFile("3OF9_NEW.TTF");
                // Select the font family to use
                _fontFamily = new FontFamily("3 of 9 Barcode", pfc);

            }

            int result;
            if (!Int32.TryParse(textBox5.Text, out result))
            {
                result = 24;
                textBox5.Text = result.ToString();
            }

            pictureBox3.Image = _helper.GenerateBarcodeImage(textBox4.Text, _fontFamily, result);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox4.Text))
                return;

            generateBarcodeImage();

            // Create a Print Document
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(PrintPage);
            //doc.PrintPage += PrintPage;
            doc.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
            doc.PrinterSettings.PrintToFile = true;
            doc.PrinterSettings.PrintFileName = textBox4.Text + ".xps";
            doc.Print();

            _fontFamily.Dispose();
            _fontFamily = null;
        }

        // Handler for PrintPageEvents
        void PrintPage(object o, PrintPageEventArgs e)
        {
            Point p = new Point(100, 100);

            StringBuilder sb = new StringBuilder();
            sb.Append("*");
            sb.Append(textBox4.Text);
            sb.Append("*");

            Font font = new Font(_fontFamily, Int32.Parse(textBox5.Text), FontStyle.Regular, GraphicsUnit.Point);
            e.Graphics.DrawString(sb.ToString(), font, new SolidBrush(Color.Black), p.X, p.Y);
            SizeF textSize = e.Graphics.MeasureString(sb.ToString(), font);
            font.Dispose();

            font = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Point);
            e.Graphics.DrawString(textBox4.Text, font, new SolidBrush(Color.Black), p.X + 10, p.Y + textSize.Height);
            font.Dispose();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox4.Text))
                return;

            generateBarcodeImage();
        }

        private void buttonReadFingers_Click(object sender, EventArgs e)
        {
            int id;

            if (TextBoxID.Text.Length == 0 || !Int32.TryParse(TextBoxID.Text, out id))
            {
                toolStripStatusLabelError.Text = "Please enter a valid ID";
                toolStripStatusLabelError.ForeColor = Color.Red;
                return;
            }
            else
                toolStripStatusLabelError.Text = "";

            toolStripStatusLabelError.Text = string.Empty;

            button4.Enabled = false;
            buttonReadFingers.Enabled = false;
            buttonLeftHand.Enabled = false;
            buttonRightHand.Enabled = false;
            buttonThumbs.Enabled = false;

            tabControl1.SelectTab("tabPage3");

            startDataServiceProcess(id);
        }

        private void processAbtainedFingers(byte[][] buffer)
        {
            //int id;

            //if (TextBoxID.Text.Length == 0 || !Int32.TryParse(TextBoxID.Text, out id)) {
            //    toolStripStatusLabelError.Text = "Please enter a valid ID";
            //    toolStripStatusLabelError.ForeColor = Color.Red;
            //    return;
            //} else
            //    toolStripStatusLabelError.Text = "";

            //toolStripStatusLabelError.Text = string.Empty;

            //startProgressBar();

            //System.Threading.Thread.Sleep(5000);

            //buttonLeftHand.Enabled = false;
            //buttonRightHand.Enabled = false;
            //buttonThumbs.Enabled = false;

            //tabControl1.SelectTab("tabPage3");
            //Application.DoEvents();



            //Dim fingersCollection As ArrayList
            //BioProcessor.BioProcessor bioProcessor = null;

            //byte[][] buffer = null;
            MemoryStream ms = null;
            //ArrayList _fingersCollection = null;
            BioProcessor.BioProcessor bioProcessor = null;

            try
            {
                if (buffer[0] == null || buffer[0].Length == 0)
                {
                    toolStripStatusLabelError.Text = "Can't read fingers' images using ID provided";
                    toolStripStatusLabelError.ForeColor = Color.Red;
                    PictureBox pbox;
                    for (int i = 0; i < 10; i++)
                    {
                        pbox = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                        pbox.Image = null;
                    }
                    return;
                }
                //if (System.Configuration.ConfigurationManager.AppSettings["Enroll"] == "service")
                //{
                //    var db = new DBUtil();
                //    buffer[0] = db.GetImageFromWebService(IMAGE_TYPE.wsq, id);
                //} 
                //else
                //{
                //    Dictionary<string, string> settings = new Dictionary<string, string>();
                //    foreach (var key in ConfigurationManager.AppSettings.AllKeys)
                //    {
                //        settings.Add(key, ConfigurationManager.AppSettings[key]);
                //    }

                //    foreach (ConnectionStringSettings cs in ConfigurationManager.ConnectionStrings)
                //    {
                //        settings.Add(cs.Name, cs.ConnectionString);
                //    }

                //    var db = new DAO.Database(settings);
                //    buffer = db.GetImage(DAO.IMAGE_TYPE.wsq, id);
                //    if (buffer[0] == null || buffer[0].Length == 0)
                //    {
                //        toolStripStatusLabelError.Text = "Can't read fingers' images using ID provided";
                //        toolStripStatusLabelError.ForeColor = Color.Red;
                //        PictureBox pbox;
                //        for (int i = 0; i < 10; i++)
                //        {
                //            pbox = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                //            pbox.Image = null;
                //        }
                //        return;
                //    }

                //    bioProcessor = new BioProcessor.BioProcessor();
                //}
                bioProcessor = new BioProcessor.BioProcessor(FingersQualityThreshold: (byte)trackBar1.Value);

                //MemoryStream ms = null;
                ms = new MemoryStream(buffer[0]);
                //Construct a BinaryFormatter and use it to deserialize the data to the stream.
                var formatter = new BinaryFormatter();

//                try
//                {
                    formatter.Binder = new WsqSerializationBinder.GenericBinder<WsqImage>();
                    _fingersCollection = formatter.Deserialize(ms) as ArrayList;
//                }
//                catch (SerializationException ex)
//                {
//                    toolStripStatusLabelError.ForeColor = Color.Red;
//                    toolStripStatusLabelError.Text = ex.Message;
//                }
//                finally
//                {
//                    ms.Close();
//                }

                PictureBox pb;
                System.Object theLock = new System.Object();

                string label = ""; Brush brush = Brushes.Transparent;
                int pct = 0;
                for (int i = 0; i <= 9; i++)
                {
                    pb = this.Controls.Find("fpPictureBox" + (i + 1 < 9 ? (i + 1).ToString() : (i + 2).ToString()), true)[0] as PictureBox;
                    if (_fingersCollection[i] != null)
                    {
                        WsqImage wsq = _fingersCollection[i] as WsqImage;
                        if (wsq == null || wsq.Content == null)
                        {
                            pb.Image = null;
                            continue;
                        }

                        lock (theLock)
                        {
                            buffer[0] = ARHScanner.ConvertWSQToBmp(wsq);
                            ARHScanner.DisposeWSQImage();
                        }

                        if (buffer[0] != null && buffer[0].Length < 2)
                            continue;

                        if (buffer[0] == null)
                            pb.Image = null;
                        else
                        {
                            using (var ms2 = new MemoryStream(buffer[0]))
                            {
                                if (bioProcessor != null)
                                {
                                    if (buffer[i + 1] != null && buffer[i + 1].Length != 0)
                                        pct = bioProcessor.getImageQuality(buffer[i + 1]);
                                    else
                                        pct = 0;

                                    if (pct > 0)
                                    {
                                        label = string.Format("Q: {0:P0}", pct / 100.0);
                                        if (pct > 79)
                                            brush = Brushes.Green;
                                        else if (pct > 39)
                                            brush = Brushes.Orange;
                                        else
                                            brush = Brushes.Red;
                                    }
                                    else
                                    {
                                        label = string.Format("Q: {0:P0}", 0);
                                        brush = Brushes.Red;
                                    }

                                    //Bitmap bmp = new Bitmap(nImage.ToBitmap(), new Size(65, 95));
                                    //Bitmap bmp2 = new Bitmap(nImage.ToBitmap(), new Size(100, 120));
                                    //Bitmap bmp2 = new Bitmap(pb.Image, new Size(100, 120));
                                    //Bitmap bmp = new Bitmap(Image.FromStream(ms2), new Size(100, 120));
                                    Bitmap bmp = new Bitmap(Image.FromStream(ms2));

                                    //RectangleF rectf = new RectangleF(0.0f, 2.0f, 65.0f, 40.0f);
                                    RectangleF rectf = new RectangleF(0.0f, 8.0f, 390.0f, 260.0f);
                                    Graphics g = Graphics.FromImage(bmp);
                                    g.SmoothingMode = SmoothingMode.AntiAlias;
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                                    g.DrawString(label, new Font("Areal", 46, FontStyle.Bold), brush, rectf);
                                    g.Flush();

                                    using (var ms3 = new MemoryStream())
                                    {
                                        bmp.Save(ms3, System.Drawing.Imaging.ImageFormat.Bmp);
                                        pb.Image = Image.FromStream(ms3);
                                    }
                                }
                                else
                                {
                                    pb.Image = Image.FromStream(ms2);
                                }

                                pb.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                    }
                    else
                    {
                        pb.Image = null;
                    }
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabelError.ForeColor = Color.Red;
                toolStripStatusLabelError.Text = ex.Message;
            }
            finally
            {
                if (ms != null)
                    ms.Close();

                if (bioProcessor != null)
                    bioProcessor.CleanBiometrics();

                buttonLeftHand.Enabled = true;
                buttonRightHand.Enabled = true;
                buttonThumbs.Enabled = true;
                buttonReadFingers.Enabled = true;
                button4.Enabled = true;

                stopProgressBar();
                TextBoxID.Focus();
            }
        }

        private void fpPictureBox_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb.Image != null)
            {
                var ratio = pb.PreferredSize.Height / pb.PreferredSize.Width;
                PictureBox4.Image = pb.Image;
                PictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
                Panel1.Width = (int)(pb.PreferredSize.Width / 1.5);
                Panel1.Height = (int)(pb.PreferredSize.Width / 1.5 * ratio);
                if (pb.Name == "fpPictureBox4" || pb.Name == "fpPictureBox5")
                    Panel1.Location = new Point(tabPage3.Width - Panel1.Width, 0);
                else
                    Panel1.Location = new Point(0, 0);

                Panel1.Visible = true;
            }
        }

        private void fpPictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            PictureBox4.Image = null;
            Panel1.Visible = false;
        }
    }
}
