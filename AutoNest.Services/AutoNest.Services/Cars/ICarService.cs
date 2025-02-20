using AutoNest.Models.Cars;

namespace AutoNest.Services.Cars
{
    public interface ICarService
    {
        IEnumerable<CarViewModel> GetAll();
        Task AddCarAsync(AddCarViewModel car);
        Task UpdateCarAsync(CarViewModel car);
        bool DeleteCar(string id);
    }
}
