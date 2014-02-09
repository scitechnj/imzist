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
        public static void ThumbnailMaker(string filelocation, string outputPath, int newPixelHeight)
        {
            using (Image originalImage = Image.FromFile(filelocation))
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
        //public static bool IsValidImage(Stream image)
        //{
        //    image
        //}
    }
}
