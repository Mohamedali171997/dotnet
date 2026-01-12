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
    public class GradeService : IGradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public GradeService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<GradeDto>> GetAllAsync()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<GradeDto?> GetByIdAsync(int id)
        {
            var grade = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            return grade == null ? null : MapToDto(grade);
        }

        public async Task<IEnumerable<GradeDto>> GetByStudentIdAsync(int studentId)
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<IEnumerable<GradeDto>> GetByCourseIdAsync(int courseId)
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(g => g.CourseId == courseId)
                .ToListAsync();

            return grades.Select(MapToDto);
        }

        public async Task<GradeDto> CreateAsync(CreateGradeDto dto)
        {
            var grade = new Grade
            {
                Value = dto.Value,
                Coefficient = dto.Coefficient,
                Type = dto.Type,
                Comment = dto.Comment,
                Date = dto.Date,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };

            await _unitOfWork.Grades.AddAsync(grade);
            await _unitOfWork.CompleteAsync();

            var createdGrade = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .FirstAsync(g => g.Id == grade.Id);

            return MapToDto(createdGrade);
        }

        public async Task<GradeDto?> UpdateAsync(int id, UpdateGradeDto dto)
        {
            var grade = await _context.Grades
                .Include(g => g.Student)
                    .ThenInclude(s => s!.User)
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null)
                return null;

            if (dto.Value.HasValue)
                grade.Value = dto.Value.Value;
            if (dto.Coefficient.HasValue)
                grade.Coefficient = dto.Coefficient.Value;
            if (!string.IsNullOrEmpty(dto.Type))
                grade.Type = dto.Type;
            if (dto.Comment != null)
                grade.Comment = dto.Comment;
            if (dto.Date.HasValue)
                grade.Date = dto.Date.Value;

            _unitOfWork.Grades.Update(grade);
            await _unitOfWork.CompleteAsync();

            return MapToDto(grade);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var grade = await _unitOfWork.Grades.GetByIdAsync(id);
            if (grade == null)
                return false;

            _unitOfWork.Grades.Remove(grade);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<double> CalculateStudentAverageAsync(int studentId)
        {
            var grades = await _context.Grades
                .Include(g => g.Course)
                    .ThenInclude(c => c!.Subject)
                .Where(g => g.StudentId == studentId)
                .ToListAsync();

            if (!grades.Any())
                return 0;

            // Group by subject and calculate weighted average
            var subjectAverages = grades
                .GroupBy(g => g.Course?.SubjectId)
                .Select(group =>
                {
                    var subject = group.First().Course?.Subject;
                    var gradeList = group.ToList();
                    var weightedSum = gradeList.Sum(g => g.Value * g.Coefficient);
                    var coeffSum = gradeList.Sum(g => g.Coefficient);
                    var average = coeffSum > 0 ? weightedSum / coeffSum : 0;
                    return new { Average = average, SubjectCoeff = subject?.Coefficient ?? 1 };
                }).ToList();

            var totalWeightedAvg = subjectAverages.Sum(s => s.Average * s.SubjectCoeff);
            var totalCoeff = subjectAverages.Sum(s => s.SubjectCoeff);

            return totalCoeff > 0 ? Math.Round(totalWeightedAvg / totalCoeff, 2) : 0;
        }

        public async Task<double> CalculateCourseAverageAsync(int courseId)
        {
            var grades = await _context.Grades
                .Where(g => g.CourseId == courseId)
                .ToListAsync();

            if (!grades.Any())
                return 0;

            // Group by student, calculate student average, then course average
            var studentAverages = grades
                .GroupBy(g => g.StudentId)
                .Select(group =>
                {
                    var weightedSum = group.Sum(g => g.Value * g.Coefficient);
                    var coeffSum = group.Sum(g => g.Coefficient);
                    return coeffSum > 0 ? weightedSum / coeffSum : 0;
                }).ToList();

            return studentAverages.Any() ? Math.Round(studentAverages.Average(), 2) : 0;
        }

        private static GradeDto MapToDto(Grade grade)
        {
            return new GradeDto
            {
                Id = grade.Id,
                Value = grade.Value,
                Coefficient = grade.Coefficient,
                Type = grade.Type,
                Comment = grade.Comment,
                Date = grade.Date,
                StudentId = grade.StudentId,
                StudentName = grade.Student?.User != null ? $"{grade.Student.User.FirstName} {grade.Student.User.LastName}" : "",
                CourseId = grade.CourseId,
                SubjectName = grade.Course?.Subject?.Name ?? ""
            };
        }
    }
}
