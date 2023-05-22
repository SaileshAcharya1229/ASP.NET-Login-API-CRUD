﻿namespace ConsumeWebApi.Models
{
    public class UserEntity
    {

        public string FullName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        public string Address { get; set; }
        public Guid Id { get; internal set; }
    }
}
