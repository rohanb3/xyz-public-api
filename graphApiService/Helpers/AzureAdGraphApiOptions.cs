﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace graphApiService.Helpers
{
    public class AzureAdGraphApiOptions
    {
        public string AppId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Resource { get; set; }
        public string GrantType { get; set; }
        public string RequestUri { get; set; }
    }
}
