using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Training;

namespace TrainingX_2.Repositories
{
    public class TrainingRepo : ITraining
    {
        private readonly ApplicationDBContext _context;
        public TrainingRepo(ApplicationDBContext context) 
        {
            _context = context;
        }
        public void Delete(Training training)
        {
            _context.Trainings.Remove(training);
        }

        public List<Training> GetAll()
        {
            return _context.Trainings.ToList();
        }

        public Training GetById(string Id)
        {
            return _context.Trainings.FirstOrDefault(x=>x.Id == Id);
        }

        public void Insert(Training training)
        {
            _context.Trainings.Add(training);
        }

        public void Update(Training training)
        {
            _context.Trainings.Update(training);
        }
    }
}
