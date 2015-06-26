namespace Mvc4EF5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Enrollment", "StudentID", "dbo.Student");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Instructor");
            DropForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.Enrollment", new[] { "StudentID" });
            DropIndex("dbo.Department", new[] { "InstructorID" });
            DropIndex("dbo.OfficeAssignment", new[] { "InstructorID" });
            DropIndex("dbo.CourseInstructor", new[] { "InstructorID" });
            RenameColumn(table: "dbo.Enrollment", name: "StudentID", newName: "PersonID");
            RenameColumn(table: "dbo.Department", name: "InstructorID", newName: "PersonID");
            RenameColumn(table: "dbo.OfficeAssignment", name: "InstructorID", newName: "PersonID");
            RenameColumn(table: "dbo.CourseInstructor", name: "InstructorID", newName: "PersonID");
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        PersonID = c.Int(nullable: false, identity: true),
                        LastName = c.String(maxLength: 50),
                        FirstName = c.String(maxLength: 50),
                        EnrollmentDate = c.DateTime(),
                        HireDate = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        OldId = c.Int(nullable: false) //Add temporary key
                    })
                .PrimaryKey(t => t.PersonID);

            // Copy existing Student and Instructor data into new Person table.
            Sql("INSERT INTO dbo.Person (LastName, FirstName, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, null AS HireDate, EnrollmentDate, 'Student' AS Discriminator, StudentId AS OldId FROM dbo.Student");
            Sql("INSERT INTO dbo.Person (LastName, FirstName, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, HireDate, null AS EnrollmentDate, 'Instructor' AS Discriminator, InstructorId AS OldId FROM dbo.Instructor");

            // Fix up existing relationships to match new PK's.
            Sql("UPDATE dbo.Enrollment SET PersonId = (SELECT PersonId FROM dbo.Person WHERE OldId = Enrollment.PersonId AND Discriminator = 'Student')");
            Sql("UPDATE dbo.Department SET PersonId = (SELECT PersonId FROM dbo.Person WHERE OldId = Department.PersonId AND Discriminator = 'Instructor')");
            Sql("UPDATE dbo.OfficeAssignment SET PersonId = (SELECT PersonId FROM dbo.Person WHERE OldId = OfficeAssignment.PersonId AND Discriminator = 'Instructor')");
            Sql("UPDATE dbo.CourseInstructor SET PersonId = (SELECT PersonId FROM dbo.Person WHERE OldId = CourseInstructor.PersonId AND Discriminator = 'Instructor')");

            // Remove temporary key
            DropColumn("dbo.Person", "OldId");
            
            AddForeignKey("dbo.Enrollment", "PersonID", "dbo.Person", "PersonID", cascadeDelete: true);
            AddForeignKey("dbo.Department", "PersonID", "dbo.Person", "PersonID");
            AddForeignKey("dbo.OfficeAssignment", "PersonID", "dbo.Person", "PersonID");
            AddForeignKey("dbo.CourseInstructor", "PersonID", "dbo.Person", "PersonID", cascadeDelete: true);
            CreateIndex("dbo.Enrollment", "PersonID");
            CreateIndex("dbo.Department", "PersonID");
            CreateIndex("dbo.OfficeAssignment", "PersonID");
            CreateIndex("dbo.CourseInstructor", "PersonID");
            DropTable("dbo.Student");
            DropTable("dbo.Instructor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        InstructorID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InstructorID);
            
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        StudentID = c.Int(nullable: false, identity: true),
                        LastName = c.String(maxLength: 20),
                        FirstName = c.String(maxLength: 20),
                        EnrollmentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StudentID);
            
            DropIndex("dbo.CourseInstructor", new[] { "PersonID" });
            DropIndex("dbo.OfficeAssignment", new[] { "PersonID" });
            DropIndex("dbo.Department", new[] { "PersonID" });
            DropIndex("dbo.Enrollment", new[] { "PersonID" });
            DropForeignKey("dbo.CourseInstructor", "PersonID", "dbo.Person");
            DropForeignKey("dbo.OfficeAssignment", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Department", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Enrollment", "PersonID", "dbo.Person");
            DropTable("dbo.Person");
            RenameColumn(table: "dbo.CourseInstructor", name: "PersonID", newName: "InstructorID");
            RenameColumn(table: "dbo.OfficeAssignment", name: "PersonID", newName: "InstructorID");
            RenameColumn(table: "dbo.Department", name: "PersonID", newName: "InstructorID");
            RenameColumn(table: "dbo.Enrollment", name: "PersonID", newName: "StudentID");
            CreateIndex("dbo.CourseInstructor", "InstructorID");
            CreateIndex("dbo.OfficeAssignment", "InstructorID");
            CreateIndex("dbo.Department", "InstructorID");
            CreateIndex("dbo.Enrollment", "StudentID");
            AddForeignKey("dbo.CourseInstructor", "InstructorID", "dbo.Instructor", "InstructorID", cascadeDelete: true);
            AddForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Instructor", "InstructorID");
            AddForeignKey("dbo.Department", "InstructorID", "dbo.Instructor", "InstructorID");
            AddForeignKey("dbo.Enrollment", "StudentID", "dbo.Student", "StudentID", cascadeDelete: true);
        }
    }
}
