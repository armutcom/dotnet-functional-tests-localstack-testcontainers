using FluentValidation;

namespace Armut.Api.Core.Models.Validators
{
    public class AddJobModelValidator : AbstractValidator<AddJobModel>
    {
        public AddJobModelValidator()
        {
            RuleFor(model => model.UserId).NotNull().NotEmpty().GreaterThan(0).WithMessage("UserId is required");
            RuleFor(model => model.ServiceId).NotNull().NotEmpty().GreaterThan(0).WithMessage("ServiceId is required");
            RuleFor(model => model.Description).NotNull().NotEmpty().WithMessage("Description is required");
            RuleFor(model => model.JobStartDateTime).NotNull().NotEmpty().WithMessage("JobStartDateTime is required");
        }
    }
}