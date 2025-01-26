using AutoNest.Models.Engines;

namespace AutoNest.Services.Engines
{
    public interface IEngineService
    {
        IEnumerable<EngineViewModel> GetAll();
        void Add(EngineAddViewModel category);
        void Update(EngineViewModel category);
        bool Delete(string id);
    }
}
