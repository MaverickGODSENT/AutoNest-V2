using AutoNest.Models.Engines;

namespace AutoNest.Services.Engines
{
    public interface IEngineService
    {
        IEnumerable<EngineViewModel> GetAll();
        Task Add(EngineAddViewModel category);
        Task Update(EngineViewModel category);
        Task<bool> Delete(string id);
    }
}
