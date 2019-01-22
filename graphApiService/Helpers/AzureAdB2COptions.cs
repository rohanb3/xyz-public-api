using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graphApiService.Helpers
{
    public class AzureAdB2COptions
    {
        public string Instance { get; set; }
        public string ClientId { get; set; }
        public string Domain { get; set; }
        public string SignUpSignInPolicyId { get; set; }
        public string ReplyUrl { get; set; }
    }
}
