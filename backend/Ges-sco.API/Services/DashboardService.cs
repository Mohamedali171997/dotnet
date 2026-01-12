using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ges_sco.API.Database;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardStatsAsync()
        {
            var totalStudents = await _context.Students
                .Include(s => s.User)
                .CountAsync(s => s.User != null && s.User.IsActive);

            var totalTeachers = await _context.Teachers
                .Include(t => t.User)
                .CountAsync(t => t.User != null && t.User.IsActive);

            var totalClasses = await _context.Classes.CountAsync();
            var totalSubjects = await _context.Subjects.CountAsync();
            var totalCourses = await _context.Courses.CountAsync();

            // Calculate average attendance rate
            var attendances = await _context.Attendances.ToListAsync();
            double avgAttendanceRate = 0;
            if (attendances.Any())
            {
                var presentOrLate = attendances.Count(a => a.Status == "Present" || a.Status == "Late");
                avgAttendanceRate = Math.Round((double)presentOrLate / attendances.Count * 100, 2);
            }

            // Calculate average grade
            var grades = await _context.Grades.ToListAsync();
            double avgGrade = grades.Any() ? Math.Round(grades.Average(g => g.Value), 2) : 0;

            return new DashboardDto
            {
                TotalStudents = totalStudents,
                TotalTeachers = totalTeachers,
                TotalClasses = totalClasses,
                TotalSubjects = totalSubjects,
                TotalCourses = totalCourses,
                AverageAttendanceRate = avgAttendanceRate,
                AverageGrade = avgGrade,
                ActiveUsersToday = totalStudents + totalTeachers
            };
        }
    }
}
