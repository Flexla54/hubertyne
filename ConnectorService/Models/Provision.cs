using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConnectorService.Models
{
    public enum Model
    {
        SHELLY_PLUG_S
    }

    public class Provision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Model Model;

        public Guid UserId { get; set; }
        
        public string? Description { get; set; }
    }
}
