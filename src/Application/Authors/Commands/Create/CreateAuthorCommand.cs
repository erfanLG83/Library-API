namespace Application.Authors.Commands.Create;

public record CreateAuthorCommand(string FirstName, string LastName) : IRequest;
