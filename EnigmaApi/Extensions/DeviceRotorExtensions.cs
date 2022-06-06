using Enigma.Common.Entities;
using Enigma.Common.Models;
using System.Linq;

namespace EnigmaApi.Extensions
{
    public static class DeviceRotorExtensions
    {
        public static DeviceEntity? ToEntity(this DeviceModel model)
        {
            return new DeviceEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                DeviceRotors = model.RotorsIds?.Select(rd => new DeviceRotorEntity { RotorId = rd}).ToList(),
            };
        }

        public static DeviceModel ToModel(this DeviceEntity entity)
        {
            return new DeviceModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                RotorsIds = entity.DeviceRotors?.Select(a => a.RotorId).ToList(),
            };
        }

        public static RotorEntity ToEntity(this RotorModel model)
        {
            return new RotorEntity
            {
                Id = model.Id,
                RotorId = model.RotorId,
                Name = model.Name,
                Type = model.Type,
                Wiring = model.Wiring,
                IsReflect = model.IsReflect,
                DeviceRotors = model.DeviceIds?.Select(rd => new DeviceRotorEntity { DeviceId = rd}).ToList(),
            };
        }

        public static RotorModel ToModel(this RotorEntity entity)
        {
            return new RotorModel
            {
                Id = entity.Id,
                RotorId = entity.RotorId,
                Name = entity.Name,
                Type = entity.Type,
                Wiring = entity.Wiring,
                IsReflect = entity.IsReflect,
                DeviceIds = entity.DeviceRotors?.Select(rd => rd.DeviceId).ToList(),

            };
        }
    }
}
