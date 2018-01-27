using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class TravelQueue
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public QueueMemberStatus Status { get; set; }
        public string MemberQueue { get; set; }
    }
}
