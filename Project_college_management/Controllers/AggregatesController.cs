using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_college_management.Controllers
{
    public class AggregatesController : Controller
    {
        private readonly CollegeDbContext db = new CollegeDbContext();
        // GET: Aggregates
        public ActionResult Index()
        {
            var data = db.Courses.ToList();
            return View(data);
        }
    }
}