using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DapperOrm.Model
{
    /// <summary>
    /// ��ѯ������
    /// </summary>
    [Serializable]
    [DataContract(Name = "CriteriaItem", Namespace = "http://GX/Search")]
    public class CriteriaItem : IEquatable<CriteriaItem>
    {
        /// <summary>
        /// ��������ʵ��
        /// </summary>
        public CriteriaItem()
        {

        }
        /// <summary>
        /// ��������ʵ��
        /// </summary>
        /// <param name="fieldName">��ѯ���ֶΣ���Ӧʵ�����Ե�����</param>
        /// <param name="value">ֵ.</param>
        /// <param name="match">ֵֵƥ������.</param>
        public CriteriaItem(string fieldName, object value, MatchType match)
        {
            this.FieldName = fieldName;
            this.Value = value;
            this.MatchType = match;
        }

        /// <summary>
        /// ��ѯƥ�䷽ʽ
        /// </summary>
        [DataMember(Order = 1)]
        public MatchType MatchType { get; set; }

        /// <summary>
        /// �ֶ���
        /// </summary>
        [DataMember(Order = 2)]
        public string FieldName { get; set; }

        /// <summary>
        /// ֵ
        /// </summary>
        [DataMember(Order = 3)]
        public object Value { get; set; }

        #region IEquatable<CriteriaItem> ��Ա
        /// <summary>
        /// �Ƚ��Ƿ����
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
