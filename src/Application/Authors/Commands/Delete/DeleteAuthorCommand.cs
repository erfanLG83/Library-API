namespace Application.Authors.Commands.Update;

public class DeleteAuthorCommand : IRequest<Result>
{
    public required string Id { get; set; }
}
