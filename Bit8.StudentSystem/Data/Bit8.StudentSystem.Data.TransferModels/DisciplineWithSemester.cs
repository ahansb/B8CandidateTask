using System;

namespace Bit8.StudentSystem.Data.TransferModels
{
    public class DisciplineWithSemester
    {
        public int Id { get; set; }

        public string DisciplineName { get; set; }

        public string ProfessorName { get; set; }

        public DisciplinesSemester Semester { get; set; }
    }
}
