using System.Threading.Tasks;
using Ges_sco.API.DTOs.Responses;

namespace Ges_sco.API.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardStatsAsync();
    }
}
