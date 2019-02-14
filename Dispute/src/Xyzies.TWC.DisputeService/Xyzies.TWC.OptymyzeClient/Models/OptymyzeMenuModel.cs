using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class OptymyzeMenuModel : BaseModelForNextRequest
    {
        public IEnumerable<MenuModel> Menu { get; set; }
    }
}
