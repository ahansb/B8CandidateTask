using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Services.Data.Interfaces
{
    public interface IAggregatedDataService
    {
        ICollection<TopStudent> GetTopTenStudents();
        ICollection<Student> GetNoScoresStudents();
    }
}
