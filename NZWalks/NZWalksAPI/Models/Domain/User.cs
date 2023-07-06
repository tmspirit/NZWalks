using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalksAPI.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }

        //Navigation
        public List<User_Role> UserRole { get; set; }

    }
}
