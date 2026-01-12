using System.Collections.Generic;
using System.Threading.Tasks;
using Ges_sco.API.DTOs.Requests;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllAsync();
        Task<SubjectDto?> GetByIdAsync(int id);
        Task<SubjectDto> CreateAsync(CreateSubjectDto dto);
        Task<SubjectDto?> UpdateAsync(int id, UpdateSubjectDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
