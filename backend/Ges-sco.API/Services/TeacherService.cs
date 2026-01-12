using System;
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
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public TeacherService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllAsync()
        {
            var teachers = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .Where(t => t.User != null && t.User.IsActive)
                .ToListAsync();

            return teachers.Select(MapToDto);
        }

        public async Task<TeacherDto?> GetByIdAsync(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            return teacher == null ? null : MapToDto(teacher);
        }

        public async Task<TeacherDto> CreateAsync(CreateTeacherDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Teacher",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var teacher = new Teacher
            {
                UserId = user.Id,
                Specialization = dto.Specialization,
                Qualification = dto.Qualification,
                HireDate = dto.HireDate
            };

            await _unitOfWork.Teachers.AddAsync(teacher);
            await _unitOfWork.CompleteAsync();

            var createdTeacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .FirstAsync(t => t.Id == teacher.Id);

            return MapToDto(createdTeacher);
        }

        public async Task<TeacherDto?> UpdateAsync(int id, UpdateTeacherDto dto)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .Include(t => t.Courses)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null || teacher.User == null)
                return null;

            if (!string.IsNullOrEmpty(dto.FirstName))
                teacher.User.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName))
                teacher.User.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.Email))
                teacher.User.Email = dto.Email;
            if (dto.Specialization != null)
                teacher.Specialization = dto.Specialization;
            if (dto.Qualification != null)
                teacher.Qualification = dto.Qualification;
            if (dto.HireDate.HasValue)
                teacher.HireDate = dto.HireDate.Value;

            _unitOfWork.Teachers.Update(teacher);
            await _unitOfWork.CompleteAsync();

            return MapToDto(teacher);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return false;

            if (teacher.User != null)
            {
                teacher.User.IsActive = false;
                _unitOfWork.Users.Update(teacher.User);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<IEnumerable<CourseDto>> GetTeacherCoursesAsync(int teacherId)
        {
            var courses = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Class)
                .Include(c => c.Teacher)
                    .ThenInclude(t => t!.User)
                .Where(c => c.TeacherId == teacherId)
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

        private static TeacherDto MapToDto(Teacher teacher)
        {
            return new TeacherDto
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                FirstName = teacher.User?.FirstName ?? "",
                LastName = teacher.User?.LastName ?? "",
                Email = teacher.User?.Email ?? "",
                Specialization = teacher.Specialization,
                Qualification = teacher.Qualification,
                HireDate = teacher.HireDate,
                IsActive = teacher.User?.IsActive ?? false,
                CoursesCount = teacher.Courses?.Count ?? 0
            };
        }
    }
}
