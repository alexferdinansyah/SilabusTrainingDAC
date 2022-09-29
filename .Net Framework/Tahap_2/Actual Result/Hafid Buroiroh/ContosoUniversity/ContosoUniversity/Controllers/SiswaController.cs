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
    public class SiswaController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Siswa
        public ActionResult Index()
        {
            ViewBag.students = db.Students.ToList();
            StudentSearchVM model = new StudentSearchVM();
            return View(model);
        }

        public ActionResult IndexProcess(StudentSearchVM model, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "last_name" : "";
            ViewBag.FirstMidNameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstMidName" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            IEnumerable<Student> students = from s in db.Students
                select s; 
            switch (sortOrder)
            {
                case "last_name":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "FirstMidName":
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    students = students.OrderBy(s => s.FirstMidName);
                    break;
            }
                if (model.EnrollmentDateFrom != null && model.EnrollmentDateUntil != null)
                {
                    students = students.Where(s => s.EnrollmentDate >= model.EnrollmentDateFrom && s.EnrollmentDate <= model.EnrollmentDateUntil);
                }
                if (!String.IsNullOrEmpty(model.LastName))
                {
                    students = students.Where(s => s.LastName.Contains(model.LastName));
                }
                if (!String.IsNullOrEmpty(model.FirstMidName))
                {
                    students = students.Where(s => s.FirstMidName.Contains(model.FirstMidName));
                }



            ViewBag.students = students.ToList();
            return View("Index", model);
        }

        // GET: Siswa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Siswa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Siswa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate,Secret")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Siswa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Siswa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate,Secret")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Siswa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Siswa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
