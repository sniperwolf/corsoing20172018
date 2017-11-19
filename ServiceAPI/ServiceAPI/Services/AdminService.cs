using System;
using System.Collections.Generic;
using System.Linq;
using ServiceAPI.Dal;
using ServiceAPI.Dtos;
using ServiceAPI.Entities;
using ServiceAPI.Helpers;

namespace ServiceAPI.Services
{
    public interface IAdminService
    {
        Admin Create(Admin admin, string password);
        void Update(Admin adminParam, string password = null);
        void Delete(int id);

        IEnumerable<Admin> GetAll();
        Admin GetById(int id);
    }

    public class AdminService : IAdminService
    {
        private ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Admin Create(Admin admin, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Admins.Any(x => x.RegistrationNumber == admin.RegistrationNumber))
            {
                throw new AppException("RegistrationNumber " + admin.RegistrationNumber + " is already taken");
            }

            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;

            _context.Admins.Add(admin);
            _context.SaveChanges();

            return (admin);
        }

        public void Update(Admin adminParam, string password = null)
        {
            var admin = _context.Admins.Find(adminParam.Id);

            if (admin == null)
            {
                throw new AppException("Admin not found");
            }

            if (adminParam.RegistrationNumber != admin.RegistrationNumber)
            {
                // registrationNumber has changed so check if the new registrationNumber is already taken
                if (_context.Admins.Any(x => x.RegistrationNumber == adminParam.RegistrationNumber))
                {
                    throw new AppException("RegistrationNumber " + adminParam.RegistrationNumber + " is already taken");
                }
            }

            // update admin properties
            admin.FirstName = adminParam.FirstName;
            admin.LastName = adminParam.LastName;
            admin.RegistrationNumber = adminParam.RegistrationNumber;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                admin.PasswordHash = passwordHash;
                admin.PasswordSalt = passwordSalt;
            }

            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var admin = _context.Admins.Find(id);

            if (admin != null)
            {
                _context.Admins.Remove(admin);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Admin> GetAll()
        {
            return (_context.Admins);
        }

        public Admin GetById(int id)
        {
            return (_context.Admins.Find(id));
        }
    }
}
