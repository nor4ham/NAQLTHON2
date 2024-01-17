
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data; 

namespace Driver.Services.GetDrivers
{
    public class GetDriversService
    {
        private readonly ApiDbContext _context;

        public GetDriversService(ApiDbContext context)
        {
            _context = context;
        }

    }
}
