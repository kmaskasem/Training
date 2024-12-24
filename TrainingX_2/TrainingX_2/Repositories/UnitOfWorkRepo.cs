using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;

namespace TrainingX_2.Repositories
{
    public class UnitOfWorkRepo : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        private ITraining _trainingRepo;
        private ICategory _categoryRepo;
        private IEnrollment _enrollmentRepo;
        public UnitOfWorkRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public ITraining Training
        {
            get { return _trainingRepo = _trainingRepo ?? new TrainingRepo(_context); }
        }

        public ICategory Category
        {
            get { return _categoryRepo = _categoryRepo ?? new CategoryRepo(_context); }
        }
        public IEnrollment Enrollment
        {
            get { return _enrollmentRepo = _enrollmentRepo ?? new EnrollmentRepo(_context); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
