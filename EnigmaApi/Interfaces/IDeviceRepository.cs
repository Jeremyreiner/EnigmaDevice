using Enigma.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EnigmaApi.Interfaces
{
    public interface IDeviceRepository
    {
        Task<IEnumerable<DeviceEntity>> GetAllAsync();

        Task<DeviceEntity?> GetDeviceByName(string name);

        Task PostNewDevice(DeviceEntity device);

        Task DeleteDevice(string name);

        Task<DeviceEntity> UpdateDeviceByName(DeviceEntity device);
    }
}
