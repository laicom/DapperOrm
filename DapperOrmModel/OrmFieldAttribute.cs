using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// 用于建立实体类属性与数据库表字段名的映射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OrmFieldAttribute : System.Attribute
    {
        public OrmFieldAttribute(string fieldName)
        {
            _fieldName = fieldName;
        }
        public OrmFieldAttribute(string fieldName,bool isKeySearchField)
        {
            _fieldName = fieldName;
            _keySearch = isKeySearchField;
        }
        public OrmFieldAttribute(string fieldName, bool isKeySearchField,bool isNullable)
        {
            _fieldName = fieldName;
            _keySearch = isKeySearchField;
            IsNullable = isNullable;
        }

        public OrmFieldAttribute()
        {

        }

        private string _fieldName;
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName=value; }
        }


        private bool _ommitted;
        /// <summary>
        /// 忽略此字段的映射关系
        /// </summary>
        public bool Ommitted 
        {
            get{return _ommitted;} 
            set 
            { 
                _ommitted = value;
                if (value) { WriteOnly = ReadOnly = value; }
            } 
        }

        /// <summary>
        /// 只写
        /// </summary>
        public bool WriteOnly { get; set; }
        /// <summary>
        /// 只读
        /// </summary>
        public bool ReadOnly { get; set; }

        private bool _keySearch;
        /// <summary>
        /// 支持通配查询的项（即KeySearch会查找该字段）
        /// </summary>
        public bool KeySearch
        {
            get { return _keySearch; }
            set { _keySearch = value; }
        }

        private Type _typeConverter = null;
        /// <summary>
        ///  需要指定自定义类型转换器类型，自定义转换器类必须实现 IORMappingTypeConverter接口
        /// </summary>
        public Type TypeConverter
        {
            get { return _typeConverter; }
            set { _typeConverter = value; }
        }

        private string _supportsLanguage;
        /// <summary>
        /// 国际化支持/多语种，以逗号分隔的字符串
        /// </summary>
        public string SupportsLanguages
        {
            get { return _supportsLanguage; }
            set { _supportsLanguage = value; }
        }

        /// <summary>
        /// 设置/获取该列是否可空
        /// </summary>
        public bool IsNullable { get; set; }

        ///// <summary>
        ///// 需要通过关联查询
        ///// </summary>
        //public string Cascade { get; set; }
    }
}
