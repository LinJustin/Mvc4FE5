using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4EF5.Models;

namespace Mvc4EF5.DAL
{
    public class CourseRepository : GenericRepository<Course>
    {
        public CourseRepository(SchoolContext context) : base(context) { }

        public int UpdateCourseCredits(int multiplier)
        {
            return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }
    }
}