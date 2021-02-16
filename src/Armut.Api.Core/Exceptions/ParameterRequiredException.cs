using System;
using System.Collections.Generic;
using System.Text;

namespace Armut.Api.Core.Exceptions
{
    public class ParameterRequiredException : BaseServiceException
    {
        protected ParameterRequiredException(string fieldName)
            : base(fieldName)
        {
            Reason = "required";
        }

        public string Reason { get; }
    }
}
