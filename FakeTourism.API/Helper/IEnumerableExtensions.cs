using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection;

namespace FakeTourism.API.Helper
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
                this IEnumerable<TSource> source,
                string fields
        ) 
        {
            if (source == null) 
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            //to avoid data traverse, we will create a property info list
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                //return all the properties from ExpandoObject
                var propertyInfos = typeof(TSource)
                    .GetProperties(
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public | BindingFlags.Instance
                    );
                propertyInfoList.AddRange(propertyInfos);
            }
            else 
            {
                //split "," for string
                var fieldAfterSplit = fields.Split(",");
                foreach (var field in fieldAfterSplit) 
                {
                    //remove the space for string head and end
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase
                    | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null) 
                    {
                        throw new Exception($"Property {propertyName} cannot find" + $"{typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (TSource sourceObject in source) 
            {
                //Create ExpandoObject, and Data Shaping object
                var dataShapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList) 
                {
                    //Acquire the real data from related properties
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                expandoObjectList.Add(dataShapedObject);
            }
            return expandoObjectList;
        }
    }
}
