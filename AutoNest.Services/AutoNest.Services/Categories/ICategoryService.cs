using AutoNest.Models.Categories;

namespace AutoNest.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAll();
        void Add(CategoryAddViewModel category);
        void Update(CategoryViewModel category);
        bool Delete(string id);
    }
}
