using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc4EF5.Models;
using Mvc4EF5.DAL;

namespace Mvc4EF5.Controllers
{
    public class CourseController : Controller
    {
        //private SchoolContext db = new SchoolContext();
        private UnitOfWork unitOfWork = new UnitOfWork();

        //
        // GET: /Course/

        public ActionResult Index()
        {
            //var courses = db.Courses.Include(c => c.Department);
            var courses = unitOfWork.CourseRepository.Get(includeProperties: "Department");
            return View(courses.ToList());
        }

        //
        // GET: /Course/Details/5

        public ActionResult Details(int id = 0)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // GET: /Course/Create

        public ActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        //
        // POST: /Course/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Courses.Add(course);
                    //db.SaveChanges();
                    unitOfWork.CourseRepository.Insert(course);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            PopulateDepartmentsDropDownList();
            return View(course);
        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            //var departmentsQuery = from d in db.Departments
            //                       orderby d.Name
            //                       select d;
            var departmentsQuery = unitOfWork.DepartmentRepository.Get(orderBy: q => q.OrderBy(d => d.Name));
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "DepartmentID", "Name", selectedDepartment);
        } 

        //
        // GET: /Course/Edit/5

        public ActionResult Edit(int id = 0)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);
            PopulateDepartmentsDropDownList(course.DepartmentID);
            return View(course);
        }

        //
        // POST: /Course/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits,DepartmentID")]Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(course).State = EntityState.Modified;
                    //db.SaveChanges();
                    unitOfWork.CourseRepository.Update(course);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateDepartmentsDropDownList();
            return View(course);
        }

        //
        // GET: /Course/Delete/5

        public ActionResult Delete(int id = 0)
        {
            //Course course = db.Courses.Find(id);
            Course course = unitOfWork.CourseRepository.GetByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        //
        // POST: /Course/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Course course = db.Courses.Find(id);
            //db.Courses.Remove(course);
            //db.SaveChanges();
            Course course = unitOfWork.CourseRepository.GetByID(id);
            unitOfWork.CourseRepository.Update(course);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}