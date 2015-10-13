using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace PSCBioVerification
{
    class MyPictureBox : PictureBox
    {
        public MyPictureBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true); 
        }

        public bool Active {get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //if (this.Name == "fpPictureBox1")
            {
                Image image = this.Image;
                if (image != null)
                {
                    //e.Graphics.FillRectangle(new SolidBrush(Color.Red), new Rectangle(0, 0, this.Width, this.Height));

                    ImageAttributes imageAttributes = new ImageAttributes();
                    int width = image.Width;
                    int height = image.Height;

                    float[][] colorMatrixElements = { 
                                            new float[] {0,  0,  0,  0, 0},         // red scaling factor of 0
                                            new float[] {0,  0,  0,  0, 0},         // green scaling factor of 0
                                            new float[] {0,  0,  0,  0, 0},         // blue scaling factor of 0
                                            new float[] {0,  0,  0,  0, 0},         // alpha scaling factor of 0
                                            new float[] {0,  0,  0,  0, 1}};        // three translations of 0

                    ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

                    //ColorMatrix colorMatrix = new ColorMatrix();
                    //colorMatrix.Matrix03 = 0.02F;
                    //colorMatrix.Matrix13 = 0.99F;
                    //colorMatrix.Matrix23 = 0.52F;
                    if (Active)
                        colorMatrix.Matrix33 = 0.00F;
                    else
                        colorMatrix.Matrix33 = 0.4F;
                    //colorMatrix.Matrix43 = 0.52F;

                    //imageAttributes.SetColorMatrix(colorMatrix);

                    imageAttributes.SetColorMatrix(
                       colorMatrix,
                       ColorMatrixFlag.Default,
                       ColorAdjustType.Bitmap);

                    //e.Graphics.DrawImage(image, 10, 10);

                    e.Graphics.DrawImage(
                       image,
                       new Rectangle(0, 0, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);

                    //this.Image = image;

                }
            }
        }
    }
}
