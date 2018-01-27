using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class QueueMember
    {
        public Guid Id { get; set; }
        public string StudentName { get; set; }
        public string StudentID { get; set; }
        public int QueuOrder { get; set; }
        public string SchoolName { get; set; }
        public QueueMemberStatus Status { get; set; }
        public Queue Queue { get; set; }
    }

    public enum QueueMemberStatus
    {
        Next = 375,
        OnQueue = 587
    }
}
