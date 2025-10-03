using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Services
{
    public class TeacherService
    {
        private readonly SchoolDbContext _context;
        public TeacherService(SchoolDbContext context)
        {
            _context = context;
        }

      
        public Teacher[] GetAllTeachersByStudent(string studentName)
        {
            return _context.Teachers
                .Where(t => t.Pupils.Any(p => p.FirstName == studentName))
                .Include(t => t.Pupils)
                .ToArray();
        }
    }

}