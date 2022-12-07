using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bs.Frmwrk.Base.Exceptions
{
    public class BsException : Exception
    {
        public long? ErrorCode { get; set; }
        public BsException() : base() { }
        public BsException(string message) : base(message) { }
        public BsException(long errorCode) : base()
        {
            ErrorCode = errorCode;
        }
        public BsException(string message, Exception ex) : base(message, ex) { }
        public BsException(long errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
        public BsException(long errorCode, string message, Exception ex) : base(message, ex)
        {
            ErrorCode = errorCode;
        }
    }
}
