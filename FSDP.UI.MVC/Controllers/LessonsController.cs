﻿using System;
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
using Owin;
using FSDP.UI.MVC.Models;

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

        // GET: actionlink from Courses/Details to view only lessons with course ID
        public ActionResult CourseLessons(int? id)
        {
            var lesson = uow.LessonsRepository.Find(id);//.Where(lsn => lsn.CourseID == id));

            //auto-generate CourseCompletion once all lessons have been viewed
            //create CourseCompletion obj
            CourseCompletion cc = new CourseCompletion();

            //get count of active lessons in course
            int totalLsnCnt = uow.LessonsRepository.Get().Where(x => x.CourseID == lesson.CourseID).Count();

            //get count of user's lesson views of course lessons
            string currentUser = User.Identity.Name;
            int userLsnVwsOfCourse = uow.LessonViewsRepository.Get().Where(x => x.UserID == currentUser && x.DateViewed.Year == DateTime.Now.Year && x.Lesson.CourseID == lesson.CourseID).Count();

            //check for any existing CourseCompletions and set current course and year
            var ccExists = uow.CourseCompletionsRepository.Get().Where(x => x.CourseID == lesson.CourseID && x.UserID == currentUser && x.DateCompleted.Year == DateTime.Now.Year);
            if (ccExists.Count() == 0)
            {
                if (totalLsnCnt == userLsnVwsOfCourse)
                {
                    cc.DateCompleted = DateTime.Now;
                    cc.UserID = currentUser;
                    cc.CourseID = lesson.CourseID;
                    uow.CourseCompletionsRepository.Add(cc);
                    uow.Save();

                    //TODO: setup manager email confirmation
                    //send manager auto-gen email
                    //get list of Roles and users in manager role
                    //var managerRole = HttpContext.;
                    //string managerEmail = managerRole.
                }
            }

            if (User.IsInRole("Employee"))
            {
                var lessonEmps = uow.LessonsRepository.Get().Where(lsn => lsn.CourseID == id && lsn.IsActive == true);

                return View(lessonEmps);
            }
            else
            {
                var lessons = uow.LessonsRepository.Get().Where(lsn => lsn.CourseID == id);
                return View(lessons);
            }
            /*
            //auto-generate CourseCompletion once all lessons have been viewed
            //create CourseCompletion obj
            CourseCompletion cc = new CourseCompletion();

            //get count of active lessons in course
            int totalLsnCnt = uow.LessonsRepository.Get().Where(x => x.CourseID == lesson.CourseID).Count();

            //get count of user's lesson views of course lessons
            string currentUser = User.Identity.Name;
            int userLsnVwsOfCourse = uow.LessonViewsRepository.Get().Where(x => x.UserID == currentUser && x.DateViewed.Year == DateTime.Now.Year && x.Lesson.CourseID == lesson.CourseID).Count();

            */

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
            if (lesson != null)
            {
                ViewBag.Message = "";

                //create LessonView obj
                LessonView lv = new LessonView();

                //get current user
                string currentUser = User.Identity.Name;

                //check for any existing record of current year view
                var exists = uow.LessonViewsRepository.Get().Where(x => x.LessonID == lesson.LessonID && x.UserID == currentUser && x.DateViewed.Year == DateTime.Now.Year);

                //if no existance, create entry
                if (exists.Count() == 0)
                {
                    lv.DateViewed = DateTime.Now;
                    lv.UserID = currentUser;
                    lv.LessonID = lesson.LessonID;
                    uow.LessonViewsRepository.Add(lv);
                    uow.Save();
                }
                else if (exists.Count() > 0)
                {
                    ViewBag.Message += "You've already viewed this lesson";
                }

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
        public ActionResult Edit([Bind(Include = "LessonID,LessonTitle,CourseID,Introduction,VideoUrl,PdfFilename,IsActive")] Lesson lesson, HttpPostedFileBase PdfFileName)
        {
            if (ModelState.IsValid)
            {
                string oldFileName = uow.LessonsRepository.UntrackedFind(lesson.LessonID).PdfFilename;
                if (PdfFileName != null)
                {
                    var savePath = Server.MapPath("~/Content/img/pdfs/");
                    FileUtilities.Delete(savePath, oldFileName);

                    var newPdfName = PdfFileName.FileName;
                    string pdfExt = ".pdf";
                    string[] allowedExtensions = { ".pdf", ".docx", ".xslx" };

                    if (allowedExtensions.Contains(pdfExt))
                    {
                        newPdfName = Path.GetFileName(PdfFileName.FileName);
                        string newSavePath = Server.MapPath("~/Content/img/pdfs/");
                        FileUtilities.UploadFile(newSavePath, newPdfName, PdfFileName);

                        lesson.PdfFilename = newPdfName;

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

        public Lesson UntrackedFindPdf(int id)
        {
            return uow.LessonsRepository.UntrackedFind(id);
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
