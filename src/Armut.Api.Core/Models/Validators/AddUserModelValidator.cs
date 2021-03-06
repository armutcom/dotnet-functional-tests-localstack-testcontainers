﻿using FluentValidation;

namespace Armut.Api.Core.Models.Validators
{
    public class AddUserModelValidator : AbstractValidator<AddUserModel>
    {
        public AddUserModelValidator()
        {
            RuleFor(model => model.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Invalid mail address");
            RuleFor(model => model.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required");
            RuleFor(model => model.LastName).NotNull().NotEmpty().WithMessage("LastName is required");
        }
    }

    public class AddUserViewModelValidator : AbstractValidator<AddUserViewModel>
    {
        public AddUserViewModelValidator()
        {
            RuleFor(model => model.Email).NotNull().NotEmpty().EmailAddress().WithMessage("Invalid mail address");
            RuleFor(model => model.FirstName).NotNull().NotEmpty().WithMessage("FirstName is required");
            RuleFor(model => model.LastName).NotNull().NotEmpty().WithMessage("LastName is required");
            RuleFor(model => model.ProfilePictureBase64).NotNull().NotEmpty().WithMessage("ProfilePictureBase64 is required");
        }
    }
}
