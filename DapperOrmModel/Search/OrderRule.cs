using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DapperOrm.Model
{
    /// <summary>
    /// ���򷽷�
    /// </summary>
    [DataContract(Name = "Order", Namespace = "http://GX/Search")]
    public enum Order
    {
        /// <summary>
        /// ����
        /// </summary>
        [EnumMember(Value = "ASC")]
        ASC,
        /// <summary>
        /// ����
        /// </summary>
        [EnumMember(Value = "DESC")]
        DESC
    }

    /// <summary>
    /// �������
    /// </summary>
    [Serializable]
    [DataContract(Name = "OrderRule", Namespace = "http://GX/Search")]
    public class OrderRule
    {
        public OrderRule(string orderFieldName, Order order)
        {
            this.Field = orderFieldName;
            this.Order = order;
        }

        public OrderRule(string orderFieldName)
            : this(orderFieldName, Order.DESC) //Ĭ�Ͻ�������
        {
        }

        [DataMember(Order = 1)]
        public string Field { get; set; }

        [DataMember(Order = 2)]
        public Order Order { get; set; }

        public static string ToString(IEnumerable<OrderRule> ruleList)
        {
            string orderString = string.Empty;
            if (ruleList != null)
            {
                foreach (OrderRule rule in ruleList)
                {
                    if (!string.IsNullOrEmpty(orderString))
                    {
                        orderString += " , ";
                    }
                    orderString += rule.ToString();
                }
            }
            return orderString;
        }

        public override string ToString()
        {
            return this.Field + " " + this.Order.ToString();
        }
    }
}
