using Enigma.Common.Entities;
using EnigmaApi.Infrastructure.SQL;
using EnigmaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnigmaApi.Repositories
{
    public class DeviceRepository : IDeviceRepository
    {
        private readonly ApplicationDbContext _DbContext;


        //class injection of applicationdb into rotorrepop
        public DeviceRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }
        public async Task<IEnumerable<DeviceEntity>> GetAllAsync() => await _DbContext.Devices.Include(d => d.DeviceRotors).ToListAsync();
        public async Task<DeviceEntity> GetDeviceByName(string name) => await _DbContext.Devices.Include(d => d.DeviceRotors).SingleOrDefaultAsync(x => x.Name == name);

        public async Task<ActionResult<DeviceEntity>> UpdateDeviceByName(DeviceEntity device)
        {
            _DbContext.Devices.Update(device);

            await _DbContext.SaveChangesAsync();

            return device;

        }
        public async Task PostNewDevice(DeviceEntity device)
        {
            await _DbContext.Devices.AddAsync(device);
            await _DbContext.SaveChangesAsync();
        }

        public async Task DeleteDevice(string name)
        {
            var device = await _DbContext.Devices.FirstOrDefaultAsync(x => x.Name == name);

            _DbContext.Devices.Remove(device);
            await _DbContext.SaveChangesAsync();
        }
    }   
}
