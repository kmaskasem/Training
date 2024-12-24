using TrainingX_2.Models;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Interfaces
{
    public interface ITraining
    {
        List<Training> GetAll();
        Training GetById(string Id);
        void Insert(Training training);
        void Update(Training training);
        void Delete(Training training);
    }
}
