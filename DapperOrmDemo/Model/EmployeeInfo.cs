using System;
using System.Collections.Generic;
using System.Web;
using DapperOrm.Model;

namespace DbAccessDemo.Model
{
    [Serializable()]
    [OrmTable("Employee", "EmployeeId")]
    public class EmployeeInfo
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        [OrmField("EmployeeId")]
        public Int64 EmployeeId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [OrmField(FieldName = "DepartmentId")]
        public int DepartmentId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        [OrmField(FieldName = "FullName", KeySearch = true)]
        public string FullName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        //此SexConverter为自定义的转换
        [OrmField(FieldName = "Gender",TypeConverter=typeof(SexConverter)) ]
        public string Sex { get; set; }

        //TypeConverter=(typeof(SexConverter)))

        /// <summary>
        /// 出生年月
        /// </summary>
        //时间转换
        [OrmField(FieldName = "Birthday", TypeConverter = (typeof(DateTimeConverter)))]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [OrmField(FieldName = "CreateDate",TypeConverter = (typeof(DateTimeConverter)))]
        public DateTime CreateDate { get; set; }

   }
}