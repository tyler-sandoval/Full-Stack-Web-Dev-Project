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

        public static void UploadVideo(string fileName, string embedUrl)
        {
            
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

        public static void DeletePDF(string path, string pdfFileName)
        {
            // Get info about the PDF
            FileInfo pdf = new FileInfo(path + pdfFileName);

            // Check to see if it exists
            if (pdf.Exists)
            {
                // If so, delete it
                pdf.Delete();
            }
        }


    }
}