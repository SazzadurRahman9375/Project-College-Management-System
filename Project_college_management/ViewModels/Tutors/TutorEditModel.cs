using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_college_management.ViewModels
{
    public class TutorEditModel
    {
        public int TutorId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Full Name")]
        public string TutorName { get; set; }
        [Required, StringLength(15)]
        public string TutorPhone { get; set; }
        [Required, StringLength(40)]
        public string TutorEmail { get; set; }
        public HttpPostedFileBase TutorPicture { get; set; }
        public virtual Course Course { get; set; }

    }
}