using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace DapperOrm.Model
{
    /// <summary>
    /// 数据查询条件基类，搜索关键字,记录分页和排序相关的条件
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
        /// 当前页编号(从0开始)
        /// </summary>
        [DataMember(Order = 1)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        [DataMember(Order = 2)]
        public int PageSize { get; set; }

        /// <summary>
        /// 结果页数
        /// </summary>
        [DataMember(Order = 3)]
        public int PageCount { get; set; }

        /// <summary>
        /// 结果记录总数
        /// </summary>
        [DataMember(Order = 4)]
        public long RecordCount { get; set; }

        /// <summary>
        /// 指定用于分组查询的字段，以逗号分隔，
        /// </summary>
        [DataMember(Order = 5)]
        public string GroupFields { get; set; }

        private string _key;
        /// <summary>
        /// 查询关键字
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
        /// 查询的字段列表，以逗号分隔，默认值为"*",
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
        /// 排序规则
        /// </summary>
        [DataMember(Order = 9)]
        public List<OrderRule> OrderRules { get; set; }

        /// <summary>
        /// 指定搜索的字段
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
        /// 添加一条数据排序规则
        /// 后添加的排序规则优先
        /// </summary>
        /// <param name="fieldName">排序的字段</param>
        /// <param name="order">排序方法</param>
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
                        this.OrderRules.RemoveAt(i);//避免添加重复orderfield
                        break;
                    }
                }

            }
            this.OrderRules.Insert(0, rule);
        }

        /// <summary>
        /// 添加一条数据排序规则
        /// 如果指定字段的排序已存在，则反转并提升优先顺序
        /// 如果指定字段的排序不存在则默认降序排序
        /// </summary>
        /// <param name="fieldName">排序的字段</param>
        /// <param name="order">排序方法</param>
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
                        rule.Order = item.Order == Order.ASC ? Order.DESC : Order.ASC; //反转排序
                        this.OrderRules.RemoveAt(i);
                        break;
                    }
                }
            }
            this.OrderRules.Insert(0, rule);
        }
        /// <summary>
        /// 移除排序项
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
        /// 增加查询条件.
        /// </summary>
        /// <param name="fieldName">查询的字段（对应实体属性的名）</param>
        /// <param name="value">值.</param>
        /// <param name="match">值值匹配类型.</param>
        public void AddCriteria(string fieldName, object value, MatchType match)
        {
            if (value is string)
                value = RemoveMuzzleChar((string)value);
            CriteriaItem item = new CriteriaItem(fieldName, value, match);
            this.Criterias.Remove(item); //如果存在先移除
            this.Criterias.Add(item);
        }

        /// <summary>
        /// 增加查询条件.
        /// </summary>
        /// <param name="fieldName">查询的字段（对应实体属性的名）.</param>
        /// <param name="value">值</param>
        /// <param name="match">值值匹配类型.</param>
        /// <param name="ommitValue">当value为此值时不要添加</param>
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
        /// 移除查询条件.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="match"></param>
        public void RemoveCriteria(string fieldName, MatchType match)
        {
            CriteriaItem item = new CriteriaItem(fieldName, null, match);
            this.Criterias.Remove(item);
        }

        /// <summary>
        /// 判断是否存在此查询条件
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
        ///   移除查询条件.
        /// </summary>
        /// <param name="criteriaItem"></param>
        public void RemoveCriteria(CriteriaItem criteriaItem)
        {
            this.Criterias.Remove(criteriaItem);
        }
    }
}