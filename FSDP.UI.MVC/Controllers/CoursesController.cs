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
using Microsoft.AspNet.Identity;


namespace FSDP.UI.MVC.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        //private FSDPEntities1 db = new FSDPEntities1();
        UnitOfWork uow = new UnitOfWork();

        // GET: Courses
        public ActionResult Index()
        {                

            if (User.IsInRole("Employee") || User.IsInRole("Manager"))
            {            
                //for progress bar js  

                //string userID = User.Identity.Name.ToString();

                var empcrs = uow.CoursesRepository.Get().Where(x => x.IsActive == true); //&& x.CourseID.Equals(uow.CourseAssignmentsRepository.Get().Where(y => y.UserID == userID)));
                ViewBag.Progress = CourseProgression();
                return View(empcrs);
            }
            else
            {
            var courses = uow.CoursesRepository.Get();
            ViewBag.Progress = CourseProgression();

            return View(courses);
            }
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = uow.CoursesRepository.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        public double CourseProgression()
        {
            //get current user
            string currentUser = User.Identity.Name;
            //get count of active courses
            var totalCourses = uow.CoursesRepository.Get().Where(x => x.IsActive == true);
            double tcCount = totalCourses.Count();
            //get count of course completions for user
            var courseCompletions = uow.CourseCompletionsRepository.Get().Where(x => x.UserID == currentUser);
            double countsCourseCompletions = 0;
            foreach (var course in totalCourses)
            {
                foreach (var completion in courseCompletions)
                {
                    if (course.CourseID == completion.CourseID)
                    {
                        countsCourseCompletions++;
                    }
                }
            }

            double progress = (countsCourseCompletions / tcCount) * 100;
            ViewBag.Progress = progress;
            return ViewBag.Progress;
        }

        

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,CourseName,Description,IsActive")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                uow.CoursesRepository.Add(cours);
                uow.Save();
                return RedirectToAction("Index");
            }

            return View(cours);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = uow.CoursesRepository.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,CourseName,Description,IsActive")] Cours cours)
        {
            if (ModelState.IsValid)
            {
                uow.CoursesRepository.Update(cours);
                uow.Save();
                return RedirectToAction("Index");
            }
            return View(cours);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cours cours = uow.CoursesRepository.Find(id);
            if (cours == null)
            {
                return HttpNotFound();
            }
            return View(cours);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cours cours = uow.CoursesRepository.Find(id);
            uow.CoursesRepository.Remove(cours);
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
