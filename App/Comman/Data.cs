using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Data; 
public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
        
    }
    
            public DbSet<Customer.Models.Customer> Customers {get;set;}

        public DbSet<Driver.Models.Driver> Drivers {get;set;}
        public DbSet<Comman.Models.ApplicationUser> ApplicationUser {get;set;}
        public DbSet<IdentityRole> Roles { get; set; }



}
