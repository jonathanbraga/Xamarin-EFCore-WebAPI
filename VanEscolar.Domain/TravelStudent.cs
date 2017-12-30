using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class TravelStudent
    {
        public Guid Id { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime FinishAt { get; set; }
        public Student Student { get; set; }
        public Travel Travel { get; set; }
    }
}
