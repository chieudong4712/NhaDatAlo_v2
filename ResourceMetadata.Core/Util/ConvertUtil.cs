using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using ResourceMetadata.Core.ComponentModel;

namespace ResourceMetadata.Core.Util
{
    /// <summary>
    /// Summary description for ConvertUtil
    /// </summary>
    public class ConvertUtil
    {
        public ConvertUtil()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static long? ToLong(object str)
        {
            if (str == null)
            {
                return null;
            }
            string st = str.ToString();
            long result = 0;
            if (String.IsNullOrEmpty(st))
            {
                return null;
            }
            if (long.TryParse(st, out result))
            {
                return result;
            }
            return null;
        }
        public static char? ToChar(object obj)
        {
            if (obj != null)
            {
                char r;
                if (char.TryParse(obj.ToString(), out r))
                {
                    return r;
                }

            }
            return null;
        }
        public static string ToString(object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return String.Empty;
        }
        public static int? ToInt(object str)
        {
            int result = 0;
            if (str == null)
            {
                return null;
            }
            if (int.TryParse(str.ToString(), out result))
            {
                return result;
            }
            return null;
        }
        public static DateTime? ToDateTime(object obj)
        {
            DateTime date;
            if (obj != null && DateTime.TryParse(obj.ToString(), out date))
            {
                return date;
            }
            else
            {
                return null;
            }
        }
        public static short? ToShort(string str, short? defaultIfNull = null)
        {
            short result = 0;
            if (String.IsNullOrEmpty(str))
            {
                if (defaultIfNull.HasValue)
                {
                    return defaultIfNull;
                }
                return null;
            }
            if (short.TryParse(str, out result))
            {
                return result;
            }
            return null;
        }
        public static T ToEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static TypeConverter GetCustomTypeConverter(Type type)
        {
            //we can't use the following code in order to register our custom type descriptors
            //TypeDescriptor.AddAttributes(typeof(List<int>), new TypeConverterAttribute(typeof(GenericListTypeConverter<int>)));
            //so we do it manually here

            if (type == typeof(List<int>))
                return new GenericListTypeConverter<int>();
            if (type == typeof(List<decimal>))
                return new GenericListTypeConverter<decimal>();
            if (type == typeof(List<string>))
                return new GenericListTypeConverter<string>();
            
            return TypeDescriptor.GetConverter(type);
        }

        #region To
        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType, CultureInfo culture)
        {
            if (value != null)
            {
                var sourceType = value.GetType();

                TypeConverter destinationConverter = GetCustomTypeConverter(destinationType);
                TypeConverter sourceConverter = GetCustomTypeConverter(sourceType);
                if (destinationConverter != null && destinationConverter.CanConvertFrom(value.GetType()))
                    return destinationConverter.ConvertFrom(null, culture, value);
                if (sourceConverter != null && sourceConverter.CanConvertTo(destinationType))
                    return sourceConverter.ConvertTo(null, culture, value, destinationType);
                if (destinationType.IsEnum && value is int)
                    return Enum.ToObject(destinationType, (int)value);
                if (!destinationType.IsAssignableFrom(value.GetType()))
                    return Convert.ChangeType(value, destinationType, culture);
            }
            return value;
        }


        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="destinationType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public static object To(object value, Type destinationType)
        {
            return To(value, destinationType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a value to a destination type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <returns>The converted value.</returns>
        public static T To<T>(object value)
        {
            //return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            return (T)To(value, typeof(T));
        }
        #endregion

        #region SelectList
        /// <summary>
        /// Convert IEnumerable to SelectList
        /// T must have Id, Title (or ...Name) fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(IEnumerable<T> list, string selectedValue = "", string emptyString = "", string textField = null, string valueField = null) where T : class
        {
            var source = typeof(T);
            var target = typeof(SelectListItem);

            textField = textField ?? "Title";
            valueField = valueField ?? "Id";

            var textSource = source.GetProperty(textField);
            var valueSource = source.GetProperty(valueField);
            var propertyWithName = source.GetProperties().FirstOrDefault(x => x.Name.Contains("Name"));
            if (textSource == null && propertyWithName != null)
            {
                textSource = source.GetProperty(propertyWithName.Name);
            }

            if (textSource == null || valueSource == null)
            {
                throw new Exception("The T must have Id, Title or a field contain 'Name' ");
            }

            var t = Expression.Parameter(source, "t");

            var sourceText = Expression.MakeMemberAccess(t, textSource);
            var sourceId = Expression.MakeMemberAccess(t, valueSource);
            var sourceValue = Expression.Call(Expression.Convert(sourceId, typeof(object)),
                typeof(object).GetMethod("ToString"));

            //assign to SelectListItem
            var assignName = Expression.Bind(target.GetProperty("Text"), sourceText);
            var assignValue = Expression.Bind(target.GetProperty("Value"), sourceValue);
            var targetNew = Expression.New(target);
            var init = Expression.MemberInit(targetNew, assignName, assignValue);

            var lambda = (Expression<Func<T, SelectListItem>>)Expression.Lambda(init, t);
            var selectListItem = list.AsQueryable().Select(lambda).ToList();

            if (!String.IsNullOrEmpty(emptyString))
            {
                selectListItem.Insert(0, new SelectListItem { Text = emptyString, Value = null, Selected = false});
            }

            if (!String.IsNullOrEmpty(selectedValue))
            {
                var item = selectListItem.FirstOrDefault(x => x.Value == selectedValue);
                if (item != null) item.Selected = true;
            }


            return selectListItem;
        }

        /// <summary>
        /// Convert enum type to SelectListItem
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedValue"></param>
        /// <param name="emptyString"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>(string selectedValue = "", string emptyString = "") where T : struct, IConvertible
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);
            var selectListItem = new List<SelectListItem>();
            foreach (var value in source)
            {
                var field = value.GetType().GetField(value.ToString());

                var attrs = (DisplayAttribute)field.
                              GetCustomAttributes(displayAttributeType, false).FirstOrDefault();

                var text = attrs != null ? attrs.GetName() : field.Name;

                selectListItem.Add(new SelectListItem()
                {
                    Value = ((int)value).ToString(),
                    Text = text,
                });
            }

            if (!String.IsNullOrEmpty(selectedValue))
            {
                var item = selectListItem.FirstOrDefault(x => x.Value == selectedValue);
                if (item != null) item.Selected = true;
            }

            return selectListItem;
        }
        #endregion
    }
}