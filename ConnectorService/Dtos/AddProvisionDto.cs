using ConnectorService.Models;
using System.ComponentModel.DataAnnotations;

namespace ConnectorService.Dtos
{
    public class AddProvisionDto
    {
        [EnumDataType(typeof(Model))]
        [Required]
        public Model Model { get; set; }

        public string? Description;
    }
}

