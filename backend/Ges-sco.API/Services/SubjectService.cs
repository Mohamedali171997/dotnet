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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public SubjectService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Courses)
                .ToListAsync();

            return subjects.Select(MapToDto);
        }

        public async Task<SubjectDto?> GetByIdAsync(int id)
        {
            var subject = await _context.Subjects
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            return subject == null ? null : MapToDto(subject);
        }

        public async Task<SubjectDto> CreateAsync(CreateSubjectDto dto)
        {
            var subject = new Subject
            {
                Name = dto.Name,
                Code = dto.Code,
                Coefficient = dto.Coefficient,
                Credits = dto.Credits,
                Description = dto.Description
            };

            await _unitOfWork.Subjects.AddAsync(subject);
            await _unitOfWork.CompleteAsync();

            return MapToDto(subject);
        }

        public async Task<SubjectDto?> UpdateAsync(int id, UpdateSubjectDto dto)
        {
            var subject = await _context.Subjects
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
                return null;

            if (!string.IsNullOrEmpty(dto.Name))
                subject.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Code))
                subject.Code = dto.Code;
            if (dto.Coefficient.HasValue)
                subject.Coefficient = dto.Coefficient.Value;
            if (dto.Credits.HasValue)
                subject.Credits = dto.Credits.Value;
            if (dto.Description != null)
                subject.Description = dto.Description;

            _unitOfWork.Subjects.Update(subject);
            await _unitOfWork.CompleteAsync();

            return MapToDto(subject);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);
            if (subject == null)
                return false;

            _unitOfWork.Subjects.Remove(subject);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        private static SubjectDto MapToDto(Subject subject)
        {
            return new SubjectDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Code = subject.Code,
                Coefficient = subject.Coefficient,
                Credits = subject.Credits,
                Description = subject.Description,
                CoursesCount = subject.Courses?.Count ?? 0
            };
        }
    }
}
