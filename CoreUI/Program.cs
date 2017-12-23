using System;
using VanEscolar.Data;
using VanEscolar.Domain;

namespace CoreUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InserUser();
        }

        private static void InserUser()
        {
            var user = new User
            {
                Name = "Jonathan Braga",
                Authorize = true,
                CreatedAt = DateTime.UtcNow,
                Link = new Link { Role = Role.Manage},
                Email = "jonathanb2br@gmail.com",
                Password = "#Braga123",
                Phone = "96109343"
            };

            using (var context = new VanEscolarContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}
