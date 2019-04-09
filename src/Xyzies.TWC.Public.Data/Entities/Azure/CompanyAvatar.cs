using Microsoft.AspNetCore.Http;
using Xyzies.TWC.Public.Data.Attributes;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities.Azure
{
    public class CompanyAvatar: BaseEntity<string>
    {
        public override string Id { get; set; }

        [FileType(".jpg,.jpeg,.png,.ico", 102400)]
        public IFormFile File { get; set; }
    }
}
