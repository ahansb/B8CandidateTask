using System;

namespace Bit8.StudentSystem.Data.TransferModels
{
    public class Discipline
    {
        public int Id { get; set; }

        public string DisciplineName { get; set; }

        public string ProfessorName { get; set; }

        public int SemesterId { get; set; }

        public Semester Semester { get; set; }

        public int? Score { get; set; }
    }
}
