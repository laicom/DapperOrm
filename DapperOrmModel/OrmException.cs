using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// ORM exception Class
    /// ���ڶ���ʵ��ӳ������쳣
    /// </summary>
    public class OrmException: Exception
    {
        /// <summary>
        /// ���캯�� <see cref="OrmException"/> class.
        /// </summary>
        public OrmException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public OrmException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OrmException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public OrmException(string message, Exception innerException)
            : base(message, innerException)
        { 
        
        }
    }
}
