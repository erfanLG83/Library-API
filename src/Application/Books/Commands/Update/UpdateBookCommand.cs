﻿using Domain.Entities.BookAggregate;
using Domain.Entities.BookAggregate.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Books.Commands.Update;

public class UpdateBookCommand : IRequest<Result>
{
    public required string Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string PublicationDate { get; set; }
    public required BookLanguage Language { get; set; }
    public string? Interpreters { get; set; }
    public required List<BookInBranch> BookInBranches { get; set; }
    public IFormFile? NewImage { get; set; }

    public required string AuthorId { get; set; }
    public required string PublisherId { get; set; }
    public required string CategoryId { get; set; }
}
