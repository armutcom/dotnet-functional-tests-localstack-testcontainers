using System;

namespace Armut.Api.Core.Exceptions
{
    [Serializable]
    public class UserExistsException : BaseExistsException
    {
        public UserExistsException(string message) : base(message)
        {
        }
    }
}