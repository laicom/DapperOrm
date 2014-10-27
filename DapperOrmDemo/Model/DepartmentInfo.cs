using System;
using System.Collections.Generic;
using System.Web;
//引用Orm DLL
using DapperOrm.Model;

namespace DbAccessDemo.Model
{
    
    /// <summary>
    /// 部门信息
    /// </summary>
    [Serializable()]
    [OrmTable("Department", "DepartmentId")]
    public class DepartmentInfo
    {
        /// <summary>
        ///部门Id 
        /// </summary>
        [OrmField("DepartmentId")]
        public int DepartmentId { get; set; }

       
        /// <summary>
        /// 部门名称
        /// </summary>
        [OrmField(FieldName = "DepartmentName", KeySearch = true)]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门描述
        /// </summary>
        //如果字段的类型为string且可以为空时 需转换 
        [OrmField(FieldName = "Description", TypeConverter = (typeof(NullableStringConverter)))]
        public string Description { get; set; }
    }
}