using Models;
using System.Collections.Generic;
using System.Linq;

namespace Service.Collections
{
    public class UsersCollection
    {
        public static List<User> GetUsers()
        {
            return (new[] {
                new User { Id = 1, Name = "Juan Perez" },
                new User { Id = 2, Name = "Luis Lopez" },
                new User { Id = 3, Name = "Maria Hernandez" },
                new User { Id = 4, Name = "Ana Sanchez" }
            }).ToList();
        }
    }
}