using System;

namespace Armut.Api.Core.Exceptions
{
    [Serializable]
    public abstract class InvalidServiceOperationException : BaseServiceException
    {
        protected InvalidServiceOperationException(string message) : base(message)
        {
        }
    }
}