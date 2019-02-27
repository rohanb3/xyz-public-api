using System.ComponentModel;

namespace Xyzies.TWC.Public.Data.Entities
{
    public enum BranchStatus
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("Active")]
        Active = 1,
        [Description("Inactive")]
        Inactive = 2
    }
}
