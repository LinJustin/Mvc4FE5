using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4EF5.ViewModels
{
    public class AssignedCourseData
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
    }
}