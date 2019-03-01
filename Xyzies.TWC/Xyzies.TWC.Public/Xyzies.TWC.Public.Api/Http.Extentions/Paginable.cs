using System;

namespace Xyzies.TWC.Public.Api.Controllers.Http.Extentions
{
    /// <summary>
    /// Represents a pagination model
    /// </summary>
    public class Paginable
    {
        /// <summary>
        /// 15 items per page by default. You can change this value.
        /// </summary>
        public static int DEFAULT_PER_PAGE = 5;

        /// <summary>
        /// From
        /// </summary>
        public virtual Nullable<int> Skip { get; set; } = 0;

        /// <summary>
        /// Take next
        /// </summary>
        public virtual Nullable<int> Take { get; set; } = DEFAULT_PER_PAGE;
    }
}