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
    public class EnrollmentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Enrollment
        public ActionResult Index(string sortOrder, string currentFilter, string currentFiltered,string currentFilters, string searchCourse, string searchName, string searchGrade, int? page)
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title"; 
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.GradeSortParm = sortOrder == "Grade" ? "grade_desc" : "Grade";

            if (searchCourse != null)
            {
                page = 1;
            }
            else
            {
                searchCourse = currentFilter;
            }

            if (searchName != null)
            {
                page = 1;
            }
            else
            {
                searchName = currentFiltered;
            }

            if (searchGrade != null)
            {
                page = 1;
            }
            else
            {
                searchGrade = currentFilters;
            }

            ViewBag.CurrentFilter = searchCourse;
            ViewBag.CurrentFiltered = searchName;
            ViewBag.CurrentFilters = searchGrade;

            var enroll = from s in db.Enrollments
                         select s;

            if (!String.IsNullOrEmpty(searchCourse))
            {
                enroll = enroll.Where(s => s.Course.Title.Contains(searchCourse));
            }
            if (!String.IsNullOrEmpty(searchName)){
                enroll = enroll.Where(s => s.Student.FirstMidName.Contains(searchName));
            } 
            if (!String.IsNullOrEmpty(searchGrade)){
                enroll = enroll.Where(s => s.Grade.ToString().Contains(searchGrade));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    enroll = enroll.OrderByDescending(s => s.Course.Title);
                    break;  
                case "name_desc":
                    enroll = enroll.OrderByDescending(s => s.Student.FirstMidName);
                    break;  
                case "grade_desc":
                    enroll = enroll.OrderByDescending(s => s.Grade);
                    break;
                case "Title":
                    enroll = enroll.OrderBy(s => s.Course.Title);
                    break; 
                case "Name":
                    enroll = enroll.OrderBy(s => s.Student.FirstMidName);
                    break;
                default:
                    enroll = enroll.OrderBy(s => s.Grade);
                    break;

            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(enroll.ToPagedList(pageNumber, pageSize));
        }

        // GET: Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "FirstMidName", "LastName");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentID,CourseID,StudentID,Grade")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
