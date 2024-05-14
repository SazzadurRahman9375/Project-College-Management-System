using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_college_management.ViewModels.Students
{
    public class StudentInputModel
    {
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Full Name")]
        public string StudentName { get; set; }
        [Required, StringLength(15)]
        public string StudentPhone { get; set; }
        [Required, StringLength(40)]
        public string StudentEmail { get; set; }
        [Required]
        public HttpPostedFileBase StudentPicture { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}