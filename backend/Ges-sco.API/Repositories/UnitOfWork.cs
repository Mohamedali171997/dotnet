using Ges_sco.API.Database;
using System.Threading.Tasks;
using Ges_sco.API.Models;

namespace Ges_sco.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        
        private IRepository<User>? _userRepository;
        private IRepository<Student>? _studentRepository;
        private IRepository<Teacher>? _teacherRepository;
        private IRepository<Class>? _classRepository;
        private IRepository<Subject>? _subjectRepository;
        private IRepository<Course>? _courseRepository;
        private IRepository<Grade>? _gradeRepository;
        private IRepository<Attendance>? _attendanceRepository;

        public UnitOfWork(AppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return _serviceProvider.GetService(typeof(IRepository<T>)) as IRepository<T> 
                ?? throw new InvalidOperationException($"Repository for {typeof(T).Name} not found");
        }

        public IRepository<User> Users => _userRepository ??= GetRepository<User>();
        public IRepository<Student> Students => _studentRepository ??= GetRepository<Student>();
        public IRepository<Teacher> Teachers => _teacherRepository ??= GetRepository<Teacher>();
        public IRepository<Class> Classes => _classRepository ??= GetRepository<Class>();
        public IRepository<Subject> Subjects => _subjectRepository ??= GetRepository<Subject>();
        public IRepository<Course> Courses => _courseRepository ??= GetRepository<Course>();
        public IRepository<Grade> Grades => _gradeRepository ??= GetRepository<Grade>();
        public IRepository<Attendance> Attendances => _attendanceRepository ??= GetRepository<Attendance>();

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
