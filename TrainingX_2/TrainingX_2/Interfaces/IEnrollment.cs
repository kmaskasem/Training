using TrainingX_2.Models.Training;

namespace TrainingX_2.Interfaces
{
    public interface IEnrollment
    {
        List<Enrollment> GetAll();
        Enrollment GetById(string Id);
        void Insert(Enrollment enrollment);
        void Update(Enrollment enrollment);
        void Delete(Enrollment enrollment);
    }
}
