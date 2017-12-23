using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class Travel
    {
        public Guid Id { get; set; }
        public bool NeedTravel { get; set; }
        public TravelStatus Status { get; set; }
        public Student Student { get; set; }
    }

    public enum TravelStatus
    {
        AtScholl = 70,
        AtHome  = 75,
        Trasnporting = 80
    }
}
