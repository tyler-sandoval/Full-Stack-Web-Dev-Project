using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Gnostice.StarDocsSDK;
using FSDP.DATA.EF;
using FSDP.UI.MVC.Utilties;

namespace FSDP.UI.MVC.Controllers
{
    public class StarDocsController : Controller
    {
        UnitOfWork uow = new UnitOfWork();

        //// GET: StarDocs
        //public ActionResult Index(Lesson lesson)
        //{
        //    //string pdf = lesson.PdfFilename;
        //    //string file = @"(C:\docs\fw9.pdf";
        //    //ViewerSettings viewerSettings = new ViewerSettings();
        //    //viewerSettings.VisibleFileOperationControls.Open = true;
        //    //ViewResponse viewResponse = MvcApplication.starDocs.Viewer.CreateView(new FileObject(file), null, viewerSettings);

        //    //return new RedirectResult(viewResponse.Url);
        //}

        public FileResult DisplayPDF(int? id)
        {
            var pdfPath = Server.MapPath("~/Content/img/pdf") + uow.LessonsRepository.Find(id).PdfFilename.ToString();
            return File("pdfPath", "application/pdf");

            //Lesson userLesson = uow.LessonsRepository.Find(id);

            //string fileName = userLesson.PdfFilename;
            //string filepath = Server.MapPath("/Content/img/pdfs/");

            //var fullFilePath = filepath + fileName;
            //byte[] pdfbyte = FileUtilities.GetBytesFromFile(fullFilePath);
            //return File(pdfbyte, "application/pdf");
            
            //id = uow.LessonsRepository.Get("PdfFileName");
            //string filepath = Server.MapPath(uow.LessonsRepository.Find(id);
            //byte[] pdfbyte = FileUtilities.GetBytesFromFile(filepath);
            //return File(pdfbyte, "application/pdf");
        }


    }
}