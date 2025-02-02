using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public interface IPartService
    {
        IEnumerable<PartViewModel> GetAll();
        Task Add(PartAddViewModel partAddViewModel,string imagePath);
        void Update(PartViewModel partViewModel);
        bool Delete(string id);
    }
}
