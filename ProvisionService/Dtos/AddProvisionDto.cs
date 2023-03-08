using ProvisionService.Models;
using System.ComponentModel.DataAnnotations;

namespace ProvisionService.Dtos
{
    public class AddProvisionDto
    {
        [Required]
        public string? Description;
    }
}

