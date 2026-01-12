using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<IEnumerable<StudentDto>> GetByClassIdAsync(int classId);
        Task<StudentDto> CreateAsync(CreateStudentDto dto);
        Task<StudentDto?> UpdateAsync(int id, UpdateStudentDto dto);
        Task<bool> DeleteAsync(int id);
        Task<StudentGradesReportDto?> GetGradesReportAsync(int studentId);
        Task<StudentAttendanceReportDto?> GetAttendanceReportAsync(int studentId);
    }
}
