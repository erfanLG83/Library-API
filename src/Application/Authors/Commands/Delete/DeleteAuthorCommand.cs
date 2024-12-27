namespace Application.Authors.Commands.Delete;

public class DeleteAuthorCommand : IRequest<Result>
{
    public required string Id { get; set; }
}
