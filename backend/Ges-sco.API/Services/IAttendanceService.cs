using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAsync();
        Task<AttendanceDto?> GetByIdAsync(int id);
        Task<IEnumerable<AttendanceDto>> GetByStudentIdAsync(int studentId);
        Task<IEnumerable<AttendanceDto>> GetByCourseIdAsync(int courseId);
        Task<AttendanceDto> CreateAsync(CreateAttendanceDto dto);
        Task<IEnumerable<AttendanceDto>> CreateBulkAsync(BulkAttendanceDto dto);
        Task<AttendanceDto?> UpdateAsync(int id, UpdateAttendanceDto dto);
        Task<bool> DeleteAsync(int id);
        Task<double> CalculateStudentAttendanceRateAsync(int studentId);
        Task<double> CalculateCourseAttendanceRateAsync(int courseId);
    }
}
