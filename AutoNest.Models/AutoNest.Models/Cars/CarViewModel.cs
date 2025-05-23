﻿namespace AutoNest.Models.Cars
{
    public class CarViewModel
    {
        public string Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public int ModelYear { get; set; }

        public IEnumerable<string> CompatibleEngineNames { get; set; } = new List<string>();
        public IEnumerable<string> CompatibleEngineIds { get; set; } = new List<string>();

        public IEnumerable<string> CompatibleParts { get; set; } = new List<string>();
    }

}
