using System;
using System.Collections.Generic;
using System.Text;

namespace DapperOrm.Model
{
    /// <summary>
    /// ORM exception Class
    /// 用于对象实体映射操作异常
    /// </summary>
    public class OrmException: Exception
    {
        /// <summary>
        /// 构造函数 <see cref="OrmException"/> class.
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
