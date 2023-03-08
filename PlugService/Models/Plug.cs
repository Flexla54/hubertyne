using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlugService.Models
{
    public class Plug
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsConnected { get; set; }
        public bool IsTurnedOn { get; set; }
        public Guid UserId { get; set; }

    }
}
