using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace HateoasDemo.Models;

public record LinkDto(string Href, string Rel, string Method);

public static class BookHateoasExtensions
{
    public static object ToDto(this Book book, HttpContext httpContext)
    {
        var urlHelperFactory = httpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
        var actionContextAccessor = httpContext.RequestServices.GetRequiredService<IActionContextAccessor>();
        
        var urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext 
            ?? new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()));

        return new
        {
            book.Id,
            book.Title,
            book.Author,
            book.IsAvailable,
            Links = new Dictionary<string, LinkDto?>
            {
                ["self"] = new LinkDto(
                    urlHelper.Link("GetBook", new { id = book.Id }) ?? string.Empty,
                    "self",
                    "GET"
                ),
                ["update"] = new LinkDto(
                    urlHelper.Link("UpdateBook", new { id = book.Id }) ?? string.Empty,
                    "update",
                    "PUT"
                ),
                ["delete"] = new LinkDto(
                    urlHelper.Link("DeleteBook", new { id = book.Id }) ?? string.Empty,
                    "delete",
                    "DELETE"
                ),
                ["borrow"] = book.IsAvailable
                    ? new LinkDto(
                        urlHelper.Link("BorrowBook", new { id = book.Id }) ?? string.Empty,
                        "borrow",
                        "POST"
                    )
                    : null,
                ["return"] = book.IsAvailable
                    ? null
                    : new LinkDto(
                        urlHelper.Link("ReturnBook", new { id = book.Id }) ?? string.Empty,
                        "return",
                        "POST"
                    )
            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
        };
    }
}