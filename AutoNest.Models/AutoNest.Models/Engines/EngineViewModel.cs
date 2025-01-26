using AutoNest.Data.Entities;

namespace AutoNest.Models.Engines
{
    public class EngineViewModel
    {
        public string Id { get; set; }
        public string EngineCode { get; set; }
        public float EngineDisplacement { get; set; }
        public float EngineHorsePower { get; set; }
        public string Transmission { get; set; }
        public string Drivetrain { get; set; }
        public IEnumerable<Car>? CompatibleCars { get; set; }
    }
}
