using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace AdminSoft.WebSite.Helpers
{
    public class FileUploadHelper
    {
        public static string folder = "~/Images/files";
        public static string folderThumbnail = "~/Images/files/Thumbnail";
        public string SaveImageFile(HttpPostedFileBase file, string imageName = null)
        {
            if (file == null)
                return null;

            var urlFolder = HttpContext.Current.Server.MapPath(folder);
            var urlFolderThumbnail = HttpContext.Current.Server.MapPath(folderThumbnail);
            var fileName = Path.GetFileName(file.FileName);
            var newName = imageName == null ? Guid.NewGuid().ToString() + Path.GetExtension(fileName) : imageName;
            var imageUrl = Path.Combine(urlFolder, newName);
            var imageThumbnailUrl = Path.Combine(urlFolderThumbnail, newName);


            if (!Directory.Exists(urlFolder))
                Directory.CreateDirectory(urlFolder);

            if (!Directory.Exists(urlFolderThumbnail))
                Directory.CreateDirectory(urlFolderThumbnail);

            if (File.Exists(imageUrl))
                File.Delete(imageUrl);

            if(File.Exists(imageThumbnailUrl))
                File.Delete(imageThumbnailUrl);

            file.SaveAs(imageUrl);
            CreateThumbnail(100, imageUrl, imageThumbnailUrl);

            return newName;
        }

        public void DeleteImagenFile(string imageName) {
            if (string.IsNullOrEmpty(imageName))
                return;

            var urlFolder = HttpContext.Current.Server.MapPath( folder);
            var urlFolderThumbnail = HttpContext.Current.Server.MapPath(folderThumbnail);
            var imageUrl = Path.Combine(urlFolder, imageName);
            var imageThumbnailUrl = Path.Combine(urlFolder, urlFolderThumbnail);

            if (File.Exists(imageUrl))
                File.Delete(imageUrl);

            if (File.Exists(imageThumbnailUrl))
                File.Delete(imageThumbnailUrl);
        }


        private void CreateThumbnail(int thumbnailMax, string originalImagePath, string thumbnailImagePath)
        {
            // Loads original image from file
            var imgOriginal = Image.FromFile(originalImagePath);
            // Finds height and width of original image
            float originalHeight = imgOriginal.Height;
            float originalWidth = imgOriginal.Width;
            // Finds height and width of resized image
            int thumbnailWidth;
            int thumbnailHeight;
            if (originalHeight > originalWidth)
            {
                thumbnailHeight = thumbnailMax;
                thumbnailWidth = (int)((originalWidth / originalHeight) * (float)thumbnailMax);
            }
            else
            {
                thumbnailWidth = thumbnailMax;
                thumbnailHeight = (int)((originalHeight / originalWidth) * (float)thumbnailMax);
            }
            // Create new bitmap that will be used for thumbnail
            Bitmap thumbnailBitmap = new Bitmap(thumbnailWidth, thumbnailHeight);
            Graphics resizedImage = Graphics.FromImage(thumbnailBitmap);
            // Resized image will have best possible quality
            resizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
            resizedImage.CompositingQuality = CompositingQuality.HighQuality;
            resizedImage.SmoothingMode = SmoothingMode.HighQuality;
            // Draw resized image
            resizedImage.DrawImage(imgOriginal, 0, 0, thumbnailWidth, thumbnailHeight);
            // Save thumbnail to file
            if (!File.Exists(thumbnailImagePath))
                thumbnailBitmap.Save(thumbnailImagePath);
        }
    }
}