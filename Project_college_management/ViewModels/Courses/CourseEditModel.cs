using Project_college_management.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_college_management.ViewModels.Courses
{
    public class CourseEditModel
    {
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        public int DurationYear { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CourseFee { get; set; }
        [Required, ForeignKey("Tutors")]
        public int TutorId { get; set; }

        public List<Tutor> Tutors { get; set; } = new List<Tutor>();
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}