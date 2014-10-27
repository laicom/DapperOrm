using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace DapperOrm.Model
{
    /// <summary>
    /// ���ݲ�ѯ�������࣬�����ؼ���,��¼��ҳ��������ص�����
    /// </summary>
    [Serializable]
    [DataContract(Name = "SearchCriteria", Namespace = "http://GX/Search")]
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            this.PageSize = 0;
            this.PageIndex = 0;
            this.PageCount = 0;
            this.Criterias = new List<CriteriaItem>();
        }

        public SearchCriteria(int pageIndex, int pageSize, string orderField, Order order)
            : this()
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.AddOrderRule(orderField, order);
        }

        /// <summary>
        /// ��ǰҳ���(��0��ʼ)
        /// </summary>
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }

        /// <summary>
        /// ÿҳ��ʾ��¼����
        /// </summary>
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        [DataMember(Order = 3)]
        public int PageCount { get; set; }

        /// <summary>
        /// �����¼����
        /// </summary>
        [DataMember(Order = 4)]
        public long RecordCount { get; set; }

        /// <summary>
        /// ָ�����ڷ����ѯ���ֶΣ��Զ��ŷָ���
        /// </summary>
        [DataMember(Order = 5)]
        public string GroupFields { get; set; }

        private string _key;
        /// <summary>
        /// ��ѯ�ؼ���
        /// </summary>
        /// <value>The key.</value>
        [DataMember(Order = 6)]
        public string Key
        {
            get { return _key; }
            set
            {
                _key = RemoveMuzzleChar(value);
            }
        }

        private string RemoveMuzzleChar(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return Regex.Replace(value, @"^\s+|['`!#]|\s$", "");
            }
            return value;
        }

        private string _searchFields;
        /// <summary>
        /// Gets the search fields.
        /// ��ѯ���ֶ��б��Զ��ŷָ���Ĭ��ֵΪ"*",
        /// </summary>
        /// <value>The search fields.</value>
        [DataMember(Order = 7)]
        public string SearchFields
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchFields))
                {
                    return _searchFields;
                }
                else
                {
                    return "*";
                }
            }
            set
            {
                _searchFields = value;
            }
        }

        [DataMember(Order = 8)]
        public List<CriteriaItem> Criterias { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        [DataMember(Order = 9)]
        public List<OrderRule> OrderRules { get; set; }

        /// <summary>
        /// ָ���������ֶ�
        /// </summary>
        /// <param name="fields"></param>
        public void SetSearchFields(params string[] fields)
        {
            foreach (string field in fields)
            {
                if (string.IsNullOrEmpty(_searchFields))
                {
                    _searchFields = field;
                }
                else
                {
                    _searchFields += "," + field;
                }
            }
        }

        /// <summary>
        /// ���һ�������������
        /// ����ӵ������������
        /// </summary>
        /// <param name="fieldName">������ֶ�</param>
        /// <param name="order">���򷽷�</param>
        public void AddOrderRule(string fieldName, Order order)
        {
            OrderRule rule = new OrderRule(fieldName, order);
            if (this.OrderRules == null) this.OrderRules = new List<OrderRule>();
            else
            {
                for (int i = 0; i < this.OrderRules.Count; i++)
                {
                    OrderRule item = this.OrderRules[i];
                    if (rule.Field.Equals(item.Field, StringComparison.OrdinalIgnoreCase))
                    {
                        this.OrderRules.RemoveAt(i);//��������ظ�orderfield
                        break;
                    }
                }

            }
            this.OrderRules.Insert(0, rule);
        }

        /// <summary>
        /// ���һ�������������
        /// ���ָ���ֶε������Ѵ��ڣ���ת����������˳��
        /// ���ָ���ֶε����򲻴�����Ĭ�Ͻ�������
        /// </summary>
        /// <param name="fieldName">������ֶ�</param>
        /// <param name="order">���򷽷�</param>
        public void AddOrderRule(string fieldName)
        {
            OrderRule rule = new OrderRule(fieldName);
            if (this.OrderRules == null)
            {
                this.OrderRules = new List<OrderRule>();
            }
            else
            {
                for (int i = 0; i < this.OrderRules.Count; i++)
                {
                    OrderRule item = this.OrderRules[i];
                    if (rule.Field.Equals(item.Field, StringComparison.OrdinalIgnoreCase))
                    {
                        rule.Order = item.Order == Order.ASC ? Order.DESC : Order.ASC; //��ת����
                        this.OrderRules.RemoveAt(i);
                        break;
                    }
                }
            }
            this.OrderRules.Insert(0, rule);
        }
        /// <summary>
        /// �Ƴ�������
        /// </summary>
        /// <param name="fieldName"></param>
        public void RemoveOrderRule(string fieldName)
        {
            if (this.OrderRules != null && this.OrderRules.Count > 0)
            {
                for (int i = 0; i < this.OrderRules.Count; i++)
                {
                    OrderRule item = this.OrderRules[i];
                    if (item.Field == fieldName)
                    {
                        this.OrderRules.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// ���Ӳ�ѯ����.
        /// </summary>
        /// <param name="fieldName">��ѯ���ֶΣ���Ӧʵ�����Ե�����</param>
        /// <param name="value">ֵ.</param>
        /// <param name="match">ֵֵƥ������.</param>
        public void AddCriteria(string fieldName, object value, MatchType match)
        {
            if (value is string)
                value = RemoveMuzzleChar((string)value);
            CriteriaItem item = new CriteriaItem(fieldName, value, match);
            this.Criterias.Remove(item); //����������Ƴ�
            this.Criterias.Add(item);
        }

        /// <summary>
        /// ���Ӳ�ѯ����.
        /// </summary>
        /// <param name="fieldName">��ѯ���ֶΣ���Ӧʵ�����Ե�����.</param>
        /// <param name="value">ֵ</param>
        /// <param name="match">ֵֵƥ������.</param>
        /// <param name="ommitValue">��valueΪ��ֵʱ��Ҫ���</param>
        public void AddCriteria(string fieldName, object value, MatchType match, object ommitValue)
        {
            if (value is string)
                value = RemoveMuzzleChar((string)value);
            CriteriaItem item = new CriteriaItem(fieldName, value, match);
            this.Criterias.Remove(item);
            if (!value.Equals(ommitValue))
                this.Criterias.Add(item);
        }

        /// <summary>
        /// �Ƴ���ѯ����.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="match"></param>
        public void RemoveCriteria(string fieldName, MatchType match)
        {
            CriteriaItem item = new CriteriaItem(fieldName, null, match);
            this.Criterias.Remove(item);
        }

        /// <summary>
        /// �ж��Ƿ���ڴ˲�ѯ����
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool Contain(string fieldName, MatchType match)
        {
            CriteriaItem item = new CriteriaItem(fieldName, null, match);
            return this.Criterias.Contains(item);
        }

        /// <summary>
        ///   �Ƴ���ѯ����.
        /// </summary>
        /// <param name="criteriaItem"></param>
        public void RemoveCriteria(CriteriaItem criteriaItem)
        {
            this.Criterias.Remove(criteriaItem);
        }
    }
}