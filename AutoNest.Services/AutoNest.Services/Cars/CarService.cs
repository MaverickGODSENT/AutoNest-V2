using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Cars;

namespace AutoNest.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly IDeletableEntityRepository<Car> _carRepository;
        private readonly IDeletableEntityRepository<Engine> _engineRepository;
        private readonly IDeletableEntityRepository<Part> _partRepository;

        public CarService(IDeletableEntityRepository<Car> carRepository, IDeletableEntityRepository<Engine> engineRepository, IDeletableEntityRepository<Part> partRepository)
        {
            _carRepository = carRepository;
            _engineRepository = engineRepository;
            _partRepository = partRepository;
        }

        public async Task AddCarAsync(CarInputModel car)
        {
            Car newCar = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                ModelYear = car.ModelYear,
            };

           
            foreach(var item in car.CompatibleEngineIds)
            {
                var engine = _engineRepository.GetById(item);
                if(engine != null)
                {
                    newCar.CompatibleEngines.Add(engine);
                }
            }

            foreach(var item in car.CompatiblePartIds)
            {
                var part = _partRepository.GetById(item);
                if (part != null)
                {
                    newCar.CompatibleParts.Add(part);
                }
            }
            
            await _carRepository.AddAsync(newCar);
            await _carRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteCar(string id)
        {
            var car = _carRepository.GetById(id);
            if (car != null)
            {
                _carRepository.Delete(car);
                await _carRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public IEnumerable<CarViewModel> GetAll()
        {
            return _carRepository.AllAsNoTracking().Select(c => new CarViewModel
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                ModelYear = c.ModelYear,
                CompatibleEngines = c.CompatibleEngines.Select(e => e.Id),
                CompatibleParts = c.CompatibleParts.Select(p => p.Id),
            });
        }
        
        public Car GetCarById(string id)
        {
            return _carRepository.GetById(id);
        }


        public async Task UpdateCarAsync(CarViewModel car)
        {
            _carRepository.Update(new Car
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                ModelYear = car.ModelYear,
                CompatibleEngines = car.CompatibleEngines.Select(e => _engineRepository.GetById(e)).ToList(),
            });

            await _carRepository.SaveChangesAsync();
        }
    }
}
