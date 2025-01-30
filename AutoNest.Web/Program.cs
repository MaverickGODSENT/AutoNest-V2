using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Repositories;
using AutoNest.Web.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoNest.Services.Categories;
using AutoNest.Services.Engines;
using AutoNest.Data.Entities;
using AutoNest.Services.Parts;
using AutoNest.Services.Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Repositories
builder.Services.AddScoped<IDeletableEntityRepository<Category>, DeletableEntityRepository<Category>>();
builder.Services.AddScoped<IDeletableEntityRepository<Engine>, DeletableEntityRepository<Engine>>();
builder.Services.AddScoped<IDeletableEntityRepository<Part>, DeletableEntityRepository<Part>>();
builder.Services.AddScoped<IDeletableEntityRepository<Payment>, DeletableEntityRepository<Payment>>();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//Services
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEngineService, EngineService>();
builder.Services.AddTransient<IPartService, PartService>();
// builder.Services.AddTransient<IStripeService, StripeService>(); TO DO

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
