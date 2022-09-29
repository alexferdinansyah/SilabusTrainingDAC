using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Courses
        public ViewResult Index(string sortOrder, string currentFilter, string searchID, string searchTitle, string searchCredit, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdSortParm = sortOrder == "id" ? "id_desc" : "id";
            ViewBag.CreditsSortParm = sortOrder == "credit" ? "credits_desc" : "credit";
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            var courses = from c in db.Courses
                           select c;

            ViewBag.CurrentFilter = searchID;
            ViewBag.CurrentFilter = searchTitle;
            ViewBag.CurrentFilter = searchCredit;
            if (!String.IsNullOrEmpty(searchID))
            {
                courses = courses.Where(c => c.CourseID.ToString().Contains(searchID));
            }

            if (!String.IsNullOrEmpty(searchTitle))
            {
                courses = courses.Where(c => c.Title.Contains(searchTitle));
            }

            if (!String.IsNullOrEmpty(searchCredit))
            {
                courses = courses.Where(c => c.Credits.ToString().Contains(searchCredit));
            }

            if (searchID != null)
            {
                page = 1;
            }
            else
            {
                searchID = currentFilter;
            }

            if (searchTitle != null)
            {
                page = 1;
            }
            else
            {
                searchTitle = currentFilter;
            }

            if (searchCredit != null)
            {
                page = 1;
            }
            else
            {
                searchCredit = currentFilter;
            }

            switch (sortOrder)
            {
                case "id_desc":
                    courses = courses.OrderByDescending(c => c.CourseID);
                    break;
                case "id":
                    courses = courses.OrderBy(c => c.CourseID);
                    break;
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                case "credit":
                    courses = courses.OrderBy(c => c.Credits);
                    break;
                case "title_desc":
                    courses = courses.OrderByDescending(c => c.Title);
                    break;
                default:  // Name ascending 
                    courses = courses.OrderBy(c => c.Title);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
