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
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string readSrcSql="select * from hm_inpatient";

            //IOrmDal orm = OrmDal.Create("SqlServerDb");
            IDataFactory dbSrc = DataFactory.Create("TempSourceDb");
            IDataFactory dbDst = DataFactory.Create("DistinationDb");

            using (IDataReader dr = dbSrc.ExecuteReader(CommandType.Text, readSrcSql, null))
            {

                while (dr.Read())
                {
                    StringBuilder insertSb = new StringBuilder(2048);
                    insertSb.Append(@"insert  into `hm_inpatient`(`InPatientID`,`InPatientNo`,`PatientID`,`PatientName`,`Hospital`,`DoctorName`,`StartTime`,`EndTime`,`InDepartment`,`InDiagnose`,`OutDepartment`,`TreatProcess`,`OutDiagnose`,`CreateTime`,`CheckUp`,`OutAttention`,`Invalid`,`Version`,`IsAudit`,`Reviewer`,`MemberID`,`CreateID`,`CreateName`,`Operation`,`cishu`,`hukoudizhi`,`hukouyoubian`,`lianxiren`,`yubingrenguanxi`,`lianxirendizhi`,`lianxirendianhua`,`ruyuantujing`,`age`,`tianshu`,`chuyuanqingkuang`,`ruyuanbinshi`,`shoucizhuankekebei`,`chuyuanbinshi`,`menzhenzhenduan`,`icdma`,`zyzdjbmc`,`zyzdicdma`,`fszdjbmc`,`fszdicdma`,`sszdjbmc`,`sszdicdma`,`binglizhenduan`,`binglihao`,`guomingyaowu`,`shifoushijian`,`kezhuren`,`zhurenyishen`,`zhuzhiyishen`,`shoushuma`,`shoushuriqi`,`shoushumingchen`,`mazuifenji`,`chuyuanfangshi`,`IsAgainInpatientInOneMonth`,`fukuanfangshi`,`qiekouyuhedengji`,`binlizhenduanjibinbianma`,`chuyuanzhenduanjibinbianma`,`liaoxiao`,`fushuzhenduanjieguo`,`zhuyaozhenduanliaoxiao`,`yuanneiganranicdma`,`yuanneiganranjibinname`,`yuanneiganranjieguo`,`binfazhenicdma`,`binfazhenname`,`binfazhenjieguo`,`quanbushoushuma`,`quanbushoushuname`,`quanbushoushuriqi`,`ruyuanshiqingkuang`,`hbsag`,`hcvab`,`hivab`,`shifousuizhen`,`shuizhenqixian`,`zhubinzhongyima`,`zhubinzhongyiname`,`zhubinyizhenjieguo`,`zhuzhenzhongyima`,`zhuzhenzhoongyiname`,`zhuzhenzhongyijieguo`,`qitazhongyima`,`qitazhongyiname`,`qitazhoangyijieguo`,`shangkoufenji`,`zhongliufenqi`,`fangliaofangshi`,`fangliaochengxu`,`fangliaozhuangzhi`,`yuanfazhaojiliang`,`yuanfazhaocishu`,`yuanfazhaotianshu`,`yuanfazhaokaishiriqi`,`yuanfazhaojieshuriqi`,`quyulinbajiejiliang`,`qulinbajiecishu`,`qulinbajietianshu`,`quyulinbajiekaishiriqi`,`quyulinbajiejieshuriqi`,`zhuanyizhaoname`,`zhuanyizhaojiliang`,`zhuanyizhaocishu`,`zhuanyizhaotianshu`,`zhuanyizhaokaishiriqi`,`zhuangyizhaojieshuriqi`,`hualiaofangshi`,`huliaofangfa`,`DataSource`,`qzrq`)
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
    }
}
