using AutoNest.Models.Cars;

namespace AutoNest.Services.Cars
{
    public interface ICarService
    {
        IEnumerable<CarViewModel> GetAll();
        Task AddCarAsync(CarInputModel car);
        Task UpdateCarAsync(CarViewModel car);
        Task<bool> DeleteCar(string id);
    }
}
