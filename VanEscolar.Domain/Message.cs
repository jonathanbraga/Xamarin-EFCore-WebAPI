using System;
using System.Collections.Generic;
using System.Text;

namespace VanEscolar.Domain
{
    public class Message
    {
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public MessageType MessageType { get; set; }
        public Parent Parent { get; set; }

    }

    public enum MessageType
    {
        Info = 59,
        Status = 79
    }

}
