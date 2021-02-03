using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;
using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface IStudentService
    {
        ICollection<Student> GetAll();
        int Create(StudentCreateModel model);
    }
}
