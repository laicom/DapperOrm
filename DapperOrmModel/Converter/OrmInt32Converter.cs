using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{ 
    /// <summary>
    /// 其它数值类型(如：Int64)强制转为Int32
    /// </summary>
    public class OrmInt32Converter : IOrmTypeConverter
    {
        /// <summary>
        /// 从关系数据到实体数据的类型转换
        /// </summary>
        /// <param name="src">源数据</param>
        /// <returns>实体的数据</returns>
        public object ConvertToObj(object src)
        {
            if (src == null || src is DBNull)
                return 0;
            return Int32.Parse(src.ToString());
        }

        /// <summary>
        /// 从实体数据到关系数据的类型转换
        /// </summary>
        /// <param name="obj">实体数据</param>
        /// <returns>转换后的值</returns>
        public object ConvertFromObj(object obj)
        {
            return Int32.Parse(obj.ToString());
        }
    }
}
