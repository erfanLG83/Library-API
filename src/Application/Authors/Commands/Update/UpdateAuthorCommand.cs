namespace Application.Authors.Commands.Update;

public class UpdateAuthorCommand : IRequest<Result>
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
