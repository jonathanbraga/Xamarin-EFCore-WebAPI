using System;
using System.Collections.Generic;

namespace VanEscolar.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime StartScholl { get; set; }
        public DateTime EndScholl { get; set; }
        public Parent Parent { get; set; }
        public Scholl Scholl { get; set; }
        public Travel Travel { get; set; }
    }
}
