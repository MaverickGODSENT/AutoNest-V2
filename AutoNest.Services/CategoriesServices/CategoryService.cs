using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Services.CategoriesServices;

namespace AutoNest.Data.CategoriesServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async void Add(CategoryAddViewModel category)
        {
            Category category1 = new Category
            {
                Name = category.Name,
                Description = category.Description
            };

            try
            {
                await _categoryRepository.AddAsync(category1);
                var result = await _categoryRepository.SaveChangesAsync() > 0;
                if (!result)
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IEnumerable<CategoryViewModel> GetAll()
        {
            return _categoryRepository.All().Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Parts = c.Parts
            });
        }

        public void Update(CategoryViewModel category)
        {
            // TODO: Implement Update method
        }
    }
}
