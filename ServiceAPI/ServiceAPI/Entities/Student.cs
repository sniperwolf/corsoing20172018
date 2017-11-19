using System;

namespace ServiceAPI.Entities
{
    public class Student : User
    {
        // "1": Male, "0": Female
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; }
    }
}
