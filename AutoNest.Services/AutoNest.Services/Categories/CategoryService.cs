using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Categories;

namespace AutoNest.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> _categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task AddCategoryAsync(CategoryAddViewModel category)
        {
            Category category1 = new Category
            {
                Name = category.Name,
                Description = category.Description
            };
            await _categoryRepository.AddAsync(category1);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var list = _categoryRepository.All().ToList();
            foreach (var category in list)
            {
                if (category.Id == id)
                {
                    _categoryRepository.Delete(category);
                    await _categoryRepository.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<CategoryViewModel> GetAll()
        {
            return _categoryRepository.AllAsNoTracking().Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Parts = c.Parts
            });
        }

        public async Task UpdateCategoryAsync(CategoryViewModel category)
        {
            _categoryRepository.Update(new Category
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
