using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Comman.Models; // Replace with your namespace

public class SeedData : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedData(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Driver"))
        {
            var role = new IdentityRole { Name = "Driver" };
            await roleManager.CreateAsync(role);
        }

        // Add more roles as needed
    }

    private async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.FindByNameAsync("admin") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                // Add other user properties as needed
            };

            var result = await userManager.CreateAsync(user, "password123"); // Set the user's password
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Driver");
            }
        }

        // Add more users as needed
    }
}
