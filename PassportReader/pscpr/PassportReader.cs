using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace pscpr
{
    public class PassportReader
    {
        private ARHPassportReader _sc = null;
        private IList _list = null;
        private byte[] _barcodeImageBytes = null;
        private byte[] _MRZImageBytes = null;

        bool protect = false;

        public PassportReader()
        {
            _sc = new ARHPassportReader();
        }

        public int connect()
        {
            if (protect)
                return 0;

            int errorCode = 0;

            if ((errorCode = _sc.prConnect()) != 0)
                return errorCode;
            else
                return 0;
        }

        public int readBarcode()
        {
            if (protect)
                return 0;
            try
            {
                if (_list != null) { _list.Clear(); _list = null; }
                if (_barcodeImageBytes != null) { _barcodeImageBytes = null; }

                int errorCode = 0;

                TimeSpan timeout = new TimeSpan();
                timeout.Add(TimeSpan.FromSeconds(30.0));    // 30 seconds timeout

                while (true)
                {
                    if ((errorCode = _sc.prListen()) != 0)
                        return errorCode;

                    if (_sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_OUT && timeout < new TimeSpan())
                    {
                        _sc.ErrorMessage = "No document found";
                        return 1303;
                    }
                    else if (_sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_NOMOVE)
                        break;
                }

                if ((errorCode = _sc.prCaptureImage()) != 0)
                    return errorCode;

                if ((errorCode = _sc.prCaptureBarcode()) != 0)
                    return errorCode;

                _list = new ArrayList();
                if ((errorCode = _sc.prGetBarcodeData(_list)) != 0)
                    return errorCode;

                if ((errorCode = _sc.prGetBarcodeImage(out _barcodeImageBytes)) != 0)
                    return errorCode;

//                if ((errorCode = _fps.prReleaseDocument()) != 0)
//                    return errorCode;

/*
                byte[] buff = null;
                if ((errorCode = _fps.prGetBarcodeImage(out buff)) != 0)
                    return errorCode;

                if (buff != null)
                {
                    MemoryStream stream = new MemoryStream(buff);
                    _barcodeImageBytes = Image.FromStream(stream);
                }
*/
            }
            catch (Exception e)
            {
                _sc.ErrorMessage = e.Message + " --- readBarcode()";
                return 1305;
            }

            return 0;
        }

        public int readMRZ()
        {
            if (protect)
                return 0;

            try
            {
                if (_list != null) { _list.Clear(); _list = null; }
                if (_MRZImageBytes != null) { _MRZImageBytes = null; }

                int errorCode = 0;

                TimeSpan timeout = new TimeSpan();
                timeout.Add(TimeSpan.FromSeconds(30.0));    // 30 seconds timeout

                while (true)
                {
                    if ((errorCode = _sc.prListen()) != 0)
                        return errorCode;

                    if (_sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_OUT && timeout < new TimeSpan())
                    {
                        _sc.ErrorMessage = "No document found";
                        return 1303;
                    }
                    else if (_sc.DeviceState == (int)pr.PR_TESTDOC.PR_TD_NOMOVE)
                        break;
                }

                if ((errorCode = _sc.prCaptureImage()) != 0)
                    return errorCode;

                if ((errorCode = _sc.prCaptureMRZ()) != 0)
                    return errorCode;

                _list = new ArrayList();
                if ((errorCode = _sc.prGetMRZData(_list)) != 0)
                    return errorCode;

                if ((errorCode = _sc.prGetMRZImage(out _MRZImageBytes)) != 0)
                    return errorCode;
                /*
                                byte[] buff = null;
                                if ((errorCode = _fps.prGetMRZImage(out buff)) != 0)
                                    return errorCode;

                                if (buff != null)
                                {
                                    MemoryStream stream = new MemoryStream(buff);
                                    _MRZImageBytes = Image.FromStream(stream);
                                }
                */

//                if ((errorCode = _fps.prReleaseDocument()) != 0)
//                    return errorCode;
            }
            catch (Exception e)
            {
                _sc.ErrorMessage = e.Message + " --- readMRZ()";
                return 1305;
            }

            return 0;
        }

        public int disconnect()
        {
            if (protect)
                return 0;

            return _sc.prDisconnect();
        }

        public string ErrorMessage
        {
            get
            {
                return _sc.ErrorMessage;
            }
        }

        public IList Data
        {
            get
            {
                return _list;
            }
        }

        public byte[] BarcodeImageBytes
        {
            get
            {
                return _barcodeImageBytes;
            }
        }

        public byte[] MRZImageBytes
        {
            get
            {
                return _MRZImageBytes;
            }
        }
    }
}
