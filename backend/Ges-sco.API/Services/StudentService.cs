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
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public StudentService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .Where(s => s.User != null && s.User.IsActive)
                .ToListAsync();

            return students.Select(MapToDto);
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            return student == null ? null : MapToDto(student);
        }

        public async Task<IEnumerable<StudentDto>> GetByClassIdAsync(int classId)
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .Where(s => s.ClassId == classId && s.User != null && s.User.IsActive)
                .ToListAsync();

            return students.Select(MapToDto);
        }

        public async Task<StudentDto> CreateAsync(CreateStudentDto dto)
        {
            // Create user first
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Student",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            // Create student
            var student = new Student
            {
                UserId = user.Id,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                Phone = dto.Phone,
                ParentContact = dto.ParentContact,
                MedicalInfo = dto.MedicalInfo,
                StudentIdentificationNumber = dto.StudentIdentificationNumber,
                ClassId = dto.ClassId
            };

            await _unitOfWork.Students.AddAsync(student);
            await _unitOfWork.CompleteAsync();

            // Reload with relationships
            var createdStudent = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstAsync(s => s.Id == student.Id);

            return MapToDto(createdStudent);
        }

        public async Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto dto)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null || student.User == null)
                return null;

            // Update user fields
            if (!string.IsNullOrEmpty(dto.FirstName))
                student.User.FirstName = dto.FirstName;
            if (!string.IsNullOrEmpty(dto.LastName))
                student.User.LastName = dto.LastName;
            if (!string.IsNullOrEmpty(dto.Email))
                student.User.Email = dto.Email;

            // Update student fields
            if (dto.DateOfBirth.HasValue)
                student.DateOfBirth = dto.DateOfBirth.Value;
            if (dto.Address != null)
                student.Address = dto.Address;
            if (dto.Phone != null)
                student.Phone = dto.Phone;
            if (dto.ParentContact != null)
                student.ParentContact = dto.ParentContact;
            if (dto.MedicalInfo != null)
                student.MedicalInfo = dto.MedicalInfo;
            if (dto.StudentIdentificationNumber != null)
                student.StudentIdentificationNumber = dto.StudentIdentificationNumber;
            if (dto.ClassId.HasValue)
                student.ClassId = dto.ClassId.Value;

            _unitOfWork.Students.Update(student);
            await _unitOfWork.CompleteAsync();

            return MapToDto(student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return false;

            // Soft delete - deactivate user
            if (student.User != null)
            {
                student.User.IsActive = false;
                _unitOfWork.Users.Update(student.User);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<StudentGradesReportDto?> GetGradesReportAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null || student.User == null)
                return null;

            var grades = await _context.Grades
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            var subjectGrades = grades
                .GroupBy(g => g.Course?.SubjectId)
                .Select(group =>
                {
                    var course = group.First().Course;
                    var subject = course?.Subject;
                    var gradeList = group.ToList();
                    var weightedSum = gradeList.Sum(g => g.Value * g.Coefficient);
                    var coeffSum = gradeList.Sum(g => g.Coefficient);
                    var average = coeffSum > 0 ? weightedSum / coeffSum : 0;

                    return new SubjectGradesDto
                    {
                        SubjectName = subject?.Name ?? "",
                        SubjectCode = subject?.Code ?? "",
                        Coefficient = subject?.Coefficient ?? 1,
                        Credits = subject?.Credits ?? 0,
                        Grades = gradeList.Select(g => new GradeDto
                        {
                            Id = g.Id,
                            Value = g.Value,
                            Coefficient = g.Coefficient,
                            Type = g.Type,
                            Comment = g.Comment,
                            Date = g.Date,
                            StudentId = g.StudentId,
                            StudentName = $"{student.User.FirstName} {student.User.LastName}",
                            CourseId = g.CourseId,
                            SubjectName = subject?.Name ?? ""
                        }).ToList(),
                        Average = Math.Round(average, 2),
                        IsPassed = average >= 10
                    };
                }).ToList();

            var totalWeightedAvg = subjectGrades.Sum(s => s.Average * s.Coefficient);
            var totalCoeff = subjectGrades.Sum(s => s.Coefficient);
            var generalAverage = totalCoeff > 0 ? Math.Round(totalWeightedAvg / totalCoeff, 2) : 0;

            return new StudentGradesReportDto
            {
                StudentId = studentId,
                StudentName = $"{student.User.FirstName} {student.User.LastName}",
                ClassName = student.Class?.Name ?? "",
                AcademicYear = student.Class?.AcademicYear ?? "",
                SubjectGrades = subjectGrades,
                GeneralAverage = generalAverage,
                TotalCredits = subjectGrades.Where(s => s.IsPassed).Sum(s => s.Credits),
                Mention = GetMention(generalAverage)
            };
        }

        public async Task<StudentAttendanceReportDto?> GetAttendanceReportAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null || student.User == null)
                return null;

            var attendances = await _context.Attendances
                .Include(a => a.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(a => a.StudentId == studentId)
                .ToListAsync();

            var presentCount = attendances.Count(a => a.Status == "Present");
            var absentCount = attendances.Count(a => a.Status == "Absent");
            var lateCount = attendances.Count(a => a.Status == "Late");
            var justifiedCount = attendances.Count(a => a.IsJustified);
            var total = attendances.Count;

            return new StudentAttendanceReportDto
            {
                StudentId = studentId,
                StudentName = $"{student.User.FirstName} {student.User.LastName}",
                ClassName = student.Class?.Name ?? "",
                TotalSessions = total,
                PresentCount = presentCount,
                AbsentCount = absentCount,
                LateCount = lateCount,
                JustifiedAbsences = justifiedCount,
                AttendanceRate = total > 0 ? Math.Round((double)(presentCount + lateCount) / total * 100, 2) : 0,
                Attendances = attendances.Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    Date = a.Date,
                    Status = a.Status,
                    IsJustified = a.IsJustified,
                    Justification = a.Justification,
                    StudentId = a.StudentId,
                    StudentName = $"{student.User.FirstName} {student.User.LastName}",
                    CourseId = a.CourseId,
                    SubjectName = a.Course?.Subject?.Name ?? ""
                }).ToList()
            };
        }

        private static StudentDto MapToDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id,
                UserId = student.UserId,
                FirstName = student.User?.FirstName ?? "",
                LastName = student.User?.LastName ?? "",
                Email = student.User?.Email ?? "",
                DateOfBirth = student.DateOfBirth,
                Address = student.Address,
                Phone = student.Phone,
                ParentContact = student.ParentContact,
                MedicalInfo = student.MedicalInfo,
                StudentIdentificationNumber = student.StudentIdentificationNumber,
                ClassId = student.ClassId,
                ClassName = student.Class?.Name ?? "",
                IsActive = student.User?.IsActive ?? false
            };
        }

        private static string GetMention(double average)
        {
            return average switch
            {
                >= 16 => "Très Bien",
                >= 14 => "Bien",
                >= 12 => "Assez Bien",
                >= 10 => "Passable",
                _ => "Ajourné"
            };
        }
    }
}
