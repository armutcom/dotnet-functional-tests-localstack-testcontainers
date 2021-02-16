using System;
using FluentValidation;

namespace Armut.Tests.Common.Components
{
    public class TestValidatorFactory : ValidatorFactoryBase {
        private readonly IServiceProvider _serviceProvider;

        public TestValidatorFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public override IValidator CreateInstance(Type validatorType) {
            return _serviceProvider.GetService(validatorType) as IValidator;
        }
    }
}
