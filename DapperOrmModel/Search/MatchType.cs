using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DapperOrm.Model
{

    /// <summary>
    /// ���������Ƚ����ͣ��߼���ϣ�
    /// </summary>
    [Flags]
    [DataContract(Name = "MatchType", Namespace = "http://GX/Search")]
    public enum MatchType
    {
        /// <summary>
        /// ����ֵ�Ƚ����
        /// </summary>
        [EnumMember(Value = "Equal")]
        Equal = 1,

        /// <summary>
        /// ���оٵķ�Χ��(����ֵΪ�ö��ŷָ����ַ����������б�)
        /// </summary>
        [EnumMember(Value = "In")]
        In = 2,

        /// <summary>
        /// ��
        /// </summary>
        [EnumMember(Value = "Not")]
        Not = 4,

        /// <summary>
        /// ����
        /// </summary>
        [EnumMember(Value = "Greater")]
        Greater = 8,

        /// <summary>
        /// С��
        /// </summary>
        [EnumMember(Value = "Smaller")]
        Smaller = 16,

        /// <summary>
        /// �ַ����Ƚ�
        /// </summary>
        [EnumMember(Value = "String")]
        String = 32,

        /// <summary>
        /// �ַ���ƥ��
        /// </summary>
        [EnumMember(Value = "Like")]
        Like = 64 | String,

        /// <summary>
        /// �ַ����Զ�ͨ���ѯ
        /// </summary>
        [EnumMember(Value = "WildCard")]
        WildCard = 128 | Like,

        /// <summary>
        /// ʱ��Ƚ�
        /// </summary>
        [EnumMember(Value = "TimeCompare")]
        TimeCompare = 256 | String,

        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareSecond")]
        TimeCompareSecond = TimeCompare | 512,

        /// <summary>
        /// ʱ��Ƚ�_��_���
        /// </summary>
        [EnumMember(Value = "TimeEqualSecond")]
        TimeEqualSecond = TimeCompareSecond | Equal,

        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareMinute")]
        TimeCompareMinute = TimeCompare | 1024,

        /// <summary>
        /// ʱ��Ƚ�_ʱ
        /// </summary>
        [EnumMember(Value = "TimeCompareHour")]
        TimeCompareHour = TimeCompare | 2048,

        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareDay")]
        TimeCompareDay = TimeCompare | 4096,


        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareWeek")]
        TimeCompareWeek = TimeCompare | 8192,


        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareMonth")]
        TimeCompareMonth = TimeCompare | 16384,


        /// <summary>
        /// ʱ��Ƚ�_��
        /// </summary>
        [EnumMember(Value = "TimeCompareYear")]
        TimeCompareYear = TimeCompare | 32768,


        /// <summary>
        /// ָ����ĳ�գ�ʱ���붼Ϊ0��ʱ�����24Сʱ
        /// </summary>
        ToDate=65536,


        /// <summary>
        /// ������(Not | Equal)
        /// </summary>
        [EnumMember(Value = "NotEqual")]
        NotEqual = Not | Equal,

        /// <summary>
        /// �����оٷ�Χ��(Not | In)
        /// </summary>
        [EnumMember(Value = "NotInt")]
        NotIn = Not | In,

        /// <summary>
        /// ��ƥ���(Not | Like)
        /// </summary>
        [EnumMember(Value = "NotLike")]
        NotLike = Not | Like,

        /// <summary>
        /// ���ڻ����(Equal | Greater)
        /// </summary>
        [EnumMember(Value = "EqualOrGreater")]
        EqualOrGreater = Equal | Greater,

        /// <summary>
        /// С�ڻ����( Equal | Smaller)
        /// </summary>
        [EnumMember(Value = "EqualOrSmaller")]
        EqualOrSmaller = Equal | Smaller,

        /// <summary>
        /// �ַ����ıȽ����(String | Equal)
        /// </summary>
        [EnumMember(Value = "StringEqual")]
        StringEqual = String | Equal,

        /// <summary>
        /// �оٵ��ַ�����Χ(In | String)
        /// </summary>
        [EnumMember(Value = "InString")]
        InString = In | String,

        /// <summary>
        /// �����оٵ��ַ�����Χ(In | String | Not)
        /// </summary>
        [EnumMember(Value = "NotInString")]
        NotInString = InString | Not,

        /// <summary>
        /// ��ѯʱ�䷶Χ��ʼ(EqualOrGreater | TimeCompare)
        /// </summary>
        [EnumMember(Value = "TimeFrom")]
        TimeFrom = EqualOrGreater | TimeCompare,

        /// <summary>
        /// ��ѯʱ�䷶Χ����
        /// </summary>
        [EnumMember(Value = "TimeTo")]
        TimeTo = EqualOrSmaller | TimeCompare ,

        /// <summary>
        /// ��ѯʱ���Χ����������ʱ���������ʱ����Ϊ0���Զ�����һ�죨��ʾ��ĳ�����ʱ��
        /// </summary>
        DateOrTimeTo = EqualOrSmaller | TimeCompare | ToDate,

        /// <summary>
        /// ʱ�����
        /// </summary>
        [EnumMember(Value = "TimeEqual")]
        TimeEqual = TimeCompare | Equal,


        /// <summary>
        /// λ�����(������0)
        /// </summary>
        [EnumMember(Value = "BitAnd")]
        BitAnd = 131072 | Equal,

        /// <summary>
        /// λ�벻����(����0)
        /// </summary>
       [EnumMember(Value = "BitAndNot")]
        BitAndNot = BitAnd | Not,

        /// <summary>
        /// ���ִ���null
        /// </summary>
        [EnumMember(Value = "NullOrEmpty")]
        NullOrEmpty = String | 262144,

        /// <summary>
        /// �ǿ��ִ���null
        /// </summary>
        [EnumMember(Value = "NotNullOrEmpty")]
        NotNullOrEmpty = NullOrEmpty | Not,

        ///// <summary>
        ///// λ��
        ///// </summary>
        //BitOr=131072,

        ///// <summary>
        ///// λ���
        ///// </summary>
        //BitXor=262144,

    }

}
