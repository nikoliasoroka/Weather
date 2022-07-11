namespace Weather.Infrastructure.Models.Result.Abstractions.Generics
{
    public interface IResult<out TData> : IResult
    {
        TData Data { get; }
    }
}
