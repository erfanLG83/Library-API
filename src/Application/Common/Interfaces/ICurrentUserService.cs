using MongoDB.Bson;
using System.Security.Claims;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
}