using ResourceMetadata.Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ResourceMetadata.Core.Util
{
    public class FileUtil
    {
        public static string FullPathPicture(string fileName, PictureType pictureType, PictureSize pictureSize)
        {
            var path = "~/Images/" + pictureType + "/" + pictureSize + "/" + fileName;
            return path;
        }
        public static void CreateThumbPicture(string sourcePath, string thumbPath, PictureSize pictureSize)
        {
            var fileExtension = Path.GetExtension(sourcePath);
            if (!File.Exists(sourcePath)) return;


            using (var b = new Bitmap(sourcePath))
            {
                var targetSize = 0;
                switch (pictureSize)
                {
                    case PictureSize.Tiny:
                        targetSize = 50;
                        break;
                    case PictureSize.Small:
                        targetSize = 100;
                        break;
                    case PictureSize.Medium:
                        targetSize = 150;
                        break;
                    case PictureSize.Large:
                        targetSize = 250;
                        break;
                }
                var newSize = CalculateDimensions(b.Size, targetSize);

                if (newSize.Width < 1)
                    newSize.Width = 1;
                if (newSize.Height < 1)
                    newSize.Height = 1;

                //temp
                //_mediaSettings.DefaultImageQuality = 80;

                using (var newBitMap = new Bitmap(newSize.Width, newSize.Height))
                {
                    using (var g = Graphics.FromImage(newBitMap))
                    {
                        int DefaultImageQuality = 80;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                        var ep = new EncoderParameters();
                        ep.Param[0] = new EncoderParameter(Encoder.Quality, DefaultImageQuality);
                        ImageCodecInfo ici = GetImageCodecInfoFromExtension(fileExtension);
                        if (ici == null)
                            ici = GetImageCodecInfoFromMimeType("image/jpeg");
                        newBitMap.Save(thumbPath, ici, ep);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        private static Size CalculateDimensions(Size originalSize, int targetSize)
        {
            var newSize = new Size();
            if (originalSize.Height > originalSize.Width) // portrait 
            {
                newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
                newSize.Height = targetSize;
            }
            else // landscape or square
            {
                newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
                newSize.Width = targetSize;
            }
            return newSize;
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified extension.
        /// </summary>
        /// <param name="fileExt">File extension</param>
        /// <returns>ImageCodecInfo</returns>
        private static ImageCodecInfo GetImageCodecInfoFromExtension(string fileExt)
        {
            fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    return GetImageCodecInfoFromMimeType("image/jpeg");
                case "png":
                    return GetImageCodecInfoFromMimeType("image/png");
                case "gif":
                    //use png codec for gif to preserve transparency
                    //return GetImageCodecInfoFromMimeType("image/gif");
                    return GetImageCodecInfoFromMimeType("image/png");
                default:
                    return GetImageCodecInfoFromMimeType("image/jpeg");
            }
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>ImageCodecInfo</returns>
        private static ImageCodecInfo GetImageCodecInfoFromMimeType(string mimeType)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            foreach (var ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    return ici;
            return null;
        }


    }
}
