using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IStudentRepository
    {
        ICollection<Student> All();
        Student GetById(int id);
        int Add(Student student);
    }
}
