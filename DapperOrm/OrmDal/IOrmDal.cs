using System;
using System.Collections.Generic;
using System.Text;
using DapperOrm.Model;
using System.Data.SqlClient;

namespace DapperOrm
{

    /// <summary>
    ///          IOrmDal Interface
    /// Description:   ORM�������ݷ��ʲ����Ľӿ� 
    /// Author:     ������  Guoxin.lai@gmail.com
    /// Create Date: 2009-6-17
    /// Update: 
    /// 
    /// </summary>
    public interface IOrmDal
    {

        /// <summary>
        /// ִ������������һ��List
        /// �����ı���ʵ����T��ӳ����Ϣ���
        /// </summary>
        /// <typeparam name="T">���ؽ��������</typeparam>
        /// <param name="criteria">��ѯ���� .</param>
        /// <returns>���ؽ���б�</returns>
        List<T> GetList<T>(SearchCriteria criteria);

        /// <summary>
        /// ָ����/��ͼ��ִ������������һ��List
        /// </summary>
        /// <typeparam name="T">���ؽ��������</typeparam>
        /// <param name="criteria">��ѯ����.</param>
        /// <param name="tableName">ָ������.</param>
        /// <returns>���ؽ���б�</returns>
        List<T> GetList<T>(SearchCriteria criteria, string tableName);

        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(int id);


        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(string id);

        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        T GetObjectById<T>(Int64 id);


        /// <summary>
        /// ͨ��ָ���в��ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="searchField">���ҵ��ֶ�</param>
        /// <param name="value">ƥ���ֵ</param>
        /// <param name="matchType">ƥ�������</param>
        /// <returns></returns>
        T GetObjectByField<T>(string searchField, object value, MatchType matchType);

        /// <summary>
        /// ��ѯ����ָ�������������Ƿ����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="existField">���ҵ��ֶ�</param>
        /// <param name="value">ƥ���ֵ</param>
        /// <param name="matchType">ƥ�������</param>
        /// <returns></returns>
        bool Exists<T>(string existField, object value, MatchType matchType);

        /// <summary>
        /// ��ô������������صĽ������
        /// </summary>
        /// <param name="criteria">��������</param>
        /// <returns></returns>
        long GetRecordCount<T>(SearchCriteria criteria);

        /// <summary>
        /// ����
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="t">Ҫ����Ķ���</param>
        /// <returns></returns>
        bool Save<T>(T t);

        /// <summary>
        /// ����,֧������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="trans">�������</param>
        /// <param name="t">Ҫ����Ķ���</param>
        /// <returns></returns>
        bool Save<T>(System.Data.IDbTransaction trans, T t);

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        bool RemoveById<T>(int id);

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        bool RemoveById<T>(Int64 id);


        /// <summary>
        /// ɾ��
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveById<T>(string id);

        /// <summary>
        /// ɾ������������¼
        /// </summary>
        /// <param name="criteria">��ѯ����</param>
        /// <returns>����true/false</returns>
        bool Remove<T>(SearchCriteria criteria);


        /// <summary>
        /// �����ַ���
        /// </summary>
        string ConnectionString { get; set; }
    }
}
