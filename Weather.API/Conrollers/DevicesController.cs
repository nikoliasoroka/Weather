using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Weather.API.Extensions;
using Weather.BusinessLogic.Models;
using Weather.BusinessLogic.Services.Abstractions;
using Weather.Infrastructure.Models.Result.Implementations;

namespace Weather.API.Conrollers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        /// <summary>
        /// Get the measurements for device by deviceId and date and sensor type
        /// </summary>
        /// <param name="deviceId">Device identifier</param>
        /// <param name="date">Requested measurements date</param>
        /// <param name="sensorType">Requested sensor type of device</param>
        /// <returns>Measurements for device by sensor type and date</returns>
        [HttpGet("{deviceId}/data/{date}/{sensorType}")]
        [ProducesResponseType(typeof(DeviceMeasurement), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetData(string deviceId, DateTime date, string sensorType)
        {
            var result = await _deviceService.GetMeasurements(deviceId, date, sensorType);

            return result.ToActionResult();
        }

        /// <summary>
        /// Get all measurements for device by deviceId and date
        /// </summary>
        /// <param name="deviceId">Device identifier</param>
        /// <param name="date">Requested measurements date</param>
        /// <returns>List of measurements for device by sensor type and date</returns>
        [HttpGet("{deviceId}/data/{date}")]
        [ProducesResponseType(typeof(List<DeviceMeasurement>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorInfo), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDataForDevice(string deviceId, DateTime date)
        {
            var result = await _deviceService.GetDeviceMeasurements(deviceId, date);

            return result.ToActionResult();
        }
    }
}
