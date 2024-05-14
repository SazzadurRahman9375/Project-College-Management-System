using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using X.PagedList;
using System.IO;
using Project_college_management.ViewModels.Courses;
using Project_college_management.ViewModels;

namespace Project_college_management.Controllers
{
    [Authorize(Roles = "Admin, Members")]
    public class EnrollmentsController : Controller
    {
      
        private readonly CollegeDbContext db = new CollegeDbContext();
        // GET: Enrollment
        public ActionResult Index(int pg=1)
        {
            var data = db.Enrollments.Include(x=>x.Student).Include(y=>y.Course).ToList();
            return View(data.OrderBy(x=>x.StudentId).ToPagedList(pg,1));
        }
        public ActionResult Create()
        {
            var data = new StudentEnrollmentModel();
            ViewBag.Courses = db.Courses.ToList();
            ViewBag.Tutors = db.Tutors.ToList();
            return View(data);

        }
        [HttpPost]
        public ActionResult Create(StudentEnrollmentModel model, string act = "")
        {
            if (act == "insert")
            {
                var course = new Course
                {
                    CourseName = model.CourseName,
                    CourseFee = model.CourseFee,
                    DurationYear = model.DurationYear,
                    TutorId = model.TutorId
                };
                db.Courses.Add(course);
                db.SaveChanges();

            }

            if (ModelState.IsValid)
            {
                var enrollment = new Enrollment
                {
                    CourseId = model.CourseId,
                    EnrollmentDate = model.EnrollmentDate,
                    Paymentdate = model.Paymentdate,
                    CourseFeePayment = model.CourseFeePayment,


                };

                //enrollment.Student.StudentName = model.StudentName;
                //enrollment.Student.StudentEmail = model.StudentEmail;
                //enrollment.Student.StudentPhone = model.StudentPhone;


                var student = new Student
                {
                    StudentName = model.StudentName,
                    StudentPhone = model.StudentPhone,
                    StudentEmail = model.StudentEmail

                };
                string ext = Path.GetExtension(model.StudentPicture.FileName);
                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                model.StudentPicture.SaveAs(savePath);
                student.StudentPicture = f;
                //enrollment.Student.StudentPicture = f;

                    db.Students.Add(student);  
                    db.Enrollments.Add(enrollment);


                db.SaveChanges();
                ViewBag.Courses = db.Courses.ToList();
                ViewBag.Tutors = db.Tutors.ToList();
                return PartialView("_Success");
            }
            return PartialView("_Fail");


        }

        //[HttpPost]
        //public ActionResult Save(StudentEnrollmentModel model,string act="")
        //{
        //    var course = new Course
        //    {
        //        CourseName = model.CourseName,
        //        CourseFee = model.CourseFee,
        //        DurationYear = model.DurationYear,
        //        TutorId = model.TutorId
        //    };
        //    db.Courses.Add(course);
        //    return View(model);
        //}




    }

}