namespace Application.Books.Common;

public static class BooksErrors
{
    public static readonly Error BookNotFound = new("کتاب وجود ندارد", "book_not_found");
    public static readonly Error UserAlreadyBorrowedBook = new("کتاب از پیش قرض گرفته شده است", "UserAlreadyBorrowedBook");
    public static readonly Error BookIsNotAvailable = new("کتاب موجود نیست", "BookIsNotAvailable");
}
