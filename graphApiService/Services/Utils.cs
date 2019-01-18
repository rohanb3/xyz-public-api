using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using graphApiService.Dtos.User;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace graphApiService.Services
{
    public static class Utils
    {
        private static bool IsPropertyAccessible(PropertyInfo prop)
        {
            return prop.CanRead && prop.CanWrite;
        }

        public static void MergeObjects<T1, T2>(T1 source, T2 destination)
        {
            Type t1Type = typeof(T1);
            Type t2Type = typeof(T2);

            IEnumerable<PropertyInfo> t1PropertyInfos =
                t1Type.GetProperties().Where(IsPropertyAccessible);
            IEnumerable<PropertyInfo> t2PropertyInfos =
                t2Type.GetProperties().Where(IsPropertyAccessible);


            foreach (PropertyInfo t1PropertyInfo in t1PropertyInfos)
            {
                var propertyValue = t1PropertyInfo.GetValue(source);
                if (propertyValue != null)
                {
                    foreach (PropertyInfo t2PropertyInfo in t2PropertyInfos)
                    {
                        if (t1PropertyInfo.Name == t2PropertyInfo.Name)
                        {
                            t2PropertyInfo.SetValue(destination, propertyValue);
                            break;
                        }
                    }
                }
            }

        }
    }
}