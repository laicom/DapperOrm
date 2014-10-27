using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DapperOrm;
using DapperOrm.Model;
using System.Data;
namespace DapperOrmTest
{
    [TestClass]
    public class AccessImportTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string readSrcSql="select * from testtab";

            //IOrmDal orm = OrmDal.Create("SqlServerDb");
            IDataFactory dbSrc = DataFactory.Create("TestAccessDb");
            IDataFactory dbDst = DataFactory.Create("DistinationDb");

            using (IDataReader dr = dbSrc.ExecuteReader(CommandType.Text, readSrcSql, null))
            {

                while (dr.Read())
                {
                    StringBuilder insertSb = new StringBuilder(2048);
                    insertSb.Append(@"insert  into `testtab`(`Id`,`DName`,`LastUpdate`,`Remark`,`Amount`)
                                    values (");

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        insertSb.Append(pareItem(dr[i]));
                        if (i != dr.FieldCount - 1)
                            insertSb.Append(",");
                    }
                    insertSb.Append(");");
                    dbDst.ExecuteNonQuery(CommandType.Text, insertSb.ToString());
                }
            }
        }

        [TestMethod]
        public void DirectTabImportTest()
        {
            DirectTableImport("testtab", "TestAccessDb", "DistinationDb");
        }


        private readonly string[] stringType = new string[] {"String","MySqlDateTime","DateTime"};
        private string pareItem(object item)
        {
            if (item is DBNull)
                return "NULL";
            else
            {
                string itemType = item.GetType().Name;
                if (stringType.Contains(itemType))
                    return "'" + item.ToString() + "'";
                else return item.ToString();
            }
        }

        private void DirectTableImport(string tabName,string srcConnect,string dstConnect)
        {
            string readSrcSql = "select * from " +tabName;

            IDataFactory dbSrc = DataFactory.Create(srcConnect);
            IDataFactory dbDst = DataFactory.Create(dstConnect);

            using (IDataReader dr = dbSrc.ExecuteReader(CommandType.Text, readSrcSql, null))
            {

                while (dr.Read())
                {
                    StringBuilder insertSb = new StringBuilder(2048);
                    insertSb.Append(@"insert  into ").Append(tabName).Append(" values (");

                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        insertSb.Append(pareItem(dr[i]));
                        if (i != dr.FieldCount - 1)
                            insertSb.Append(",");
                    }
                    insertSb.Append(");");
                    dbDst.ExecuteNonQuery(CommandType.Text, insertSb.ToString());
                }
            }
        }
    }
}
