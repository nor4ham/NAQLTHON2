using Data;
using Comman.InterfaceCacheService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Driver.Controller;
[Authorize(Policy = "Driver")]
[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ApiDbContext _context;
    private readonly ICacheService _cacheService;
    public DriversController(
        ILogger<DriversController> logger,
        ApiDbContext context,ICacheService cacheService)
    {
        _logger = logger;
        _context = context;
        _cacheService=cacheService ;
    }

    // GET: api/Drivers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Driver>>> GetDrivers()
    {
        return await _context.Drivers.FromSqlRaw("SELECT * FROM drivers").ToListAsync();

    }


    // GET: api/Drivers
    [HttpGet("Cache")]
    public async Task<ActionResult> GetCache()
    {
        
        var cacheData = _cacheService.GetData<IEnumerable<Models.Driver>>("drivers");
        if (cacheData != null && cacheData.Count() > 0)
         return Ok(cacheData);
        cacheData = await _context.Drivers.ToListAsync(); 
        var  expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<IEnumerable<Models.Driver>>("drivers",cacheData,expiryTime);
        return Ok(cacheData);
    }


   // POST: api/Drivers
    [HttpPost]
    public async Task<ActionResult> PostDriver(DriverRequestDto driver)
    {
        
        // Insert query
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO drivers (drivername, licensenumber, dateOfbirth, vehicletype, email, phonenumber, status)
            VALUES ({driver.DriverName}, {driver.LicenseNumber}, {driver.DateOfBirth}, {driver.VehicleType}, {driver.Email}, {driver.PhoneNumber}, {driver.Status})
        ");        

        // Assuming an auto-generated ID, fetch it back
        var newDriver = await _context.Drivers.OrderByDescending(d => d.Id).FirstOrDefaultAsync();

        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<Models.Driver>($"drivers{newDriver.Id}", newDriver, expiryTime);

        return Ok(newDriver);
    }


    // GET: api/Drivers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Driver>> GetDriver(int id)
    {
        var driver = await _context.Drivers.FromSqlInterpolated($"SELECT * FROM drivers WHERE Id = {id}")
        .FirstOrDefaultAsync();
;

        if (driver == null)
        {
            return NotFound();
        }

        return driver;
    }

    // PUT: api/Drivers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDriver(int id, DriverRequestDto driver)
    {
        // Update query
        var result = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE drivers SET
                drivername = {driver.DriverName},
                licensenumber = {driver.LicenseNumber},
                dateofbirth = {driver.DateOfBirth},
                vehicletype = {driver.VehicleType},
                email = {driver.Email},
                phonenumber = {driver.PhoneNumber},
                status = {driver.Status}
            WHERE id = {id}
        ");

        if (result == 0) // No rows affected
        {
            return NotFound();
        }

        return NoContent();
    }


    // DELETE: api/Drivers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDriver(int id)
    {
        var result = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            DELETE FROM drivers WHERE id = {id}
        ");

        if (result == 0) // No rows affected
        {
            return NotFound();
        }

        return NoContent();
    }

// DELETE: api/Drivers/Cache/{id}
[HttpDelete("Cache/{id}")]
public async Task<IActionResult> DeleteCache(int id)
{
    return Ok(_cacheService.RemoveData($"drivers{id}"));

}


    
}
