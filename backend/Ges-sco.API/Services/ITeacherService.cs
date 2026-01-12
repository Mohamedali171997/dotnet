using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllAsync();
        Task<TeacherDto?> GetByIdAsync(int id);
        Task<TeacherDto> CreateAsync(CreateTeacherDto dto);
        Task<TeacherDto?> UpdateAsync(int id, UpdateTeacherDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<CourseDto>> GetTeacherCoursesAsync(int teacherId);
    }
}
