using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PictureBoxStatus
{
    public static class BitmapRotation
    {
        /// <summary>
        /// Method to rotate an Image object. The result can be one of three cases:
        /// - upsizeOk = true: output image will be larger than the input, and no clipping occurs 
        /// - upsizeOk = false and clipOk = true:  output same size as input, clipping occurs
        /// - upsizeOk = false and clipOk = false: output same size as input, image reduced, no clipping
        /// 
        /// A background color must be specified, and this color will fill the edges that are not 
        /// occupied by the rotated image. If color = transparent the output image will be 32-bit, 
        /// otherwise the output image will be 24-bit.
        ///   
        /// Note that this method always returns a new Bitmap object, even if rotation is zero - in 
        /// which case the returned object is a clone of the input object. 
        /// </summary>
        /// <param name="inputImage">input Image object, is not modified</param>
        /// <param name="angleDegrees">angle of rotation, in degrees</param>
        /// <param name="upsizeOk">see comments above</param>
        /// <param name="clipOk">see comments above, not used if upsizeOk = true</param>
        /// <param name="backgroundColor">color to fill exposed parts of the background</param>
        /// <returns>new Bitmap object, may be larger than input image</returns>
        public static Bitmap RotateImage(this Image image, float angleDegrees, bool upsizeOk,
                                         bool clipOk, Color? backgroundColor = null)
        {
            // Test for zero rotation and return a clone of the input image
            if (angleDegrees == 0f)
                return (Bitmap)image.Clone();

            // Set up old and new image dimensions, assuming upsizing not wanted and clipping OK
            int oldWidth = image.Width;
            int oldHeight = image.Height;
            int newWidth = oldWidth;
            int newHeight = oldHeight;
            float scaleFactor = 1f;

            // If upsizing wanted or clipping not OK calculate the size of the resulting bitmap
            if (upsizeOk || !clipOk)
            {
                double angleRadians = angleDegrees * Math.PI / 180d;

                double cos = Math.Abs(Math.Cos(angleRadians));
                double sin = Math.Abs(Math.Sin(angleRadians));
                newWidth = (int)Math.Round(oldWidth * cos + oldHeight * sin);
                newHeight = (int)Math.Round(oldWidth * sin + oldHeight * cos);
            }

            // If upsizing not wanted and clipping not OK need a scaling factor
            if (!upsizeOk && !clipOk)
            {
                scaleFactor = Math.Min((float)oldWidth / newWidth, (float)oldHeight / newHeight);
                newWidth = oldWidth;
                newHeight = oldHeight;
            }

            // Create the new bitmap object. If background color is transparent it must be 32-bit, 
            //  otherwise 24-bit is good enough.
            Bitmap newBitmap;

            if (!backgroundColor.HasValue)
                newBitmap = new Bitmap(newWidth, newHeight);
            else
                newBitmap = new Bitmap(newWidth, newHeight, backgroundColor.Value == Color.Transparent ?
                                                 PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            newBitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            // Create the Graphics object that does the work
            using (Graphics graphicsObject = Graphics.FromImage(newBitmap))
            {
               
                graphicsObject.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsObject.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphicsObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;


                // Fill in the specified background color if necessary
                if (backgroundColor != Color.Transparent && backgroundColor.HasValue)
                    graphicsObject.Clear(backgroundColor.Value);

                // Set up the built-in transformation matrix to do the rotation and maybe scaling
                graphicsObject.TranslateTransform(newWidth / 2f, newHeight / 2f);

                if (scaleFactor != 1f)
                    graphicsObject.ScaleTransform(scaleFactor, scaleFactor);

                graphicsObject.RotateTransform(angleDegrees);
                graphicsObject.TranslateTransform(-oldWidth / 2f, -oldHeight / 2f);

                // Draw the result 
                graphicsObject.DrawImage(image, 0, 0);
            }

            return newBitmap;
        }

        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate.</param>
        /// <param name="offset">The position to rotate from.</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees.</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> of the same size rotated.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static Bitmap RotateImage(this Image image, PointF offset, float angle)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            //create a new empty bitmap to hold rotated image
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {

                //Put the rotation point in the center of the image
                g.TranslateTransform(offset.X, offset.Y);

                //rotate the image
                g.RotateTransform(angle);

                //move the image back
                g.TranslateTransform(-offset.X, -offset.Y);

                //draw passed in image onto graphics object
                g.DrawImage(image, new PointF(0, 0));

            }

            return rotatedBmp;
        }

        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// </summary>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees</param>
        /// <returns>A new <see cref="System.Drawing.Bitmap"/> of the same size rotated.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static Bitmap RotateImage(this Image image, float angle)
        {
            /// Below method have Garbage Collection and disposed old image from reference, 
            /// So you are not us that for this app.
            /// That use for image changer and for RAN Overflow...

            //return RotateImage(image, new PointF((float)image.Width / 2, (float)image.Height / 2), angle);
            
            ///
            /// Rotate without Disposing...
            return image.RotateImage(angle, false, true, null);
        }

        /// <summary>
        /// Creates a new Image containing the same image only rotated
        /// Note: if you use only one Image, do not use this method, because this function 
        /// after create new image , disposed old image, so that is incorrect for your next job...
        /// </summary>
        /// <param name="pb">The <see cref="System.Windows.Forms.PictureBox"/> to show on that's.</param>
        /// <param name="image">The <see cref="System.Drawing.Image"/> to rotate.</param>
        /// <param name="angle">The amount to rotate the image, clockwise, in degrees.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if <see cref="image"/> is null.</exception>
        public static void RotateImage(this PictureBox pb, Image image, float angle)
        {
            if (image == null || pb.Image == null)
                return;

            Image oldImage = pb.Image;
            Bitmap newImage = RotateImage(image, angle);
            pb.Image = newImage;

            if (oldImage != null)
            {
              // oldImage.Dispose();
            }
        }
    }
}
