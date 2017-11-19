using System;
using System.Collections.Generic;
using System.Linq;
using ServiceAPI.Dal;
using ServiceAPI.Dtos;
using ServiceAPI.Entities;
using ServiceAPI.Helpers;

namespace ServiceAPI.Services
{
    public interface IAuthService
    {
        User Authenticate(string registrationNumber, string password, Boolean role);
    }

    public class AuthService : IAuthService
    {
        private ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        private User FindUserByRole(string registrationNumber, Boolean role)
        {
            if (role)
            {
                return (_context.Teachers.SingleOrDefault(x => x.RegistrationNumber == registrationNumber));
            }

            return (_context.Students.SingleOrDefault(x => x.RegistrationNumber == registrationNumber));
        }

        public User Authenticate(string registrationNumber, string password, Boolean role)
        {
            if (string.IsNullOrEmpty(registrationNumber) || string.IsNullOrEmpty(password))
            {
                return (null);
            }

            var user = FindUserByRole(registrationNumber, role);

            // check if registrationNumber exists
            if (user == null)
            {
                return (null);
            }

            // check if password is correct
            if (!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return (null);
            }

            // authentication successful
            return (user);
        }
    }
}
