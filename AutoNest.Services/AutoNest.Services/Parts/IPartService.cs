using AutoNest.Models.Parts;

namespace AutoNest.Services.Parts
{
    public interface IPartService
    {
        IEnumerable<PartViewModel> GetAll();
        void Add(PartAddViewModel partAddViewModel,string imagePath);
        void Update(PartViewModel partViewModel);
        bool Delete(string id);
    }
}
