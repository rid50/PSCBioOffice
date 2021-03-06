/******************************************************************************/
/* Library functions for the pr sample programs.                              */
/******************************************************************************/
//using gx;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Drawing.Imaging;

/******************************************************************************/
class Helper
{
    /**************************************************************************/
    /* Set the code and description of the underlying GX exception.      */
    /**************************************************************************/
    //public int GetErrorMessage(gxException e, out string errorMessage)
    //{
    //    errorMessage = string.Empty;

    //    if (gxSystem.GetErrorCode() != 0)
    //    {
    //        errorMessage = gxSystem.GetErrorString();
    //        return gxSystem.GetErrorCode();
    //    }
    //    else
    //        return 0;
    //}

    /**************************************************************************/
    /* Waits for a time specified in miliseconds.                             */
    /**************************************************************************/
    public void Wait(int ms)
    {
        System.Threading.Thread.Sleep(ms);
    }

    public Bitmap GenerateBarcodeImage(string text, FontFamily fontFamily, int fontSizeInPoints)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("*");
        sb.Append(text);
        sb.Append("*");

        Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        Font font = new Font(fontFamily, fontSizeInPoints, FontStyle.Regular, GraphicsUnit.Point);

        Graphics graphics = Graphics.FromImage(bmp);

        SizeF textSize = graphics.MeasureString(sb.ToString(), font);

        bmp = new Bitmap(bmp, textSize.ToSize());
        
        graphics = Graphics.FromImage(bmp);
        graphics.Clear(Color.White);
        graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;
        graphics.DrawString(sb.ToString(), font, new SolidBrush(Color.Black), 0, 0);
        graphics.Flush();

        font.Dispose();
        graphics.Dispose();

        return bmp;
    }
	
    public int GetErrorMessage(Pr22.Exceptions.General e, out string errorMessage)
    {
        errorMessage = e.Message;
        if (e.GetType() == typeof(Pr22.Exceptions.EntryNotFound)) return (int)Pr22.Exceptions.ErrorCodes.ENOENT;
        if (e.GetType() == typeof(Pr22.Exceptions.MemoryAllocation)) return (int)Pr22.Exceptions.ErrorCodes.ENOMEM;
        if (e.GetType() == typeof(Pr22.Exceptions.PermissionDenied)) return (int)Pr22.Exceptions.ErrorCodes.EACCES;
        if (e.GetType() == typeof(Pr22.Exceptions.ProgramFault)) return (int)Pr22.Exceptions.ErrorCodes.EFAULT;
        if (e.GetType() == typeof(Pr22.Exceptions.ResourceBusy)) return (int)Pr22.Exceptions.ErrorCodes.EBUSY;
        if (e.GetType() == typeof(Pr22.Exceptions.FileExists)) return (int)Pr22.Exceptions.ErrorCodes.EEXIST;
        if (e.GetType() == typeof(Pr22.Exceptions.NoSuchDevice)) return (int)Pr22.Exceptions.ErrorCodes.ENODEV;
        if (e.GetType() == typeof(Pr22.Exceptions.InvalidParameter)) return (int)Pr22.Exceptions.ErrorCodes.EINVAL;
        if (e.GetType() == typeof(Pr22.Exceptions.DataOutOfRange)) return (int)Pr22.Exceptions.ErrorCodes.ERANGE;
        if (e.GetType() == typeof(Pr22.Exceptions.NoDataAvailable)) return (int)Pr22.Exceptions.ErrorCodes.EDATA;
        if (e.GetType() == typeof(Pr22.Exceptions.CommunicationError)) return (int)Pr22.Exceptions.ErrorCodes.ECOMM;
        if (e.GetType() == typeof(Pr22.Exceptions.FunctionTimedOut)) return (int)Pr22.Exceptions.ErrorCodes.ETIMEDOUT;
        if (e.GetType() == typeof(Pr22.Exceptions.InvalidImage)) return (int)Pr22.Exceptions.ErrorCodes.EINVIMG;
        if (e.GetType() == typeof(Pr22.Exceptions.InvalidFunction)) return (int)Pr22.Exceptions.ErrorCodes.EINVFUNC;
        if (e.GetType() == typeof(Pr22.Exceptions.HardwareKey)) return (int)Pr22.Exceptions.ErrorCodes.EHWKEY;
        if (e.GetType() == typeof(Pr22.Exceptions.InvalidVersion)) return (int)Pr22.Exceptions.ErrorCodes.EVERSION;
        if (e.GetType() == typeof(Pr22.Exceptions.AssertionOccurred)) return (int)Pr22.Exceptions.ErrorCodes.EASSERT;
        if (e.GetType() == typeof(Pr22.Exceptions.DeviceIsDisconnected)) return (int)Pr22.Exceptions.ErrorCodes.EDISCON;
        if (e.GetType() == typeof(Pr22.Exceptions.ImageProcessingFailed)) return (int)Pr22.Exceptions.ErrorCodes.EIMGPROC;
        if (e.GetType() == typeof(Pr22.Exceptions.AuthenticityFailed)) return (int)Pr22.Exceptions.ErrorCodes.EAUTH;
        if (e.GetType() == typeof(Pr22.Exceptions.FileOpen)) return (int)Pr22.Exceptions.ErrorCodes.EOPEN;
        if (e.GetType() == typeof(Pr22.Exceptions.FileCreation)) return (int)Pr22.Exceptions.ErrorCodes.ECREAT;
        if (e.GetType() == typeof(Pr22.Exceptions.FileRead)) return (int)Pr22.Exceptions.ErrorCodes.EREAD;
        if (e.GetType() == typeof(Pr22.Exceptions.FileWrite)) return (int)Pr22.Exceptions.ErrorCodes.EWRITE;
        if (e.GetType() == typeof(Pr22.Exceptions.InvalidFileContent)) return (int)Pr22.Exceptions.ErrorCodes.EFILE;
        if (e.GetType() == typeof(Pr22.Exceptions.ImageScanFailed)) return (int)Pr22.Exceptions.ErrorCodes.ECAPTURE;
        if (e.GetType() == typeof(Pr22.Exceptions.InsufficientHardware)) return (int)Pr22.Exceptions.ErrorCodes.EWEAKDEV;
        if (e.GetType() == typeof(Pr22.Exceptions.CertificateExpired)) return (int)Pr22.Exceptions.ErrorCodes.CERT_EXPIRED;
        if (e.GetType() == typeof(Pr22.Exceptions.CertificateRevoked)) return (int)Pr22.Exceptions.ErrorCodes.CERT_REVOKED;
        if (e.GetType() == typeof(Pr22.Exceptions.ValidationCheckingFailed)) return (int)Pr22.Exceptions.ErrorCodes.ECHECK;
        return -1;
    }

}

class NoDocumentFoundException : System.Exception
{
    public NoDocumentFoundException(string message) : base(message)
    {
    }
}