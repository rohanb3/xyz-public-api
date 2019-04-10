using Microsoft.AspNetCore.Http;
using Xyzies.TWC.Public.Data.Attributes;

namespace Xyzies.TWC.Public.Api.Models
{
    public class AvatarModel
    {
        [FileType(".jpg,.jpeg,.png,.ico", 102400)]
        public IFormFile File { get; set; }
    }
}
