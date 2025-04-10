using ISO810_ComprasProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ISO810_ComprasProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using ISO810_ComprasProject.Controllers;


var builder = WebApplication.CreateBuilder(args);

// Configure Connection String 
builder.Services.AddDbContext<ComprasDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/"+nameof(AccountController).Replace("Controller","") + "/" +nameof(AccountController.Login);
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
    });

//builder.Services.AddDefaultIdentity<Users>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ComprasDBContext>();

//// Add Identity
builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<ComprasDBContext>()
    .AddDefaultTokenProviders();




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization();


var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ComprasDBContext>();
    context.Database.EnsureCreated(); // Ensure the database is created if it doesn't exist
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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
    name: "default",
    pattern: "{controller=OrdenCompras}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
