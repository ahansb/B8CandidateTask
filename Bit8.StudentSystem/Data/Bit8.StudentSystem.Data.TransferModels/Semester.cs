using System;
using System.Collections.Generic;
using System.Text;

namespace Bit8.StudentSystem.Data.TransferModels
{
    public class Semester
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Discipline> Disciplines { get; set; }
    }
}
