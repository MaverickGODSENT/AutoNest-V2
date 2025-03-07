using AutoNest.Models.Cars;
using AutoNest.Models.Categories;

namespace AutoNest.Models.Home
{
    public class HomeViewModel
    {
        public string SelectedCarId { get; set; }
        public string SelectedBrand { get; set; }
        public string SelectedModel { get; set; }
        public string SelectedEngine { get; set; }

        public List<CarViewModel> Cars { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
