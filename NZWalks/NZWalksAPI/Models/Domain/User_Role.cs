﻿namespace NZWalksAPI.Models.Domain
{
    public class User_Role
    {
        public Guid Id { get; set; }
        //NAvigation propperties
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
