using Enigma.Common.Entities;
using Enigma.Common.Models;
using EnigmaApi.Extensions;
using EnigmaApi.Interfaces;
using EnigmaApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EnigmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceRepository _DeviceRepository;

        public DeviceController(IDeviceRepository deviceRepository)
        {
            _DeviceRepository = deviceRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<DeviceModel>> GetAllAsync()
        {
            var entities = await _DeviceRepository.GetAllAsync();
            
            List<DeviceModel> models = new();

            foreach (var entity in entities)
            {
                var model = entity.ToModel();
                models.Add(model);
            }
            return models;
        }

        [HttpGet("{name}")]
        public async Task<DeviceEntity> GetDeviceByName(string name) => await _DeviceRepository.GetDeviceByName(name);

        [HttpPost]
        public async Task PostNewDevice(DeviceModel device) => await _DeviceRepository.PostNewDevice(device.ToEntity());

        [HttpPut]
        public async Task<ActionResult<DeviceEntity>> UpdateDeviceByName(DeviceModel request)
        {
            var deviceEntity = request.ToEntity();
            var dbDeviceEntity = await _DeviceRepository.GetDeviceByName(deviceEntity.Name);

            if (dbDeviceEntity == null)
                return BadRequest("Device Not Found");

            dbDeviceEntity.Id = deviceEntity.Id;
            dbDeviceEntity.Name = deviceEntity.Name;
            dbDeviceEntity.Description = deviceEntity.Description;
            dbDeviceEntity.DeviceRotors = deviceEntity.DeviceRotors;


            return await _DeviceRepository.UpdateDeviceByName(dbDeviceEntity);

        }


        [HttpDelete("{name}")]
        public async Task DeleteDevice(string name) => await _DeviceRepository.DeleteDevice(name);
    }
}
