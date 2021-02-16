using System;
using JetBrains.Annotations;

namespace Armut.Api.Core.Components
{
    public static class Contract
    {
        [AssertionMethod]
        public static void Requires<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)]bool condition) where TException : Exception
        {
            if (condition)
            {
                return;
            }

            var exception = Activator.CreateInstance<TException>();
            throw exception;
        }

        [AssertionMethod]
        public static void Requires<TException>([AssertionCondition(AssertionConditionType.IS_TRUE)]bool condition, string userMessage) where TException : Exception
        {
            if (condition)
            {
                return;
            }

            var exception = (TException)Activator.CreateInstance(typeof(TException), userMessage);
            throw exception;
        }
    }
}
