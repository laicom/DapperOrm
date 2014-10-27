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
    /// �������ݲ�����ʵ�֣����󹤳���
    /// Create:  2010-08-06 ������  Guoxin.lai@gmail.com 
    /// Update: 
    /// </summary>
    public abstract class DataFactory : IDataFactory
    {
        /// <summary>
        /// �����ַ���
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// ��ȡConnection����
        /// </summary>
        /// <returns></returns>
        protected abstract IDbConnection GetConnection();

        /// <summary>
        /// ��ȡDbCommand����
        /// </summary>
        /// <returns></returns>
        protected abstract IDbCommand GetCommand();

        /// <summary>
        /// ��ȡDataAdapter����
        /// </summary>
        /// <returns></returns>
        protected abstract IDataAdapter GetDataAdapter();

        /// <summary>
        /// ���Ĭ�ϵ�DataFactory
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
        /// ����һ��DataFactory
        /// </summary>
        public static IDataFactory Create(string connectionSettingName)
        {
            ConnectionStringSettings connectSetting = ConfigurationManager.ConnectionStrings[connectionSettingName];
            if(connectSetting==null)
                throw new Exception(string.Format("Data connection setting named {0} not found",connectionSettingName));
            return Create(connectSetting.ProviderName, connectSetting.ConnectionString);
        }

        /// <summary>
        /// �ڲ��������
        /// </summary>
        protected static Hashtable factoryCaching=Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ����һ��DataFactory
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
                    throw new Exception(string.Format("Load type for data provider named ��[{0}]�� failed", providerName));
                dataFactory = type.Assembly.CreateInstance(type.FullName) as IDataFactory;
            }else
            {
                dataFactory= (typeof(DataFactory)).Assembly.CreateInstance("DapperOrm." + providerName + "DataFactory") as IDataFactory;
            }
            if (dataFactory == null)
                throw new Exception(string.Format("The data provider named [{0}] not sported", providerName));

            dataFactory.ConnectionString = connectionString;

            lock (factoryCaching)//���洦��
            {
                factoryCaching.Add(connectionString, dataFactory);
            }
            return dataFactory;
        }


        /// <summary>
        /// ִ�зǲ�ѯSQL
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����Ӱ������</returns>
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
        /// ��ʼ������
        /// </summary>
        /// <returns>�����������</returns>
        public virtual IDbTransaction BeginTrans()
        {
            IDbConnection conn = GetConnection();
            conn.Open();
            IDbTransaction trans = conn.BeginTransaction();
            return trans;
        }


        /// <summary>
        /// ��ʼ������
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
        /// �ύ������
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
        /// �´��ع�
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
        /// ִ�зǲ�ѯSQL(֧������)
        /// </summary>
        /// <param name="trans">�������</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����Ӱ������</returns>
        public virtual int ExecuteNonQuery(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            if (trans == null) return ExecuteNonQuery(commandType, commandText, commandParameters);
            IDbCommand cmd = GetCommand();
            PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            return val;
        }



        /// <summary>
        /// ִ�����ݶ�ȡ
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>DataReader����</returns>
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
        /// ִ�з��ص�ֵ�Ĳ�ѯ
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����</returns>
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
        /// ִ�з��ص�ֵ�Ĳ�ѯ
        /// </summary>
        /// <param name="trans">�������</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����</returns>
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
        /// ִ�з��ص�ֵ�Ĳ�ѯ
        /// </summary>
        /// <typeparam name="T">���صĶ�������</typeparam>
        /// <param name="trans">�������</param>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����</returns>
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
        /// ִ�з��ص������ݵĲ�ѯ
        /// </summary>
        /// <typeparam name="T">���ض�������</typeparam>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdName">��������</param>
        /// <param name="param">����</param>
        /// <returns>�����б�</returns>
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
        /// ִ�з���һ��ʵ�����ʵ���б�Ĳ�ѯ
        /// </summary>
        /// <typeparam name="T">���ض�������</typeparam>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdName">��������</param>
        /// <param name="param">����</param>
        /// <returns>�����б�</returns>
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
        /// ִ�з��ص��еĲ�ѯ
        /// </summary>
        /// <typeparam name="T">���ض�������</typeparam>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdName">��������</param>
        /// <param name="param">����</param>
        /// <returns>����</returns>
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
        /// ִ�з���DataSet�Ĳ�ѯ
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters"></param>
        /// <returns>����DataSet</returns>
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
        /// ִ�з���DataTable�Ĳ�ѯ
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <param name="commandText">��������</param>
        /// <param name="commandParameters">����</param>
        /// <returns>����DataTable</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params IDataParameter[] commandParameters)
        {
            DataSet ds=ExecuteDataSet(commandType, commandText, commandParameters);
            return ds.Tables[0];
        }
        /// <summary>
        /// �����������̵��ò����ķ���
        /// </summary>
        /// <param name="parameterName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="dbType">������������</param>
        /// <returns>���ز���</returns>
        public abstract IDataParameter CreateParameter(string parameterName, object value);
        /// <summary>
        /// ����ָ�����ȵ�һ����̵��ò����ķ���
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public abstract IDataParameter[] CreateParameter(int count);

        /// <summary>
        /// ����һ��SQL���̵��ò����ķ���
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataParameter[] CreateParameter(params IDataParameter[] parameters)
        {
            return parameters;
        }
        /// <summary>
        /// �洢���̲�����ǰ׺
        /// </summary>
        protected abstract string StoreProcParameterNamePrefix { get; }

        /// <summary>
        /// ��ѯ�ַ���������ǰ׺
        /// </summary>
        protected abstract string SqlTextParameterNamePrefix { get; }

        /// <summary>
        /// �����������̵��ò����ķ���
        /// </summary>
        /// <param name="parameterName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="parameterNamePrefix">�����������Զ���ǰ׺</param>
        /// <returns>���ز���</returns>
        public IDataParameter CreateParameter(string parameterName, object value, string parameterNamePrefix)
        {
            return CreateParameter(parameterNamePrefix + parameterName, value);
        }

        /// <summary>
        /// �����洢���̵��ò����ķ���
        /// �÷�������parameterNamePrefixType�Զ����ϲ�����ǰ׺��
        /// ˵����
        /// 1.�����parameterName��Ӧ����ǰ׺��������֧�ֶ����ݿ�ͳһ������������
        /// 2. ��Sqlserver�Զ����ϡ�@������
        /// 3. MySql��Sql��ѯ�����Զ����ϡ�����ǰ׺
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
        /// �����������̵��ò����ķ���
        /// </summary>
        /// <param name="parameterName">������</param>
        /// <param name="value">ֵ</param>
        /// <param name="dbType">������������</param>
        /// <returns>���ز���</returns>
        public virtual IDataParameter CreateParameter(string parameterName, object value, DbType dbType)
        {
            IDataParameter prm= CreateParameter( parameterName,  value);
            prm.DbType = dbType;
            return prm;
        }
    }
}