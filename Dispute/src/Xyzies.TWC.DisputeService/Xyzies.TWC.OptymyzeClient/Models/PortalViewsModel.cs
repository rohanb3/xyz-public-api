using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class PortalViewsModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ViewId { get; set; }

        public PortalViewDataModel Data { get; set; }
    }
}
