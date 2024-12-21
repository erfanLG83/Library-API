using Domain.Entities.AuthorAggregate;
using Domain.Entities.BookAggregate;
using Domain.Entities.CategoryAggregate;
using Domain.Entities.UserAggregate;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    public IMongoCollection<User> Users { get; }
    public IQueryable<User> UsersQuery { get; }

    public IMongoCollection<Book> Books { get; }
    public IQueryable<Book> BooksQuery { get; }

    public IMongoCollection<Author> Authors { get; }
    public IQueryable<Author> AuthorsQuery { get; }

    public IMongoCollection<Category> Categories { get; }
    public IQueryable<Category> CategoriesQuery { get; }

    public IMongoCollection<BorrowedBook> BorrowedBooks { get; }
    public IQueryable<BorrowedBook> BorrowedBooksQuery { get; }
}