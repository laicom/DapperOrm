using System;
using System.Collections.Generic;
using System.Text;
using DapperOrm.Model.Resource;

namespace DapperOrm.Model
{
    /// <summary>
    /// �Ա�����ݿ��ֶ� ΪBit��תΪ�Ա���ַ�����ʾ
    /// </summary>
    public class SexConverter : IOrmTypeConverter
    {
        /// <summary>
        /// �ӹ�ϵ���ݵ�ʵ�����ݵ�����ת��
        /// </summary>
        /// <param name="src">Դ����</param>
        /// <returns>ʵ�������</returns>
        public object ConvertToObj(object src)
        {
            return ((bool)src) ? TextResc.Male : TextResc.Female;
        }

        /// <summary>
        /// ��ʵ�����ݵ���ϵ���ݵ�����ת��
        /// </summary>
        /// <param name="obj">ʵ������</param>
        /// <returns>ת�����ֵ</returns>
        public object ConvertFromObj(object obj)
        {
            return (obj.ToString() == TextResc.Male);
        }
    }
}
