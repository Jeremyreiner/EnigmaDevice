namespace Enigma.Common.Entities
{
    public class DeviceRotorEntity
    {
        public Guid DeviceId { get; set; }
        public DeviceEntity? Device { get; set; }

        public Guid RotorId { get; set; }
        public RotorEntity? Rotor { get; set; }
    }
}
