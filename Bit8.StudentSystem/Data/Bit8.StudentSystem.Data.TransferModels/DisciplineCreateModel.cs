using System;
using System.Collections.Generic;
using System.Text;

namespace Bit8.StudentSystem.Data.TransferModels
{
    public class DisciplineCreateModel
    {
        public string DisciplineName { get; set; }

        public string ProfessorName { get; set; }

        public int SemesterId { get; set; }
    }
}
