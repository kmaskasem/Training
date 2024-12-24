using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Repositories
{
    public class EnrollmentRepo : IEnrollment
    {
        private readonly ApplicationDBContext _context;
        public EnrollmentRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Delete(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
        }

        public List<Enrollment> GetAll()
        {
            return _context.Enrollments.ToList();
        }

        public Enrollment GetById(string Id)
        {
            return _context.Enrollments.FirstOrDefault(x => x.Id == Id);
        }

        public void Insert(Enrollment enrollment)
        {
            _context.Enrollments.Add(enrollment);
        }

        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }
    }
}
