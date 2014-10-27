using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace DapperOrm
{
    /// <summary>
    /// MySql数据库操作的实现
    /// </summary>
    public class MySqlDataFactory : DataFactory
    {

        /// <summary>
        /// 获取Connection对象的方法
        /// </summary>
        /// <returns></returns>
        protected override IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// 获取DbCommand对象的方法
        /// </summary>
        /// <returns></returns>
        protected override IDbCommand GetCommand()
        {
            return new MySqlCommand();
        }

        /// <summary>
        /// 获取DataAdapter对象的方法
        /// </summary>
        /// <returns></returns>
        protected override IDataAdapter GetDataAdapter()
        {
            return new MySqlDataAdapter();
        }

        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>返回参数</returns>
        public override IDataParameter CreateParameter(string parameterName, object value)
        {
            return new MySqlParameter(parameterName, value);
        }

        /// <summary>
        /// 创建指定长度的一组过程调用参数的方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public override IDataParameter[] CreateParameter(int count)
        {
            return new MySqlParameter[count];
        }

        /// <summary>
        /// 存储过程参数名前缀
        /// </summary>
        /// <value></value>
        protected override string StoreProcParameterNamePrefix
        {
            get { return "_"; }
        }

        /// <summary>
        /// 查询字符串参数名前缀
        /// </summary>
        /// <value></value>
        protected override string SqlTextParameterNamePrefix
        {
            get { return "?"; }
        }

        protected override void AddCommandParameters(IDbCommand cmd, IDataParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                foreach (IDataParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add
                        (
                        new MySqlParameter() 
                        { 
                             Value=parm.Value,
                             DbType=parm.DbType,
                             MySqlDbType = ToMySqlDbType(parm.DbType),
                              Direction=parm.Direction,
                               ParameterName= parm.ParameterName,
                        }
                        );
                }
            }
        }


        /// <summary>
        /// 将dbType数据类型转成对应的MySqlDbType类型
        /// </summary>
        /// <param name="_dbType">dbType类型</param>
        /// <returns>MySqlDbType类型数</returns>
        private MySqlDbType ToMySqlDbType(DbType _dbType)
        {
            switch (_dbType)
            {
                case DbType.Int64:
                    return MySqlDbType.Int64;
                case DbType.Int32:
                    return MySqlDbType.Int32;
                case DbType.Int16:
                    return MySqlDbType.Int16;
                case DbType.Byte:
                    return MySqlDbType.Byte;
                case DbType.Boolean:
                    return MySqlDbType.Bit;
                case DbType.Binary:
                    return MySqlDbType.Binary;
                case DbType.Date:
                    return MySqlDbType.Date;
                case DbType.DateTime:
                    return MySqlDbType.DateTime;
                case DbType.Decimal:
                    return MySqlDbType.Decimal;
                case DbType.Double:
                    return MySqlDbType.Double;
                case DbType.Single:
                    return MySqlDbType.Float;
                default:
                    return MySqlDbType.VarChar;
            }
        }


    }
}
