namespace NZWalksAPI.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public String Name { get; set; }

        //Navigation
        public List<User_Role> UserRole { get; set; }
    }
}
