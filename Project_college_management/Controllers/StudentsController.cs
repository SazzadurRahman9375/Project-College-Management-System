using Project_college_management.Models;
using Project_college_management.ViewModels;
using Project_college_management.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;


namespace Project_college_management.Controllers
{
    [Authorize(Roles = "Admin, Members")]
    public class StudentsController : Controller
    {
        private readonly CollegeDbContext db = new CollegeDbContext();
        // GET: Students
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult StudentsTable(int pg = 1)
        {
            var data = db.Students.OrderBy(x => x.StudentId).ToPagedList(pg, 1);
            return PartialView("_StudentsTable", data);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentInputModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    StudentName = model.StudentName,
                    StudentEmail = model.StudentEmail,
                    StudentPhone = model.StudentPhone
                };
                string ext = Path.GetExtension(model.StudentPicture.FileName);
                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                model.StudentPicture.SaveAs(savePath);
                student.StudentPicture = f;
                db.Students.Add(student);
                db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("_Fail");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var data = db.Students.FirstOrDefault(x => x.StudentId == id);
            if (data == null) return new HttpNotFoundResult();
            var model = new StudentEditModel
            {
                StudentId = id,
                StudentName = data.StudentName,
                StudentEmail = data.StudentEmail,
                StudentPhone = data.StudentPhone,
            };
            ViewBag.CurrentPic = data.StudentPicture;

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(StudentEditModel model)
        {
            var s = db.Students.FirstOrDefault(x => x.StudentId == model.StudentId);

            if (ModelState.IsValid)
            {
                s.StudentName = model.StudentName;
                s.StudentEmail = model.StudentEmail;
                s.StudentPhone = model.StudentPhone;

                if (model.StudentPicture != null)
                {
                    string ext = Path.GetExtension(model.StudentPicture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                    model.StudentPicture.SaveAs(savePath);
                    s.StudentPicture = f;

                }
                db.SaveChanges();
                return PartialView("_Success");

            }
            return PartialView("_Fail");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var student = db.Students.FirstOrDefault(x => x.StudentId == id);
            if (student == null) return new HttpNotFoundResult();
            db.Students.Remove(student);
            db.SaveChanges();
            return Json(new { success = true });
        }



    }
}