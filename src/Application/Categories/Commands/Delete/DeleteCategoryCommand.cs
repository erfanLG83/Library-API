namespace Application.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest<Result>
{
    public required string Id { get; set; }
}
