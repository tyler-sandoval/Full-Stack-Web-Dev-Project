using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;
using FSDP.DATA.EF.Repositories;
using System.IO;
using System.Drawing;
using FSDP.UI.MVC.Utilties;

namespace FSDP.UI.MVC.Controllers
{
    [Authorize]
    public class LessonsController : Controller
    {
        //private FSDPEntities1 db = new FSDPEntities1();
        UnitOfWork uow = new UnitOfWork();

        // GET: Lessons
        public ActionResult Index()
        {
            //var lessons = db.Lessons.Include(l => l.Cours);
            var lesson = uow.LessonsRepository.Get(includeProperties: "Cours");

            return View(lesson);
        }

        // GET: Lessons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = uow.LessonsRepository.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // GET: Lessons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonID,LessonTitle,CourseID,Introduction,VideoUrl,PdfFilename,IsActive")] Lesson lesson, HttpPostedFileBase PdfFilename)
        {
            if (ModelState.IsValid)
            {
                string pdfName = "";

                if (PdfFilename != null)
                {
                    string pdfExt = Path.GetExtension(PdfFilename.FileName).ToLower();
                    string allowedExtensions = ".pdf";

                    if (allowedExtensions.Contains(pdfExt))
                    {
                        //save with original file
                        pdfName = Path.GetFileName(PdfFilename.FileName).ToString();

                        //set path on server where pdfs stored
                        string savePath = Server.MapPath("~/Content/img/pdfs/");

                        //upload the file
                        FileUtilities.UploadFile(savePath, pdfName, PdfFilename);


                    }
                }

                lesson.PdfFilename = pdfName;
                uow.LessonsRepository.Add(lesson);
                uow.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // GET: Lessons/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = uow.LessonsRepository.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonID,LessonTitle,CourseID,Introduction,VideoUrl,PdfFilename,IsActive")] Lesson lesson, HttpPostedFileBase PdfFilename)
        {
            string pdfName = "";
            if (ModelState.IsValid)
            {
                string oldFileName = uow.LessonsRepository.UntrackedFind(lesson.LessonID).PdfFilename;
                if (PdfFilename != null)
                {
                    var savePath = Server.MapPath("~/Content/img/pdfs/");
                    FileUtilities.Delete(savePath, oldFileName);

                    pdfName = PdfFilename.FileName;
                    string pdfExt = ".png";
                    string[] allowedExtensions = { ".pdf", ".docx", ".xslx" };

                    if (allowedExtensions.Contains(pdfExt))
                    {
                        pdfName = Path.GetFileName(PdfFilename.FileName);
                        string newSavePath = Server.MapPath("~/Content/img/pdfs/");
                        FileUtilities.UploadFile(newSavePath, pdfName, PdfFilename);

                    }
                    else
                    {
                        lesson.PdfFilename = oldFileName;
                    }

                }
                uow.LessonsRepository.Update(lesson);
                uow.Save();
                return RedirectToAction($"Details/{lesson.LessonID}");
            }
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName", lesson.CourseID);
            return View(lesson);

        }
        // GET: Lessons/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = uow.LessonsRepository.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

        // POST: Lessons/Delete/5        
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lesson lesson = uow.LessonsRepository.Find(id);
            uow.LessonsRepository.Remove(lesson);
            uow.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
