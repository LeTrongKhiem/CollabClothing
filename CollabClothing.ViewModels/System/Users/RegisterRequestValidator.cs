using System;
using FluentValidation;

namespace CollabClothing.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name is required!!!")
                                    .MaximumLength(200).WithMessage("User Name is not over 200 character!!!")
                                    .MinimumLength(4).WithMessage("User Name over 4 character");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required!!!").MaximumLength(200).WithMessage("FirstName is not over 200 character!!!");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required!!!").MaximumLength(200).WithMessage("LastName is not over 200 character!!!");
            RuleFor(x => x.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday cannot Greater Than 100 year!!!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!!").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email is not format");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required!!").Matches(@"(84|0[3|5|7|8|9])+([0-9]{8})\b").WithMessage("Phone number is not format");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is Required")
                        .MinimumLength(6).WithMessage("Password is at least 6 letters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ComfirmPassword)
                {
                    context.AddFailure("Comfirm password is incorrect!!!");
                }
            });
        }
    }
}