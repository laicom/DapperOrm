using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using DapperOrm;

namespace DapperOrm
{
    /// <summary>
    /// 数据数据操作的实现（抽象工厂）
    /// Create:  2010-08-06 赖国欣  Guoxin.lai@gmail.com 
    /// Update: 
    /// </summary>
    public abstract class DataFactory : IDataFactory
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取Connection对象
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection GetConnection();

        /// <summary>
        /// 获取DbCommand对象
        /// </summary>
        /// <returns></returns>
        protected abstract IDbCommand GetCommand();

        /// <summary>
        /// 获取DataAdapter对象
        /// </summary>
        /// <returns></returns>
        protected abstract IDataAdapter GetDataAdapter();

        /// <summary>
        /// 获得默认的DataFactory
        /// </summary>
        public static IDataFactory Create()
        {
            ConnectionStringSettings connectSetting = ConfigurationManager.ConnectionStrings["Default"];
            if (connectSetting == null)
                connectSetting = ConfigurationManager.ConnectionStrings[0];
            if (connectSetting == null)
                throw new Exception("No default data connection setting defined!");
            return Create(connectSetting.ProviderName, connectSetting.ConnectionString);
        }
        /// <summary>
        /// 创建一个DataFactory
        /// </summary>
        public static IDataFactory Create(string connectionSettingName)
        {
            ConnectionStringSettings connectSetting = ConfigurationManager.ConnectionStrings[connectionSettingName];
            if(connectSetting==null)
                throw new Exception(string.Format("Data connection setting named {0} not found",connectionSettingName));
            return Create(connectSetting.ProviderName, connectSetting.ConnectionString);
        }

        /// <summary>
        /// 内部缓存加速
        /// </summary>
        protected static Hashtable factoryCaching=Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 创建一个DataFactory
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static IDataFactory Create(string providerName, string connectionString)
        {
            if (factoryCaching.Contains(connectionString))
            {
                return (IDataFactory)factoryCaching[connectionString];
            }
            
            IDataFactory dataFactory;
            if(providerName.Contains(","))
            {
                Type type=Type.GetType(providerName);
                if(type==null)
                    throw new Exception(string.Format("Load type for data provider named “[{0}]” failed", providerName));
                dataFactory = type.Assembly.CreateInstance(type.FullName) as IDataFactory;
            }else
            {
                dataFactory= (typeof(DataFactory)).Assembly.CreateInstance("DapperOrm." + providerName + "DataFactory") as IDataFactory;
            }
            if (dataFactory == null)
                throw new Exception(string.Format("The data provider named [{0}] not sported", providerName));

            dataFactory.ConnectionString = connectionString;

            lock (factoryCaching)//缓存处理
            {
                factoryCaching.Add(connectionString, dataFactory);
            }
            return dataFactory;
        }


        /// <summary>
        /// 执行非查询SQL
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回影响行数</returns>
        public virtual int ExecuteNonQuery(CommandType commandType, string commandText, params IDataParameter[] commandParameters) {

            IDbCommand cmd = GetCommand();
            using (IDbConnection conn = GetConnection())
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }


        /// <summary>
        /// 开始事务处理
        /// </summary>
        /// <returns>返回事务对象</returns>
        public virtual IDbTransaction BeginTrans()
        {
            IDbConnection conn = GetConnection();
            conn.Open();
            IDbTransaction trans = conn.BeginTransaction();
            return trans;
        }


        /// <summary>
        /// 开始事务处理
        /// </summary>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        public virtual IDbTransaction BeginTrans(IsolationLevel iLevel)
        {
            IDbConnection conn = GetConnection();
            conn.Open();
            IDbTransaction trans = conn.BeginTransaction(iLevel);
            return trans;
        }

        /// <summary>
        /// 提交事务处理
        /// </summary>
        /// <param name="trans"></param>
        public virtual void CommitTrans(IDbTransaction trans)
        {
            if (trans == null) return;
            IDbConnection conn = trans.Connection;
            trans.Commit();
            trans.Dispose();
            if (conn != null && conn.State == ConnectionState.Open)conn.Close();
        }

        /// <summary>
        /// 事处回滚
        /// </summary>
        /// <param name="trans"></param>
        public virtual void RollbackTrans(IDbTransaction trans)
        {
            if (trans == null) return;
            IDbConnection conn = trans.Connection;
            trans.Rollback();
            trans.Dispose();
            if(conn!=null && conn.State==ConnectionState.Open)conn.Close();
        }


        /// <summary>
        /// 执行非查询SQL(支持事务)
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回影响行数</returns>
        public virtual int ExecuteNonQuery(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            if (trans == null) return ExecuteNonQuery(commandType, commandText, commandParameters);
            IDbCommand cmd = GetCommand();
            PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            return val;
        }



        /// <summary>
        /// 执行数据读取
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>DataReader对象</returns>
        public virtual IDataReader ExecuteReader(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            IDbCommand cmd =GetCommand();
            IDbConnection conn = GetConnection();

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, commandParameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        public virtual object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            IDbCommand cmd = GetCommand();
            using (IDbConnection conn = GetConnection())
            {
                PrepareCommand(cmd, conn, null, commandType, commandText, commandParameters);
                object val = cmd.ExecuteScalar();
                 cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        public virtual object ExecuteScalar(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            if (trans == null) return ExecuteScalar(commandType, commandText, commandParameters);

            IDbCommand cmd = GetCommand();
            PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, commandParameters);
           object val = cmd.ExecuteScalar();
           cmd.Parameters.Clear();
            return val;
        }
        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        public virtual T ExecuteScalar<T>(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            object result = ExecuteScalar(trans, commandType, commandText, commandParameters);
            if (result == null)
            {
                return default(T);
            }
            else
            {
                return (T)result;
            }
        }


        /// <summary>
        /// Prepares the command.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <param name="conn">The conn.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="cmdParms">The CMD parms.</param>
        private void PrepareCommand(IDbCommand cmd, IDbConnection conn, IDbTransaction trans, CommandType commandType, string commandText, IDataParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = commandText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = commandType;

            AddCommandParameters(cmd, cmdParms);
        }

        protected virtual void AddCommandParameters(IDbCommand cmd, IDataParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                foreach (IDataParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                        parm.Value = DBNull.Value;
                    cmd.Parameters.Add(parm);
                }
            }
        }

        /// <summary>
        /// 执行返回单行数据的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象列表</returns>
        public T ExecuteGetSingleRow<T>(CommandType cmdType, string cmdName, IDataParameter[] param)
        {
            using (IDataReader reader = ExecuteReader(cmdType, cmdName, param))
            {
                if (reader.Read())
                {
                    T result= ORMHelper.ReaderResult<T>(reader);
                    reader.Close();
                    return result;
                }
                else
                {
                    return default(T);
                }
            }
        }



        /// <summary>
        /// 执行返回一个实体对象实例列表的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象列表</returns>
        public List<T> ExecuteGetList<T>(CommandType cmdType, string cmdName, IDataParameter[] param)
        {
            using (IDataReader reader = ExecuteReader(cmdType, cmdName, param))
            {
                List<T> listT = new List<T>();
                while (reader.Read())
                {
                    T t = ORMHelper.ReaderResult<T>(reader);
                    listT.Add(t);
                }
                reader.Close();
                return listT;
            }
        }


        /// <summary>
        /// 执行返回单列的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象</returns>
        public List<T> ExecuteGetSingleFieldList<T>(CommandType cmdType, string cmdName, IDataParameter[] param)
        {
            using (IDataReader reader = ExecuteReader(cmdType, cmdName, param))
            {
                List<T> listT = new List<T>();
                while (reader.Read())
                {
                    T t = (T)reader[0];
                    listT.Add(t);
                }
                reader.Close();
                return listT;
            }
        }



        /// <summary>
        /// 执行返回DataSet的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters"></param>
        /// <returns>返回DataSet</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            using (IDbConnection conn = GetConnection())
            {
                DbCommand cmd = (DbCommand)GetCommand();
                PrepareCommand(cmd, conn, null, commandType, commandText, commandParameters);
                DbDataAdapter da = (DbDataAdapter)GetDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }


        /// <summary>
        /// 执行返回DataTable的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回DataTable</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            DataSet ds=ExecuteDataSet(commandType, commandText, commandParameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="dbType">参数数据类型</param>
        /// <returns>返回参数</returns>
        public abstract IDataParameter CreateParameter(string parameterName, object value);
        /// <summary>
        /// 创建指定长度的一组过程调用参数的方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract IDataParameter[] CreateParameter(int count);

        /// <summary>
        /// 创建一组SQL过程调用参数的方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataParameter[] CreateParameter(params IDataParameter[] parameters)
        {
            return parameters;
        }
        /// <summary>
        /// 存储过程参数名前缀
        /// </summary>
        protected abstract string StoreProcParameterNamePrefix { get; }

        /// <summary>
        /// 查询字符串参数名前缀
        /// </summary>
        protected abstract string SqlTextParameterNamePrefix { get; }

        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="parameterNamePrefix">参数名加上自定义前缀</param>
        /// <returns>返回参数</returns>
        public IDataParameter CreateParameter(string parameterName, object value, string parameterNamePrefix)
        {
            return CreateParameter(parameterNamePrefix + parameterName, value);
        }

        /// <summary>
        /// 创建存储过程调用参数的方法
        /// 该方法根据parameterNamePrefixType自动加上参数名前缀。
        /// 说明：
        /// 1.传入的parameterName不应包括前缀，适用于支持多数据库统一参数调用命名
        /// 2. 如Sqlserver自动加上“@”符号
        /// 3. MySql的Sql查询参数自动加上“？”前缀
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="autoParameterNamePrefixType"></param>
        /// <returns></returns>
        public IDataParameter CreateParameter(string parameterName, object value, CommandType autoParameterNamePrefixType)
        {
            if (autoParameterNamePrefixType == CommandType.Text)
                return CreateParameter(parameterName, value, SqlTextParameterNamePrefix);
            else if (autoParameterNamePrefixType == CommandType.StoredProcedure)
                return CreateParameter(parameterName, value, StoreProcParameterNamePrefix);
            else
                return CreateParameter(parameterName, value,"");
        }


        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="dbType">参数数据类型</param>
        /// <returns>返回参数</returns>
        public virtual IDataParameter CreateParameter(string parameterName, object value, DbType dbType)
        {
            IDataParameter prm= CreateParameter( parameterName,  value);
            prm.DbType = dbType;
            return prm;
        }
    }
}