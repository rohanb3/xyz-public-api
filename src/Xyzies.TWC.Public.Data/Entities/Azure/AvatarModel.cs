using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Xyzies.TWC.Public.Data.Attributes;

namespace Xyzies.TWC.Public.Data.Entities.Azure
{
    public class AvatarModel
    {
        [FileType(".jpg,.jpeg,.png,.ico", 102400)]
        public IFormFile File { get; set; }
    }
}
