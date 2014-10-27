using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using DapperOrm.Model;

namespace DapperOrm
{

    /// <summary>
    /// Description:   针对Mysql数据库ORM操作的特殊实现 
    /// Author:     赖国欣  Guoxin.lai@gmail.com
    /// Create Date: 2010-08-15
    /// Update: 
    /// </summary>
    public class MySqlOrmDal : OrmDal
    {
        /// <summary>
        /// Sql语句中的参数字符
        /// Mysql中为?，Sqlserver中为@
        /// </summary>
        /// <value></value>
        protected override string SqlParameterChar
        {
            get { return "?"; }
        }

        /// <summary>
        /// 插入后获取Id的查询语句代码
        /// </summary>
        /// <value></value>
        protected override string SelectIdentitySql
        {
            get { return "Select @@IDENTITY"; }
        }

        /// <summary>
        /// SQL日期比较查询时，时间字符的表示方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override string GetCompareTimeSqlFormat(MatchType type)
        {
            {
                if ((type & MatchType.TimeCompareSecond) == MatchType.TimeCompareSecond) return "TimeStampDiff(SECOND,{0},{1})";
                if ((type & MatchType.TimeCompareDay) == MatchType.TimeCompareDay)       return "TimeStampDiff(DAY,{0},{1})";
                if ((type & MatchType.TimeCompareHour) == MatchType.TimeCompareHour)     return "TimeStampDiff(HOUR,{0},{1})";
                if ((type & MatchType.TimeCompareMinute) == MatchType.TimeCompareMinute) return "TimeStampDiff(MINUTE,{0},{1})";
                if ((type & MatchType.TimeCompareMonth) == MatchType.TimeCompareMonth)   return "TimeStampDiff(MONTH,{0},{1})";
                if ((type & MatchType.TimeCompareWeek) == MatchType.TimeCompareWeek)     return "TimeStampDiff(WEEK,{0},{1})";
                if ((type & MatchType.TimeCompareYear) == MatchType.TimeCompareYear)     return "TimeStampDiff(YEAR,{0},{1})";
                return string.Empty;
            }
        }

        /// <summary>
        /// 执行搜索，返回一个List
        /// 搜索的表由实体类T的映射信息获得
        /// </summary>
        /// <typeparam name="T">Search result type</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public override List<T> GetList<T>(SearchCriteria criteria, string tableName)
        {
            string sWhere = BuildWhereClause<T>(criteria);
            if (criteria.PageSize > 0)
            {
                if (!string.IsNullOrEmpty(criteria.GroupFields))
                {
                    string sql = string.Format("select count(*) from (select * from {0} where {1} group by {2}) as t", tableName, sWhere, criteria.GroupFields);
                    criteria.RecordCount = long.Parse(DbFactory.ExecuteScalar(CommandType.Text, sql, null).ToString());
                }
                else
                {
                    criteria.RecordCount = base.GetRecordCount(tableName, sWhere);
                }
                criteria.PageCount = (int)Math.Ceiling((criteria.RecordCount * 1.0) / criteria.PageSize);
            }
            StringBuilder sbSql = new StringBuilder(1024);
            sbSql.AppendFormat("select {0} from {1} where {2} ", criteria.SearchFields,tableName,sWhere);

            if (!string.IsNullOrEmpty(criteria.GroupFields))
                sbSql.AppendFormat(" group by {0} ", criteria.GroupFields);
            string orderString = OrderRule.ToString(criteria.OrderRules);

            if (!string.IsNullOrEmpty(orderString))
                sbSql.AppendFormat(" order by {0} ", orderString);
            if(criteria.PageSize>0)
                sbSql.AppendFormat(" limit {0},{1};", criteria.PageSize * criteria.PageIndex, criteria.PageSize);

            return DbFactory.ExecuteGetList<T>(CommandType.Text, sbSql.ToString(), null);
        }

        private IDataFactory _dbFactory;
        /// <summary>
        /// 内部使用的IDataFactory接口实例.
        /// </summary>
        /// <value>The db factory.</value>
        protected override IDataFactory DbFactory
        {
            get
            {
                if (_dbFactory == null)
                {
                    _dbFactory = new MySqlDataFactory();
                }   
                return _dbFactory;
            }
        }
    }
}
