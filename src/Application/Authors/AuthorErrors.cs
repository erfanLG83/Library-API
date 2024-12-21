namespace Application.Authors;

public static class AuthorErrors
{
    public static readonly Error AuthorNotFound = new("نویسنده وجود ندارد", "author_not_found");
    public static readonly Error AuthorHasBookAndCanNotDelete = new("نویسنده دارای کتاب هست و امکان حذف اش وجود ندارد", "author_has_book_and_can_not_delete");
}
