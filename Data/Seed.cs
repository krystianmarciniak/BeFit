using System;
using System.Linq;
using System.Threading.Tasks;
using BeFit.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BeFit
{
  public static class Seed
  {
    public static async Task EnsureAsync(IServiceProvider services)
    {
      using var scope = services.CreateScope();
      var sp = scope.ServiceProvider;

      var db = sp.GetRequiredService<ApplicationDbContext>();
      var users = sp.GetRequiredService<UserManager<ApplicationUser>>();
      var roles = sp.GetRequiredService<RoleManager<IdentityRole>>();
      var hasher = sp.GetRequiredService<IPasswordHasher<ApplicationUser>>();

      // 0) Migracje bazy (SQLite/SQL) – bez tego rola/userzy mogą się nie dodać
      await db.Database.MigrateAsync();

      // 1) Role – utwórz, jeśli nie istnieją
      string[] roleNames = { "Admin", "User" };
      foreach (var r in roleNames)
        if (!await roles.RoleExistsAsync(r))
          _ = await roles.CreateAsync(new IdentityRole(r));

      // 2) Helper do tworzenia/aktualizacji użytkownika + ról
      async Task<ApplicationUser> EnsureUserAsync(string email, string password, params string[] rolesToAdd)
      {
        var u = await users.FindByEmailAsync(email);

        if (u is null)
        {
          // Tworzymy od razu z hasłem (prostsze)
          u = new ApplicationUser
          {
            UserName = email,
            Email = email,
            EmailConfirmed = true
          };

          var create = await users.CreateAsync(u, password);
          if (!create.Succeeded)
            throw new Exception("Nie udało się utworzyć użytkownika: " +
                                string.Join("; ", create.Errors.Select(e => e.Description)));
        }
        else
        {
          // Użytkownik istnieje – wymuś/odśwież hasło (bez tokenów resetu)
          u.PasswordHash = hasher.HashPassword(u, password);
          var upd = await users.UpdateAsync(u);
          if (!upd.Succeeded)
            throw new Exception("Nie udało się zaktualizować hasła: " +
                                string.Join("; ", upd.Errors.Select(e => e.Description)));
        }

        // Role – dodaj wszystkie brakujące
        foreach (var r in rolesToAdd.Distinct())
        {
          if (!await users.IsInRoleAsync(u, r))
          {
            var addRole = await users.AddToRoleAsync(u, r);
            if (!addRole.Succeeded)
              throw new Exception($"Nie udało się dodać roli '{r}': " +
                                  string.Join("; ", addRole.Errors.Select(e => e.Description)));
          }
        }

        return u;
      }

      // 3) Użytkownicy domyślni
      // Admin ma obie role: Admin + User
      await EnsureUserAsync("admin@befit.local", "Admin#12345", "Admin", "User");

      // Konto demonstracyjne – zwykły użytkownik
      await EnsureUserAsync("demo@befit.local", "User#12345", "User");
    }
  }
}
