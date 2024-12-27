namespace Application.Categories;

public static class CategoriesErrors
{
    public static readonly Error CategoryNotFound = new("دسته بندی وجود ندارد", "category_not_found");
    public static readonly Error CategoryHasBookAndCanNotDelete = new("دسته بندی دارای کتاب هست و امکان حذف اش وجود ندارد", "category_has_book_and_can_not_delete");
}
