using System.Threading.Tasks;

namespace Ges_sco.API.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        IRepository<Ges_sco.API.Models.User> Users { get; }
        Task<int> SaveChangesAsync();
    }
}