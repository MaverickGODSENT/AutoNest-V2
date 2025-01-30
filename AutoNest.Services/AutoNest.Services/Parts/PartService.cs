using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public class PartService : IPartService
    {
        private readonly IDeletableEntityRepository<Part> _partRepository;
        private readonly IDeletableEntityRepository<Category> _categoryRepository;
        public PartService(IDeletableEntityRepository<Part> partRepository, IDeletableEntityRepository<Category> categoryRepository)
        {
            _partRepository = partRepository;
            _categoryRepository = categoryRepository;
        }



        public async void Add(PartAddViewModel partAddViewModel, string imageParth)
        {
            Part part = new Part
            {
                Brand = partAddViewModel.Brand,
                Model = partAddViewModel.Model,
                Description = partAddViewModel.Description,
                CategoryId = partAddViewModel.CategoryId,
                Quantity = partAddViewModel.Quantity,
                Price = partAddViewModel.Price,
            };

            Directory.CreateDirectory($"{imageParth}/parts/");

            var allowedExtensions = new[] { "jpg", "jpeg", "png", "gif" };




            await _partRepository.AddAsync(part);
            await _partRepository.SaveChangesAsync();
        }
        public bool Delete(string id)
        {
            var list = _partRepository.All().ToList();
            foreach (var part in list)
            {
                if (part.Id == id)
                {
                    _partRepository.Delete(part);
                    _partRepository.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<PartViewModel> GetAll()
        {
            var categories = _categoryRepository.AllAsNoTracking();



            return _partRepository.AllAsNoTracking().Select(p => new PartViewModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Model = p.Model,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                CategoryName = categories.Where(c => c.Id == p.CategoryId).FirstOrDefault().Name,
            });
        }

        public void Update(PartViewModel partViewModel)
        {
            _partRepository.Update(new Part
            {
                Id = partViewModel.Id,
                Brand = partViewModel.Brand,
                Model = partViewModel.Model,
                Description = partViewModel.Description,
                Quantity = partViewModel.Quantity,
                Price = partViewModel.Price,
            });
            _partRepository.SaveChangesAsync();
        }
    }
}
