using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Cars;

namespace AutoNest.Services.Cars
{
    public class CarService : ICarService
    {
        private readonly IDeletableEntityRepository<Car> _carRepository;
        private readonly IDeletableEntityRepository<Engine> _engineRepository;

        public CarService(IDeletableEntityRepository<Car> carRepository, IDeletableEntityRepository<Engine> engineRepository)
        {
            _carRepository = carRepository;
            _engineRepository = engineRepository;
        }

        public async Task AddCarAsync(AddCarViewModel car)
        {
            Car newCar = new Car
            {
                Brand = car.Brand,
                Model = car.Model,
                ModelYear = car.ModelYear,
            };

            await _carRepository.AddAsync(newCar);
            await _carRepository.SaveChangesAsync();
        }

        public bool DeleteCar(string id)
        {
            var car = _carRepository.GetById(id);
            if (car != null)
            {
                _carRepository.Delete(car);
                _carRepository.SaveChangesAsync();
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

        public async Task UpdateCarAsync(CarViewModel car)
        {
            _carRepository.Update(new Car
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                ModelYear = car.ModelYear,
            });

            await _carRepository.SaveChangesAsync();
        }
    }
}
