using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// DateTime����ת����
    /// </summary>
    public class DateTimeConverter : IOrmTypeConverter
    {

        public object ConvertToObj(object src)
        {
            if (src is DBNull)
                return default(DateTime);
            else
            {
                DateTime result;
                DateTime.TryParse(src.ToString(), out result);
                return result;
            }
        }

        public object ConvertFromObj(object obj)
        {
            if (((DateTime)obj).Ticks == 0)
                return DBNull.Value;
            return obj;
        }
    }
}
