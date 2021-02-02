using System;
using System.Collections.Generic;
using System.Text;

using Bit8.StudentSystem.Data.Models;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface ISemesterService
    {
        ICollection<Semester> GetAll();
    }
}
