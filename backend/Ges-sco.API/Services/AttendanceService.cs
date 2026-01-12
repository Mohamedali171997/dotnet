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
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public AttendanceService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
        {
            var attendances = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .ToListAsync();
            return attendances.Select(MapToDto);
        }

        public async Task<AttendanceDto?> GetByIdAsync(int id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);
            return attendance == null ? null : MapToDto(attendance);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByStudentIdAsync(int studentId)
        {
            var attendances = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .Where(a => a.StudentId == studentId)
                .ToListAsync();
            return attendances.Select(MapToDto);
        }

        public async Task<IEnumerable<AttendanceDto>> GetByCourseIdAsync(int courseId)
        {
            var attendances = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
            return attendances.Select(MapToDto);
        }

        public async Task<AttendanceDto> CreateAsync(CreateAttendanceDto dto)
        {
            var attendance = new Attendance
            {
                Date = dto.Date,
                Status = dto.Status,
                IsJustified = dto.IsJustified,
                Justification = dto.Justification,
                StudentId = dto.StudentId,
                CourseId = dto.CourseId
            };
            await _unitOfWork.Attendances.AddAsync(attendance);
            await _unitOfWork.CompleteAsync();

            var created = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .FirstAsync(a => a.Id == attendance.Id);
            return MapToDto(created);
        }

        public async Task<IEnumerable<AttendanceDto>> CreateBulkAsync(BulkAttendanceDto dto)
        {
            var attendances = dto.Attendances.Select(item => new Attendance
            {
                Date = dto.Date,
                Status = item.Status,
                IsJustified = false,
                StudentId = item.StudentId,
                CourseId = dto.CourseId
            }).ToList();

            await _unitOfWork.Attendances.AddRangeAsync(attendances);
            await _unitOfWork.CompleteAsync();

            var ids = attendances.Select(a => a.Id).ToList();
            var created = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .Where(a => ids.Contains(a.Id))
                .ToListAsync();
            return created.Select(MapToDto);
        }

        public async Task<AttendanceDto?> UpdateAsync(int id, UpdateAttendanceDto dto)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student).ThenInclude(s => s!.User)
                .Include(a => a.Course).ThenInclude(c => c!.Subject)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (attendance == null) return null;

            if (dto.Date.HasValue) attendance.Date = dto.Date.Value;
            if (!string.IsNullOrEmpty(dto.Status)) attendance.Status = dto.Status;
            if (dto.IsJustified.HasValue) attendance.IsJustified = dto.IsJustified.Value;
            if (dto.Justification != null) attendance.Justification = dto.Justification;

            _unitOfWork.Attendances.Update(attendance);
            await _unitOfWork.CompleteAsync();
            return MapToDto(attendance);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var attendance = await _unitOfWork.Attendances.GetByIdAsync(id);
            if (attendance == null) return false;
            _unitOfWork.Attendances.Remove(attendance);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<double> CalculateStudentAttendanceRateAsync(int studentId)
        {
            var attendances = await _context.Attendances.Where(a => a.StudentId == studentId).ToListAsync();
            if (!attendances.Any()) return 0;
            var presentOrLate = attendances.Count(a => a.Status == "Present" || a.Status == "Late");
            return Math.Round((double)presentOrLate / attendances.Count * 100, 2);
        }

        public async Task<double> CalculateCourseAttendanceRateAsync(int courseId)
        {
            var attendances = await _context.Attendances.Where(a => a.CourseId == courseId).ToListAsync();
            if (!attendances.Any()) return 0;
            var presentOrLate = attendances.Count(a => a.Status == "Present" || a.Status == "Late");
            return Math.Round((double)presentOrLate / attendances.Count * 100, 2);
        }

        private static AttendanceDto MapToDto(Attendance a)
        {
            return new AttendanceDto
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
            };
        }
    }
}
