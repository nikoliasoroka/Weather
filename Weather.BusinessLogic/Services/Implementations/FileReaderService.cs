using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Weather.BusinessLogic.Models.Interfaces;
using Weather.BusinessLogic.Services.Abstractions;
using Weather.Infrastructure.Models;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;
using Weather.Infrastructure.Models.Result.Implementations.Generics;

namespace Weather.BusinessLogic.Services.Implementations
{
    public class FileReaderService<T> : IFileReaderService<T> where T : IFileReader, new()
    {
        private AppSettings Settings { get; }

        public FileReaderService(IOptions<AppSettings> option)
        {
            Settings = option.Value;
        }

        public async Task<IResult<List<T>>> ReadFile(Stream stream)
        {
            try
            {
                var result = new List<T>();

                using var sr = new StreamReader(stream);
                string? line;

                while ((line = await sr.ReadLineAsync()) != null)
                {
                    var values = line.Split(Settings.CsvDelimiter);
                    var data = new T();
                    data.Populate(values[0], values[1]);
                    result.Add(data);
                }

                return Result<List<T>>.CreateSuccess(result);
            }
            catch (Exception e)
            {
                return Result<List<T>>.CreateFailed(e.Message);
            }
        }
    }
}
