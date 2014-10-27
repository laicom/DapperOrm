using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using DapperOrm.Model;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Globalization;


namespace DapperOrm
{
    /// <summary>
    ///          IOrmDal Interface
    /// Description:   ORM�������ݷ��ʲ����Ľӿ� 
    /// Create:   2009-6-17  ������  Guoxin.lai@gmail.com
    /// Update: 
    /// 
    /// </summary>
    public abstract class OrmDal : IOrmDal
    {
        /// <summary>
        /// �ڲ�ʹ�õ�IDataFactory�ӿ�ʵ��.
        /// </summary>
        /// <value>The db factory.</value>
        protected abstract IDataFactory DbFactory { get; }
        
        /// <summary>
        /// ������ȡId�Ĳ�ѯ������
        /// </summary>
        protected abstract string SelectIdentitySql { get; }

        /// <summary>
        /// Sql����еĲ����ַ�
        /// Mysql��Ϊ?��Sqlserver��Ϊ@
        /// </summary>
        protected abstract string SqlParameterChar { get; }

        /// <summary>
        /// �����ַ���
        /// </summary>
        public virtual string ConnectionString
        {
            get
            {
                return DbFactory.ConnectionString;
            }
            set
            {
                DbFactory.ConnectionString = value;
            }
        }


        /// <summary>
        /// SQL���ڱȽϲ�ѯʱ��ʱ���ַ��ı�ʾ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract string GetCompareTimeSqlFormat(MatchType type);


        /// <summary>
        /// ʹ��Ĭ������(������Ϊ"Default")����һ��ʵ��.
        /// </summary>
        /// <returns></returns>
        public static IOrmDal Create()
        {
                ConnectionStringSettings connectSetting = ConfigurationManager.ConnectionStrings["Default"];
                if (connectSetting == null)
                    connectSetting = ConfigurationManager.ConnectionStrings[0];
                if (connectSetting == null)
                    throw new Exception("No default data connection setting defined!");
                return Create(connectSetting.ProviderName,connectSetting.ConnectionString);
        }


        /// <summary>
        ///��ָ������������һ��IOrmDal��ʵ��.
        /// </summary>
        /// <param name="connectionSettingName">Name of the connection setting.</param>
        /// <returns></returns>
        public static IOrmDal Create(string connectionSettingName)
        {
            ConnectionStringSettings connectSetting = ConfigurationManager.ConnectionStrings[connectionSettingName];
            if (connectSetting == null)
                throw new Exception(string.Format("Data connection setting named {0} not found", connectionSettingName));
            return Create(connectSetting.ProviderName,connectSetting.ConnectionString);
        }

        /// <summary>
        ///  �ڲ��������
        /// </summary>
        protected static Hashtable ormObjectCaching = Hashtable.Synchronized(new Hashtable());
        /// <summary>
        /// ����ָ���������ӵ�ʵ��
        /// </summary>
        /// <param name="providerName">������</param>
        /// <param name="connectionString">�����ַ���</param>
        /// <returns></returns>
        public static IOrmDal Create(string providerName, string connectionString)
        {
            if (ormObjectCaching.Contains(connectionString))
            {
                return (IOrmDal)ormObjectCaching[connectionString];
            }
            IOrmDal ormDal;
            if (providerName.Contains(","))
            {
                Type type = Type.GetType(providerName);
                if (type == null)
                    throw new Exception(string.Format("Load type for orm provider named ��[{0}]�� failed", providerName));
                ormDal = type.Assembly.CreateInstance(type.FullName) as IOrmDal;
            }
            else
            {
                ormDal = ormDal = (typeof(OrmDal)).Assembly.CreateInstance("DapperOrm." + providerName + "OrmDal") as IOrmDal;
            }
            if (ormDal == null)
                throw new Exception(string.Format("Orm provider named [{0}] not sported", providerName));
            //((OrmDal)ormDal).DbFactory = DataFactory.Create(providerName,connectionString);
            ormDal.ConnectionString = connectionString;

            lock (ormObjectCaching)//���洦��
            {
                ormObjectCaching.Add(connectionString, ormDal);
            }
            return ormDal;
        }


        /// <summary>
        /// ִ������������һ��List
        /// �����ı���ʵ����T��ӳ����Ϣ���
        /// </summary>
        /// <typeparam name="T">Search result type</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(SearchCriteria criteria)
        {
            OrmTableAttribute mappingTable = ORMHelper.GetMappingTable(typeof(T));
            return GetList<T>(criteria, mappingTable.TableName);
        }

        /// <summary>
        /// ָ����/��ͼ��ִ������������һ��List
        /// </summary>
        /// <typeparam name="T">Search result type</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual List<T> GetList<T>(SearchCriteria criteria, string tableName)
        {
            string sWhere = BuildWhereClause<T>(criteria);
            if (criteria.PageSize > 0)
            {
                criteria.RecordCount = GetRecordCount(tableName, sWhere);
                criteria.PageCount = (int)Math.Ceiling((criteria.RecordCount * 1.0) / criteria.PageSize);
            }
            IDataParameter[] prms = PrepareSearchParameters(criteria, tableName, sWhere);
            return DbFactory.ExecuteGetList<T>(CommandType.StoredProcedure, "usp_Data_Search", prms);
        }




        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������.</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public virtual T GetObjectById<T>(int id)
        {
            return GetObjectById<T>(id.ToString());
        }


        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T GetObjectById<T>(long id)
        {
            return GetObjectById<T>(id.ToString());
        }

        /// <summary>
        /// ͨ��ָ���в��ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="searchField">���ҵ��ֶ�</param>
        /// <param name="value">ƥ���ֵ</param>
        /// <param name="matchType">ƥ�������</param>
        /// <returns></returns>
        public virtual T GetObjectByField<T>(string searchField, object value, MatchType matchType)
        {
            SearchCriteria criteria = new SearchCriteria();
            criteria.AddCriteria(searchField, value, matchType);

            List<T> results = this.GetList<T>(criteria);
            if (results.Count >= 1)
                return results[0];

            return default(T);
        }

        /// <summary>
        /// ��ѯ����ָ�������������Ƿ����
        /// </summary>
        /// <typeparam name="T">���ؽ���Ķ�������</typeparam>
        /// <param name="existField">���ҵ��ֶ�</param>
        /// <param name="value">ƥ���ֵ</param>
        /// <param name="matchType">ƥ�������</param>
        /// <returns></returns>
        public virtual bool Exists<T>(string existField, object value, MatchType matchType)
        {
            T result = this.GetObjectByField<T>(existField, value, matchType);
            return result != null;
        }

        /// <summary>
        /// ��ô��������������صĽ������
        /// </summary>
        /// <param name="criteria">��������</param>
        /// <returns></returns>
        public virtual long GetRecordCount<T>(SearchCriteria criteria)
        {
            StringBuilder sbWhere = new StringBuilder("1=1 ", 1024); ;
            string sWhere = BuildWhereClause<T>(criteria);
            OrmTableAttribute mappingTable = ORMHelper.GetMappingTable(typeof(T));
            return GetRecordCount(mappingTable.TableName, sWhere);
        }

        /// <summary>
        /// ͨ��ORM�ı������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Save<T>(T t)
        {
            return Save<T>(null, t);
        }

        /// <summary>
        /// ͨ��ORM�ı������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public virtual bool Save<T>(IDbTransaction trans, T t)
        {
            Type type = typeof(T);
            OrmTableAttribute tableMapping = ORMHelper.GetMappingTable(type);
            if (string.IsNullOrEmpty(tableMapping.IdField))
            {
                throw new OrmException("ORM Exception in Save operation. No IdField attribue specify for" + typeof(T).FullName);
            }
            string idField = ORMHelper.GetMappingField(type, tableMapping.IdField);
            object idFieldValue = type.InvokeMember(tableMapping.IdField, BindingFlags.GetProperty, null, t, null);
            //int id = int.Parse(idFieldValue.ToString());

            //StringBuilder sbSql = new StringBuilder("1=1", 1024);
            string sql = string.Empty;
            PropertyInfo[] propList = type.GetProperties();//ȡ�����������
            List<string> fieldList = new List<string>();
            List<object> valueList = new List<object>();
            foreach (PropertyInfo prop in propList)
            {
                if (prop.PropertyType.IsNotPublic || !prop.CanWrite || prop.Name.Equals(tableMapping.IdField, StringComparison.OrdinalIgnoreCase))
                { //����ֻ������public�����ԣ�������Id����
                    continue;
                }
                OrmFieldAttribute attrib = ORMHelper.GetMappingFieldAttrib(type, prop);
                Type convertType = null;
                string fieldName = prop.Name;
                if (attrib != null)
                {
                    if (attrib.ReadOnly) continue;
                    if (!string.IsNullOrEmpty(attrib.FieldName))
                        fieldName = attrib.FieldName;
                    convertType = attrib.TypeConverter;
                }

                object fieldValue = type.InvokeMember(prop.Name, BindingFlags.GetProperty, null, t, null);
                if (convertType != null)
                {
                    IOrmTypeConverter converter = (IOrmTypeConverter)convertType.Assembly.CreateInstance(convertType.FullName);
                    fieldValue = converter.ConvertFromObj(fieldValue);
                }
                fieldList.Add(fieldName);
                if(fieldValue==null)
                    valueList.Add(DBNull.Value);
                else
                    valueList.Add(fieldValue);
            }
            //if (idFieldValue==null || Int64.Parse(idFieldValue.ToString())==0)//for insert

            bool isInsert = false;
            if (!tableMapping.IsAutoGeneratedId)
            {
                isInsert = (null == GetObjectById<T>(idFieldValue.ToString()));
                if (isInsert)
                {
                    fieldList.Add(idField);
                    valueList.Add(idFieldValue);
                }
            }
            else if ((idFieldValue == null || idFieldValue.ToString() == "0" || idFieldValue.ToString() == string.Empty))//for insert
            {
                if (idFieldValue == null || idFieldValue.ToString() == string.Empty)
                {
                    idFieldValue = Guid.NewGuid().ToString();
                    fieldList.Add(idField);
                    valueList.Add(idFieldValue);
                }
                isInsert = true;
            }
            if (isInsert) //����
            {
                string fieldsCommaString = string.Join(",", fieldList.ToArray());
                string[] qsPrmArray = new string[fieldList.Count];
                for (int i = 0; i < qsPrmArray.Length; i++)
                {
                    qsPrmArray[i] = string.Format("{0}p{1}", SqlParameterChar, i);
                }
                string qsPrmString = string.Join(",", qsPrmArray);
                sql = string.Format(@"INSERT INTO {0}({1}) VALUES({2});  {3}; " //LAST_INSERT_ID()
                                        , tableMapping.TableName, fieldsCommaString, qsPrmString, SelectIdentitySql);
            }
            else //for update
            {
                string[] setvalueArray = new string[fieldList.Count];
                for (int i = 0; i < setvalueArray.Length; i++)
                {
                    setvalueArray[i] = string.Format("{0}={1}p{2}", fieldList[i], SqlParameterChar, i);
                }
                string setvalueString = string.Join(",", setvalueArray);
                sql = string.Format(@"UPDATE {0} SET {1}
                                        WHERE {2}='{3}';
                                 SELECT '{3}';                    ", tableMapping.TableName, setvalueString, idField, idFieldValue.ToString());
            }
            IDataParameter[] paramArray = DbFactory.CreateParameter(fieldList.Count);
            for (int i = 0; i < paramArray.Length; i++)
            {
                paramArray[i] = DbFactory.CreateParameter("p"+i,  valueList[i], CommandType.Text);
            }
            //id = (int)DbFactory.ExecuteScalar<Int64>(trans, CommandType.Text, sql, paramArray);
            object retVal = DbFactory.ExecuteScalar(trans, CommandType.Text, sql, paramArray);
            if (idFieldValue is Int32)
            {
                idFieldValue = Int32.Parse(retVal.ToString());
            }
            else if (idFieldValue is Int64)
            {
                idFieldValue = Int64.Parse(retVal.ToString());
            }

            PropertyInfo propId = type.GetProperty(tableMapping.IdField);

            propId.SetValue(t, idFieldValue, null);

            return true;
        }


        /// <summary>
        /// Builds the where clause.
        /// ��������ƥ�����ͺͲ�ѯ�������ɲ�ѯ���Ĺ���
        /// </summary>
        /// <typeparam name="T">The type of the esult.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        /// Create By Guoxin,Lai 2009-06-18
        protected virtual string BuildWhereClause<T>(SearchCriteria criteria)
        {
            StringBuilder sbWhere = new StringBuilder("1=1 ", 1024);
            if (!string.IsNullOrEmpty(criteria.Key))
            {
                List<string> keyFields = ORMHelper.GetKeySearchFields(typeof(T));
                if (keyFields.Count == 0)
                {
                    string message = string.Format("No Field specify for key search in [{0}]", typeof(T).Name);
                    throw new OrmException(message);
                }
                string[] keys = criteria.Key.Split(new char[] { ' ' });
                foreach (string k in keys)
                {
                    bool appended = false;
                    if (string.IsNullOrEmpty(k)) continue;
                    string key = k;
                    if (key.Contains("*")) //����ͨ���ѯ*
                    {
                        key = key.Replace('*', '%');
                    }
                    else if (key.StartsWith("\"") && key.EndsWith("\"")) //Ҫ���ϸ�ƥ��
                    {
                        key = key.Substring(1, key.Length - 2);
                    }
                    else //Ĭ��ͨ���ѯ
                    {
                        key = string.Format("%{0}%", key);
                    }

                    for (int i = 0; i < keyFields.Count; i++)
                    {
                        string sBre = appended ? " OR " : " AND (";
                        sbWhere.AppendFormat("{0}{1} LIKE '{2}'", sBre, keyFields[i], key);
                        appended = true;
                    }
                    if (appended) sbWhere.Append(")");
                }
            }
            foreach (CriteriaItem item in criteria.Criterias)
            {
                string fieldName = ORMHelper.GetMappingField(typeof(T), item.FieldName);
                string matchValue = string.Empty;
                bool isMatchString = ((item.MatchType & MatchType.String) != 0);
                if (item.Value is bool) isMatchString = false;
                bool isMatchIn = ((item.MatchType & MatchType.In) != 0);
                if (item.Value == null)
                {
                    matchValue="null";
                }
                else if (isMatchIn)
                {
                    if (!isMatchString && item.Value is string) //����ֵ��Inƥ�䣬ƥ��ֵΪ���ŷָ����ַ����������ر���
                    {
                        matchValue = item.Value.ToString();
                    }
                    else
                    {
                        IEnumerator em;
                        if (item.Value is string) //����ƥ��ֵΪ���ŷָ����ַ���
                        {
                            em = item.Value.ToString().Replace("'", "").Split(new char[] { ',' }).GetEnumerator();
                        }
                        else if (item.Value is IEnumerable) //����ƥ��ֵΪ�б������������
                        {
                            em = ((IEnumerable)item.Value).GetEnumerator();
                        }
                        else
                        {
                            throw new OrmException("For MatchType.In the value should be a string or Enumerable object type");
                        }

                        while (em.MoveNext())
                        {
                            string quot = isMatchString ? "'" : ""; //�ַ���ƥ��Ҫ����������
                            if (string.IsNullOrEmpty(matchValue))
                            {
                                matchValue = quot + em.Current.ToString() + quot;
                            }
                            else
                            {
                                matchValue += "," + quot + em.Current.ToString() + quot;
                            }
                        }
                    }
                }
                else if (isMatchString)
                {
                    if (item.Value is DateTime)
                    {
                        DateTime dt = (DateTime)item.Value;
                        if ((item.MatchType & MatchType.ToDate) == MatchType.ToDate && dt.TimeOfDay.TotalSeconds == 0)
                            dt = dt.AddDays(1); //��ָ�����ڣ����24Сʱ
                        matchValue = dt.ToString("yyyy-MM-dd HH:mm:ss");//������ʻ����ڸ�ʽ������
                    }
                    else
                        matchValue = item.Value.ToString();

                    if (item.MatchType == MatchType.WildCard) //�Զ�ͨ���ѯ
                    {
                        matchValue = matchValue.Replace('*', '%');
                        if (!matchValue.StartsWith("%"))
                            matchValue = matchValue.Insert(0, "%");
                        if (!matchValue.EndsWith("%"))
                            matchValue += "%";
                    }
                    matchValue = "'" + matchValue + "'"; //�ַ����Ƚ�Ҫ�õ�����������
                }
                else
                {
                    matchValue = item.Value.ToString();
                }

                if ((item.MatchType & MatchType.Like) == MatchType.Like)
                {
                    if (criteria.Contain(item.FieldName, MatchType.Equal)) continue; //�Ż���ʩ��������Ӧequal��ѯʱ������Like��ѯ

                    if ((item.MatchType & MatchType.Not) != 0)
                        sbWhere.AppendFormat(" AND {0} not like {1} ", fieldName, matchValue);
                    else
                        sbWhere.AppendFormat(" AND {0} like {1} ", fieldName, matchValue);
                }
                else if ((item.MatchType & MatchType.In) == MatchType.In)
                {
                    if (criteria.Contain(item.FieldName, MatchType.Equal)) continue; //�Ż���ʩ��������Ӧequal��ѯʱ������In��ѯ
                    if (!string.IsNullOrEmpty(matchValue))
                    {
                        if ((item.MatchType & MatchType.Not) != 0)
                            sbWhere.AppendFormat(" AND {0} not in({1}) ", fieldName, matchValue);
                        else
                            sbWhere.AppendFormat(" AND {0} in({1}) ", fieldName, matchValue);
                    }
                    else
                    {//ʹ��In��ѯ������matchVaueΪ��ʱ�������б��Ϊ��
                        sbWhere.Append(" AND 1=0");
                    }
                }
                else if ((item.MatchType & MatchType.NullOrEmpty) == MatchType.NullOrEmpty)
                {
                    if ((item.MatchType & MatchType.Not) != 0)
                        sbWhere.AppendFormat(" AND ({0}<>'' AND {0} is not null)", fieldName);
                    else
                        sbWhere.AppendFormat(" AND ({0}='' OR  {0} is null)", fieldName);
                }
                else
                {
                    string cmpStr = "";
                    if ((item.MatchType & MatchType.Greater) != 0)
                    {
                        if ((item.MatchType & MatchType.Equal) != 0)
                            cmpStr = ">=";
                        else
                            cmpStr = ">";
                    }
                    else if ((item.MatchType & MatchType.Smaller) != 0)
                    {
                        if ((item.MatchType & MatchType.Equal) != 0)
                            cmpStr = "<=";
                        else
                            cmpStr = "<";
                    }
                    else if ((item.MatchType & MatchType.Equal) != 0)
                    {
                        if (item.Value == null)
                        {
                            matchValue = "null";
                            if ((item.MatchType & MatchType.Not) != 0)
                                cmpStr = " is not ";
                            else
                                cmpStr = " is ";                            
                        }
                        else
                        {
                            if ((item.MatchType & MatchType.Not) != 0)
                                cmpStr = "<>";
                            else
                                cmpStr = "=";
                        }
                    }

                    if ((item.MatchType & MatchType.TimeCompare) == MatchType.TimeCompare) //ʹ��DateDiff���бȽϲ�ѯ
                    {
                        string cmpFormat = GetCompareTimeSqlFormat(item.MatchType);
                        if (!string.IsNullOrEmpty(cmpFormat))
                        {
                            fieldName = string.Format(cmpFormat, fieldName, matchValue.ToString(CultureInfo.InvariantCulture));
                            matchValue = "0";
                        }
                    }
                    else if ((item.MatchType & MatchType.BitAnd) == MatchType.BitAnd) //ʹ���߼�λ��������бȽ�
                    {
                        fieldName = string.Format(" ({0} & {1}) ", fieldName, matchValue);
                        matchValue = "0";
                    }

                    sbWhere.AppendFormat(" AND {0} {1} {2} ", fieldName, cmpStr, matchValue);
                }
            }
            return sbWhere.ToString();
        }




        /// <summary>
        /// Prepares the search parameters.
        /// </summary>
        /// <param name="cri">The cri.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="sWhere">The s where.</param>
        /// <returns></returns>
        protected virtual IDataParameter[] PrepareSearchParameters(SearchCriteria cri, string tableName, string sWhere)
        {
            string orderString = string.Empty;//string.Format("{0} {1}", cri.OrderField, cri.OrderAsc ? "Asc" : "Desc");
            if (cri.OrderRules != null)
            {
                foreach (OrderRule rule in cri.OrderRules)
                {
                    if (!string.IsNullOrEmpty(orderString))
                    {
                        orderString += " , ";
                    }
                    orderString += rule.Field + " " + rule.Order.ToString();
                }
            }
            IDataParameter[] searchPrms=DbFactory.CreateParameter
                (
                    DbFactory.CreateParameter("TableSource", tableName, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("SearchFields", cri.SearchFields, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("OrderFields", orderString, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("GroupFields", cri.GroupFields, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("sWhere", sWhere, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("pageSize", cri.PageSize, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("pageIndex", cri.PageIndex, CommandType.StoredProcedure),
                    DbFactory.CreateParameter("recordCount", cri.RecordCount, CommandType.StoredProcedure)                    
                );
                

 
            return searchPrms;
        }


        /// <summary>
        /// ��ô������������صĽ������
        /// </summary>
        /// <param name="criteria">��������</param>
        /// <returns></returns>
        protected long GetRecordCount(string tableName, string sWhere)
        {
            string sql = string.Format("select count(*) from {0} where {1}",tableName,sWhere);
            return long.Parse(DbFactory.ExecuteScalar(CommandType.Text, sql,null).ToString());
        }

        /// <summary>
        /// ͨ��Id���ҷ���һ�����
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public T GetObjectById<T>(string id)
        {
            Type type = typeof(T);
            OrmTableAttribute tableMapping = ORMHelper.GetMappingTable(type);
            if (string.IsNullOrEmpty(tableMapping.IdField))
            {
                throw new OrmException("ORM Exception in GetById operation.No IdField attribue specify for" + typeof(T).FullName);
            }
            string idField = ORMHelper.GetMappingField(type, tableMapping.IdField);
            string sql = string.Format("select * from {0} where {1}='{2}'", tableMapping.TableName, idField, id);
            return DbFactory.ExecuteGetSingleRow<T>(CommandType.Text, sql, null);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool RemoveById<T>(string Id)
        {
            Type type = typeof(T);
            OrmTableAttribute tableMapping = ORMHelper.GetMappingTable(type);
            if (string.IsNullOrEmpty(tableMapping.IdField))
            {
                throw new OrmException("ORM Exception in Remove operation.No IdField attribue specify for" + typeof(T).FullName);
            }
            string idField = ORMHelper.GetMappingField(type, tableMapping.IdField);
            string sql = string.Format("delete from {0} where {1}='{2}'", tableMapping.TableName, idField, Id);
            DbFactory.ExecuteNonQuery(CommandType.Text, sql, null);
            return true;
        }


        /// <summary>
        /// ͨ��ORM��ɾ������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual bool RemoveById<T>(int Id)
        {
            return RemoveById<T>(Id.ToString());
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <typeparam name="T">��������.</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        public bool RemoveById<T>(long Id)
        {
            return RemoveById<T>(Id.ToString());
        }

        /// <summary>
        /// ɾ������������¼
        /// </summary>
        /// <param name="criteria">��ѯ����</param>
        /// <returns>����true/false</returns>
        public bool Remove<T>(SearchCriteria criteria)
        {
            string sWhere = BuildWhereClause<T>(criteria);

            Type type = typeof(T);
            OrmTableAttribute tableMapping = ORMHelper.GetMappingTable(type);
            if (string.IsNullOrEmpty(tableMapping.IdField))
            {
                throw new OrmException("ORM Exception in Remove operation.No IdField attribue specify for" + typeof(T).FullName);
            }
            string idField = ORMHelper.GetMappingField(type, tableMapping.IdField);
            string sql = string.Format("delete from {0} where {1}", tableMapping.TableName,sWhere);
            DbFactory.ExecuteNonQuery(CommandType.Text, sql, null);
            return true;
        }
    }
}
