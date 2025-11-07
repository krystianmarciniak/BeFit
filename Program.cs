using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;   // ApplicationDbContext, ApplicationUser
using BeFit;

var builder = WebApplication.CreateBuilder(args);

// === DB: SQLite ===
var cs = builder.Configuration.GetConnectionString("DefaultConnection")
         ?? "Data Source=Data/app.db";
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlite(cs));

// === Identity z wbudowanym UI (Razor Pages /Identity/...) ===
builder.Services.AddDefaultIdentity<ApplicationUser>(o =>
{
    o.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// === pipeline ===
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection(); // tylko w produkcji
}

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // musi być przed ewentualnym fallbackiem do Home

await Seed.EnsureAsync(app.Services);
app.Run();
