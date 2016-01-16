using System;
//using PrFps22;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using Pr22.Task;
using Pr22.Util;

enum BarcodeTypes {
        // Summary:
        //     8 char article numbering code
        EAN8 = 1,
        //
        // Summary:
        //     13 char article numbering code
        EAN13 = 2,
        //
        // Summary:
        //     General text code.
        Code39 = 3,
        //
        // Summary:
        //     General text code.
        Code128 = 4,
        //
        // Summary:
        //     2D binary / text code
        PDF417 = 5,
        //
        // Summary:
        //     General number code.
        ITF = 6,
        //
        // Summary:
        //     2D binary / text code
        DataMatrix = 7,
        //
        // Summary:
        //     2D binary / text code
        QR = 8,
        //
        // Summary:
        //     2D binary / text code
        Aztec = 9,
        //
        // Summary:
        //     Universal Postal Union code.
        UPU = 10,
};

class ARHScanner
{
    Pr22.DocumentReaderDevice _pr = null;
    Pr22.FingerprintScannerDevice _fps = null;

    PresenceState Detect = PresenceState.NoMove;

    int Resolution = 0;
    int ScanState = 0;
    int Progress = 0;
    int[] Quality = new int[4];
    string Message = "";

    //Lib _lib;
    Helper _helper = null;
    //    prDoc _doc;
    Pr22.Processing.Document ndoc;

    string _errorMessage;
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }

    public int DeviceState { get; set; }

    private IList _arrayOfWSQ = null;
    public IList ArrayOfWSQ
    {
        get
        {
            return _arrayOfWSQ;
        }
    }

    private IList _arrayOfBMP = null;
    public IList ArrayOfBMP
    {
        get
        {
            return _arrayOfBMP;
        }
    }

    private byte[] _nistImageBytes = null;
    public byte[] NistImageBytes
    {
        get
        {
            return _nistImageBytes;
        }
    }

    public int fpsConnect()
    {
        if (_helper == null)
            _helper = new Helper();

        try
        {
            fpsDisconnect();

            /* Opening the FPS system */
            _fps = new Pr22.FingerprintScannerDevice();	/* Object for the FPS system */

            //_fps.PreviewCaptured += new System.EventHandler<Pr22.Events.PreviewEventArgs>(PreviewCaptured);
            //_fps.ImageScanned += new System.EventHandler<Pr22.Events.FingerImageEventArgs>(ImageScanned);
            //_fps.FingerScanned += new System.EventHandler<Pr22.Events.FingerEventArgs>(FingerScanned);
            //_fps.PresenceStateChanged += new System.EventHandler<Pr22.Events.DetectionEventArgs>(PresenceStateChanged);

            /* Connecting to the first device */
            _fps.UseDevice(0);
        }
        catch (Pr22.Exceptions.General e)
        {
            //_errorMessage = e.Message + " --- fpsConnect()";
            //return 1305;
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- fpsConnect()";
            return 1305;
        }

        return 0;
    }


    ////----------------------------------------------------------------------

    //void PreviewCaptured(object a, Pr22.Events.PreviewEventArgs e)
    //{
    //    Resolution = (int)(_fps.Scanner.GetLiveImage().HRes / 39.37);
    //    ScanState++;
    //    PrintState();
    //}
    ////----------------------------------------------------------------------

    //void ImageScanned(object a, Pr22.Events.FingerImageEventArgs e)
    //{
    //    int[] pos = { 0, 2, 0, 1, 2, 3, 1, 3, 2, 1, 0 };
    //    int ix = (int)e.position > 10 ? 0 : (int)e.position;
    //    Quality[pos[ix]] = _fps.Scanner.GetFinger(e.position, Pr22.Imaging.ImpressionType.Plain).GetQuality();
    //    PrintState();
    //}
    ////----------------------------------------------------------------------

    //void FingerScanned(object a, Pr22.Events.FingerEventArgs e)
    //{
    //    Message = e.fingerFailureMask.ToString();
    //    PrintState();
    //}
    ////----------------------------------------------------------------------

    //void PresenceStateChanged(object a, Pr22.Events.DetectionEventArgs e)
    //{
    //    Message = e.State.ToString();
    //    Detect = e.State;
    //    PrintState();
    //}

    //void PrintState()
    //{
    //    string States = "|/-\\";
    //    string Prog = "##########";

    //    System.Console.Write(" {0} {1,3} DPI Progress:[{2,-10}] [{3,-21}] Q1:{4,4} Q2:{5,4} Q3:{6,4} Q4:{7,4}\r",
    //        States[ScanState % 4], Resolution, Prog.Substring(10 - Progress / 10),
    //        Message, Quality[0], Quality[1], Quality[2], Quality[3]);
    //}

    ////----------------------------------------------------------------------


    public int fpsGetFingersImages(int handAndFingerMask, bool saveFingerAsFile)
    {
        try
        {
            if (_arrayOfBMP != null) { _arrayOfBMP.Clear(); _arrayOfBMP = null; }
            if (_arrayOfWSQ != null) { _arrayOfWSQ.Clear(); _arrayOfWSQ = null; }

            /* Clears internal stored finger buffers */
            _fps.Scanner.CleanUpData();


            /* Starts an asynchronous capture process
            // params: time in usec, quality in per-thousand, mode of live scan, fingerlist
            //
            // The finger list has the format 0hhh 0000 iiii mmmm rrrr llll tttt ssss
            //	h - scan object: 001 left hand, 010 right hand, 011 same fingers of both hands
            //	i - index finger	|
            //	m - middle finger	|
            //	r - ring finger		|--> value of FPS_PRESENCE   FPS_AVAILABLE = 3 
            //	l - little finger	|
            //	t - left thumb		|
            //	s - right thumb		|
            */

            Pr22.Control.StatusLed.Color color = Pr22.Control.StatusLed.Color.Off;
            Pr22.Imaging.FingerPosition index = Pr22.Imaging.FingerPosition.Unknown;
            Pr22.Imaging.FingerPosition middle = Pr22.Imaging.FingerPosition.Unknown;
            Pr22.Imaging.FingerPosition ring = Pr22.Imaging.FingerPosition.Unknown;
            Pr22.Imaging.FingerPosition little = Pr22.Imaging.FingerPosition.Unknown;
            string wsqIndexFileName = String.Empty;
            string wsqMiddleFileName = String.Empty;
            string wsqRingFileName = String.Empty;
            string wsqLittleFileName = String.Empty;

            short idx = 0;
            foreach (Pr22.Control.StatusLed statled in _fps.Peripherals.StatusLeds)
            {
                if (idx++ > 11)
                    break;

                statled.Turn(color); // off
            }

            color = Pr22.Control.StatusLed.Color.Green;

            int fingerMask = 0x00;
            switch (handAndFingerMask & 0xff000000)
            {
                case 0x10000000:    //0x10333300    left hand
                case 0x20000000:    //0x20333300    right hand
                    fingerMask |= (handAndFingerMask & 0x00300000) != 0 ? 0x08 : 0x00;
                    fingerMask |= (handAndFingerMask & 0x00030000) != 0 ? 0x04 : 0x00;
                    fingerMask |= (handAndFingerMask & 0x00003000) != 0 ? 0x02 : 0x00;
                    fingerMask |= (handAndFingerMask & 0x00000300) != 0 ? 0x01 : 0x00;
                    break;
                case 0x30000000:    //0x20000033    thumbs
                    fingerMask |= (handAndFingerMask & 0x00000030) != 0 ? 0x04 : 0x00;
                    fingerMask |= (handAndFingerMask & 0x00000003) != 0 ? 0x02 : 0x00;
                    break;
            }

            int lampMask = fingerMask;
            switch (handAndFingerMask & 0xff000000)
            {
                case 0x10000000:    //0x10333300  left hand
                    index = Pr22.Imaging.FingerPosition.LeftIndex;
                    middle = Pr22.Imaging.FingerPosition.LeftMiddle;
                    ring = Pr22.Imaging.FingerPosition.LeftRing;
                    little = Pr22.Imaging.FingerPosition.LeftLittle;
                    wsqIndexFileName = "lindex.wsq";
                    wsqMiddleFileName = "lmiddle.wsq";
                    wsqRingFileName = "lring.wsq";
                    wsqLittleFileName = "llittle.wsq";
                    lampMask = 0x80;
                    lampMask |= (fingerMask & 0x00000001) != 0 ? 0x08 : 0x00;
                    lampMask |= (fingerMask & 0x00000002) != 0 ? 0x04 : 0x00;
                    lampMask |= (fingerMask & 0x00000004) != 0 ? 0x02 : 0x00;
                    lampMask |= (fingerMask & 0x00000008) != 0 ? 0x01 : 0x00;
                    break;
                case 0x20000000:    //0x20333300
                    index = Pr22.Imaging.FingerPosition.RightIndex;
                    middle = Pr22.Imaging.FingerPosition.RightMiddle;
                    ring = Pr22.Imaging.FingerPosition.RightRing;
                    little = Pr22.Imaging.FingerPosition.RightLittle;
                    wsqIndexFileName = "rindex.wsq";
                    wsqMiddleFileName = "rmiddle.wsq";
                    wsqRingFileName = "rring.wsq";
                    wsqLittleFileName = "rlittle.wsq";
                    lampMask |= 0x40;
                    break;
                case 0x30000000:    //0x30000033
                    middle = Pr22.Imaging.FingerPosition.LeftThumb;
                    ring = Pr22.Imaging.FingerPosition.RightThumb;
                    wsqMiddleFileName = "lthumb.wsq";
                    wsqRingFileName = "rthumb.wsq";
                    lampMask |= 0x20;
                    break;
            }

            /* Turning the display leds depending on the mask */
            for (int i = 0; i < 32; i++)
                if ((lampMask & (1 << i)) != 0)
                    _fps.Peripherals.StatusLeds[i].Turn(color);

            ////starting detection
            //TaskControl LiveTask =_fps.Scanner.StartTask(FingerTask.Detection());

            //int timeout = 10;  // in 100 milliseconds
            ////int i;

            //for (int i = 0; i < timeout && Detect != PresenceState.Present; ++i)
            //    System.Threading.Thread.Sleep(100);

            Pr22.Task.FingerTask ftask = Pr22.Task.FingerTask.PlainScan(700, 3000);
            //ftask.Add(index).Add(middle).Add(ring).Add(little).Del(Pr22.Imaging.FingerPosition.Unknown);
            //if (index != Pr22.Imaging.FingerPosition.Unknown) ftask.Add(index);
            //if (middle != Pr22.Imaging.FingerPosition.Unknown) ftask.Add(middle);
            //if (ring != Pr22.Imaging.FingerPosition.Unknown) ftask.Add(ring);
            //if (little != Pr22.Imaging.FingerPosition.Unknown) ftask.Add(little);

            int mask = 0x10;
            for (int i = 0; i < 4; i++)
            {
                mask >>= 1;
                switch (fingerMask & mask)
                {
                    case 0x08:
                        ftask.Add(index);
                        break;
                    case 0x04:
                        ftask.Add(middle);
                        break;
                    case 0x02:
                        ftask.Add(ring);
                        break;
                    case 0x01:
                        ftask.Add(little);
                        break;
                }
            }

            Pr22.Task.TaskControl tc = _fps.Scanner.StartTask(ftask);

            //for (stat = 0; stat < 100; )
            int status = 0;
            while (status < 100)
            {
                /* Test if better images are captured or capture has accomplished */
                status = tc.GetState();

                _helper.Wait(100);
            }

            /* Closing the capture sequence */
            tc.Wait();

            //for (int i = 0; i < timeout && Detect == PresenceState.Present; ++i)
            //    System.Threading.Thread.Sleep(100);

            //LiveTask.Stop();

            color = Pr22.Control.StatusLed.Color.Off;
            idx = 0;
            foreach (Pr22.Control.StatusLed statled in _fps.Peripherals.StatusLeds)
            {
                if (idx++ > 11)
                    break;

                statled.Turn(color); // off
            }

            /* Save individual finger images */
            Pr22.Imaging.RawImage rawImage;
            Pr22.Imaging.FingerImage fingerImage = null;
            mask = 0x10;

            _arrayOfBMP = new ArrayList();
            _arrayOfWSQ = new ArrayList();

            for (int i = 0; i < 4; i++)
            {
                mask >>= 1;
                bool valid = true;
                switch (fingerMask & mask)
                {
                    case 0x08:
                        try
                        {
                            fingerImage = _fps.Scanner.GetFinger(index, Pr22.Imaging.ImpressionType.Plain);
                            if (saveFingerAsFile)
                                fingerImage.GetImage().Save(Pr22.Imaging.RawImage.FileFormat.Wsq).Save(wsqIndexFileName);
                        }
                        catch
                        {
                            valid = false;
                            if (saveFingerAsFile)
                                File.Delete(wsqIndexFileName);
                        }
                        break;
                    case 0x04:
                        try
                        {
                            fingerImage = _fps.Scanner.GetFinger(middle, Pr22.Imaging.ImpressionType.Plain);
                            if (saveFingerAsFile)
                                fingerImage.GetImage().Save(Pr22.Imaging.RawImage.FileFormat.Wsq).Save(wsqMiddleFileName);
                        }
                        catch
                        {
                            valid = false;
                            if (saveFingerAsFile)
                                File.Delete(wsqMiddleFileName);
                        }
                        break;
                    case 0x02:
                        try
                        {
                            fingerImage = _fps.Scanner.GetFinger(ring, Pr22.Imaging.ImpressionType.Plain);
                            if (saveFingerAsFile)
                                fingerImage.GetImage().Save(Pr22.Imaging.RawImage.FileFormat.Wsq).Save(wsqRingFileName);
                        }
                        catch
                        {
                            valid = false;
                            if (saveFingerAsFile)
                                File.Delete(wsqRingFileName);
                        }
                        break;
                    case 0x01:
                        try
                        {
                            fingerImage = _fps.Scanner.GetFinger(little, Pr22.Imaging.ImpressionType.Plain);
                            if (saveFingerAsFile)
                                fingerImage.GetImage().Save(Pr22.Imaging.RawImage.FileFormat.Wsq).Save(wsqLittleFileName);
                        }
                        catch
                        {
                            valid = false;
                            if (saveFingerAsFile)
                                File.Delete(wsqLittleFileName);
                        }
                        break;
                    default:
                        _arrayOfBMP.Add(new Byte[] { new Byte() });
                        _arrayOfWSQ.Add(new WsqImage());
                        //_arrayOfWSQ.Add(new Byte[] { new Byte() });
                        continue;
                }

                if (valid)
                {
                    rawImage = fingerImage.GetImage();

                    WsqImage im = new WsqImage();
                    im.Content = rawImage.Save(Pr22.Imaging.RawImage.FileFormat.Wsq).ToByteArray();
                    //im.Content = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_BMP);
                    im.XRes = rawImage.HRes / (10000 / 254);
                    im.YRes = rawImage.VRes / (10000 / 254);
                    im.XSize = rawImage.Size.Width;
                    im.YSize = rawImage.Size.Height;
                    im.PixelFormat = (int)rawImage.Format;
                    _arrayOfWSQ.Add(im);

                    _arrayOfBMP.Add(rawImage.Save(Pr22.Imaging.RawImage.FileFormat.Bmp).ToByteArray());
                    //_arrayOfWSQ.Add(img.SaveToMem((int)GX_IMGFILEFORMATS.GX_WSQ));
                    rawImage = null;
                }
                else
                {
                    //list.Add(new Byte[] { new Byte() });
                    _arrayOfBMP.Add(null);
                    _arrayOfWSQ.Add(null);
                }

                if (fingerImage != null)
                    fingerImage = null;
            }
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- fpsGetFingersImages()";
            return 1305;
        }

        return 0;
    }

    public int fpsGetNist(bool saveFingersAsFile)
    {
        _nistImageBytes = null;
        try
        {
            /* This section modifies the values of nist record */

            Pr22.Util.Variant v = new Pr22.Util.Variant(0, Pr22.Util.Variant.ListT.List);	/* General list */

            /* List for storing the type-1 record data */
            Pr22.Util.Variant v1 = new Pr22.Util.Variant(1, Pr22.Util.Variant.ListT.List);
            v.List += v1;

            v1.List += new Pr22.Util.Variant(4, "ATP");	/* (field id) - (field value) */

            Pr22.Task.FingerTask ftask = Pr22.Task.FingerTask.PlainScan(1, 1);
            ftask.Add(Pr22.Imaging.FingerPosition.PlainLeft4Fingers).
                Add(Pr22.Imaging.FingerPosition.PlainRight4Fingers).
                Add(Pr22.Imaging.FingerPosition.PlainThumbs);

            /* Saves all the captured fingers */

            Pr22.Processing.BinData nist = _fps.Scanner.GetFingerCollection(ftask).
                Save(Pr22.Processing.FingerCollection.FileFormat.Nist, v);

            if (saveFingersAsFile)
                nist.Save("mynist.nist");

            _nistImageBytes = nist.ToByteArray();
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- fpsGetFingersToNist()";
            return 1305;
        }

        return 0;
    }

    public int prConnect()
    {
        if (_helper == null)
            _helper = new Helper();

        try
        {
            /* Opening the PR system */
            _pr = new Pr22.DocumentReaderDevice();	/* Object for the PR system */

            _pr.PresenceStateChanged += new EventHandler<Pr22.Events.DetectionEventArgs>(Motdet);

            /*
                        if (_pr.TestPowerState() != 0)
                        {
                            ErrorMessage = "The power is off";
                            return -1;
                        }
            */
            /* Validity check */
            /*            if (!_pr.IsValid())
                        {
                            ErrorMessage = "Failed to initialize!";
                            return 1303;
                        }
             */

            /* Connecting to the first device */
            _pr.UseDevice(0);

        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prConnect()";
            return 1305;
        }

        return 0;
    }

    void Motdet(object a, Pr22.Events.DetectionEventArgs args)
    {
        if (DeviceState != (int)Pr22.Util.PresenceState.Present)
            DeviceState = (int)args.State;

        Pr22.Control.StatusLed.Color color = Pr22.Control.StatusLed.Color.Off;
        switch (DeviceState)
        {
            case (int)Pr22.Util.PresenceState.Empty: color = Pr22.Control.StatusLed.Color.Green; break;
            case (int)Pr22.Util.PresenceState.Moving: color = Pr22.Control.StatusLed.Color.On; break;
            case (int)Pr22.Util.PresenceState.NoMove: color = Pr22.Control.StatusLed.Color.Red; break;
        }
        foreach (Pr22.Control.StatusLed statled in _pr.Peripherals.StatusLeds)
            statled.Turn(color);

    }

    public int prListen()
    {
        /* Enabling motion detection */
        try
        {
            _pr.Scanner.StartTask(Pr22.Task.FreerunTask.Detection());
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }

        try
        {
            /* If the start button is not pressed testing the document detection */
            int state = DeviceState;
            if (DeviceState == (int)Pr22.Util.PresenceState.Present) DeviceState = (int)Pr22.Util.PresenceState.NoMove;

            /* Turning the display leds depending on the status */
            _helper.Wait(200);
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prListen()";
            return 1305;
        }

        return 0;
    }

    public int prCaptureImage()
    {
        try
        {
            /* Capturing images */
            Pr22.Task.DocScannerTask sct = new Pr22.Task.DocScannerTask().Add(Pr22.Imaging.Light.All).
                Del(Pr22.Imaging.Light.CleanOVD).Del(Pr22.Imaging.Light.CleanUV);
            _pr.Scanner.Scan(sct, Pr22.Imaging.PagePosition.First);
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prCaptureImage()";
            return 1305;
        }

        return 0;
    }

    public int prCaptureMRZ()
    {
        try
        {
            /* Capturing images */
            //_pr.Capture();

            /* Getting the MRZ data */
            ndoc = _pr.Engine.Analyze(_pr.Scanner.GetPage(0).Del(Pr22.Imaging.Light.UV),
                new Pr22.Task.EngineTask().Add(Pr22.Processing.FieldSource.Mrz, Pr22.Processing.FieldId.All));

            if (ndoc.GetFields().Count == 0)
                throw new NoDocumentFoundException("No MRZ data found");
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (NoDocumentFoundException e)
        {
            _errorMessage = e.Message;
            return 1303;
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prCaptureMRZ()";
            return 1305;
        }

        return 0;
    }

    public int prGetMRZData()
    {
        return 0;
    }

    public int prGetMRZData(System.Collections.IList list)
    {
        try
        {
            System.Collections.Generic.List<Pr22.Processing.FieldReference> fields;
            fields = ndoc.GetFields(new Pr22.Task.EngineTask().Add(Pr22.Processing.FieldSource.Mrz, Pr22.Processing.FieldId.All));

            string text;
            foreach (Pr22.Processing.FieldReference fref in fields)
            {
                text = ndoc.GetField(fref).GetRawStringValue();
                text = text.Replace('<', ' ').Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    text = fref.Id.ToString() + ": " + text;
                    list.Add(text);
                }
            }

        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prGetMRZData()";
            return 1305;
        }

        return 0;
    }

    public int prCaptureBarcode()
    {
        try
        {
            /* Capturing images */
            //_pr.Capture();

            /* Reading barcode from infra image */
            ndoc = _pr.Engine.Analyze(_pr.Scanner.GetPage(0).Del(Pr22.Imaging.Light.UV),
                new Pr22.Task.EngineTask().Add(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.All));

            if (ndoc.GetFields().Count == 0)
                throw new NoDocumentFoundException("No barcode found");
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (NoDocumentFoundException e)
        {
            _errorMessage = e.Message;
            return 1303;
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prCaptureBarcode()";
            return 1305;
        }

        return 0;
    }

    public int prGetGTIN()
    {
        return 0;
    }

    public int prGetGTIN(out string gtin, out string barcodeType)
    {
        gtin = ""; barcodeType = "";

        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                int type = -1;

                type = ndoc.ToVariant().GetChild((int)Pr22.Util.VariantId.Barcode, 0).ToInt();

                barcodeType = ((BarcodeTypes)type).ToString();

                gtin = ndoc.GetField(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.Composite1).GetBasicStringValue() as string;
            }
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prGetBarcodeData()";
            return 1305;
        }

        return 0;
    }

    public int prGetBarcodeData()
    {
        return 0;
    }

    public int prGetBarcodeData(System.Collections.IList list)
    {
        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                /* Searching for the barcode and displaying it */
                int type = -1;

                type = ndoc.ToVariant().GetChild((int)Pr22.Util.VariantId.Barcode, 0).ToInt();

                string barcodeType = ((BarcodeTypes)type).ToString();
                list.Add(String.Format("TYPE: {0}", barcodeType));      //barcode type
                list.Add(String.Format( // checksum
                    "CHECKSUM: {0}",
                    ndoc.GetField(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.Composite1).GetStatus() == Pr22.Processing.Status.Ok ? "Ok" : "No checksum"));

                if (barcodeType == "PDF417")
                {
                    System.Collections.Generic.List<Pr22.Processing.FieldReference> fields;
                    fields = ndoc.GetFields(new Pr22.Task.EngineTask().Add(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.All));

                    string text;
                    int j = "PR_DF_".Length;
                    foreach (Pr22.Processing.FieldReference fref in fields)
                    {
                        try
                        {
                            text = ndoc.GetField(fref).GetFormattedStringValue().Trim();
                            if (!string.IsNullOrEmpty(text))
                            {
                                text = fref.Id.ToString() + ": " + text;
                                list.Add(text);
                            }
                        }
                        catch (Pr22.Exceptions.General) { }
                    }
                }
                else
                    list.Add("DATA: " + ndoc.GetField(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.Composite1).GetBasicStringValue() as string);
            }
        }
        catch (Pr22.Exceptions.General e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prGetBarcodeData()";
            return 1305;
        }

        return 0;
    }

    public int prGetBarcodeImage()
    {
        return 0;
    }

    public int prGetBarcodeImage(out byte[] buff)
    {
        buff = null;

        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                /* Creating a barcode image */
                buff = ndoc.GetField(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.Composite1).GetImage()
                .Save(Pr22.Imaging.RawImage.FileFormat.Jpeg).ToByteArray();
            }
        }
        catch (Pr22.Exceptions.General) { }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prGetBarcodeImage()";
            return 1305;
        }

        return 0;
    }

    public int prGetMRZImage()
    {
        return 0;
    }

    public int prGetMRZImage(out byte[] buff)
    {
        buff = null;

        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                /* Creating a MRZ image */
                buff = ndoc.GetField(Pr22.Processing.FieldSource.Mrz, Pr22.Processing.FieldId.All).GetImage().
                    Save(Pr22.Imaging.RawImage.FileFormat.Jpeg).ToByteArray();
            }
        }
        catch (Pr22.Exceptions.General) { }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prGetMRZImage()";
            return 1305;
        }

        return 0;
    }

    public int prSaveBarcodeImage()
    {
        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                /* Saving the barcode image */
                ndoc.GetField(Pr22.Processing.FieldSource.Barcode, Pr22.Processing.FieldId.All).GetImage().
                    Save(Pr22.Imaging.RawImage.FileFormat.Jpeg).Save("barcode.jpg");
                //_doc.Free();
                //_doc = null;
                //_pr.ResetDocument();
            }
        }
        catch (Pr22.Exceptions.General) { }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prSaveBarcodeImage()";
            return 1305;
        }

        return 0;
    }

    public int prSaveMRZImage()
    {
        try
        {
            if (ndoc.GetFields().Count != 0)
            {
                /* Saving the MRZ image */
                ndoc.GetField(Pr22.Processing.FieldSource.Mrz, Pr22.Processing.FieldId.All).GetImage().
                    Save(Pr22.Imaging.RawImage.FileFormat.Jpeg).Save("mrz.jpg");
                //_doc.Free();
                //_doc = null;
                //_pr.ResetDocument();
            }
        }
        catch (Pr22.Exceptions.General) { }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prSaveMRZImage()";
            return 1305;
        }

        return 0;
    }

    public int prReleaseDocument()
    {
        try
        {
            if (ndoc != null)
            {
                ndoc.Dispose();
                ndoc = null;
            }
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prReleaseDocument()";
            return 1305;
        }

        return 0;
    }

    public int fpsDisconnect()
    {
        /* Closing the device */
        while (_fps != null)
        {
            try
            {
                _fps.Close();
                _fps.Dispose();
                _fps = null;

                break;
            }
            catch (Exception ex)
            {
                //if (gxSystem.GetErrorCode() == (int)GX_ERROR_CODES.GX_EBUSY)
                continue;
            }
        }
        return 0;
    }

    public int prDisconnect()
    {
        /* Closing the device */
        while (_pr != null)
        {
            try
            {
                if (ndoc != null)
                {
                    ndoc.Dispose();
                    ndoc = null;
                }

                _pr.Scanner.CleanUpData();

                _pr.Close();
                _pr.Dispose();
                _pr = null;

                break;
            }
            catch (Exception)
            {
                //if (gxSystem.GetErrorCode() == (int)GX_ERROR_CODES.GX_EBUSY)
                continue;
            }
        }
        return 0;
    }

    static Pr22.Imaging.RawImage _wsqImg = null;

    public static byte[] ConvertWSQToBmp(WsqImage wsq)
    {
        _wsqImg = new Pr22.Imaging.RawImage();
        _wsqImg.Load(new Pr22.Processing.BinData(wsq.Content));
        return _wsqImg.Save(Pr22.Imaging.RawImage.FileFormat.Bmp).ToByteArray();
    }

    public static void DisposeWSQImage()
    {
        _wsqImg = null;
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

