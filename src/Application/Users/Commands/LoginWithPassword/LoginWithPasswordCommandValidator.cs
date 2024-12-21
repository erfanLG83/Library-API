namespace Application.Users.Commands.LoginWithPassword;

public class LoginWithPasswordCommandValidator : AbstractValidator<LoginWithPasswordCommand>
{
    public LoginWithPasswordCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty()
            .WithMessage("شماره تلفن نمیتواند خالی باشد.");

        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("رمز عبور نمی تواند خالی باشد.");
    }
}