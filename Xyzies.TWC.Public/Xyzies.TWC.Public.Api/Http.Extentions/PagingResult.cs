using System.Collections.Generic;

namespace Xyzies.TWC.Public.Api.Controllers.Http.Extentions
{
    /// <summary>
    /// Represents a pagination result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class PagingResult<T> : IModelHttpResult
        where T : class
    {
        /// <summary>
        /// Collection of data
        /// </summary>
        public IList<T> Data { get; set; }

        /// <summary>
        /// The actual result of returned rows
        /// </summary>
        public int ReturnedRow { get => Data == null ? 0 : Data.Count; }

        /// <summary>
        /// Total length
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int ItemsPerPage { get; set; }
    }
}