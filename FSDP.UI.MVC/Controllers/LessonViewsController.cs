﻿using System;
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
    public class LessonViewsController : Controller
    {
        //private FSDPEntities1 db = new FSDPEntities1();
        UnitOfWork uow = new UnitOfWork();

        // GET: LessonViews

        public ActionResult Index()
        {
            //var lessonViews = db.LessonViews.Include(l => l.Lesson);
            var lessonViews = uow.LessonViewsRepository.Get(includeProperties: "Lesson");

            if (User.IsInRole("Employee"))
            {
                var empLsnViews = uow.LessonViewsRepository.Get(includeProperties: "Lesson").Where(x => x.UserID == User.Identity.Name);
                return View(empLsnViews.ToList());
            }
            return View(lessonViews.ToList());
        }

        // GET: LessonViews/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = uow.LessonViewsRepository.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // GET: LessonViews/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.LessonID = new SelectList(uow.LessonsRepository.Get(), "LessonID", "LessonTitle");
            return View();
        }

        // POST: LessonViews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LessonViewID,UserID,LessonID,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                uow.LessonViewsRepository.Add(lessonView);
                uow.Save();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.LessonID = new SelectList(uow.LessonsRepository.Get(), "LessonID", "LessonTitle");
            return View(lessonView);
        }

        // GET: LessonViews/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = uow.LessonViewsRepository.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.LessonID = new SelectList(uow.LessonsRepository.Get(), "LessonID", "LessonTitle", lessonView.LessonID);
            return View(lessonView);
        }

        // POST: LessonViews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LessonViewID,UserID,LessonID,DateViewed")] LessonView lessonView)
        {
            if (ModelState.IsValid)
            {
                uow.LessonViewsRepository.Update(lessonView);
                uow.Save();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(uow.AspNetUsersRepository.Get(), "Id", "Email");
            ViewBag.LessonID = new SelectList(uow.LessonsRepository.Get(), "LessonID", "LessonTitle", lessonView.LessonID);
            return View(lessonView);
        }

        // GET: LessonViews/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LessonView lessonView = uow.LessonViewsRepository.Find(id);
            if (lessonView == null)
            {
                return HttpNotFound();
            }
            return View(lessonView);
        }

        // POST: LessonViews/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LessonView lessonView = uow.LessonViewsRepository.Find(id);
            uow.LessonViewsRepository.Remove(lessonView);
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
