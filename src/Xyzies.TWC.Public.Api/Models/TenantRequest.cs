using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api.Models
{
    public class TenantRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }
}