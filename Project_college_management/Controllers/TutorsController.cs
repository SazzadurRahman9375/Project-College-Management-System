using Project_college_management.Models;
using Project_college_management.ViewModels;
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
    public class TutorsController : Controller
    {
        private readonly CollegeDbContext db = new CollegeDbContext();
        // GET: Tutors
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult TutorsTable(int pg = 1)
        {
            var data = db.Tutors.OrderBy(x => x.TutorId).ToPagedList(pg, 1);
            return PartialView("_TutorTableView", data);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TutorInputModel model)
        {
            if (ModelState.IsValid)
            {
                var tutor = new Tutor
                {
                    TutorName = model.TutorName,
                    TutorEmail = model.TutorEmail,
                    TutorPhone = model.TutorPhone,
                };
                string ext = Path.GetExtension(model.TutorPicture.FileName);
                string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                model.TutorPicture.SaveAs(savePath);
                tutor.TutorPicture = f;
                db.Tutors.Add(tutor);
                db.SaveChanges();
                return PartialView("_Success");
            }
            return PartialView("_Fail");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var data = db.Tutors.FirstOrDefault(x => x.TutorId == id);
            if (data == null) return new HttpNotFoundResult();
            var model = new TutorEditModel
            {
                TutorId = id,
                TutorName = data.TutorName,
                TutorEmail = data.TutorEmail,
                TutorPhone = data.TutorPhone,
            };
            ViewBag.CurrentPic = data.TutorPicture;

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(TutorEditModel model)
        {
            var t = db.Tutors.FirstOrDefault(x => x.TutorId == model.TutorId);

            if (ModelState.IsValid)
            {
                t.TutorName = model.TutorName;
                t.TutorEmail = model.TutorEmail;
                t.TutorPhone = model.TutorPhone;

                if (model.TutorPicture != null)
                {
                    string ext = Path.GetExtension(model.TutorPicture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Pictures"), f);
                    model.TutorPicture.SaveAs(savePath);
                    t.TutorPicture = f;

                }
                db.SaveChanges();
                return PartialView("_Success");

            }
            return PartialView("_Fail");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var tutor = db.Tutors.FirstOrDefault(x => x.TutorId == id);
            if (tutor == null) return new HttpNotFoundResult();
            db.Tutors.Remove(tutor);
            db.SaveChanges();
            return Json(new { success = true });
        }


    }
}