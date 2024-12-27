namespace Application.Categories.Commands.Update;

public class UpdateCategoryCommand : IRequest<Result>
{
    public required string Id { get; set; }
    public required string Title { get; set; }
}
