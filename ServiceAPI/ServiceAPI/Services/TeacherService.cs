using System;
using System.Collections.Generic;
using System.Linq;
using ServiceAPI.Dal;
using ServiceAPI.Dtos;
using ServiceAPI.Entities;
using ServiceAPI.Helpers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServiceAPI.Services
{
    public interface ITeacherService
    {
        Teacher Create(Teacher teacher, string password);
        void Update(Teacher teacherParam, string password);
        void Delete(int id);

        IEnumerable<Teacher> GetAll();
        Teacher GetById(int id);
    }

    public class TeacherService : ITeacherService
    {
        private ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Teacher Create(Teacher teacher, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Teachers.Any(x => x.RegistrationNumber == teacher.RegistrationNumber))
            {
                throw new AppException("RegistrationNumber " + teacher.RegistrationNumber + " is already taken");
            }

            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            teacher.PasswordHash = passwordHash;
            teacher.PasswordSalt = passwordSalt;

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            return (teacher);
        }

        public void Update(Teacher teacherParam, string password = null)
        {
            var teacher = _context.Teachers.Find(teacherParam.Id);

            if (teacher == null)
            {
                throw new AppException("Teacher not found");
            }

            if (teacherParam.RegistrationNumber != teacher.RegistrationNumber)
            {
                // registrationNumber has changed so check if the new registrationNumber is already taken
                if (_context.Teachers.Any(x => x.RegistrationNumber == teacherParam.RegistrationNumber))
                {
                    throw new AppException("RegistrationNumber " + teacherParam.RegistrationNumber + " is already taken");
                }
            }

            // update teacher properties
            teacher.FirstName = teacherParam.FirstName;
            teacher.LastName = teacherParam.LastName;
            teacher.RegistrationNumber = teacherParam.RegistrationNumber;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                teacher.PasswordHash = passwordHash;
                teacher.PasswordSalt = passwordSalt;
            }

            _context.Teachers.Update(teacher);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var teacher = _context.Teachers.Find(id);

            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Teacher> GetAll()
        {
            return (_context.Teachers);
        }

        /*public async Task<Teacher> GetById(int id)
         * {
         *  return await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);
         * }*/
        public Teacher GetById(int id)
        {
            return (_context.Teachers.Find(id));
        }
    }
}
