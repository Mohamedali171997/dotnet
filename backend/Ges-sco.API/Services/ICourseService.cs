using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CreateCourseDto dto);
        Task<CourseDto?> UpdateAsync(int id, UpdateCourseDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<GradeDto>> GetCourseGradesAsync(int courseId);
        Task<IEnumerable<AttendanceDto>> GetCourseAttendanceAsync(int courseId);
    }
}
