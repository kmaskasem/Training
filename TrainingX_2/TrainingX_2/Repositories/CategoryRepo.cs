using TrainingX_2.Areas.Identity.Data;
using TrainingX_2.Interfaces;
using TrainingX_2.Models.Base;

namespace TrainingX_2.Repositories
{
    public class CategoryRepo : ICategory
    {
        private readonly ApplicationDBContext _context;
        public CategoryRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int Id)
        {
            return _context.Categories.FirstOrDefault(x => x.Id == Id);
        }

        public void Insert(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
