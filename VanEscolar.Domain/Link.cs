using System;

namespace VanEscolar.Domain
{
    public class Link
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Manage = 40,
        Parente = 45,
        Student = 50
    }
}
