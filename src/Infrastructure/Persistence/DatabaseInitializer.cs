using Application.Common.Interfaces;
using Application.Common.Settings;
using Application.Common.Utilities;
using Domain.Entities.UserAggregate;
using Domain.Entities.UserAggregate.Enums;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Persistence;

public class DatabaseInitializer
{
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IApplicationDbContext _dbContext;
    private readonly AdminData _adminData;

    public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IApplicationDbContext dbContext, AdminData adminData)
    {
        _logger = logger;
        _dbContext = dbContext;
        _adminData = adminData;
    }

    public virtual async Task SeedDataAsync()
    {
        // every seed method should save changes independently
        await SeedAdminUserAsync();
    }

    private async Task SeedAdminUserAsync()
    {
        try
        {
            if (await _dbContext.UsersQuery.AnyAsync() == false)
            {
                var hashedPassword = PasswordHash.HashPassword(_adminData.Password);
                var adminUser = new User
                {
                    PasswordHash = hashedPassword,
                    FirstName = _adminData.FirstName,
                    LastName = _adminData.LastName,
                    PhoneNumber = _adminData.PhoneNumber,
                    Role = UserRole.SuperAdmin,

                };

                await _dbContext.Users.InsertOneAsync(adminUser);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to seed admin user and admin role. details: {exceptionMessage}", ex.Message);
            throw;
        }
    }
}