using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperOrm
{
    /// <summary>
    /// Oracle数据库操作类
    /// </summary>
    class OracleDataFactory : DataFactory
    {
        /// <summary>
        /// 获取Connection对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取DbCommand对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDbCommand GetCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取DataAdapter对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDataAdapter GetDataAdapter()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>返回参数</returns>
        public override System.Data.IDataParameter CreateParameter(string parameterName, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建指定长度的一组过程调用参数的方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override System.Data.IDataParameter[] CreateParameter(int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 存储过程参数名前缀
        /// </summary>
        /// <value></value>
        protected override string StoreProcParameterNamePrefix
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 查询字符串参数名前缀
        /// </summary>
        /// <value></value>
        protected override string SqlTextParameterNamePrefix
        {
            get { throw new NotImplementedException(); }
        }
    }
}
