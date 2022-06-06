namespace Enigma.Common.Models
{
    public class RotorModel
    {
        public Guid Id { get; set; }

        public int? RotorId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Wiring { get; set; }

        public bool? IsReflect { get; set; }

        public virtual ICollection<Guid>? DeviceIds { get; set; }//nav propetry in table
    }
}
