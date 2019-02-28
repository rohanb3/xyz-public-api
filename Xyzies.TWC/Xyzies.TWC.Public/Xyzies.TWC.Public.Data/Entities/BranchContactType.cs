using System.Collections.Generic;
using Xyzies.TWC.Public.Data.Core;

namespace Xyzies.TWC.Public.Data.Entities
{
    public class BranchContactType : BaseEntity<int>
    {
        public new int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BranchContact> BranchContacts { get; set; } = new List<BranchContact>();
    }
}