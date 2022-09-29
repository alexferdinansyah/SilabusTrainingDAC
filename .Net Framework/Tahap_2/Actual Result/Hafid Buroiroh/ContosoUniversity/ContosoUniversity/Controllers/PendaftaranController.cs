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
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class PendaftaranController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Pendaftaran
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
            ViewBag.enrollments = db.Enrollments.ToList();
            PendaftaranSearchVM model = new PendaftaranSearchVM();
            return View(model);
        }

        public ActionResult IndexProcess(PendaftaranSearchVM model, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = sortOrder == "Judul" ? "judul_desc" : "Judul";
            ViewBag.NameSortParm = sortOrder == "Nama" ? "nama_desc" : "Nama";
            ViewBag.GradeSortParm = String.IsNullOrEmpty(sortOrder) ? "Grade" : "";
            IEnumerable<Enrollment> enroll = from s in db.Enrollments
                                          select s;
            switch (sortOrder)
            {
                case "judul_desc":
                    enroll = enroll.OrderByDescending(s => s.Course.Title);
                    break;
                case "nama_desc":
                    enroll = enroll.OrderByDescending(s => s.Student.FirstMidName);
                    break;
                case "Judul":
                    enroll = enroll.OrderBy(s => s.Course.Title);
                    break;
                case "Nama":
                    enroll = enroll.OrderBy(s => s.Student.FirstMidName);
                    break;
                case "Grade":
                    enroll = enroll.OrderByDescending(s => s.Grade);
                    break;
                default:
                    enroll = enroll.OrderBy(s => s.Grade);
                    break;
            }

            if (!String.IsNullOrEmpty(model.CourseID))
            {
                enroll = enroll.Where(s => s.Course.Title.Contains(model.CourseID));
            }
            if (!String.IsNullOrEmpty(model.StudentID))
            {
                enroll = enroll.Where(s => s.Student.FirstMidName.Contains(model.StudentID));
            }
            if (!String.IsNullOrEmpty(model.Grade))
            {
                enroll = enroll.Where(s => s.Grade.ToString().Contains(model.Grade));
            }


            ViewBag.enrollments = enroll.ToList();
            return View("Index", model);
        }


        // GET: Pendaftaran/Details/5
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

        // GET: Pendaftaran/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "ID", "LastName");
            return View();
        }

        // POST: Pendaftaran/Create
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

        // GET: Pendaftaran/Edit/5
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

        // POST: Pendaftaran/Edit/5
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

        // GET: Pendaftaran/Delete/5
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

        // POST: Pendaftaran/Delete/5
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
