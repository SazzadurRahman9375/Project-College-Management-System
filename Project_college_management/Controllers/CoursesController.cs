using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Project_college_management.ViewModels.Courses;
using X.PagedList;
using System.IO;

namespace Project_college_management.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CollegeDbContext db = new CollegeDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CoursesTable(int pg = 1)
        {
            var data = db.Courses.Include(x => x.Tutors).OrderBy(x => x.CourseId).ToPagedList(pg, 1);
            return PartialView("_CoursesPartialTable", data);
        }

        public ActionResult Create()
        {
            var data = new CourseInputModel();
            data.Tutors.Add(new Tutor());
            ViewBag.Tutors = db.Tutors.ToList();
            return View(data);

        }
        [HttpPost]
        public ActionResult Create(CourseInputModel model, string act = "")
        {

            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    CourseName = model.CourseName,
                    CourseFee = model.CourseFee,
                    DurationYear = model.DurationYear,
                    TutorId = model.TutorId
                };
                var t = db.Tutors.FirstOrDefault(x => x.TutorId == course.TutorId);
                course.Tutors.Add(t);
                db.Courses.Add(course);
                db.SaveChanges();
                ViewBag.Tutors = db.Tutors.ToList();
                return PartialView("_Success");
            }
            return PartialView("_Fail");


        }


        public ActionResult Edit(int id)
        {
            var model = new CourseEditModel();
            var data = db.Courses.FirstOrDefault(x => x.CourseId == id);
            model.CourseName = data.CourseName;
            model.CourseFee = data.CourseFee;
            model.DurationYear = data.DurationYear;
            model.TutorId = data.TutorId;
            model.CourseId = data.CourseId;
            model.Tutors  =data.Tutors.ToList();

            if (data == null) return new HttpNotFoundResult();

            ViewBag.Tutors = db.Tutors.ToList();
            return View(model);


        }
        [HttpPost]
        public ActionResult Edit(CourseEditModel model)
        {
            var course = db.Courses.FirstOrDefault(x => x.CourseId == model.CourseId);

            if (ModelState.IsValid)
            {
                course.CourseName = model.CourseName;
                course.CourseFee = model.CourseFee;
                course.DurationYear = model.DurationYear;
                course.TutorId = model.TutorId;

                db.SaveChanges();
                ViewBag.Tutors = db.Tutors.ToList();
                return PartialView("_Success");

            }
            return PartialView("_Fail");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var course = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (course == null) return new HttpNotFoundResult();
            db.Courses.Remove(course);
            db.SaveChanges();
            return Json(new { success = true });
        }



    }
}