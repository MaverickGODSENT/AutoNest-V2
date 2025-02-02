using AutoNest.Models.Engines;

namespace AutoNest.Services.Engines
{
    public interface IEngineService
    {
        IEnumerable<EngineViewModel> GetAll();
        Task Add(EngineAddViewModel category);
        void Update(EngineViewModel category);
        bool Delete(string id);
    }
}
