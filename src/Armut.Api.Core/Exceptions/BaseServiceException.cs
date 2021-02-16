using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Armut.Api.Core.Exceptions
{
    [Serializable]
    public abstract class BaseServiceException : BaseException
    {
        protected BaseServiceException()
        {
        }

        protected BaseServiceException(string message) : base(message)
        {
        }

        protected BaseServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BaseServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
