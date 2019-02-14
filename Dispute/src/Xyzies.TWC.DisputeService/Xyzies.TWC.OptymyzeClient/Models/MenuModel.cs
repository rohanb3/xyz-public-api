using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class MenuModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        [JsonProperty("portalViews")]
        public List<PortalViewsModel> PortalViews { get; set; }
    }
}
