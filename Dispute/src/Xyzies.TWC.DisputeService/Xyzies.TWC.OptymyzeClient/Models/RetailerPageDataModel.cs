using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class RetailerPageDataModel : BaseModelForNextRequest
    {
        public string HtmlPage { get; set; }

        public string ViewId { get; set; }

        public string FilterCookie { get; set; }
    }
}
