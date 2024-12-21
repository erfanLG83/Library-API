namespace Application.Users.Commands.VerifyPhoneNumber;

public class VerifyPhoneNumberCommandValidator : AbstractValidator<VerifyPhoneNumberCommand>
{
    public VerifyPhoneNumberCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره تلفن نمی تواند خالی باشد");

        RuleFor(x => x.OtpCode)
            .NotEmpty()
            .WithMessage("کد ورود نمی تواند خالی باشد");
    }
}