using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public interface IPartService
    {
        IEnumerable<PartViewModel> GetAll();
        IEnumerable<PartViewModel> GetPartsForCar(string carId);
        Task AddPartAsync(PartAddViewModel partAddViewModel, string imagePath);
        Task UpdatePartAsync(PartViewModel partViewModel);
        Task<bool> DeletePartAsync(string id);
    }
}
