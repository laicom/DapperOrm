using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace DapperOrm
{
    /// <summary>
    /// OleDb数据库操作的类
    /// </summary>
    class OleDbDataFactory : DataFactory
    {
        /// <summary>
        /// 获取Connection对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectionString);
        }

        /// <summary>
        /// 获取DbCommand对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDbCommand GetCommand()
        {
            return new OleDbCommand();
        }

        /// <summary>
        /// 获取DataAdapter对象的方法
        /// </summary>
        /// <returns></returns>
        protected override System.Data.IDataAdapter GetDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>返回参数</returns>
        public override System.Data.IDataParameter CreateParameter(string parameterName, object value)
        {
            return new OleDbParameter(parameterName,value);
        }

        /// <summary>
        /// 创建指定长度的一组过程调用参数的方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override System.Data.IDataParameter[] CreateParameter(int count)
        {
            return new OleDbParameter[count];
        }

        /// <summary>
        /// 存储过程参数名前缀
        /// </summary>
        /// <value></value>
        protected override string StoreProcParameterNamePrefix
        {
            get { return "@"; }
        }

        /// <summary>
        /// 查询字符串参数名前缀
        /// </summary>
        /// <value></value>
        protected override string SqlTextParameterNamePrefix
        {
            get { return "@"; }
        }

    }
}
