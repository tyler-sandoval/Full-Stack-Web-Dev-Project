using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FSDP.UI.MVC.Utilties
{
    public class FileUtilities
    {
        public static void UploadFile(string savePath, string fileName, HttpPostedFileBase file)
        {
            file.SaveAs(savePath + fileName);
        }

        public static void DeleteFile(string path)
        {
            // get info about the specified file
            FileInfo file = new FileInfo(path);

            // check if the file exists
            if (file.Exists)
            {
                // If so delete the file
                file.Delete();
            }
        }

        public static void ResizeImage(string savePath, string fileName, Image image, int maxImgSize, int maxThumbSize)
        {
            // Get new proportional image dimensions based off current image size and maxImageSize
            int[] newImageSizes = GetNewSize(image.Width, image.Height, maxImgSize);

            // Resize the image to the new dimensions returned from above
            Bitmap newImage = DoResizeImage(newImageSizes[0], newImageSizes[1], image);

            // Save the new image to the specified path with the specified file name
            newImage.Save(savePath + fileName);

            // Calculate the proportional size for the thumbnail based off the maxThumbSize
            int[] newThumbSizes = GetNewSize(newImage.Width, newImage.Height, maxThumbSize);

            // Create the thumbnail image
            Bitmap newThumb = DoResizeImage(newThumbSizes[0], newThumbSizes[1], image);

            // Save the thumbnail image with the t_ prefix
            newThumb.Save(savePath + "t_" + fileName);

            // Clean up our service (free up memory)
            newImage.Dispose();
            newThumb.Dispose();
            image.Dispose();
        }

        public static int[] GetNewSize(int imgWidth, int imgHeight, int maxImgSize)
        {
            // Calculate which dimension is being changed the most and use that as the aspect ratio for both sides
            float ratioX = (float)maxImgSize / (float)imgWidth;
            float ratioY = (float)maxImgSize / (float)imgHeight;
            float ratio = Math.Min(ratioX, ratioY);

            // Calculate the new width and height based on aspect ratio
            int[] newImgSizes = new int[2];
            newImgSizes[0] = (int)(imgWidth * ratio);
            newImgSizes[1] = (int)(imgHeight * ratio);

            // Return the new porportional image sizes
            return newImgSizes;
        }

        public static Bitmap DoResizeImage(int imgWidth, int imgHeight, Image image)
        {
            // Convert other formats (including CMYK) to RGB.
            Bitmap newImage = new Bitmap(imgWidth, imgHeight, PixelFormat.Format24bppRgb);

            // If the image format supports transparency apply transparency
            newImage.MakeTransparent();

            // Set image to screen resolution of 72 dpi (the resolution of monitors)
            newImage.SetResolution(72, 72);

            // Do the resize
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, imgWidth, imgHeight);
            }

            // Return the resized image
            return newImage;
        }

        public static void DeleteImage(string path, string imgName)
        {
            // Get info about the image and the thumbnails
            FileInfo fullImg = new FileInfo(path + imgName);
            FileInfo thumbImg = new FileInfo(path + "t_" + imgName);

            // Check to see if the full size img exists
            if (fullImg.Exists)
            {
                // If so, delete it
                fullImg.Delete();
            }

            // Check if the thumbnail image exists
            if (thumbImg.Exists)
            {
                thumbImg.Delete();
            }
        }
    }
}