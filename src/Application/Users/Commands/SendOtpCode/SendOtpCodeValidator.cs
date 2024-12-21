namespace Application.Users.Commands.SendOtpCode;

public class SendOtpCodeValidator : AbstractValidator<SendOtpCodeCommand>
{
    public SendOtpCodeValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("شماره تلفن نمی تواند خالی باشد.")
            .Length(11)
            .WithMessage("فرمت شماره تلفن صحیح نمی باشد");
    }
}