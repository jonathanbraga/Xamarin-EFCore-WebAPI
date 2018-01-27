using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class Queue
    {
        public Guid Id { get; set; }
        public int QueuePosition { get; set; }
        public string Name { get; set; }
        public List<QueueMember> QueueMembers { get; set; }
    }
}
