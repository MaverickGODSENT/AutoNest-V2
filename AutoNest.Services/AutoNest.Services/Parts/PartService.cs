using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNest.Services.Parts
{
    public class PartService : IPartService
    {
        private readonly IRepository<Part> _partRepository;
        public PartService(IRepository<Part> partRepository)
        {
            _partRepository = partRepository;
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

            

            try
            {
                await _partRepository.AddAsync(part);
                var result = await _partRepository.SaveChangesAsync() > 0;
                if(!result)
                {
                    throw new ArgumentException();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
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
            return _partRepository.AllAsNoTracking().Select(p => new PartViewModel
            {
                Id = p.Id,
                Brand = p.Brand,
                Model = p.Model,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
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
