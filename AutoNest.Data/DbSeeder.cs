using AutoNest.Data.Entities;
using AutoNest.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoNest.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            await context.Database.MigrateAsync();

            // Check if the database is created
            if (!context.Categories.Any())
            {
                Category category1 = new Category
                {
                    Name = "Suspension",
                    Description = "Suspension components for your car"
                };
                Category category2 = new Category
                {
                    Name = "Brakes",
                    Description = "Braking components for your car"
                };
                Category category3 = new Category
                {
                    Name = "Air",
                    Description = "Air filters, Cold Air Intakes"
                };
                await context.Categories.AddRangeAsync(category1, category2, category3);
                await context.SaveChangesAsync();

                Engine engine1 = new Engine
                {
                    EngineCode = "2JZ-GTE",
                    EngineDisplacement = 3.0f,
                    EngineHorsePower = 276,
                    Transmission = "6-speed manual",
                    Drivetrain = "RWD",
                };
                Engine engine2 = new Engine
                {
                    EngineCode = "SR20DET",
                    EngineDisplacement = 2.0f,
                    EngineHorsePower = 250,
                    Transmission = "5-speed manual",
                    Drivetrain = "RWD",
                };
                await context.Engines.AddRangeAsync(engine1, engine2);
                await context.SaveChangesAsync();

                Image image1 = new Image
                {
                    RemoteImageUrl = "/images/parts/bcr-suspension.jpg",
                    Extension = "jpg",
                };
                Image image2 = new Image
                {
                    RemoteImageUrl = "/images/parts/hks-suspension.jpg",
                    Extension = "jpg",
                };
                Image image3 = new Image
                {
                    RemoteImageUrl = "/images/parts/brembo-brakekit.jpg",
                    Extension = "jpg",
                };
                Image image4 = new Image
                {
                    RemoteImageUrl = "/images/parts/apr-brakekit.jpg",
                    Extension = "jpg",
                };
                Image image5 = new Image
                {
                    RemoteImageUrl = "/images/partskn-airfilter.jpeg",
                    Extension = "jpeg",
                };
                Image image6 = new Image
                {
                    RemoteImageUrl = "/images/parts/aem-airfilter.jpg",
                    Extension = "jpg",
                };
                await context.Images.AddRangeAsync(image1, image2, image3, image4, image5, image6);
                await context.SaveChangesAsync();


                Part part1 = new Part
                {
                    Brand = "BC Racing",
                    Model = "BR Series",
                    Description = "Coilovers for 2JZ-GTE",
                    Price = 1200,
                    SalesCount = 0,
                    Quantity = 10,
                    Category = category1,
                    CategoryId = category1.Id,
                    ImageId = image1.Id,
                };

                Part part2 = new Part
                {
                    Brand = "HKS",
                    Model = "Hipermax IV GT",
                    Description = "Coilovers for SR20DET",
                    Price = 1500,
                    SalesCount = 0,
                    Quantity = 5,
                    Category = category1,
                    CategoryId = category1.Id,
                    ImageId = image2.Id,
                };

                Part part3 = new Part
                {
                    Brand = "Brembo",
                    Model = "GT-R Brake Kit",
                    Description = "High-performance brake kit for 2JZ-GTE",
                    Price = 2500,
                    SalesCount = 0,
                    Quantity = 3,
                    Category = category2,
                    CategoryId = category2.Id,
                    ImageId = image3.Id,
                };
                Part part4 = new Part
                {
                    Brand = "AP Racing",
                    Model = "Brake Kit",
                    Description = "High-performance brake kit for SR20DET",
                    Price = 2000,
                    SalesCount = 0,
                    Quantity = 2,
                    Category = category2,
                    CategoryId = category2.Id,
                    ImageId = image4.Id,
                };

                Part part5 = new Part
                {
                    Brand = "K&N",
                    Model = "Cold Air Intake",
                    Description = "High-performance cold air intake for 2JZ-GTE",
                    Price = 300,
                    SalesCount = 0,
                    Quantity = 20,
                    Category = category3,
                    CategoryId = category3.Id,
                    ImageId = image5.Id,
                };

                Part part6 = new Part
                {
                    Brand = "AEM",
                    Model = "Cold Air Intake",
                    Description = "High-performance cold air intake for SR20DET",
                    Price = 350,
                    SalesCount = 0,
                    Quantity = 15,
                    Category = category3,
                    CategoryId = category3.Id,
                    ImageId = image6.Id,
                };
                await context.Parts.AddRangeAsync(part1, part2, part3, part4, part5, part6);
                await context.SaveChangesAsync();

                image1.PartId = part1.Id;
                image2.PartId = part2.Id;
                image3.PartId = part3.Id;
                image4.PartId = part4.Id;
                image5.PartId = part5.Id;
                image6.PartId = part6.Id;

                context.Images.UpdateRange(image1, image2, image3, image4, image5, image6);
                await context.SaveChangesAsync();

                Car car1 = new Car
                {
                    Brand = "Toyota",
                    Model = "Supra",
                    ModelYear = 1994,
                    CompatibleEngines = new List<Engine> { engine1 },
                    CompatibleParts = new List<Part> { part1, part3, part5 }
                };
                Car car2 = new Car
                {
                    Brand = "Nissan",
                    Model = "240SX",
                    ModelYear = 1995,
                    CompatibleEngines = new List<Engine> { engine2 },
                    CompatibleParts = new List<Part> { part2, part4, part6 }
                };

                part1.CompatibleCars = new List<Car> { car1 };
                part2.CompatibleCars = new List<Car> { car2 };
                part3.CompatibleCars = new List<Car> { car1 };
                part4.CompatibleCars = new List<Car> { car2 };
                part5.CompatibleCars = new List<Car> { car1 };
                part6.CompatibleCars = new List<Car> { car2 };
                context.Parts.UpdateRange(part1, part2, part3, part4, part5, part6);
                await context.SaveChangesAsync();

                await context.Cars.AddRangeAsync(car1, car2);
                await context.SaveChangesAsync();
            }
        }
    }
}
