using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FSDP.DATA.EF;

namespace FSDP.UI.MVC.Controllers
{
    [Authorize]
    public class CourseCompletionsController : Controller
    {
        //private FSDPEntities1 db = new FSDPEntities1();
        UnitOfWork uow = new UnitOfWork();

        // GET: CourseCompletions
        public ActionResult Index()
        {
            var courseCompletions = uow.CourseCompletionsRepository.Get(includeProperties: "Cours");

            if (User.IsInRole("Employee"))
            {
                var employeeOnly = uow.CourseCompletionsRepository.Get(includeProperties: "Cours").Where(x => x.UserID == User.Identity.Name);

                return View(employeeOnly.ToList());
            }
            return View(courseCompletions.ToList());
        }

        // GET: CourseCompletions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletion courseCompletion = uow.CourseCompletionsRepository.Find(id);
            if (courseCompletion == null)
            {
                return HttpNotFound();
            }
            return View(courseCompletion);
        }

        // GET: CourseCompletions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View();
        }

        // POST: CourseCompletions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseCompletionID,UserID,CourseID,DateCompleted")] CourseCompletion courseCompletion)
        {
            if (ModelState.IsValid)
            {
                uow.CourseCompletionsRepository.Add(courseCompletion);
                uow.Save();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseCompletion);
        }

        // GET: CourseCompletions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletion courseCompletion = uow.CourseCompletionsRepository.Find(id);
            if (courseCompletion == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseCompletion);
        }

        // POST: CourseCompletions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseCompletionID,UserID,CourseID,DateCompleted")] CourseCompletion courseCompletion, AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                uow.CourseCompletionsRepository.Update(courseCompletion);
                uow.Save();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "UserID", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseCompletion);
        }

        // GET: CourseCompletions/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseCompletion courseCompletion = uow.CourseCompletionsRepository.Find(id);
            if (courseCompletion == null)
            {
                return HttpNotFound();
            }
            return View(courseCompletion);
        }

        // POST: CourseCompletions/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseCompletion courseCompletion = uow.CourseCompletionsRepository.Find(id);
            uow.CourseCompletionsRepository.Remove(courseCompletion);
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
