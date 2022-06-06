using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enigma.Common.Entities
{
    [Table("Devices")]
    public class DeviceEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual ICollection<DeviceRotorEntity>? DeviceRotors { get; set; }//nav propetry in table
    }
}
