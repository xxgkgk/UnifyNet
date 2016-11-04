using System;
using System.Reflection;

namespace UnCommon.Tool
{
    /// <summary>  
    /// 枚举帮助类  
    /// </summary>  
    public class UnEnumHelper
    {
        /// <summary>  
        /// 获取枚举项的Attribute  
        /// </summary>  
        /// <typeparam name="T">自定义的Attribute</typeparam>  
        /// <param name="source">枚举</param>  
        /// <returns>返回枚举,否则返回null</returns>  
        public static T getCustomAttribute<T>(Enum source) where T : Attribute
        {
            Type sourceType = source.GetType();
            string sourceName = Enum.GetName(sourceType, source);
            FieldInfo field = sourceType.GetField(sourceName);
            object[] attributes = field.GetCustomAttributes(typeof(T), false);
            foreach (object attribute in attributes)
            {
                if (attribute is T)
                    return (T)attribute;
            }
            return null;
        }

        /// <summary>  
        ///获取DescriptionAttribute描述  
        /// </summary>  
        /// <param name="source">枚举</param>  
        /// <returns>有description标记，返回标记描述，否则返回null</returns>  
        public static string getDescription(Enum source)
        {
            var attr = getCustomAttribute<System.ComponentModel.DescriptionAttribute>(source);
            if (attr == null)
                return null;

            return attr.Description;
        }
    }

}
