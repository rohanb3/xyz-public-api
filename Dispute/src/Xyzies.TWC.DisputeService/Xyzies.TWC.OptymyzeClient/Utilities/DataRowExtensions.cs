using System;
using System.Data;
using System.Reflection;

namespace Xyzies.TWC.OptymyzeClient.Utilities
{
    /// <summary>
    /// DataRow Extensions Methods and helpers
    /// </summary>
    public static class DataRowExtensions
    {
        public static TResult MapTo<TResult>(this DataRow row)
            where TResult : class, new()
        {
            if (row == null)
            {
                throw new NullReferenceException();
            }

            var @object = Activator.CreateInstance<TResult>();

            // go through each column
            foreach (DataColumn column in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo prop = @object.GetType().GetProperty(column.ColumnName);

                // if exists, set the value
                if (prop != null && row[column] != DBNull.Value)
                {
                    prop.SetValue(@object, row[column], null);
                }
            }

            return @object;
        }
    }
}
