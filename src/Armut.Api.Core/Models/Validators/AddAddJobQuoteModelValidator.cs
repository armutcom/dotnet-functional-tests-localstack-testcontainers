using FluentValidation;

namespace Armut.Api.Core.Models.Validators
{
    public class AddAddJobQuoteModelValidator : AbstractValidator<AddJobQuoteModel>
    {
        public AddAddJobQuoteModelValidator()
        {
            RuleFor(model => model.UserId).NotNull().NotEmpty().GreaterThan(0).WithMessage("UserId is required");
            RuleFor(model => model.JobId).NotNull().NotEmpty().GreaterThan(0).WithMessage("JobId is required");
            RuleFor(model => model.ProviderId).NotNull().NotEmpty().GreaterThan(0).WithMessage("ProviderId is required");
            RuleFor(model => model.QuotePrice).NotNull().NotEmpty().GreaterThan(0).WithMessage("QuotePrice is required");
        }
    }

    public class AddJobQuoteViewModelValidator : AbstractValidator<AddJobQuoteViewModel>
    {
        public AddJobQuoteViewModelValidator()
        {
            RuleFor(model => model.UserId).NotNull().NotEmpty().GreaterThan(0).WithMessage("UserId is required");
            RuleFor(model => model.ProviderId).NotNull().NotEmpty().GreaterThan(0).WithMessage("ProviderId is required");
            RuleFor(model => model.QuotePrice).NotNull().NotEmpty().GreaterThan(0).WithMessage("QuotePrice is required");
        }
    }
}