using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc4EF5.Models;

namespace Mvc4EF5.DAL
{
    public interface IStudentRepository : IDisposable
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentByID(int studentId);
        void InsertStudent(Student student);
        void DeleteStudent(int studentID);
        void UpdateStudent(Student student);
        void Save();
    }
}