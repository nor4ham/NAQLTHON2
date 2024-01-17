using Comman.CacheService;
using Comman.InterfaceCacheService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Hangfire;
using Minio;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NAQLTHON2", Version = "v1" });
    // Include XML comments if you have them
    // c.IncludeXmlComments("YourApiName.xml");

});
builder.Services.AddControllers();

// Added configuration for PostgreSQL
var configuration = builder.Configuration;

// For the ApiDbContext 
builder.Services.AddDbContext<Data.ApiDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Redis configuration
var redisConnection = configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
/* 
// Register the CacheService
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddAuthentication(options=>{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
    options.AddPolicy("Driver", policy => policy.RequireRole("Driver"));

});

builder.Services.AddIdentity<Comman.Models.ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<Data.ApiDbContext>()
        .AddDefaultTokenProviders();


// Initialize MinioClient using environment variables
var minio = new MinioClient(
    builder.Configuration["Minio:ServerAddress"],
    builder.Configuration["Minio:AccessKey"],
    builder.Configuration["Minio:SecretKey"]
);

builder.Services.AddSingleton(minio);





// Set access key and secret key using properties
minio.WithAccessKey(minioAccessKey, minioSecretKey);

builder.Services.AddSingleton(minio);

builder.Services.AddHostedService<SeedData>();

 */

var app = builder.Build();
// Configure the HTTP request pipeline.
var env = app.Services.GetRequiredService<IWebHostEnvironment>();
DatabaseInitializer.InitializeDatabase(app,env);

if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NAQLTHON2 V1");
        c.RoutePrefix = "swagger";
    });
}

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Map controller routes
// For MVC: app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
