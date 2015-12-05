using System;
using gx;
using fps;
using System.Collections;
using System.IO;

class ARHFingerScanner
{
    FingerPrintScanner _fps = null;
    Helper _helper = null;

    string _errorMessage;
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }

    public int DeviceState { get; set; }

    public int fpsConnect()
    {
        if (_helper == null)
            _helper = new Helper();

        try
        {
            fpsDisconnect();

            /* Opening the FPS system */
            _fps = new FingerPrintScanner();	/* Object for the FPS system */
            /*
                        if (_fps.TestPowerState() != 0)
                        {
                            ErrorMessage = "The power is off";
                            return -1;
                        }
            */
            /* Validity check */
            if (!_fps.IsValid())
            {
                ErrorMessage = "Failed to initialize!";
                return 1303;
            }

            /* Connecting to the first device */
            _fps.UseDevice(0, (int)FPS_USAGEMODE.FPS_UMODE_FULL_CONTROL);


        }
        catch (gxException e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- fpsConnect()";
            return 1305;
        }

        return 0;
    }

    public int fpsGetFingersImages(IList _arrayOfBMP, IList _arrayOfWSQ, int handAndFingerMask, bool saveFingerAsFile)
    {
        try
        {
            //if (_arrayOfBMP != null) { _arrayOfBMP.Clear(); _arrayOfBMP = null; }
            //if (_arrayOfWSQ != null) { _arrayOfWSQ.Clear(); _arrayOfWSQ = null; }

            /* Search Finger */
            int reqid, stat;

            /* Clears internal stored finger buffers */
            _fps.ResetFingerList();


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

            int color = (int)FPS_STATUS_LED_COLOR.FPS_SLC_OFF;
            int index = 0;
            int middle = 0;
            int ring = 0;
            int little = 0;
            string wsqIndexFileName = String.Empty;
            string wsqMiddleFileName = String.Empty;
            string wsqRingFileName = String.Empty;
            string wsqLittleFileName = String.Empty;

            _fps.SetStatusLed(0xff, color); // off
            color = (int)FPS_STATUS_LED_COLOR.FPS_SLC_GREEN;

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
                    index = (int)FPS_POSITION.FPS_POS_LEFT_INDEX;
                    middle = (int)FPS_POSITION.FPS_POS_LEFT_MIDDLE;
                    ring = (int)FPS_POSITION.FPS_POS_LEFT_RING;
                    little = (int)FPS_POSITION.FPS_POS_LEFT_LITTLE;
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
                    index = (int)FPS_POSITION.FPS_POS_RIGHT_INDEX;
                    middle = (int)FPS_POSITION.FPS_POS_RIGHT_MIDDLE;
                    ring = (int)FPS_POSITION.FPS_POS_RIGHT_RING;
                    little = (int)FPS_POSITION.FPS_POS_RIGHT_LITTLE;
                    wsqIndexFileName = "rindex.wsq";
                    wsqMiddleFileName = "rmiddle.wsq";
                    wsqRingFileName = "rring.wsq";
                    wsqLittleFileName = "rlittle.wsq";
                    lampMask |= 0x40;
                    break;
                case 0x30000000:    //0x30000033
                    middle = (int)FPS_POSITION.FPS_POS_LEFT_THUMB;
                    ring = (int)FPS_POSITION.FPS_POS_RIGHT_THUMB;
                    wsqMiddleFileName = "lthumb.wsq";
                    wsqRingFileName = "rthumb.wsq";
                    lampMask |= 0x20;
                    break;
            }

            /* Turning the display leds depending on the mask */
            _fps.SetStatusLed(lampMask, color);

            //reqid = _fps.CaptureStart(100, 100, (int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, 0x10333300);
            reqid = _fps.CaptureStart(3000, 700, (int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, handAndFingerMask);

            for (stat = 0; stat < 100; )
            {
                /* Test if better images are captured or capture has accomplished */
                stat = _fps.CaptureStatus(reqid);

                _helper.Wait(100);
            }

            /* Closing the capture sequence */
            _fps.CaptureWait(reqid);

            color = (int)FPS_STATUS_LED_COLOR.FPS_SLC_OFF;
            _fps.SetStatusLed(0xff, color); // off

            /* Save individual finger images */
            gxImage img;
            gxVariant var = null;
            int mask = 0x10;

            //_arrayOfBMP = new ArrayList();
            //_arrayOfWSQ = new ArrayList();

            for (int i = 0; i < 4; i++)
            {
                mask >>= 1;
                bool valid = true;
                switch (fingerMask & mask)
                {
                    case 0x08:
                        try
                        {
                            var = _fps.GetImage((int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, index, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER);
                            if (saveFingerAsFile)
                                _fps.SaveImage(0, index, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER, wsqIndexFileName, (int)GX_IMGFILEFORMATS.GX_WSQ);
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
                            var = _fps.GetImage((int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, middle, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER);
                            if (saveFingerAsFile)
                                _fps.SaveImage(0, middle, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER, wsqMiddleFileName, (int)GX_IMGFILEFORMATS.GX_WSQ);
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
                            var = _fps.GetImage((int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, ring, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER);
                            if (saveFingerAsFile)
                                _fps.SaveImage(0, ring, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER, wsqRingFileName, (int)GX_IMGFILEFORMATS.GX_WSQ);
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
                            var = _fps.GetImage((int)FPS_IMPRESSION_TYPE.FPS_SCAN_LIVE, little, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER);
                            if (saveFingerAsFile)
                                _fps.SaveImage(0, little, (int)FPS_IMAGE_TYPE.FPS_IT_FINGER, wsqLittleFileName, (int)GX_IMGFILEFORMATS.GX_WSQ);
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
                        continue;
                }

                if (valid)
                {
                    img = new gxImage();
                    gxVariant vtest = new gxVariant();
                    img.FromVariant(var);

                    WsqImage im = new WsqImage();
                    im.Content = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_WSQ);
                    //im.Content = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_BMP);
                    im.XRes = img.xres() / (10000 / 254);
                    im.YRes = img.yres() / (10000 / 254);
                    im.XSize = img.xsize();
                    im.YSize = img.ysize();
                    im.PixelFormat = img.format();
                    _arrayOfWSQ.Add(im);

                    _arrayOfBMP.Add(img.SaveToMem((int)GX_IMGFILEFORMATS.GX_BMP));
                    //_arrayOfWSQ.Add(img.SaveToMem((int)GX_IMGFILEFORMATS.GX_WSQ));
                    img.Dispose();
                }
                else
                {
                    //list.Add(new Byte[] { new Byte() });
                    _arrayOfBMP.Add(null);
                    _arrayOfWSQ.Add(null);
                }

                if (var != null)
                    var.Dispose();
            }
        }
        catch (gxException e)
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

    public int fpsGetNist(out byte[] _nistImageBytes, bool saveFingersAsFile)
    {
        _nistImageBytes = null;
        try
        {
            /* This section modifies the values of nist record */

            gxVariant v = new gxVariant();
            gxVariant v1 = new gxVariant();
            gxVariant v2 = new gxVariant();

            v.CreateEmptyList(0);	/* General list */

            v1.CreateEmptyList(1);		/* List for storing the type-1 record data */
            v.AddItem((int)GX_VARIANT_FLAGS.GX_VARIANT_LAST, 0, 0, v1);

            v2.Create(4, "ATP");	/* (field id) - (field value) */
            v1.AddItem((int)GX_VARIANT_FLAGS.GX_VARIANT_LAST, 0, 0, v2);

            /* Saves all the captured fingers */
            if (saveFingersAsFile)
                _fps.FingerToNist("mynist.nist", v);

            _nistImageBytes = _fps.FingerToNistMem(v);
        }
        catch (gxException e)
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

    public int fpsDisconnect()
    {
        /* Closing the device */
        while (_fps != null)
        {
            try
            {
                _fps.CloseDevice();
                _fps.Dispose();
                _fps = null;

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

    static gxImage _wsqImg = null;

    public static byte[] ConvertWSQToBmp(WsqImage wsq)
    {
        _wsqImg = new gxImage();
        _wsqImg.Create(wsq.PixelFormat, wsq.XSize, wsq.YSize, 0);
        _wsqImg.xres(wsq.XRes);
        _wsqImg.yres(wsq.YRes);
        _wsqImg.LoadFromMem(wsq.Content, wsq.PixelFormat);
        return _wsqImg.SaveToMem((int)GX_IMGFILEFORMATS.GX_BMP);
    }

    public static void DisposeWSQImage() {
        _wsqImg.Dispose();
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

