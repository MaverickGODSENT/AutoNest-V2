using AutoNest.Data.Common.Repositories;
using AutoNest.Data.Entities;
using AutoNest.Data.Repositories;
using AutoNest.Services.Carts;
using AutoNest.Services.Categories;
using AutoNest.Services.Engines;
using AutoNest.Services.Orders;
using AutoNest.Services.Parts;
using AutoNest.Services.Stripe;
using AutoNest.Web.Areas.Admin.Services;
using AutoNest.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Repositories
builder.Services.AddScoped<IDeletableEntityRepository<Category>, DeletableEntityRepository<Category>>();
builder.Services.AddScoped<IDeletableEntityRepository<Engine>, DeletableEntityRepository<Engine>>();
builder.Services.AddScoped<IDeletableEntityRepository<Part>, DeletableEntityRepository<Part>>();
builder.Services.AddScoped<IRepository<Image>, Repository<Image>>();
builder.Services.AddScoped<IDeletableEntityRepository<Cart>, DeletableEntityRepository<Cart>>();
builder.Services.AddScoped<IDeletableEntityRepository<CartItem>, DeletableEntityRepository<CartItem>>();
builder.Services.AddScoped<IDeletableEntityRepository<Payment>, DeletableEntityRepository<Payment>>();
builder.Services.AddScoped<IDeletableEntityRepository<Order>, DeletableEntityRepository<Order>>();

// Identity config
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// Services
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IEngineService, EngineService>();
builder.Services.AddTransient<IPartService, PartService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IStripeService, StripeService>();
builder.Services.AddTransient<IAdminService, AdminService>();

var app = builder.Build();

// Role check
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    await EnsureRolesExist(roleManager, userManager);
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

async Task EnsureRolesExist(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    string[] roleNames = { "Admin", "Moderator", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = "rladmin@admin.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(newAdmin, "Galin4o@125");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }


    var allUsers = userManager.Users.ToList();
    foreach (var user in allUsers)
    {
        var roles = await userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}

