using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public interface IPartService
    {
        IEnumerable<PartViewModel> GetAll();
        Task Add(PartAddViewModel partAddViewModel,string imagePath);
        Task Update(PartViewModel partViewModel);
        Task<bool> Delete(string id);
    }
}
