using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Weather.BusinessLogic.Models;
using Weather.BusinessLogic.Services.Abstractions;
using Weather.Infrastructure;
using Weather.Infrastructure.Models.Result.Abstractions.Generics;
using Weather.Infrastructure.Models.Result.Implementations.Generics;

namespace Weather.BusinessLogic.Services.Implementations
{
    public class DeviceService : IDeviceService
    {
        private readonly IBlobContainerClientFactory _blobContainerClient;
        private readonly IFileReaderService<Device> _deviceReader;
        private readonly IFileReaderService<Measurement> _measurementReader;

        public DeviceService(IBlobContainerClientFactory blobContainerClient, IFileReaderService<Device> deviceReader, IFileReaderService<Measurement> measurementReader)
        {
            _blobContainerClient = blobContainerClient;
            _measurementReader = measurementReader;
            _deviceReader = deviceReader;
        }

        public async Task<IResult<DeviceMeasurement>> GetMeasurements(string deviceId, DateTime date, string sensorType)
        {
            var result = await GetMeasurementsByType(deviceId, date, sensorType);

            return result.Success
                ? Result<DeviceMeasurement>.CreateSuccess(new DeviceMeasurement() { Device = new Device() { Id = deviceId, Type = sensorType }, Measurements = result.Data.ToList() })
                : Result<DeviceMeasurement>.CreateFailed(result.ErrorInfo.Error);
        }

        public async Task<IResult<List<DeviceMeasurement>>> GetDeviceMeasurements(string deviceId, DateTime date)
        {
            var devices = await GetDevice();

            var tasks = devices.Data.Where(x => x.Id.Equals(deviceId)).Select(x => GetMeasurements(x.Id, date, x.Type)).ToList();

            var taskResults = await Task.WhenAll(tasks);

            var data = taskResults.Where(x => x.Success).Select(x => x.Data).ToList();

            return data.Any()
                ? Result<List<DeviceMeasurement>>.CreateSuccess(data)
                : Result<List<DeviceMeasurement>>.CreateFailed("No data found");
        }

        private async Task<IResult<List<Device>>> GetDevice()
        {
            var path = GetBlobDevicePath();

            var blobStream = await _blobContainerClient.DownloadBlobStream(path);

            if (!blobStream.Success)
                return Result<List<Device>>.CreateFailed(blobStream.ErrorInfo.Error);

            var result = await _deviceReader.ReadFile(blobStream.Data);

            return result.Success
                ? Result<List<Device>>.CreateSuccess(result.Data)
                : Result<List<Device>>.CreateFailed(result.ErrorInfo.Error);
        }

        private async Task<IResult<List<Measurement>>> GetMeasurementsByType(string deviceId, DateTime date, string sensorType)
        {
            try
            {
                var path = GetBlobPath(deviceId, sensorType, date);
                var blobStream = await _blobContainerClient.DownloadBlobStream(path);

                if (!blobStream.Success)
                    return await UnzipFile(deviceId, date, sensorType);

                var result = await _measurementReader.ReadFile(blobStream.Data);

                return result.Success
                    ? Result<List<Measurement>>.CreateSuccess(result.Data)
                    : Result<List<Measurement>>.CreateFailed(result.ErrorInfo.Error);
            }
            catch (Exception e)
            {
                return Result<List<Measurement>>.CreateFailed(e.Message);
            }
        }

        private async Task<IResult<List<Measurement>>> UnzipFile(string deviceId, DateTime date, string sensorType)
        {
            try
            {
                var archivePath = GetBlobArchivePath(deviceId, sensorType);
                var blobStream = await _blobContainerClient.DownloadBlobStream(archivePath);

                if (!blobStream.Success)
                    return Result<List<Measurement>>.CreateFailed(blobStream.ErrorInfo.Error);

                var historicalPath = GetBlobHistoricalPath(date);

                using var zip = new ZipArchive(blobStream.Data);
                var file = zip.Entries.FirstOrDefault(x => x.Name.Equals(historicalPath));

                if (file is null)
                    return Result<List<Measurement>>.CreateFailed("There is no data found.");

                var result = await _measurementReader.ReadFile(file.Open());

                return result.Success
                    ? Result<List<Measurement>>.CreateSuccess(result.Data)
                    : Result<List<Measurement>>.CreateFailed(result.ErrorInfo.Error);
            }
            catch (Exception e)
            {
                return Result<List<Measurement>>.CreateFailed(e.Message);
            }
        }

        private string GetBlobPath(string deviceId, string sensorType, DateTime date) => $"{deviceId}/{sensorType}/{date:yyyy-MM-dd}.csv";
        private string GetBlobArchivePath(string deviceId, string sensorType) => $"{deviceId}/{sensorType}/historical.zip";
        private string GetBlobHistoricalPath(DateTime date) => $"{date:yyyy-MM-dd}.csv";
        private string GetBlobDevicePath() => "metadata.csv";
    }
}
