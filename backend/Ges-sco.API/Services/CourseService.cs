using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ges_sco.API.Database;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;
using Ges_sco.API.Models;
using Ges_sco.API.Repositories;

namespace Ges_sco.API.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public CourseService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<CourseDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .ToListAsync();

            return courses.Select(MapToDto);
        }

        public async Task<CourseDto?> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            return course == null ? null : MapToDto(course);
        }

        public async Task<CourseDto> CreateAsync(CreateCourseDto dto)
        {
            var course = new Course
            {
                SubjectId = dto.SubjectId,
                ClassId = dto.ClassId,
                TeacherId = dto.TeacherId
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.CompleteAsync();

            var createdCourse = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .FirstAsync(c => c.Id == course.Id);

            return MapToDto(createdCourse);
        }

        public async Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return null;

            if (dto.SubjectId.HasValue)
                course.SubjectId = dto.SubjectId.Value;
            if (dto.ClassId.HasValue)
                course.ClassId = dto.ClassId.Value;
            if (dto.TeacherId.HasValue)
                course.TeacherId = dto.TeacherId.Value;

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.CompleteAsync();

            // Reload relationships
            await _context.Entry(course).Reference(c => c.Subject).LoadAsync();
            await _context.Entry(course).Reference(c => c.Class).LoadAsync();
            await _context.Entry(course).Reference(c => c.Teacher).LoadAsync();
            if (course.Teacher != null)
                await _context.Entry(course.Teacher).Reference(t => t.User).LoadAsync();

            return MapToDto(course);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null)
                return false;

            _unitOfWork.Courses.Remove(course);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<GradeDto>> GetCourseGradesAsync(int courseId)
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(g => g.CourseId == courseId)
                .ToListAsync();

            return grades.Select(g => new GradeDto
            {
                Id = g.Id,
                Value = g.Value,
                Coefficient = g.Coefficient,
                Type = g.Type,
                Comment = g.Comment,
                Date = g.Date,
                StudentId = g.StudentId,
                StudentName = g.Student?.User != null ? $"{g.Student.User.FirstName} {g.Student.User.LastName}" : "",
                CourseId = g.CourseId,
                SubjectName = g.Course?.Subject?.Name ?? ""
            });
        }

        public async Task<IEnumerable<AttendanceDto>> GetCourseAttendanceAsync(int courseId)
        {
            var attendances = await _context.Attendances
                .Include(a => a.Student)
                    .ThenInclude(s => s!.User)
                .Include(a => a.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            return attendances.Select(a => new AttendanceDto
            {
                Id = a.Id,
                Date = a.Date,
                Status = a.Status,
                IsJustified = a.IsJustified,
                Justification = a.Justification,
                StudentId = a.StudentId,
                StudentName = a.Student?.User != null ? $"{a.Student.User.FirstName} {a.Student.User.LastName}" : "",
                CourseId = a.CourseId,
                SubjectName = a.Course?.Subject?.Name ?? ""
            });
        }

        private static CourseDto MapToDto(Course course)
        {
            return new CourseDto
            {
                Id = course.Id,
                SubjectId = course.SubjectId,
                SubjectName = course.Subject?.Name ?? "",
                SubjectCode = course.Subject?.Code ?? "",
                ClassId = course.ClassId,
                ClassName = course.Class?.Name ?? "",
                TeacherId = course.TeacherId,
                TeacherName = course.Teacher?.User != null ? $"{course.Teacher.User.FirstName} {course.Teacher.User.LastName}" : "",
                Coefficient = course.Subject?.Coefficient ?? 1,
                Credits = course.Subject?.Credits ?? 0
            };
        }
    }
}
