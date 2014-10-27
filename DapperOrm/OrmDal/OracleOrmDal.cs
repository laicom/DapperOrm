using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DapperOrm.Model;

namespace DapperOrm
{
    /// <summary>
    /// 针对Oracle的数据库操作实现
    /// </summary>
    public class OracleOrmDal : OrmDal
    {

        /// <summary>
        /// 插入后获取Id的查询语句代码
        /// </summary>
        /// <value></value>
        protected override string SelectIdentitySql
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Sql语句中的参数字符
        /// Mysql中为?，Sqlserver中为@
        /// </summary>
        /// <value></value>
        protected override string SqlParameterChar
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// SQL日期比较查询时，时间字符的表示方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override string GetCompareTimeSqlFormat(MatchType type)
        {
            throw new NotImplementedException();
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
                    _dbFactory = new OracleDataFactory();
                }
                return _dbFactory;
            }
        }
    }
}
