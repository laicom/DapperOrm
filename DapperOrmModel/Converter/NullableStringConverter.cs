using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// 如果数据字段的值为String(Varchar)且可能为Null，需要加入这个转换
    /// </summary>
    public class NullableStringConverter : IOrmTypeConverter
    {
        //public object Convert(object source, bool isReadDb)
        //{
        //    if (isReadDb)
        //    {
        //        if (source is DBNull)
        //            return string.Empty;
        //    }
        //    else
        //    {
        //        if (source == null)
        //            return string.Empty;
        //    }
        //    return source;
        //}

        public object ConvertToObj(object src)
        {
            if (src is DBNull)
                    return string.Empty;
            return src;
        }

        public object ConvertFromObj(object obj)
        {
            if (obj == null)
                return string.Empty;
            return obj;
        }
    }
}
