using System;
using System.Collections.Generic;
using System.Text;
using DapperOrm.Model;
using System.Data.SqlClient;

namespace DapperOrm
{

    /// <summary>
    ///          IOrmDal Interface
    /// Description:   ORM进行数据访问操作的接口 
    /// Author:     赖国欣  Guoxin.lai@gmail.com
    /// Create Date: 2009-6-17
    /// Update: 
    /// 
    /// </summary>
    public interface IOrmDal
    {

        /// <summary>
        /// 执行搜索，返回一个List
        /// 搜索的表由实体类T的映射信息获得
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="criteria">查询条件 .</param>
        /// <returns>返回结果列表</returns>
        List<T> GetList<T>(SearchCriteria criteria);

        /// <summary>
        /// 指定表/视图，执行搜索，返回一个List
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="criteria">查询条件.</param>
        /// <param name="tableName">指定表名.</param>
        /// <returns>返回结果列表</returns>
        List<T> GetList<T>(SearchCriteria criteria, string tableName);

        /// <summary>
        /// 通过Id查找返回一个结果
        /// </summary>
        /// <typeparam name="T">返回结果的对象类型.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(int id);


        /// <summary>
        /// 通过Id查找返回一个结果
        /// </summary>
        /// <typeparam name="T">返回结果的对象类型</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(string id);

        /// <summary>
        /// 通过Id查找返回一个结果
        /// </summary>
        /// <typeparam name="T">返回结果的对象类型</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(Int64 id);


        /// <summary>
        /// 通过指定列查找返回一个结果
        /// </summary>
        /// <typeparam name="T">返回结果的对象类型</typeparam>
        /// <param name="searchField">查找的字段</param>
        /// <param name="value">匹配的值</param>
        /// <param name="matchType">匹配的类型</param>
        /// <returns></returns>
        T GetObjectByField<T>(string searchField, object value, MatchType matchType);

        /// <summary>
        /// 查询符合指定条件的数据是否存在
        /// </summary>
        /// <typeparam name="T">返回结果的对象类型</typeparam>
        /// <param name="existField">查找的字段</param>
        /// <param name="value">匹配的值</param>
        /// <param name="matchType">匹配的类型</param>
        /// <returns></returns>
        bool Exists<T>(string existField, object value, MatchType matchType);

        /// <summary>
        /// 获得此搜索条件返回的结果总数
        /// </summary>
        /// <param name="criteria">搜索条件</param>
        /// <returns></returns>
        long GetRecordCount<T>(SearchCriteria criteria);

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T">对象类型.</typeparam>
        /// <param name="t">要保存的对象</param>
        /// <returns></returns>
        bool Save<T>(T t);

        /// <summary>
        /// 保存,支持事务
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="trans">事务对象</param>
        /// <param name="t">要保存的对象</param>
        /// <returns></returns>
        bool Save<T>(System.Data.IDbTransaction trans, T t);

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">对象类型.</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        bool RemoveById<T>(int id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">对象类型.</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        bool RemoveById<T>(Int64 id);


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">对象类型.</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveById<T>(string id);

        /// <summary>
        /// 删除符合条件记录
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <returns>返回true/false</returns>
        bool Remove<T>(SearchCriteria criteria);


        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }
    }
}
