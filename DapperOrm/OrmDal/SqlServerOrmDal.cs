using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DapperOrm.Model;

namespace DapperOrm
{

    /// <summary>
    /// Description:   针对sqlserver数据库ORM操作的特殊实现 
    /// Author:     赖国欣  Guoxin.lai@gmail.com
    /// Create Date: 2010-08-15
    /// Update: 
    /// 
    /// </summary>
    public class SqlServerOrmDal : OrmDal
    {
        /// <summary>
        /// 查询字符
        /// </summary>
        protected override string SqlParameterChar
        {
            get { return "@"; }
        }


        /// <summary>
        /// 插入后获取Id的查询语句代码
        /// </summary>
        /// <value></value>
        protected override string SelectIdentitySql
        {
            get { return "DECLARE @Id INT; SET @Id=SCOPE_IDENTITY(); SELECT @Id;"; }
        }

        /// <summary>
        /// SQL日期比较查询时，时间字符的表示方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override string GetCompareTimeSqlFormat(MatchType type)
        {
            {
                if ((type & MatchType.TimeCompareSecond) == MatchType.TimeCompareSecond) return "DateDiff(ss,{0},{1})";
                if ((type & MatchType.TimeCompareDay) == MatchType.TimeCompareDay)       return "DateDiff(dd,{0},{1})";
                if ((type & MatchType.TimeCompareHour) == MatchType.TimeCompareHour)     return "DateDiff(h,{0},{1})";
                if ((type & MatchType.TimeCompareMinute) == MatchType.TimeCompareMinute) return "DateDiff(n,{0},{1})";
                if ((type & MatchType.TimeCompareMonth) == MatchType.TimeCompareMonth)   return "DateDiff(m,{0},{1})";
                if ((type & MatchType.TimeCompareWeek) == MatchType.TimeCompareWeek)     return "DateDiff(ww,{0},{1})";
                if ((type & MatchType.TimeCompareYear) == MatchType.TimeCompareYear)     return "DateDiff(yyyy,{0},{1})";
                return string.Empty;
            }
        }


        /// <summary>
        /// 执行搜索，返回一个List
        /// </summary>
        /// <typeparam name="T">Search result type</typeparam>
        /// <param name="criteria">查询条件.</param>
        /// <param name="tableName">The criteria.</param>
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
            string orderString = OrderRule.ToString(criteria.OrderRules);
            if (criteria.PageSize == 0)
            {
                sbSql.AppendFormat("select {0} from {1} where {2} ", criteria.SearchFields, tableName, sWhere);
            }
            else
            {
                if (string.IsNullOrEmpty(orderString))
                    throw new OrmException("Must specify SearchCriteria order rule to support sqlserver GetList page");
                sbSql.AppendFormat(@"
                    select {0} 
                    from (
                           select {0},ROW_NUMBER() OVER(ORDER BY {3}) AS PosNum 
                                from {1} 
                                    where {2} 
                            ) as T
                    WHERE	T.PosNum>{4}  AND T.PosNum<={5}",
                     criteria.SearchFields,
                     tableName, 
                     sWhere, 
                     orderString, 
                     criteria.PageIndex * criteria.PageSize, 
                     (criteria.PageIndex+1) * criteria.PageSize
                    );
            }

            if (!string.IsNullOrEmpty(criteria.GroupFields))
                sbSql.AppendFormat(" group by {0} ", criteria.GroupFields);

            if (!string.IsNullOrEmpty(orderString))
                sbSql.AppendFormat(" order by {0} ", orderString);
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
                    _dbFactory = new SqlServerDataFactory();
                }
                return _dbFactory;
            }
        }
    }
}
