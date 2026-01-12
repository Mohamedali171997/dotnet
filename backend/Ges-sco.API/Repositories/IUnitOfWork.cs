using System.Threading.Tasks;
using Ges_sco.API.Models;

namespace Ges_sco.API.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        IRepository<User> Users { get; }
        IRepository<Student> Students { get; }
        IRepository<Teacher> Teachers { get; }
        IRepository<Class> Classes { get; }
        IRepository<Subject> Subjects { get; }
        IRepository<Course> Courses { get; }
        IRepository<Grade> Grades { get; }
        IRepository<Attendance> Attendances { get; }
        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}