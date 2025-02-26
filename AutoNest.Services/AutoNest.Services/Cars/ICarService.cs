using AutoNest.Data.Entities;
using AutoNest.Models.Cars;

namespace AutoNest.Services.Cars
{
    public interface ICarService
    {
        IEnumerable<CarViewModel> GetAll();
        Car GetCarById(string id);
        Task AddCarAsync(CarInputModel car);
        Task UpdateCarAsync(CarViewModel car);
        Task<bool> DeleteCar(string id);
    }
}
