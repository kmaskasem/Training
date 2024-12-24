namespace TrainingX_2.Interfaces
{
    public interface IUnitOfWork
    {
        ITraining Training { get; }
        ICategory Category { get; }
        IEnrollment Enrollment { get; }
        void Save();
    }
}
