using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4EF5.Models;

namespace Mvc4EF5.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }
    }
}