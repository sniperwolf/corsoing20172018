using System;

namespace ServiceAPI.Dtos
{
    public class StudentDto : UserDto
    {
        // "1": Male, "0": Female
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
    }
}
