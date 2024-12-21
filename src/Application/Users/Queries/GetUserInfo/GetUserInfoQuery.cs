namespace Application.Users.Queries.GetUserInfo;

public record GetUserInfoQuery(string UserId) : IRequest<GetUserInfoResponse>;
