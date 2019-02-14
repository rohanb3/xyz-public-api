using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class BaseModelForNextRequest
    {
        public string Cookies { get; set; }

        public string Referer { get; set; }

        public string Location { get; set; }

        public string XBrowserId { get; set; }
    }
}
