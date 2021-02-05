using System.Collections.Generic;

using Bit8.StudentSystem.Data.TransferModels;

namespace Bit8.StudentSystem.Data.Repository.Interfaces
{
    public interface IAggregatedDataRepository
    {
        ICollection<TopStudent> GetTopTenStudentsScores();
        ICollection<Student> GetNoMarksStudents();
    }
}
