using System;
using System.ComponentModel.DataAnnotations;

namespace Xyzies.TWC.Public.Api
{
    /// <summary>
    /// Represents a data sorting model
    /// </summary>
    public partial class Sortable
    {
        /// <summary>
        /// Default column name
        /// </summary>
        public static string DEFAULT_SORT_BY = "createddate";

        /// <summary>
        /// Default sorting direction
        /// </summary>
        public static string DEFAULT_SORT_ORDER = Direction.Desc;

        private string _sortBy = DEFAULT_SORT_BY;

        #region Props

        /// <summary>
        /// Criteria of sorting (column)
        /// </summary>
        public string SortBy
        {
           get => _sortBy;
           set => _sortBy = value;
        }

        /// <summary>
        /// Direction of sorting (allowed for asc/desc)
        /// </summary>
        [RegularExpression("(^$)|(asc|desc)", ErrorMessage = "Allowed only asc/desc values")]
        public string SortOrder { get; set; } = DEFAULT_SORT_ORDER;

        #endregion

        #region Helpers

        /// <summary>
        /// Is it ascending sorting direction
        /// </summary>
        /// <returns></returns>
        public bool IsAscending() => string.IsNullOrEmpty(SortOrder) ? false :
            SortOrder.Equals(Direction.Asc, StringComparison.OrdinalIgnoreCase);
        
        /// <summary>
        /// Sorting direction options
        /// </summary>
        public static class Direction
        {
            /// <summary>
            /// Sorting by ascending
            /// </summary>
            public static readonly string Asc = "asc";

            /// <summary>
            /// Sorting by descending
            /// </summary>
            public static readonly string Desc = "desc";
        }

        #endregion
    }
}
