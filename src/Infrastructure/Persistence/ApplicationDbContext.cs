using Application.Common.Interfaces;
using Domain.Entities.AuthorAggregate;
using Domain.Entities.BookAggregate;
using Domain.Entities.CategoryAggregate;
using Domain.Entities.UserAggregate;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IApplicationDbContext
{
    private readonly MongoClient _client;
    private readonly IMongoDatabase _database;
    public ApplicationDbContext(IConfiguration configuration)
    {
        var mongoSection = configuration.GetRequiredSection("ConnectionStrings")
            .GetRequiredSection("Mongo");
        var settings = new MongoClientSettings()
        {
            AllowInsecureTls = true,
            ApplicationName = "LibraryApi",
            ConnectTimeout = TimeSpan.FromSeconds(30),
            Server = new MongoServerAddress(mongoSection["Ip"]!, int.Parse(mongoSection["Port"]!)),
            Credential = MongoCredential.CreateCredential("admin", mongoSection["Username"], mongoSection["Password"])
        };

        _client = new MongoClient(settings);
        _database = _client.GetDatabase(mongoSection["DatabaseName"]);
    }

    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    public IQueryable<User> UsersQuery => Users.AsQueryable();

    public IMongoCollection<Book> Books => _database.GetCollection<Book>("Books");
    public IQueryable<Book> BooksQuery => Books.AsQueryable();

    public IMongoCollection<Author> Authors => _database.GetCollection<Author>("Authors");
    public IQueryable<Author> AuthorsQuery => Authors.AsQueryable();

    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    public IQueryable<Category> CategoriesQuery => Categories.AsQueryable();

    public IMongoCollection<BorrowedBook> BorrowedBooks => _database.GetCollection<BorrowedBook>("BorrowedBookssers");
    public IQueryable<BorrowedBook> BorrowedBooksQuery => BorrowedBooks.AsQueryable();
}