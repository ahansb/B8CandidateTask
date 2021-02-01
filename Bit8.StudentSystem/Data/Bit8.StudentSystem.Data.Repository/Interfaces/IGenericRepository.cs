using System;
using System.Collections.Generic;
using System.Text;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        ICollection<T> All();
    }
}
