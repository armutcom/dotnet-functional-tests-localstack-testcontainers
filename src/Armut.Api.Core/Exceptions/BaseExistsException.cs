using System;

namespace Armut.Api.Core.Exceptions
{
    [Serializable]
    public abstract class BaseExistsException : InvalidServiceOperationException
    {
        public BaseExistsException(string message) : base(message)
        {
        }
    }
}