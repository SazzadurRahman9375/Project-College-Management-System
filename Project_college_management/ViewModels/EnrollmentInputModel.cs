using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_college_management.ViewModels
{
    public class StudentEnrollmentModel
    {
        public int CourseId { get; set; }
        [Required, ForeignKey("Student")]
        public int StudentId { get; set; }
        [Required, Column(TypeName = "date")]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        [Required, Column(TypeName = "date")]
        [Display(Name = "Payment Date")]
        public DateTime Paymentdate { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CourseFeePayment { get; set; }
        public string StudentName { get; set; }
        [Required, StringLength(15)]
        public string StudentPhone { get; set; }
        [Required, StringLength(40)]
        public string StudentEmail { get; set; }
        [Required]
        public HttpPostedFileBase StudentPicture { get; set; }
        public string CourseName { get; set; }
        [Required]
        public int DurationYear { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CourseFee { get; set; }
        [Required, ForeignKey("Tutors")]
        public int TutorId { get; set; }



    }
}