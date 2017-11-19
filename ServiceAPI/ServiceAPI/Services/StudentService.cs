using System;
using System.Collections.Generic;
using System.Linq;
using ServiceAPI.Dal;
using ServiceAPI.Dtos;
using ServiceAPI.Entities;
using ServiceAPI.Helpers;

namespace ServiceAPI.Services
{
    public interface IStudentService
    {
        Student Create(Student student, string password);
        void Update(Student studentParam, string password = null);
        void Delete(int id);

        IEnumerable<Student> GetAll();
        Student GetById(int id);
    }

    public class StudentService : IStudentService
    {
        private ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Student Create(Student student, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.Students.Any(x => x.RegistrationNumber == student.RegistrationNumber))
            {
                throw new AppException("RegistrationNumber " + student.RegistrationNumber + " is already taken");
            }

            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            _context.Students.Add(student);
            _context.SaveChanges();

            return (student);
        }

        public void Update(Student studentParam, string password = null)
        {
            var student = _context.Students.Find(studentParam.Id);

            if (student == null)
            {
                throw new AppException("Student not found");
            }

            if (studentParam.RegistrationNumber != student.RegistrationNumber)
            {
                // registrationNumber has changed so check if the new registrationNumber is already taken
                if (_context.Students.Any(x => x.RegistrationNumber == studentParam.RegistrationNumber))
                {
                    throw new AppException("RegistrationNumber " + studentParam.RegistrationNumber + " is already taken");
                }
            }

            // update student properties
            student.FirstName = studentParam.FirstName;
            student.LastName = studentParam.LastName;
            student.RegistrationNumber = studentParam.RegistrationNumber;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

                student.PasswordHash = passwordHash;
                student.PasswordSalt = passwordSalt;
            }

            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var student = _context.Students.Find(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Student> GetAll()
        {
            return (_context.Students);
        }

        public Student GetById(int id)
        {
            return (_context.Students.Find(id));
        }
    }
}
