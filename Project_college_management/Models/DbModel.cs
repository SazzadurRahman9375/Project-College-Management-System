using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Project_college_management.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Full Name")]
        public string TutorName { get; set; }
        [Required, StringLength(15)]
        public string TutorPhone { get; set; }
        [Required, StringLength(40)]
        public string TutorEmail { get; set; }
        [Required, StringLength(50)]
        public string TutorPicture { get; set; }
        public virtual Course Course { get; set; }

    }

    public class Course
    {
        public int CourseId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        public int DurationYear { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CourseFee { get; set; }
        [Required,ForeignKey("Tutors")]
        public int TutorId { get; set; }

        public virtual ICollection<Tutor> Tutors { get; set; } = new List<Tutor>();
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
    public class Student
    {
        public int StudentId { get; set; }
        [Required, StringLength(50)]
        [Display(Name = "Full Name")]
        public string StudentName { get; set; }
        [Required, StringLength(15)]
        public string StudentPhone { get; set; }
        [Required, StringLength(40)]
        public string StudentEmail { get; set; }
        [Required,StringLength(50)]
        public string StudentPicture { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();


    }
    public class Enrollment
    {
        [Key, Column(Order = 0)]
        public int EnrollmentId { get; set; }
        [Required, ForeignKey("Course"),Display(Name ="Courses")]
        public int CourseId { get; set; }
        [Required, ForeignKey("Student")]
        public int? StudentId { get; set; }
        [Required, Column(TypeName = "date")]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        [Required, Column(TypeName = "date")]
        [Display(Name = "Payment Date")]
        public DateTime Paymentdate { get; set; }
        [Required, Column(TypeName = "money")]
        public decimal CourseFeePayment { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }

    }

    public class CollegeDbContext :DbContext
    {
        public CollegeDbContext()
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
    }


}