using Enigma.Common.Entities;

namespace Enigma.Common.Models
{
    public class DeviceModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual List<Guid>? RotorsIds { get; set; }//nav propetry in table
    }
}
