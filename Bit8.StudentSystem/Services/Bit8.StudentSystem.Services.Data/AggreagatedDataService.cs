using System.Collections.Generic;

using Bit8.StudentSystem.Data.Repository.Interfaces;
using Bit8.StudentSystem.Data.TransferModels;
using Bit8.StudentSystem.Services.Data.Interfaces;

namespace Bit8.StudentSystem.Services.Data
{
    public class AggreagatedDataService : IAggregatedDataService
    {
        private readonly IAggregatedDataRepository repository;

        public AggreagatedDataService(IAggregatedDataRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<TopStudent> GetTopTenStudents()
        {
            var result = this.repository.GetTopTenStudentsScores();
            return result;
        }

        public ICollection<Student> GetNoScoresStudents()
        {
            var result = this.repository.GetNoMarksStudents();
            return result;
        }
    }
}
