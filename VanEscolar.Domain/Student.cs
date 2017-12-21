using System;
using System.Collections.Generic;

namespace VanEscolar.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartScholl { get; set; }
        public DateTime EndScholl { get; set; }
        public int ParentId { get; set; }
        public int SchollId { get; set; }
    }
}
