using AutoNest.Models.Categories;

namespace AutoNest.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAll();
        Task Add(CategoryAddViewModel category);
        Task Update(CategoryViewModel category);
        Task<bool> Delete(string id);
    }
}
