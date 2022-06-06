using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.Common.Entities
{
    [Table("Rotors")]
    public class RotorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public int? RotorId { get; set; }

        public string? Name { get; set; }

        public string? Type { get; set; }

        public string? Wiring { get; set; }

        public bool? IsReflect { get; set; }
        
        public virtual ICollection<DeviceRotorEntity>? DeviceRotors { get; set; }//nav propetry in table
    }
}