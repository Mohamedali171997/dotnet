using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface IGradeService
    {
        Task<IEnumerable<GradeDto>> GetAllAsync();
        Task<GradeDto?> GetByIdAsync(int id);
        Task<IEnumerable<GradeDto>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<GradeDto>> GetByCourseIdAsync(int courseId);
        Task<GradeDto> CreateAsync(CreateGradeDto dto);
        Task<GradeDto?> UpdateAsync(int id, UpdateGradeDto dto);
        Task<bool> DeleteAsync(int id);
        Task<double> CalculateStudentAverageAsync(int studentId);
        Task<double> CalculateCourseAverageAsync(int courseId);
    }
}
