using Weather.Infrastructure.Models.Result.Implementations;

namespace Weather.Infrastructure.Models.Result.Abstractions
{
    public interface IResult
    {
        bool Success { get; }

        ErrorInfo ErrorInfo { get; }
    }
}
