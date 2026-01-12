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
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public ClassService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<ClassDto>> GetAllAsync()
        {
            var classes = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Courses)
                .ToListAsync();

            return classes.Select(MapToDto);
        }

        public async Task<ClassDto?> GetByIdAsync(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);

            return classEntity == null ? null : MapToDto(classEntity);
        }

        public async Task<ClassDto> CreateAsync(CreateClassDto dto)
        {
            var classEntity = new Class
            {
                Name = dto.Name,
                Level = dto.Level,
                AcademicYear = dto.AcademicYear,
                Capacity = dto.Capacity
            };

            await _unitOfWork.Classes.AddAsync(classEntity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(classEntity);
        }

        public async Task<ClassDto?> UpdateAsync(int id, UpdateClassDto dto)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Students)
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
                return null;

            if (!string.IsNullOrEmpty(dto.Name))
                classEntity.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Level))
                classEntity.Level = dto.Level;
            if (!string.IsNullOrEmpty(dto.AcademicYear))
                classEntity.AcademicYear = dto.AcademicYear;
            if (dto.Capacity.HasValue)
                classEntity.Capacity = dto.Capacity.Value;

            _unitOfWork.Classes.Update(classEntity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(classEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var classEntity = await _unitOfWork.Classes.GetByIdAsync(id);
            if (classEntity == null)
                return false;

            _unitOfWork.Classes.Remove(classEntity);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<StudentDto>> GetClassStudentsAsync(int classId)
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .Where(s => s.ClassId == classId && s.User != null && s.User.IsActive)
                .ToListAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                UserId = s.UserId,
                FirstName = s.User?.FirstName ?? "",
                LastName = s.User?.LastName ?? "",
                Email = s.User?.Email ?? "",
                DateOfBirth = s.DateOfBirth,
                Address = s.Address,
                Phone = s.Phone,
                ParentContact = s.ParentContact,
                MedicalInfo = s.MedicalInfo,
                StudentIdentificationNumber = s.StudentIdentificationNumber,
                ClassId = s.ClassId,
                ClassName = s.Class?.Name ?? "",
                IsActive = s.User?.IsActive ?? false
            });
        }

        public async Task<IEnumerable<CourseDto>> GetClassCoursesAsync(int classId)
        {
            var courses = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .Where(c => c.ClassId == classId)
                .ToListAsync();

            return courses.Select(c => new CourseDto
            {
                Id = c.Id,
                SubjectId = c.SubjectId,
                SubjectName = c.Subject?.Name ?? "",
                SubjectCode = c.Subject?.Code ?? "",
                ClassId = c.ClassId,
                ClassName = c.Class?.Name ?? "",
                TeacherId = c.TeacherId,
                TeacherName = c.Teacher?.User != null ? $"{c.Teacher.User.FirstName} {c.Teacher.User.LastName}" : "",
                Coefficient = c.Subject?.Coefficient ?? 1,
                Credits = c.Subject?.Credits ?? 0
            });
        }

        private static ClassDto MapToDto(Class classEntity)
        {
            return new ClassDto
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                Level = classEntity.Level,
                AcademicYear = classEntity.AcademicYear,
                Capacity = classEntity.Capacity,
                StudentsCount = classEntity.Students?.Count ?? 0,
                CoursesCount = classEntity.Courses?.Count ?? 0
            };
        }
    }
}
