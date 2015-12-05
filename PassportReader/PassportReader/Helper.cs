/******************************************************************************/
/* Library functions for the pr sample programs.                              */
/******************************************************************************/
using gx;
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
    public int GetErrorMessage(gxException e, out string errorMessage)
    {
        errorMessage = string.Empty;

        if (gxSystem.GetErrorCode() != 0)
        {
            errorMessage = gxSystem.GetErrorString();
            return gxSystem.GetErrorCode();
        }
        else
            return 0;
    }

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
}

class NoDocumentFoundException : System.Exception
{
    public NoDocumentFoundException(string message) : base(message)
    {
    }
}