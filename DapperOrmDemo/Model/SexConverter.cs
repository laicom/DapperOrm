using System;
using System.Collections.Generic;
using System.Web;
using DapperOrm.Model;


namespace DbAccessDemo.Model
{ 
    /// <summary>
    /// 自定义接口转换 需实现IOrmTypeConverter接口 性别的数据库字段为Bit，转为性别的字符串表示
    /// </summary>
    public class SexConverter : IOrmTypeConverter
    {
       
        /// <summary>
        /// 从实体到关系
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public object ConvertFromObj(object obj)
        {

            return ((string)obj == "男") ? true : false;


        }

        /// <summary>
        /// 从实体到关系
        /// </summary>
        /// <param name="src">源</param>
        /// <returns></returns>
        public object ConvertToObj(object src)
        {

            return ((bool)src) ? "男" : "女";

        }
    }
}
