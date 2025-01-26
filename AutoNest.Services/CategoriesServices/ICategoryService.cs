namespace AutoNest.Services.CategoriesServices
{
    public interface ICategoryService
    {
        IEnumerable<CategoryViewModel> GetAll();
        void Add(CategoryAddViewModel category);
        void Update(CategoryViewModel category);
    }
}

