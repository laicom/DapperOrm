using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using DapperOrm.Model;

namespace DapperOrm
{
    /// <summary>
    ///          ORMHelper Class
    /// Description:      ORM映射信息获取及数据层访问的实现 
    /// Author:     赖国欣  Guoxin.lai@gmail.com
    /// Create Date: 2009-6-17
    /// Update: 
    /// 
    /// </summary>
    internal class ORMHelper
    {
        /// <summary>
        /// 通过反射来实现将datareader数据结果取出
        /// </summary>
        /// <typeparam name="T">结果类型或保存结果的实体类型</typeparam>
        /// <param name="dr">datareader</param>
        /// <returns></returns>
        public static T ReaderResult<T>(System.Data.IDataReader dr)
        {
            return ReaderResult<T>(dr, "*");
        }

        /// <summary>
        /// 通过反射来实现将datareader数据结果取出
        /// </summary>
        /// <typeparam name="T">结果类型或保存结果的实体类型</typeparam>
        /// <param name="dr">datareader</param>
        /// <returns></returns>
        public static T ReaderResult<T>(IDataReader dr, string searchFields)
        {
            if (searchFields != "*" && !searchFields.Contains(","))//查询指定的字段，结果是只有一个值
            {
                return (T)dr[0];
            }

            System.Type type = typeof(T);
            object instance = type.Assembly.CreateInstance(type.FullName);
            PropertyInfo[] props = instance.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.PropertyType.IsNotPublic || !prop.CanWrite) continue;
                OrmFieldAttribute attrib = GetMappingFieldAttrib(type, prop);
                string fieldName = prop.Name;
                Type convertType = null;
                if (attrib != null)
                {
                    if (attrib.WriteOnly) continue;
                    if (!string.IsNullOrEmpty(attrib.FieldName))
                        fieldName = attrib.FieldName;
                    convertType = attrib.TypeConverter;
                    //if (!string.IsNullOrEmpty(attrib.SupportsLanguages))
                    //{
                    //    string[] supportsLangList = attrib.SupportsLanguages.Split(',');
                    //    string language = MutiLanges.GetMatchLanguage(supportsLangList);
                    //    if (!string.IsNullOrEmpty(language))
                    //        fieldName += "_" + language;
                    //}
                }
                if (searchFields != "*" && searchFields.IndexOf(fieldName, StringComparison.InvariantCultureIgnoreCase) == -1) continue;
                try
                {
                    if (convertType != null) //需要类型转换代理
                    {
                        IOrmTypeConverter converter = (IOrmTypeConverter)convertType.Assembly.CreateInstance(convertType.FullName);
                        object value = converter.ConvertToObj(dr[fieldName]);
                        prop.SetValue(instance, value, null);
                    }
                    else
                    {
                        if (dr[fieldName] is DBNull)
                            prop.SetValue(instance, null, null);
                        else
                            prop.SetValue(instance, dr[fieldName], null);
                    }

                }
                catch (Exception ex)
                {
                    string message = string.Format("ORM Exception in ReadData. Please check the mapping between {0}.{1} and {2}. Inner Exception message:{3} .", type.Name, prop.Name, fieldName, ex.Message);
                    if (convertType != null)
                        message += "Customer type converter:" + convertType.FullName;
                    throw new OrmException(message, ex);
                } 
            }
            return (T)instance;
        }


        /// <summary>
        /// 通过反射获得实体类与表名的映射关系
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static OrmTableAttribute GetMappingTable(Type entityType)
        {
            object[] propAttribs = entityType.GetCustomAttributes(typeof(OrmTableAttribute), false);
            if (propAttribs.Length != 0)
            {
                return (OrmTableAttribute)propAttribs[0];
            }
            else
            {
                return new OrmTableAttribute(entityType.Name);
            }
        }

        /// <summary>
        /// 通过反映获取属性对应的表字段.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetMappingField(Type entityType, string propertyName)
        {
            PropertyInfo prop = entityType.GetProperty(propertyName);
            if (prop == null)
            {
                string message = string.Format("ORM Exception in GetMappingField. Property {0} in {1} not found.", propertyName, entityType.Name);
                throw new OrmException(message);
            }
            return GetMappingField(entityType, prop);
        }

        /// <summary>
        /// 通过反映获取属性对应的表字段.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="prop">The prop.</param>
        /// <returns></returns>
        public static string GetMappingField(Type entityType, PropertyInfo prop)
        {
            object[] propAttribs = prop.GetCustomAttributes(typeof(OrmFieldAttribute), false);
            if (propAttribs.Length != 0)
            {
                OrmFieldAttribute propAttrib = (OrmFieldAttribute)propAttribs[0];
                if (propAttrib.Ommitted) return null;
                if (!string.IsNullOrEmpty(propAttrib.FieldName))
                    return propAttrib.FieldName;
            }
            return prop.Name;
        }

        /// <summary>
        /// 获取映谢的字段
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static OrmFieldAttribute GetMappingFieldAttrib(Type entityType, PropertyInfo prop)
        {
            if (prop == null)
            {
                string message = string.Format("ORM Exception in GetMappingField. Property {0} in {1} not found.", prop.Name, entityType.Name);
                throw new OrmException(message);
            }
            object[] propAttribs = prop.GetCustomAttributes(typeof(OrmFieldAttribute), false);
            if (propAttribs.Length != 0)
            {
                return (OrmFieldAttribute)propAttribs[0];
            }
            return null;
        }

        /// <summary>
        /// Gets the mapping field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetMappingField<T>(string propertyName)
        {
            Type t = typeof(T);
            PropertyInfo prop = t.GetProperty(propertyName);
            return GetMappingField(t, prop);
        }

        /// <summary>
        /// 通过反映获取支持KeySearch的字段.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        public static List<string> GetKeySearchFields(Type entityType)
        {
            List<string> keyFields = new List<string>();
            PropertyInfo[] props = entityType.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] propAttribs = prop.GetCustomAttributes(typeof(OrmFieldAttribute), false);
                if (propAttribs.Length != 0)
                {
                    OrmFieldAttribute propAttrib = (OrmFieldAttribute)propAttribs[0];
                    if (propAttrib.KeySearch)
                    {
                        string fieldName = string.IsNullOrEmpty(propAttrib.FieldName) ? prop.Name : propAttrib.FieldName;
                        keyFields.Add(fieldName);
                    }
                }
            }
            return keyFields;
        }
    }
}
