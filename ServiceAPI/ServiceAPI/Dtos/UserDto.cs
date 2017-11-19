using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceAPI.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RegistrationNumber { get; set; }
        public string Password { get; set; }
        public Boolean Role { get; set; }
    }
}
