using System;
using System.Collections.Generic;
using System.Text;
using DapperOrm.Model.Resource;

namespace DapperOrm.Model
{
    /// <summary>
    /// 性别的数据库字段 为Bit，转为性别的字符串表示
    /// </summary>
    public class SexConverter : IOrmTypeConverter
    {
        /// <summary>
        /// 从关系数据到实体数据的类型转换
        /// </summary>
        /// <param name="src">源数据</param>
        /// <returns>实体的数据</returns>
        public object ConvertToObj(object src)
        {
            return ((bool)src) ? TextResc.Male : TextResc.Female;
        }

        /// <summary>
        /// 从实体数据到关系数据的类型转换
        /// </summary>
        /// <param name="obj">实体数据</param>
        /// <returns>转换后的值</returns>
        public object ConvertFromObj(object obj)
        {
            return (obj.ToString() == TextResc.Male);
        }
    }
}
