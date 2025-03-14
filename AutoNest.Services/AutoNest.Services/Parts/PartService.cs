using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public class PartService : IPartService
    {
        private readonly IDeletableEntityRepository<Part> _partRepository;
        private readonly IDeletableEntityRepository<Category> _categoryRepository;
        private readonly IRepository<Image> _imageRepository;
        public PartService(IDeletableEntityRepository<Part> partRepository,
            IDeletableEntityRepository<Category> categoryRepository,
            IRepository<Image> imageRepository)
        {
            _partRepository = partRepository;
            _categoryRepository = categoryRepository;
            _imageRepository = imageRepository;
        }



        public async Task AddPartAsync(PartAddViewModel partAddViewModel, string imagePath)
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

            await _partRepository.AddAsync(part);
            await _partRepository.SaveChangesAsync();

            Directory.CreateDirectory($"{imagePath}/parts/");

            var allowedExtensions = new[] { "jpg", "jpeg", "png", "gif" };

            var image = partAddViewModel.Image;
            var extension = Path.GetExtension(image.FileName).TrimStart('.');
            if (!allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid image extension {extension}");
            }
            var dbImage = new Image
            {
                PartId = part.Id,
                Extension = extension,
                RemoteImageUrl = $"/images/parts/{part.Id}.{extension}",
            };

            await _imageRepository.AddAsync(dbImage);
            await _imageRepository.SaveChangesAsync();

            var physicalPath = $"{imagePath}/parts/{part.Id}.{extension}";

            using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }




        }
        public async Task<bool> DeletePartAsync(string id)
        {
            var list = _partRepository.All().ToList();
            foreach (var part in list)
            {
                if (part.Id == id)
                {
                    _partRepository.Delete(part);
                    await _partRepository.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<PartViewModel> GetAll()
        {
            var categories = _categoryRepository.All();
            var images = _imageRepository.All();


            return _partRepository.AllAsNoTracking().Select(p => new PartViewModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Model = p.Model,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                CategoryName = categories.Where(c => c.Id == p.CategoryId).FirstOrDefault().Name,
                CategoryId = p.CategoryId,
                RemoteImageUrl = images.Where(i => i.PartId == p.Id).FirstOrDefault().RemoteImageUrl,
            });
        }

        public IEnumerable<PartViewModel> GetPartsForCar(string carId)
        {
            var cateogires = _categoryRepository.All();
            var images = _imageRepository.All();

            return _partRepository.All().Select(p => new PartViewModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Model = p.Model,
                Description = p.Description,
                Quantity = p.Quantity,
                InStock = p.Quantity > 0,
                Price = p.Price,
                CategoryName = cateogires.Where(c => c.Id == p.CategoryId).FirstOrDefault().Name,
                CategoryId = p.CategoryId,
                CarId = carId,
                RemoteImageUrl = images.Where(i=>i.PartId==p.Id).FirstOrDefault().RemoteImageUrl,
            });
        }

        public async Task UpdatePartAsync(PartViewModel partViewModel)
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
            await _partRepository.SaveChangesAsync();
        }
    }
}
