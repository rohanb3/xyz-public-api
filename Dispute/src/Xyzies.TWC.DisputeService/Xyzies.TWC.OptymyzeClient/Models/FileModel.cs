using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Xyzies.TWC.OptymyzeClient.Models
{
    public class FileModel
    {
        public string Name { get; set; }

        public Stream File { get; set; }

        public long Size { get; set; }
    }
}
