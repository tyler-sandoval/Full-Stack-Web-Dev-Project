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
using Microsoft.AspNet.Identity.Owin;
using FSDP.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

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

        public double LessonProgression(Lesson lesson)
        {
            //get current user
            string currentUser = User.Identity.Name;
            //get count of active lessons in course
            var lessons = uow.LessonsRepository.Get().Where(x => x.IsActive == true && x.CourseID == lesson.CourseID);
            double lessonCount = lessons.Count();
            //get count of lessons in course with a lessonview of user
            var lessonsCompleted = uow.LessonViewsRepository.Get().Where(x => x.UserID == currentUser);

            double countViewedLessons = 0;

            foreach (var l in lessons)
            {
                foreach (var lv in lessonsCompleted)
                {
                    if (l.LessonID == lv.LessonID)
                    {
                        countViewedLessons++;
                    }
                }
            }

            double progress = (countViewedLessons / lessonCount) * 100;
            return ViewBag.LessonProgress = progress;
        }

        // GET: actionlink from Courses/Details to view only lessons with course ID
        public ActionResult CourseLessons(int? id)
        {
            var lesson = uow.LessonsRepository.Find(id);//.Where(lsn => lsn.CourseID == id));

            ViewBag.Progress = LessonProgression(lesson);
            //auto-generate CourseCompletion once all lessons have been viewed
            //create CourseCompletion obj
            CourseCompletion cc = new CourseCompletion();

            //get count of active lessons in course for user
            var totalLsnCnt = uow.LessonsRepository.Get().Where(x => x.CourseID == lesson.CourseID && x.IsActive == true).Count();

            //get count of user's lesson views of course lessons
            string currentUser = User.Identity.Name;
            var userLsnVwsOfCourse = uow.LessonViewsRepository.Get().Where(x => x.UserID == currentUser && x.DateViewed.Year == DateTime.Now.Year && x.Lesson.CourseID == lesson.CourseID).Count();

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

                    //send manager auto-gen email
                    //get list of Roles and users in manager role
                    var managerRole = HttpContext.GetOwinContext().Get<ApplicationRoleManager>().FindByName("Manager");
                    string managerEmail = managerRole.Users.Select(m => uow.AspNetUsersRepository.Get().Where(mId => mId.Id == m.UserId)).FirstOrDefault().Select(e => e.Email).SingleOrDefault();
                    ViewBag.managerEmail = managerEmail;

                    //get current user's information
                    AspNetUser curUser = uow.AspNetUsersRepository.Find(currentUser);

                    try
                    {
                    //create email body of msg
                    string body = string.Format(
                        "{0}, {1}, has comepleted the {2} course on {3}.",
                        curUser.UserName,
                        curUser.Email,
                        lesson.Cours.CourseName,
                        DateTime.Now
                        );

                    
                    //create new mailMessage and send via SmtpClient
                    MailMessage complMsg = new MailMessage("no-reply@tylersandoval.com", managerEmail, "FSDP-LMS Course Completion Notification", body);
                    SmtpClient client = new SmtpClient("mail.tylersandoval.com");
                    client.Credentials = new NetworkCredential("no-reply@tylersandoval.com", "Vivian0221!");
                    client.Send(complMsg);
                    }
                    catch (Exception)
                    {
                        return ViewBag.Error = "There was an issue with sending manager email notification. Please alert your manager and submit a ticket via contact page!";
                    }
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
            lesson.VideoUrl = ParsedYoutubeUpload(lesson.VideoUrl);
            return View(lesson);
        }


        //Youtube Video Controller ActionResult to parse Admin's YoutubeUrl upload
        public string ParsedYoutubeUpload(string CompleteYouTubeURL)
        {
            string vid;
            if (CompleteYouTubeURL != null)
            {
                var v = CompleteYouTubeURL.IndexOf("v=");
                var amp = CompleteYouTubeURL.IndexOf("&", v);
                

                //2 options for getting video id
                //first, if video is last value of url
                if (amp == -1)
                {
                    vid = CompleteYouTubeURL.Substring(v + 2);
                }
                else
                {
                    vid = CompleteYouTubeURL.Substring(v + 2, amp - (v + 2));
                }

                return ViewBag.VideoID = vid;
            }
            return ViewBag.VideoID = " - Video Not Available - " ;
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
                    pdfName = PdfFilename.FileName;

                    string pdfExt = pdfName.Substring(pdfName.LastIndexOf('.'));

                    string allowedExtension = ".pdf";

                    if (pdfExt == allowedExtension)
                    {
                        //save with original file
                        PdfFilename.SaveAs(Server.MapPath("~/Content/img/pdfs/" + pdfName));
                    }
                    else
                    {
                        pdfName = "noPDF.png";
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
