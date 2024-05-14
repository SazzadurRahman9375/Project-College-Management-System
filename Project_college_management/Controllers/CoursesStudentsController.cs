using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using X.PagedList;
using Project_college_management.ViewModels.Courses;
using System.IO;

namespace Project_college_management.Controllers
{
    public class CoursesStudentsController : Controller
    {
        private readonly CollegeDbContext db = new CollegeDbContext();
        // GET: CoursesStudents
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult CoursesStudentsTable(int pg = 1)
        {
            var data = db.Courses.Include(x=>x.Enrollments).Include(x=>x.Tutors).OrderBy(x=>x.CourseId).ToPagedList(pg,1);
            return PartialView("_CoursesStudentsTable", data);
        }

        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            CourseInputModel model = new CourseInputModel();
            model.Enrollments.Add(new Enrollment());
            ViewBag.Tutors = db.Tutors.ToList();
            ViewBag.Students = db.Students.ToList();
            return PartialView("_CreateForm", model);
        }

        [HttpPost]
        public ActionResult Create(CourseInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Enrollments.Add(new Enrollment());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Enrollments.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "insert")
            {
                if (ModelState.IsValid)
                {
                    var course = new Course
                    {
                        CourseId = model.CourseId,
                        CourseName = model.CourseName,
                        CourseFee = model.CourseFee,
                        DurationYear = model.DurationYear,
                        TutorId = model.TutorId,
                    };
                    var t = db.Tutors.FirstOrDefault(x => x.TutorId == course.TutorId);
                    course.Tutors.Add(t);
                    db.Courses.Add(course);
                    db.SaveChanges();

                    foreach (var e in model.Enrollments)
                    {
                        db.Database.ExecuteSqlCommand($@"EXEC InsertEnrollment  {course.CourseId},{e.StudentId}, '{e.EnrollmentDate}', '{e.Paymentdate}',{e.CourseFeePayment}");
                    }
                    CourseInputModel newmodel = new CourseInputModel() { CourseName = "" };
                    newmodel.Enrollments.Add(new Enrollment());
                    ViewBag.Tutors = db.Tutors.ToList();
                    ViewBag.Students = db.Students.ToList();
                    foreach (var e in ModelState.Values)
                    {

                        e.Value = null;
                    }
                    return View("_CreateForm", newmodel);
                }
            }
            ViewBag.Tutors = db.Tutors.ToList();
            ViewBag.Students = db.Students.ToList();
            return View("_CreateForm", model);
        }

        public ActionResult Edit(int id)
        {

            ViewBag.Id = id;
            return View();
        }


        public ActionResult EditForm(int id)
        {
            var data = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.Enrollments).Load();
            CourseEditModel model = new CourseEditModel
            {
                CourseId = id,
                CourseName = data.CourseName,
                DurationYear = data.DurationYear,
                CourseFee = data.CourseFee,
                TutorId = data.TutorId,       
                Enrollments = data.Enrollments.ToList()
            };
            ViewBag.Tutors = db.Tutors.ToList();
            ViewBag.Students = db.Students.ToList();
            return PartialView("_EditForm", model);
        }

        [HttpPost]
        public ActionResult Edit(CourseEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.Enrollments.Add(new Enrollment());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.Enrollments.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var course = db.Courses.FirstOrDefault(x => x.CourseId == model.CourseId);
                    if (course == null) { return new HttpNotFoundResult(); }

                    course.CourseName = model.CourseName;
                    course.CourseFee = model.CourseFee;
                    course.DurationYear = model.DurationYear;
                    course.TutorId = model.TutorId;
                    db.SaveChanges();

                    //var enrollment  = db.Enrollments.Where(x=>x.CourseId == model.CourseId).ToList();
                    //for(var i  =0;i< enrollment.Count; i++)
                    //{
                    //    db.Enrollments.Remove(enrollment[i]);
                    //    course.Enrollments.Remove(enrollment[i]);
                    //}

                    //foreach (var s in model.Enrollments)
                    //{
                    //    course.Enrollments.Add(s);
                    //}
                    //db.SaveChanges();
                    //CourseEditModel newmodel = new CourseEditModel() { CourseName = "" };
                    //newmodel.Enrollments.Add(new Enrollment());
                    //ViewBag.Tutors = db.Tutors.ToList();
                    //ViewBag.Students = db.Students.ToList();
                    //foreach (var e in ModelState.Values)
                    //{

                    //    e.Value = null;
                    //}
                    //return View("_EditForm", newmodel);

                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC DeleteEnrollmentByCourseId {course.CourseId}");
                    foreach (var e in model.Enrollments)
                    {
                        db.Database.ExecuteSqlCommand($@"EXEC InsertEnrollment {course.CourseId},{e.StudentId}, '{e.EnrollmentDate}', '{e.Paymentdate}',{e.CourseFeePayment}");
                    }


                }
            }
            ViewBag.Tutors = db.Tutors.ToList();
            ViewBag.Students = db.Students.ToList();
            return View("_EditForm", model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var course = db.Courses.FirstOrDefault(x => x.CourseId == id);
            if (course == null) return new HttpNotFoundResult();
            //db.Tutors.RemoveRange(course.Tutors.ToList());
            db.Enrollments.RemoveRange(course.Enrollments.ToList());
            db.Courses.Remove(course);
            db.SaveChanges();


            return Json(new { success = true });
        }



    }
}