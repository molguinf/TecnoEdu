using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TecnoEdu.Data;
using TecnoEdu.Models.Entities;
using TecnoEdu.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TecnoEduDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TecnoEduConnection")));

// Necesario para generar AspNetUsers, AspNetRoles y sus relaciones.
// Coordinar las vistas de Login/Registro con el integrante de autenticación.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<TecnoEduDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ISeatAvailabilityService, SeatAvailabilityService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
