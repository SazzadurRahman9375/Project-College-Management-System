namespace Project_college_management.Migrations.ContextA
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        CourseName = c.String(nullable: false, maxLength: 50),
                        DurationYear = c.Int(nullable: false),
                        CourseFee = c.Decimal(nullable: false, storeType: "money"),
                        TutorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.Enrollments",
                c => new
                    {
                        EnrollmentId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        EnrollmentDate = c.DateTime(nullable: false, storeType: "date"),
                        Paymentdate = c.DateTime(nullable: false, storeType: "date"),
                        CourseFeePayment = c.Decimal(nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.EnrollmentId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.CourseId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        StudentName = c.String(nullable: false, maxLength: 50),
                        StudentPhone = c.String(nullable: false, maxLength: 15),
                        StudentEmail = c.String(nullable: false, maxLength: 40),
                        StudentPicture = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.Tutors",
                c => new
                    {
                        TutorId = c.Int(nullable: false, identity: true),
                        TutorName = c.String(nullable: false, maxLength: 50),
                        TutorPhone = c.String(nullable: false, maxLength: 15),
                        TutorEmail = c.String(nullable: false, maxLength: 40),
                        TutorPicture = c.String(nullable: false, maxLength: 50),
                        Course_CourseId = c.Int(),
                    })
                .PrimaryKey(t => t.TutorId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId)
                .Index(t => t.Course_CourseId);

            /////////////////
            ///
            CreateStoredProcedure("InsertCourse",
c => new
{
CourseName = c.String(maxLength: 50),
DurationYear = c.Int(),
CourseFee = c.Decimal(),
TutorId = c.Int()
}
, @"INSERT INTO Courses(CourseName , DurationYear,CourseFee ,TutorId)
                    VALUES
                    (@CourseName, @DurationYear,@CourseFee,@TutorId)
                    SELECT SCOPE_IDENTITY() AS CourseId
                    RETURN ");

            CreateStoredProcedure("UpdateCourse",
    c => new
    {
        CourseId = c.Int(),
        CourseName = c.String(maxLength: 50),
        DurationYear = c.Int(),
        CourseFee = c.Decimal(),
        TutorId = c.Int()
    }
                    , @"UPDATE Courses
                    SET
                    CourseName=@CourseName, 
                    DurationYear=@DurationYear,
                    CourseFee=@CourseFee,
                    TutorId=@TutorId
                    WHERE CourseId=@CourseId
                    SELECT SCOPE_IDENTITY() AS CourseId
                    RETURN ");


            CreateStoredProcedure("DeleteCourse", c => new
            {
                CourseId = c.Int()

            }, @"DELETE FROM Courses
                WHERE CourseId = @CourseId
            RETURN");


            CreateStoredProcedure("DeleteEnrollmentByCourseId", c => new
            {
                CourseId = c.Int()

            }, @"DELETE FROM Enrollments
                WHERE CourseId = @CourseId
            RETURN");

            CreateStoredProcedure("InsertEnrollment",
                c => new
                {
                    CourseId = c.Int(),
                    StudentId = c.Int(),
                    EnrollmentDate = c.DateTime(),
                    Paymentdate = c.DateTime(),
                    CourseFeePayment = c.Decimal(),

                }, @"INSERT INTO Enrollments (CourseId,StudentId, EnrollmentDate,Paymentdate,CourseFeePayment)
                    VALUES
                    (@CourseId,@StudentId, @EnrollmentDate,@Paymentdate,@CourseFeePayment)
                    SELECT SCOPE_IDENTITY() as EnrollmentId
                    RETURN");


        }

        public override void Down()
        {
            DropForeignKey("dbo.Tutors", "Course_CourseId", "dbo.Courses");
            DropForeignKey("dbo.Enrollments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Enrollments", "CourseId", "dbo.Courses");
            DropIndex("dbo.Tutors", new[] { "Course_CourseId" });
            DropIndex("dbo.Enrollments", new[] { "StudentId" });
            DropIndex("dbo.Enrollments", new[] { "CourseId" });
            DropTable("dbo.Tutors");
            DropTable("dbo.Students");
            DropTable("dbo.Enrollments");
            DropTable("dbo.Courses");
        }
    }
}
