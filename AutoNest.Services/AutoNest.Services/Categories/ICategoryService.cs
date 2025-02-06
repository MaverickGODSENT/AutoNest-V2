using AutoNest.Models.Categories;

namespace AutoNest.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAll();
        Task AddCategoryAsync(CategoryAddViewModel category);
        Task UpdateCategoryAsync(CategoryViewModel category);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
