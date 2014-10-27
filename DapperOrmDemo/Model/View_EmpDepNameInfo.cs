using System;
using System.Collections.Generic;
using System.Web;
using DapperOrm.Model;

namespace DbAccessDemo.Model
{
    [Serializable()]
    //视图
    [OrmTable("View_EmpDepName", "EmployeeId")]
    public class View_EmpDepNameInfo
    {

        [OrmField("EmployeeId")]
        public int EmployeeId { get; set; }

        [OrmField(FieldName="FullName", KeySearch = true)]
        public string FullName { get; set; }

        [OrmField(FieldName="DepartmentName", KeySearch = true)]
        public string DepartmentName { get; set; }
    }
}
