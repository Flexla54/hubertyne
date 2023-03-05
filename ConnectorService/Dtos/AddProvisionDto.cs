using ConnectorService.Models;
using System.ComponentModel.DataAnnotations;

namespace ConnectorService.Dtos
{
    public class AddProvisionDto
    {
        [Required]
        public string? Description;
    }
}

