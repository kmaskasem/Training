using TrainingX_2.Areas.Identity.Data;

namespace TrainingX_2.Helpers
{
    public interface IUpdateService
    {
        Task UpdateStateofTrainingAsync();
    }

    public class UpdateService : IUpdateService
    {
        private readonly ApplicationDBContext _context;
        public UpdateService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task UpdateStateofTrainingAsync()
        {
            var cur_date = (DateTime.Now);
            var all_t = _context.Trainings.Where(t => t.State == State_Train.โพสต์แล้ว);

            foreach (var t in all_t)
            {
                if (t.Start_train_Date >= cur_date)
                {
                    t.State = State_Train.สำเร็จ;
                    _context.Update(t);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
