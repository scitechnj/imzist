using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Web;

namespace Imzist.Logic
{
    public static class ImageHelper
    {
        public static void ThumbnailMaker(Stream image, string outputPath, int newPixelHeight)
        {
            using (Image originalImage = Image.FromStream(image))
            {
                int newWidth = (originalImage.Width * newPixelHeight) / originalImage.Height;
                using (Bitmap blankCanvas = new Bitmap(newWidth, newPixelHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(blankCanvas))
                    {
                        graphics.DrawImage(originalImage, 0, 0, blankCanvas.Width, blankCanvas.Height);
                        blankCanvas.Save(outputPath);
                    }
                }
            }

        }
        public static bool IsValidImage(Stream imageStream)
        {
            if (imageStream.Length > 0)
            {
                byte[] header = new byte[4]; // Change size if needed.
                string[] imageHeaders = new[]{
                "\xFF\xD8", // JPEG
                "BM",       // BMP
                "GIF",      // GIF
                Encoding.ASCII.GetString(new byte[]{137, 80, 78, 71})}; // PNG

                imageStream.Read(header, 0, header.Length);

                bool isImageHeader = imageHeaders.Any(str => Encoding.ASCII.GetString(header).StartsWith(str));
                if (isImageHeader)
                {
                    try
                    {
                        Image.FromStream(imageStream).Dispose();
                        imageStream.Close();
                        return true;
                    }

                    catch
                    {

                    }
                }
            }

            imageStream.Close();
            return false;
        }
    }
}
