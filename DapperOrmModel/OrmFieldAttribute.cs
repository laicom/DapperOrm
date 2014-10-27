using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// ���ڽ���ʵ�������������ݿ���ֶ�����ӳ��
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
        /// ���Դ��ֶε�ӳ���ϵ
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
        /// ֻд
        /// </summary>
        public bool WriteOnly { get; set; }
        /// <summary>
        /// ֻ��
        /// </summary>
        public bool ReadOnly { get; set; }

        private bool _keySearch;
        /// <summary>
        /// ֧��ͨ���ѯ�����KeySearch����Ҹ��ֶΣ�
        /// </summary>
        public bool KeySearch
        {
            get { return _keySearch; }
            set { _keySearch = value; }
        }

        private Type _typeConverter = null;
        /// <summary>
        ///  ��Ҫָ���Զ�������ת�������ͣ��Զ���ת���������ʵ�� IORMappingTypeConverter�ӿ�
        /// </summary>
        public Type TypeConverter
        {
            get { return _typeConverter; }
            set { _typeConverter = value; }
        }

        private string _supportsLanguage;
        /// <summary>
        /// ���ʻ�֧��/�����֣��Զ��ŷָ����ַ���
        /// </summary>
        public string SupportsLanguages
        {
            get { return _supportsLanguage; }
            set { _supportsLanguage = value; }
        }

        /// <summary>
        /// ����/��ȡ�����Ƿ�ɿ�
        /// </summary>
        public bool IsNullable { get; set; }

        ///// <summary>
        ///// ��Ҫͨ��������ѯ
        ///// </summary>
        //public string Cascade { get; set; }
    }
}
