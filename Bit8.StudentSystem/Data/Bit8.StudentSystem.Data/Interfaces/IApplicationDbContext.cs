using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace Bit8.StudentSystem.Data.Interfaces
{
    public interface IApplicationDbContext
    {
        MySqlConnection Connection { get; }
        void Initialize();
        string GetDatabaseName();
    }
}
