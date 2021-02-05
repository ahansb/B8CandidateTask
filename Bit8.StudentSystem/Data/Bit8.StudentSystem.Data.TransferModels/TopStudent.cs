using System;
using System.Collections.Generic;
using System.Text;

namespace Bit8.StudentSystem.Data.TransferModels
{
    public class TopStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public decimal AvarageScore { get; set; }
    }
}
