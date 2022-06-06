using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enigma.Common.Models;
using EnigmaApi.Extensions;
using EnigmaApi.Interfaces;

namespace EnigmaApi.Services
{
    public class DatabaseService
    {
        readonly IServiceScopeFactory _ServiceScopeFactory;

        public DatabaseService(IServiceScopeFactory serviceScopeFactory)
        {
            _ServiceScopeFactory = serviceScopeFactory;
        }

        public async Task<DeviceModel> UpdateDeviceByName(DeviceModel model)
        {
            var deviceEntity = model.ToEntity();
            Debug.Assert(deviceEntity != null, nameof(deviceEntity) + " != null");

            using var scope = _ServiceScopeFactory.CreateScope();

            var repo = scope.ServiceProvider.GetService<IDeviceRepository>();

            var updatedEntity = await repo!.UpdateDeviceByName(deviceEntity);

            return updatedEntity.ToModel();
        }
    }
}
