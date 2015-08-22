using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ResourceMetadata.Core.Util
{
    public static class ReflectionUtil
    {

        /// <summary>
        /// Example: a class StatusObject have const Enable="E". This functions will be return "E"
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNameOfField(Type type, string value)
        {
            object obj = Activator.CreateInstance(type);
            FieldInfo[] consts = type.GetFields();
            foreach (var item in consts)
            {
                if (item.GetValue(obj).ToString() == value)
                {
                    return item.Name;
                }
            }
            return null;
        }

        /// <summary>
        /// Get value of the property from an object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(object source, string propertyName)
        {
            if (source == null || String.IsNullOrEmpty(propertyName)) return null;
            var property = source.GetType().GetProperty(propertyName);
            if (property == null) return null;
            return property.GetValue(source, null);
        }

        /// <summary>
        /// Get all properties name of Type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String[] GetPropertiesName(Type type)
        {
            return type.GetProperties().Select(x=>x.Name).ToArray();
        }

        /// <summary>
        /// Copy values of an object to an object
        /// </summary>
        /// <param name="objTo"></param>
        /// <param name="objFrom"></param>
        public static void CopyObject(this object objTo, object objFrom)
        {
            Type tObjFrom = objFrom.GetType();
            Type tObjTo = objTo.GetType();

            var listPropObj1 = tObjFrom.GetProperties().Where(p => p.GetValue(objFrom) != null).ToList();

            foreach (var item in listPropObj1)
            {
                if (tObjTo.GetProperty(item.Name) != null)
                {
                    tObjTo.GetProperty(item.Name).SetValue(objTo, item.GetValue(objFrom));
                }
            }
        }
    }
}
