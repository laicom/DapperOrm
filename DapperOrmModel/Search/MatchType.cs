using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DapperOrm.Model
{

    /// <summary>
    /// 数据搜索比较类型（逻辑组合）
    /// </summary>
    [Flags]
    [DataContract(Name = "MatchType", Namespace = "http://GX/Search")]
    public enum MatchType
    {
        /// <summary>
        /// 数据值比较相等
        /// </summary>
        [EnumMember(Value = "Equal")]
        Equal = 1,

        /// <summary>
        /// 在列举的范围内(传用值为用逗号分隔的字符串或数组列表)
        /// </summary>
        [EnumMember(Value = "In")]
        In = 2,

        /// <summary>
        /// 非
        /// </summary>
        [EnumMember(Value = "Not")]
        Not = 4,

        /// <summary>
        /// 大于
        /// </summary>
        [EnumMember(Value = "Greater")]
        Greater = 8,

        /// <summary>
        /// 小于
        /// </summary>
        [EnumMember(Value = "Smaller")]
        Smaller = 16,

        /// <summary>
        /// 字符串比较
        /// </summary>
        [EnumMember(Value = "String")]
        String = 32,

        /// <summary>
        /// 字符串匹配
        /// </summary>
        [EnumMember(Value = "Like")]
        Like = 64 | String,

        /// <summary>
        /// 字符串自动通配查询
        /// </summary>
        [EnumMember(Value = "WildCard")]
        WildCard = 128 | Like,

        /// <summary>
        /// 时间比较
        /// </summary>
        [EnumMember(Value = "TimeCompare")]
        TimeCompare = 256 | String,

        /// <summary>
        /// 时间比较_秒
        /// </summary>
        [EnumMember(Value = "TimeCompareSecond")]
        TimeCompareSecond = TimeCompare | 512,

        /// <summary>
        /// 时间比较_秒_相等
        /// </summary>
        [EnumMember(Value = "TimeEqualSecond")]
        TimeEqualSecond = TimeCompareSecond | Equal,

        /// <summary>
        /// 时间比较_分
        /// </summary>
        [EnumMember(Value = "TimeCompareMinute")]
        TimeCompareMinute = TimeCompare | 1024,

        /// <summary>
        /// 时间比较_时
        /// </summary>
        [EnumMember(Value = "TimeCompareHour")]
        TimeCompareHour = TimeCompare | 2048,

        /// <summary>
        /// 时间比较_日
        /// </summary>
        [EnumMember(Value = "TimeCompareDay")]
        TimeCompareDay = TimeCompare | 4096,


        /// <summary>
        /// 时间比较_周
        /// </summary>
        [EnumMember(Value = "TimeCompareWeek")]
        TimeCompareWeek = TimeCompare | 8192,


        /// <summary>
        /// 时间比较_月
        /// </summary>
        [EnumMember(Value = "TimeCompareMonth")]
        TimeCompareMonth = TimeCompare | 16384,


        /// <summary>
        /// 时间比较_年
        /// </summary>
        [EnumMember(Value = "TimeCompareYear")]
        TimeCompareYear = TimeCompare | 32768,


        /// <summary>
        /// 指定到某日（时分秒都为0）时，须加24小时
        /// </summary>
        ToDate=65536,


        /// <summary>
        /// 不等于(Not | Equal)
        /// </summary>
        [EnumMember(Value = "NotEqual")]
        NotEqual = Not | Equal,

        /// <summary>
        /// 不在列举范围内(Not | In)
        /// </summary>
        [EnumMember(Value = "NotInt")]
        NotIn = Not | In,

        /// <summary>
        /// 不匹配的(Not | Like)
        /// </summary>
        [EnumMember(Value = "NotLike")]
        NotLike = Not | Like,

        /// <summary>
        /// 大于或等于(Equal | Greater)
        /// </summary>
        [EnumMember(Value = "EqualOrGreater")]
        EqualOrGreater = Equal | Greater,

        /// <summary>
        /// 小于或等于( Equal | Smaller)
        /// </summary>
        [EnumMember(Value = "EqualOrSmaller")]
        EqualOrSmaller = Equal | Smaller,

        /// <summary>
        /// 字符串的比较相等(String | Equal)
        /// </summary>
        [EnumMember(Value = "StringEqual")]
        StringEqual = String | Equal,

        /// <summary>
        /// 列举的字符串范围(In | String)
        /// </summary>
        [EnumMember(Value = "InString")]
        InString = In | String,

        /// <summary>
        /// 不在列举的字符串范围(In | String | Not)
        /// </summary>
        [EnumMember(Value = "NotInString")]
        NotInString = InString | Not,

        /// <summary>
        /// 查询时间范围开始(EqualOrGreater | TimeCompare)
        /// </summary>
        [EnumMember(Value = "TimeFrom")]
        TimeFrom = EqualOrGreater | TimeCompare,

        /// <summary>
        /// 查询时间范围结束
        /// </summary>
        [EnumMember(Value = "TimeTo")]
        TimeTo = EqualOrSmaller | TimeCompare ,

        /// <summary>
        /// 查询时间或范围结束，根据时间输入如果时分秒为0，自动增加一天（表示到某天结束时）
        /// </summary>
        DateOrTimeTo = EqualOrSmaller | TimeCompare | ToDate,

        /// <summary>
        /// 时间相等
        /// </summary>
        [EnumMember(Value = "TimeEqual")]
        TimeEqual = TimeCompare | Equal,


        /// <summary>
        /// 位与包含(不等于0)
        /// </summary>
        [EnumMember(Value = "BitAnd")]
        BitAnd = 131072 | Equal,

        /// <summary>
        /// 位与不包含(等于0)
        /// </summary>
       [EnumMember(Value = "BitAndNot")]
        BitAndNot = BitAnd | Not,

        /// <summary>
        /// 空字串或null
        /// </summary>
        [EnumMember(Value = "NullOrEmpty")]
        NullOrEmpty = String | 262144,

        /// <summary>
        /// 非空字串或null
        /// </summary>
        [EnumMember(Value = "NotNullOrEmpty")]
        NotNullOrEmpty = NullOrEmpty | Not,

        ///// <summary>
        ///// 位或
        ///// </summary>
        //BitOr=131072,

        ///// <summary>
        ///// 位异或
        ///// </summary>
        //BitXor=262144,

    }

}
