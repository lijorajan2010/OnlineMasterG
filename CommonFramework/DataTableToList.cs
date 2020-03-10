using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OnlineMasterG.CommonFramework
{
    public static class DataTableToList
    {
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        var value = dr[column.ColumnName];
                        //find out the type
                        Type type = value.GetType();

                        //find the property type
                        Type propertyType = pro.PropertyType;

                        //Convert.ChangeType does not handle conversion to nullable types
                        //if the property type is nullable, we need to get the underlying type of the property
                        var targetType = IsNullableType(pro.PropertyType) ? Nullable.GetUnderlyingType(pro.PropertyType) : pro.PropertyType;

                        //Returns an System.Object with the specified System.Type and whose value is
                        //equivalent to the specified object.
                        value = Convert.ChangeType(value, targetType);

                        //Set the value of the property
                        pro.SetValue(obj, value, null);

                      //  pro.SetValue(obj, dr[column.ColumnName], null);
                    }

                      
                    else
                        continue;
                }
            }
            return obj;
        }
        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }

    }

}