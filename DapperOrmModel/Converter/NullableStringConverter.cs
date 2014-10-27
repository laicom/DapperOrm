using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// ��������ֶε�ֵΪString(Varchar)�ҿ���ΪNull����Ҫ�������ת��
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
