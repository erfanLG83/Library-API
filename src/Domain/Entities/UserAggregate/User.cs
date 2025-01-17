﻿using Domain.Common;
using Domain.Entities.BookAggregate;
using Domain.Entities.UserAggregate.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.UserAggregate;

public class User : Entity
{
    public User()
    {
    }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required UserRole Role { get; set; }
    public string? PasswordHash { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    [BsonIgnore]
    public List<BorrowedBook>? BorrowedBooks { get; set; }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}