using System;

namespace Xyzies.TWC.Public.Api.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class BranchContactModel
    {
        public Guid Id { get; set; }

        public string PersonName { get; set; }

        public string PersonLastName { get; set; }

        public string PersonTitle { get; set; }

        public string Value { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
