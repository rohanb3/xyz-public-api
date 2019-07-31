using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Xyzies.TWC.Public.Data.Attributes;

namespace Xyzies.TWC.Public.Api.Models
{
    public class AvatarModel
    {
        [Required]
        [FileType(".jpg,.jpeg,.png,.ico", 102400)]
        public IFormFile File { get; set; }
    }
}
