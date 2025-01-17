using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisLibrary.Repositories.Interface;
using VisLibrary.Utilities;

namespace VisLibrary.Repositories.Base
{
    public class PropertyProcessorBase: IPropertyProcessor
    {
        protected readonly ExceptionHandler _exceptionHandler;

        public PropertyProcessorBase(ExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// 去除所有字符串屬性的前後空白。
        /// </summary>
        public void TrimStringProperties<T>(T instance)
        {
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                var stringProperties = instance.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(string));

                foreach (var property in stringProperties)
                {
                    var currentValue = property.GetValue(instance) as string;
                    if (currentValue != null)
                    {
                        property.SetValue(instance, currentValue.Trim());
                    }
                }
            });
        }

        /// <summary>
        /// 將所有 DateTime 類型的屬性格式化為 "yyyy-MM-dd" 格式的字符串。
        /// </summary>
        public void FormatDateTimeProperties<T>(T instance)
        {
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                var properties = instance.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTime?) || p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var currentValue = property.GetValue(instance);

                    if (currentValue != null)
                    {
                        if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(instance, ((DateTime)currentValue).ToString("yyyy-MM-dd"));
                        }
                        else if (property.PropertyType == typeof(string) && DateTime.TryParse((string)currentValue, out DateTime dateValue))
                        {
                            property.SetValue(instance, dateValue.ToString("yyyy-MM-dd"));
                        }
                    }
                }
            });
        }

        /// <summary>
        /// 處理所有字符串和 DateTime 屬性，去除字符串前後空白並格式化 DateTime 屬性。
        /// </summary>
        public void ProcessProperties<T>(T instance)
        {
            _exceptionHandler.ExecuteWithExceptionHandling(() =>
            {
                TrimStringProperties(instance);
                //FormatDateTimeProperties(instance);
            });
        }

        /// <summary>
        /// 處理列表中每個對象的屬性。
        /// </summary>
        /// <param name="list">要處理的對象列表。</param>
        /// <returns>處理後的對象列表。</returns>
        public List<T> ProcessPropertiesForList<T>(List<T> list)
        {
            foreach (var data in list)
            {
                ProcessProperties(data);
            }
            return list;
        }
    }
}
