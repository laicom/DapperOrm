using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// ����ORM���ݸ�ֵʱ�Զ������������ת���ӿ�
    /// </summary>
    public interface IOrmTypeConverter
    {
        /// <summary>
        /// Convert.
        /// </summary>
        /// <param name="source">����Դ</param>
        /// <param name="isReadDb">
        /// �Ƿ��ȡ����.  
        /// true��������
        /// false��д����</param>
        /// <returns></returns>
        //object Convert(object source, bool isReadDb);
        
        /// <summary>
        /// �ӹ�ϵ���ݵ�ʵ�����ݵ�����ת��
        /// </summary>
        /// <param name="src">Դ����</param>
        /// <returns>ʵ�������</returns>
        object ConvertToObj(object src);

        /// <summary>
        /// ��ʵ�����ݵ���ϵ���ݵ�����ת��
        /// </summary>
        /// <param name="obj">ʵ������</param>
        /// <returns>ת�����ֵ</returns>
        object ConvertFromObj(object obj);
    }
}
