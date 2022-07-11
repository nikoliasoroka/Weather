using Microsoft.AspNetCore.Mvc;
using Weather.Infrastructure.Models.Result.Abstractions;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;

namespace Weather.API.Extensions
{
    public static class ActionResultExtension
    {
        public static IActionResult ToActionResult<T>(this IResult<T> result) =>
            result.Success
                ? (IActionResult)new OkObjectResult(result.Data)
                : new BadRequestObjectResult(result.ErrorInfo);

        public static IActionResult ToActionResult(this IResult result) =>
            result.Success
                ? (IActionResult)new OkResult()
                : new BadRequestObjectResult(result.ErrorInfo);
    }
}
