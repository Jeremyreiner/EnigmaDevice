using Enigma.Common.Entities;
using Enigma.Common.Models;
using EnigmaApi.Extensions;
using EnigmaApi.Interfaces;
using EnigmaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using EnigmaApi.Services;

namespace EnigmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        readonly DatabaseService _DatabaseService;
        private readonly IDeviceRepository _DeviceRepository;

        public DeviceController(
            DatabaseService databaseService,
            IDeviceRepository deviceRepository)
        {
            _DatabaseService = databaseService;
            _DeviceRepository = deviceRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceModel>> GetAllAsync()
        {
            var entities = await _DeviceRepository.GetAllAsync();

            return entities.Select(e => e.ToModel());
        }

        [HttpGet("{name}")]
        public async Task<DeviceEntity?> GetDeviceByName(string name) => await _DeviceRepository.GetDeviceByName(name);

        [HttpPost]
        public async Task PostNewDevice(DeviceModel device) => await _DeviceRepository.PostNewDevice(device.ToEntity());

        
        [HttpPut]
        public async Task<ActionResult<DeviceEntity>> UpdateDeviceByName(DeviceModel model)
        {
            try
            {
                var updatedModel = await _DatabaseService.UpdateDeviceByName(model);
                return Ok(updatedModel);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpDelete("{name}")]
        public async Task DeleteDevice(string name) => await _DeviceRepository.DeleteDevice(name);
    }
}
