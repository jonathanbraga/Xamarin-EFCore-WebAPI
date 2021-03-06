﻿using System;
using System.Collections.Generic;

namespace VanEscolar.Domain
{
    public class Student
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool NeedTravel { get; set; }
        public DateTime StartScholl { get; set; }
        public DateTime EndScholl { get; set; }
        public Parent Parent { get; set; }
        public School School { get; set; }
        public List<TravelStudent> TravelsStudent { get; set; }
    }
}
