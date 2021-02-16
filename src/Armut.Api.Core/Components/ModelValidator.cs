using System.Threading;
using System.Threading.Tasks;
using Armut.Api.Core.Contracts;
using FluentValidation;

namespace Armut.Api.Core.Components
{
    public class ModelValidator : IModelValidator
    {
        private readonly IValidatorFactory _validatorFactory;

        public ModelValidator(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public Task ValidateAndThrowAsync<TModel>(TModel model, CancellationToken token = default)
        {
            IValidator<TModel> validator = _validatorFactory.GetValidator<TModel>();
            return validator.ValidateAndThrowAsync(model, cancellationToken: token);
        }
    }
}
