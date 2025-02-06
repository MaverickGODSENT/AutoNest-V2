using AutoNest.Models.Engines;

namespace AutoNest.Services.Engines
{
    public interface IEngineService
    {
        IEnumerable<EngineViewModel> GetAll();
        Task AddEngineAsync(EngineAddViewModel category);
        Task UpdateEngineAsync(EngineViewModel category);
        Task<bool> DeleteEngineAsync(string id);
    }
}
