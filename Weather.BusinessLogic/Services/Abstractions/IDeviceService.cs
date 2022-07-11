using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.BusinessLogic.Models;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;

namespace Weather.BusinessLogic.Services.Abstractions
{
    public interface IDeviceService
    {
        Task<IResult<DeviceMeasurement>> GetMeasurements(string deviceId, DateTime date, string sensorType);
        Task<IResult<List<DeviceMeasurement>>> GetDeviceMeasurements(string deviceId, DateTime date);
    }
}
