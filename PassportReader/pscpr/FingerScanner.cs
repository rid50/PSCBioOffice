using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace pscpr
{
    public class FingerScanner
    {
        private ARHFingerScanner _fps = null;
        private IList _arrayOfBMP = null;
        private IList _arrayOfWSQ = null;
        private byte[] _nistImageBytes = null;

        bool protect = false;

        public FingerScanner()
        {
            _fps = new ARHFingerScanner();
        }

        public int connect()
        {
            if (protect)
                return 0;

            int errorCode = 0;

            if ((errorCode = _fps.fpsConnect()) != 0)
                return errorCode;
            else
                return 0;
        }

        public int getFingersImages(int handAndFingerMask, bool saveFingerAsFile)
        {
            if (protect)
                return 0;

            int errorCode = 0;
            if (_arrayOfBMP != null) { _arrayOfBMP.Clear(); }
            if (_arrayOfWSQ != null) { _arrayOfWSQ.Clear(); }

            try
            {
                _arrayOfBMP = new ArrayList();
                _arrayOfWSQ = new ArrayList();
                if ((errorCode = _fps.fpsGetFingersImages(_arrayOfBMP, _arrayOfWSQ, handAndFingerMask, saveFingerAsFile)) != 0)
                    return errorCode;
            }
            catch (Exception e)
            {
                _fps.ErrorMessage = e.Message + " --- fpsGetFingersImages()";
                return 1305;
            }

            return 0;
        }

        public int getNist()
        {
            if (protect)
                return 0;

            int errorCode = 0;
            if (_nistImageBytes != null) { _nistImageBytes = null; }

            try
            {
                if ((errorCode = _fps.fpsGetNist(out _nistImageBytes, false)) != 0)
                    return errorCode;
            }
            catch (Exception e)
            {
                _fps.ErrorMessage = e.Message + " --- fpsGetNist()";
                return 1305;
            }

            return 0;
        }

        public byte[] ConvertWSQToBmp(WsqImage wsq)
        {
            return ARHFingerScanner.ConvertWSQToBmp(wsq);
        }

        public void DisposeWSQImage()
        {
            ARHFingerScanner.DisposeWSQImage();
        }

        public int disconnect()
        {
            if (protect)
                return 0;

            return _fps.fpsDisconnect();
        }

        public string ErrorMessage
        {
            get
            {
                return _fps.ErrorMessage;
            }
        }

        public IList ArrayOfWSQ
        {
            get
            {
                return _arrayOfWSQ;
            }
        }

        public IList ArrayOfBMP
        {
            get
            {
                return _arrayOfBMP;
            }
        }

        public byte[] NistImageBytes
        {
            get
            {
                return _nistImageBytes;
            }
        }
    }
}
