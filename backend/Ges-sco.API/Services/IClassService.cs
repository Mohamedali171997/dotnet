using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface IClassService
    {
        Task<IEnumerable<ClassDto>> GetAllAsync();
        Task<ClassDto?> GetByIdAsync(int id);
        Task<ClassDto> CreateAsync(CreateClassDto dto);
        Task<ClassDto?> UpdateAsync(int id, UpdateClassDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<StudentDto>> GetClassStudentsAsync(int classId);
        Task<IEnumerable<CourseDto>> GetClassCoursesAsync(int classId);
    }
}
