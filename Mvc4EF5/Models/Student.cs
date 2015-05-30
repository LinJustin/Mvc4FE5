﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mvc4EF5.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(20, ErrorMessage="max len => 20!!!")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Column("FirstName")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }

        public string FullName
        {
            get { return LastName + ',' + FirstMidName; }
        }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}