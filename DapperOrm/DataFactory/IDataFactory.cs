using System.Data;
using System.Data.Common;
using System.Collections.Generic;
 
namespace DapperOrm
{
    /// <summary>
    /// 数据层操作的接口
    /// Create:  2010-08-06 赖国欣  Guoxin.lai@gmail.com 
    /// Update: 
    /// </summary>
    public interface IDataFactory
    {
        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>返回参数</returns>
        IDataParameter CreateParameter(string parameterName,object value);
        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <param name="dbType">参数数据类型</param>
        /// <returns>返回参数</returns>
        IDataParameter CreateParameter(string parameterName, object value,DbType dbType);

        /// <summary>
        /// 创建单个过程调用参数的方法
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        /// <returns>返回参数</returns>
        /// <param name="parameterNamePrefix">参数名加上自定义前缀</param>
        /// <returns></returns>
        IDataParameter CreateParameter(string parameterName, object value, string parameterNamePrefix);

        /// <summary>
        /// 创建存储过程调用参数的方法
        /// 该方法根据parameterNamePrefixType自动加上参数名前缀。
        /// 说明：
        ///     1.传入的parameterName不应包括前缀，适用于支持多数据库统一参数调用命名
        ///     2. 如Sqlserver自动加上“@”符号
        ///     3. MySql的Sql查询参数自动加上“？”前缀
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IDataParameter CreateParameter(string parameterName, object value, CommandType autoParameterNamePrefixType);




        /// <summary>
        /// 创建指定长度的一组过程调用参数的方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        IDataParameter[] CreateParameter(int count);

        /// <summary>
        /// 创建一组SQL过程调用参数的方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IDataParameter[] CreateParameter(params System.Data.IDataParameter[] parameters);

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString{get;set;}

        /// <summary>
        /// 开始事务处理
        /// </summary>
        /// <returns>返回事务对象</returns>
        IDbTransaction BeginTrans();

        /// <summary>
        /// 开始事务处理
        /// </summary>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        IDbTransaction BeginTrans(IsolationLevel iLevel);

        /// <summary>
        /// 事务回滚处理
        /// </summary>
        /// <param name="trans"></param>
        void RollbackTrans(IDbTransaction trans);

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="trans"></param>
         void CommitTrans(IDbTransaction trans);

        /// <summary>
        /// 执行返回DataSet的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters"></param>
        /// <returns>返回DataSet</returns>
         DataSet ExecuteDataSet(CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        ///  执行返回DataTable的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回DataTable</returns>
        DataTable ExecuteDataTable(CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行非查询SQL
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回影响行数</returns>
        int ExecuteNonQuery(CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        //int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行非查询SQL(支持事务)
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>返回影响行数</returns>
        int ExecuteNonQuery(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行数据读取
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>DataReader对象</returns>
        IDataReader ExecuteReader(CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        object ExecuteScalar(CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        //object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        object ExecuteScalar(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters);
        
        /// <summary>
        /// 执行返回单值的查询
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="trans">事务对象</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令内容</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>对象</returns>
        T ExecuteScalar<T>(IDbTransaction trans, CommandType commandType, string commandText, params IDataParameter[] commandParameters);

        ///// <summary>
        ///// 执行返回单值的查询
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cmdType"></param>
        ///// <param name="cmdName"></param>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //T ExeSqlGetScalar<T>(CommandType cmdType, string cmdName, IDataParameter[] param);

        
        /// <summary>
        /// 执行返回一个实体对象实例列表的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象列表</returns>
        List<T> ExecuteGetList<T>(CommandType cmdType, string cmdName, IDataParameter[] param);

        /// <summary>
        /// 执行返回单行数据的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象列表</returns>
        T ExecuteGetSingleRow<T>(CommandType cmdType, string cmdName, IDataParameter[] param);

        /// <summary>
        /// 执行返回单列的查询
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdName">命令内容</param>
        /// <param name="param">参数</param>
        /// <returns>对象</returns>
        List<T> ExecuteGetSingleFieldList<T>(CommandType cmdType, string cmdName, IDataParameter[] param);
   }
}
