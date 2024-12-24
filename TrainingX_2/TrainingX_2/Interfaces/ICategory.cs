using TrainingX_2.Models;
using TrainingX_2.Models.Base;

namespace TrainingX_2.Interfaces
{
    public interface ICategory
    {
        List<Category> GetAll();
        Category GetById(int Id);
        void Insert (Category category);
        void Update (Category category);
        void Delete (Category category);
    }
}
