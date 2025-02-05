using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Models.Engines;

namespace AutoNest.Services.Engines
{
    public class EngineService : IEngineService
    {

        private readonly IDeletableEntityRepository<Engine> _engineRepository;

        public EngineService(IDeletableEntityRepository<Engine> engineRepository)
        {
            _engineRepository = engineRepository;
        }

        public async Task Add(EngineAddViewModel engine)
        {
            Engine engine1 = new Engine
            {
                EngineCode = engine.EngineCode,
                EngineDisplacement = engine.EngineDisplacement,
                EngineHorsePower = engine.EngineHorsePower,
                Transmission = engine.Transmission,
                Drivetrain = engine.Drivetrain
            };


            await _engineRepository.AddAsync(engine1);
            await _engineRepository.SaveChangesAsync();



        }

        public async Task<bool> Delete(string id)
        {
            var list = _engineRepository.All().ToList();
            foreach (var engine in list)
            {
                if (engine.Id == id)
                {
                    _engineRepository.Delete(engine);
                    await _engineRepository.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<EngineViewModel> GetAll()
        {
            return _engineRepository.AllAsNoTracking().Select(e => new EngineViewModel
            {
                Id = e.Id,
                EngineCode = e.EngineCode,
                EngineDisplacement = e.EngineDisplacement,
                EngineHorsePower = e.EngineHorsePower,
                Transmission = e.Transmission,
                Drivetrain = e.Drivetrain,
            });
        }

        public async Task Update(EngineViewModel engine)
        {
            _engineRepository.Update(new Engine
            {
                Id = engine.Id,
                EngineCode = engine.EngineCode,
                EngineDisplacement = engine.EngineDisplacement,
                EngineHorsePower = engine.EngineHorsePower,
                Transmission = engine.Transmission,
                Drivetrain = engine.Drivetrain
            });
            await _engineRepository.SaveChangesAsync();
        }
    }
}
