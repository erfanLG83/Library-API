namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("نام نمی تواند خالی باشد")
            .MaximumLength(50)
            .WithMessage("نام نمی تواند بیشتر از 50 کارکتر باشد");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("نام خانوادگی نمی تواند خالی باشد.")
            .MaximumLength(50)
            .WithMessage("نام خانوادگی نمی تواند بیشتر از 50 کارکتر باشد");
    }
}