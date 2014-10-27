using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DapperOrm.Model
{
    /// <summary>
    /// 查询条件项
    /// </summary>
    [Serializable]
    [DataContract(Name = "CriteriaItem", Namespace = "http://GX/Search")]
    public class CriteriaItem : IEquatable<CriteriaItem>
    {
        /// <summary>
        /// 构造类型实例
        /// </summary>
        public CriteriaItem()
        {

        }
        /// <summary>
        /// 构造类型实例
        /// </summary>
        /// <param name="fieldName">查询的字段（对应实体属性的名）</param>
        /// <param name="value">值.</param>
        /// <param name="match">值值匹配类型.</param>
        public CriteriaItem(string fieldName, object value, MatchType match)
        {
            this.FieldName = fieldName;
            this.Value = value;
            this.MatchType = match;
        }

        /// <summary>
        /// 查询匹配方式
        /// </summary>
        [DataMember(Order = 1)]
        public MatchType MatchType { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [DataMember(Order = 2)]
        public string FieldName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [DataMember(Order = 3)]
        public object Value { get; set; }

        #region IEquatable<CriteriaItem> 成员
        /// <summary>
        /// 比较是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CriteriaItem other)
        {
            return (this.FieldName.Equals(other.FieldName, StringComparison.OrdinalIgnoreCase) && this.MatchType == other.MatchType);
        }

        #endregion
    }
}
