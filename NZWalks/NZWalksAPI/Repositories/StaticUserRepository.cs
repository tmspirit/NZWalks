using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    FirsName="Read Only", LastName="User",EmailAddress="readonly@user.com",
            //    Id= Guid.NewGuid(), Username="readonly@user.com", Password="1234",
            //    Roles=new List<string>{"reader"}
            //},
            //new User()
            //{
            //    FirsName="Read Write", LastName="User",EmailAddress="readwrite@user.com",
            //    Id= Guid.NewGuid(), Username="readwrite@user.com", Password="12345",
            //    Roles=new List<string>{"reader","writer"}
            //}
        };
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user =  Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password == password);

            return user;
        }
    }
}
