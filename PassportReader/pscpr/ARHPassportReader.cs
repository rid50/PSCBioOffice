using System;
using gx;
using pr;

class ARHPassportReader
{
    PassportReader _pr = null;
    Helper _helper;
    prDoc _doc;

    string _errorMessage;
    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }

    public int DeviceState { get; set; }

    public int prConnect()
    {
        _helper = new Helper();

        try
        {
            /* Opening the PR system */
            _pr = new PassportReader();	/* Object for the PR system */
/*
            if (_pr.TestPowerState() != 0)
            {
                ErrorMessage = "The power is off";
                return -1;
            }
*/
            /* Validity check */
            if (!_pr.IsValid())
            {
                ErrorMessage = "Failed to initialize!";
                return 1303;
            }

            /* Connecting to the first device */
            _pr.UseDevice(0, (int)PR_USAGEMODE.PR_UMODE_FULL_CONTROL);

        }
        catch (gxException e)
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

    public int prListen()
    {
        /* Enabling motion detection */
        try
        {
            _pr.SetProperty("freerun_mode", (int)PR_FREERUNMODE.PR_FRMODE_TESTDOCUMENT);
        }
        catch (gxException e)
        {
            return _helper.GetErrorMessage(e, out _errorMessage);
        }

        try
        {
            /* If the start button is not pressed testing the document detection */
            int state = _pr.TestDocument(0);

            /* Turning the display leds depending on the status */
            int color = (int)PR_STATUS_LED_COLOR.PR_SLC_OFF;
            switch (state)
            {
                case (int)PR_TESTDOC.PR_TD_OUT: color = (int)PR_STATUS_LED_COLOR.PR_SLC_GREEN; break;
                case (int)PR_TESTDOC.PR_TD_MOVE: color = (int)PR_STATUS_LED_COLOR.PR_SLC_ANY; break;
                case (int)PR_TESTDOC.PR_TD_NOMOVE: color = (int)PR_STATUS_LED_COLOR.PR_SLC_RED; break;
            }
            _pr.SetStatusLed(0xff, color);

            DeviceState = state;

            _helper.Wait(200);
        }
        catch (gxException e)
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
            _pr.Capture();
        }
        catch (gxException e)
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
            _doc = _pr.GetMrz(0, (int)PR_LIGHT.PR_LIGHT_INFRA, (int)PR_IMAGE_TYPE.PR_IT_ORIGINAL);

            if (!_doc.IsValid())
                throw new NoDocumentFoundException("No MRZ data found");
        }
        catch (gxException e)
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
            if (_doc.IsValid())
            {
                string fieldName, text;
                int j = "PR_DF_MRZ_".Length;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (int i in Enum.GetValues(typeof(PR_DOCFIELD)))
                {
                    if (i <= (int)PR_DOCFIELD.PR_DF_MRZ_FIELDS)
                        continue;

                    fieldName = Enum.GetName(typeof(PR_DOCFIELD), i);
                    if (fieldName.StartsWith("PR_DF_MRZ_"))
                    {
                        text = _doc.Field(i);
                        text = text.Replace('<', ' ').Trim();
                        if (!string.IsNullOrEmpty(text))
                        {
                            text = fieldName.Substring(j).Replace('_', ' ') + ": " + text;
                            list.Add(text);
                        }
                    }
                }
            }
        }
        catch (gxException e)
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
            _doc = _pr.GetBarcode(0, (int)PR_LIGHT.PR_LIGHT_INFRA, (int)PR_IMAGE_TYPE.PR_IT_ORIGINAL, 0, 0);

            if (!_doc.IsValid())
            {
                /* Reading barcode from white image */
                _doc = _pr.GetBarcode(0, (int)PR_LIGHT.PR_LIGHT_WHITE, (int)PR_IMAGE_TYPE.PR_IT_ORIGINAL, 0, 0);

                //                bool statusOk = _doc.FieldStatus((int)PR_DOCFIELD.PR_DF_BC1) == 0;
                //              if (!statusOk)
                //                _doc = _pr.GetBarcode(0, (int)PR_LIGHT.PR_LIGHT_UV, (int)PR_IMAGE_TYPE.PR_IT_ORIGINAL, 0, 0);
            }

            if (!_doc.IsValid())
                throw new NoDocumentFoundException("No barcode found");
        }
        catch (gxException e)
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
            if (_doc.IsValid())
            {
                int type = -1;

                gxVariant pdoc = _doc.ToVariant();
                gxVariant v = new gxVariant();
                if (pdoc.GetChild(v, (int)GX_VARIANT_FLAGS.GX_VARIANT_BY_ID, (int)PR_VAR_ID.PRV_BARCODE, 0))
                {
                    type = v.GetInt();
                    v.Dispose();
                }

                barcodeType = System.Enum.GetName(typeof(PR_BCTYPE), type);
                barcodeType = barcodeType.Substring(barcodeType.LastIndexOf("_") + 1);

                gtin = _doc.Field((int)PR_DOCFIELD.PR_DF_BC1) as string;
            }
        }
        catch (gxException e)
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
            if (_doc.IsValid())
            {
                /* Searching for the barcode and displaying it */
                int type = -1;

                gxVariant pdoc = _doc.ToVariant();
                gxVariant v = new gxVariant();
                if (pdoc.GetChild(v, (int)GX_VARIANT_FLAGS.GX_VARIANT_BY_ID, (int)PR_VAR_ID.PRV_BARCODE, 0))
                {
                    type = v.GetInt();
                    v.Dispose();
                }

                string barcodeType = System.Enum.GetName(typeof(PR_BCTYPE), type);
                barcodeType = barcodeType.Substring(barcodeType.LastIndexOf("_") + 1);
                list.Add(String.Format("TYPE: {0}", barcodeType));      //barcode type
                list.Add(String.Format( // checksum
                    "CHECKSUM: {0}", _doc.FieldStatus((int)PR_DOCFIELD.PR_DF_BC1) == 0 ? "Ok" : "No checksum"));

                if (barcodeType == "PDF417")
                {
                    string fieldName, text;
                    int j = "PR_DF_".Length;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (int i in Enum.GetValues(typeof(PR_DOCFIELD)))
                    {
                        if (i <= (int)PR_DOCFIELD.PR_DF_FORMATTED)
                            continue;

                        fieldName = Enum.GetName(typeof(PR_DOCFIELD), i);
                        if (fieldName.StartsWith("PR_DF_"))
                        {
                            text = _doc.Field(i).Trim();
                            if (!string.IsNullOrEmpty(text))
                            {
                                text = fieldName.Substring(j).Replace('_', ' ') + ": " + text;
                                list.Add(text);
                            }
                        }
                    }
                }
                else
                    list.Add("DATA: " + _doc.Field((int)PR_DOCFIELD.PR_DF_BC1) as string);
            }
        }
        catch (gxException e)
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
            if (_doc.IsValid())
            {
                /* Creating a barcode image */
                gxImage img = _doc.FieldImage((int)PR_DOCFIELD.PR_DF_BC1);
                if (img.IsValid())
                {
                    buff = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_JPEG);
                }
            }
        }
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
            if (_doc.IsValid())
            {
                /* Creating a MRZ image */
                gxImage img = _doc.FieldImage((int)(PR_DOCFIELD.PR_DF_MRZ1 & PR_DOCFIELD.PR_DF_MRZ2));
                if (img.IsValid())
                {
                    buff = img.SaveToMem((int)GX_IMGFILEFORMATS.GX_JPEG);
                }
            }
        }
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
            if (_doc.IsValid())
            {
                /* Saving the barcode image */
                gxImage img = _doc.FieldImage((int)PR_DOCFIELD.PR_DF_BC1);
                if (img.IsValid())
                    img.Save("barcode.jpg", (int)GX_IMGFILEFORMATS.GX_JPEG);

                //_doc.Free();
                //_doc = null;
                //_pr.ResetDocument();
            }
        }
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
            if (_doc.IsValid())
            {
                /* Saving the MRZ image */
                gxImage img = _doc.FieldImage((int)(PR_DOCFIELD.PR_DF_MRZ1 & PR_DOCFIELD.PR_DF_MRZ2));
                if (img.IsValid())
                    img.Save("mrz.jpg", (int)GX_IMGFILEFORMATS.GX_JPEG);

                //_doc.Free();
                //_doc = null;
                //_pr.ResetDocument();
            }
        }
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
            if (_doc != null && _doc.IsValid())
            {
                _doc.Free();
                _doc = null;
            }
        }
        catch (Exception e)
        {
            _errorMessage = e.Message + " --- prReleaseDocument()";
            return 1305;
        }

        return 0;
    }

    public int prDisconnect()
    {
        while (_pr != null)
        {
            try
            {
                if (_doc != null)
                    _doc.Free();

                _pr.ResetDocument(0);

                _pr.CloseDevice();
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
}

