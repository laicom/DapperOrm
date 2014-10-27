using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// 用于ORM数据赋值时自定义的数据类型转换接口
    /// </summary>
    public interface IOrmTypeConverter
    {
        /// <summary>
        /// Convert.
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="isReadDb">
        /// 是否读取数据.  
        /// true：读数据
        /// false：写数据</param>
        /// <returns></returns>
        //object Convert(object source, bool isReadDb);
        
        /// <summary>
        /// 从关系数据到实体数据的类型转换
        /// </summary>
        /// <param name="src">源数据</param>
        /// <returns>实体的数据</returns>
        object ConvertToObj(object src);

        /// <summary>
        /// 从实体数据到关系数据的类型转换
        /// </summary>
        /// <param name="obj">实体数据</param>
        /// <returns>转换后的值</returns>
        object ConvertFromObj(object obj);
    }
}
