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
    public class CourseAssignmentsController : Controller
    {
        //private FSDPEntities1 db = new FSDPEntities1();
        UnitOfWork uow = new UnitOfWork();

        // GET: CourseAssignments
        public ActionResult Index()
        {
            //var courseAssignments = db.CourseAssignments.Include(c => c.AspNetUser).Include(c => c.Cours);
            var courseAssignments = uow.CourseAssignmentsRepository.Get(includeProperties: "AspNetUser, Cours");
            return View(courseAssignments.ToList());
        }

        // GET: CourseAssignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignment courseAssignment = uow.CourseAssignmentsRepository.Find(id);
            if (courseAssignment == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            // ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email");
            // ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "CourseName");
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");

            return View();
        }

        // POST: CourseAssignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseAssignments,CourseID,UserID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                uow.CourseAssignmentsRepository.Add(courseAssignment);
                uow.Save();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(uow.CourseAssignmentsRepository.Get(), "UserID", "UserID");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignment courseAssignment = uow.CourseAssignmentsRepository.Find(id);
            if (courseAssignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseAssignment);
        }

        // POST: CourseAssignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseAssignments,CourseID,UserID")] CourseAssignment courseAssignment)
        {
            if (ModelState.IsValid)
            {
                uow.CourseAssignmentsRepository.Update(courseAssignment);
                uow.Save();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.CourseID = new SelectList(uow.CoursesRepository.Get(), "CourseID", "CourseName");
            return View(courseAssignment);
        }

        // GET: CourseAssignments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssignment courseAssignment = uow.CourseAssignmentsRepository.Find(id);
            if (courseAssignment == null)
            {
                return HttpNotFound();
            }
            return View(courseAssignment);
        }

        // POST: CourseAssignments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseAssignment courseAssignment = uow.CourseAssignmentsRepository.Find(id);
            uow.CourseAssignmentsRepository.Remove(courseAssignment);
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
