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
                
                //string userID = User.Identity.Name.ToString();

                IEnumerable<Cours> empcrs = uow.CoursesRepository.Get().Where(x => x.IsActive == true); //&& x.CourseID.Equals(uow.CourseAssignmentsRepository.Get().Where(y => y.UserID == userID)));

                return View(empcrs);
            }
            var courses = uow.CoursesRepository.Get();

            return View(courses);
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
