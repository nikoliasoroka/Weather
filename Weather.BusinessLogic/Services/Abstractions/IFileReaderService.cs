using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Weather.BusinessLogic.Models.Interfaces;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;

namespace Weather.BusinessLogic.Services.Abstractions
{
    public interface IFileReaderService<T> where T : IFileReader, new()
    {
        Task<IResult<List<T>>> ReadFile(Stream stream);
    }
}
