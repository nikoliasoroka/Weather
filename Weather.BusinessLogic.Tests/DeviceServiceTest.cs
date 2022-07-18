using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weather.API.Conrollers;
using Weather.BusinessLogic.Models;
using Weather.BusinessLogic.Services.Abstractions;
using Weather.Infrastructure.Models.Result.Implementations.Generics;

namespace Weather.BusinessLogic.Tests
{
    [TestFixture]
    public class DeviceServiceTest
    {
        private DevicesController controller;
        private Mock<IDeviceService> _deviceServiceMock;

        private List<Measurement> _measurements;

        [SetUp]
        public void Setup()
        {
            _measurements = new List<Measurement>();
            _deviceServiceMock = new Mock<IDeviceService>();
            controller = new DevicesController(_deviceServiceMock.Object);
        }

        [Test]
        public async Task GetDataTest()
        {
            var data = new DeviceMeasurement() { Device = new Device() { Id = "dockan", Type = "temperature" }, Measurements = _measurements };

            _deviceServiceMock.Setup(x => x.GetMeasurements("dockan", new DateTime(2015, 01, 01), "temperature"))
                .ReturnsAsync(Result<DeviceMeasurement>.CreateSuccess(data));

            var result = await controller.GetData("dockan", new DateTime(2015, 01, 01), "temperature");
            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }
        
        [Test]
        public async Task GetDataForDeviceTest()
        {
            var response = new List<DeviceMeasurement>();
            var device = new DeviceMeasurement() { Device = new Device() { Id = "dockan", Type = "temperature" }, Measurements = _measurements };
            response.Add(device);

            _deviceServiceMock.Setup(x => x.GetDeviceMeasurements("dockan", new DateTime(2015, 01, 01)))
                .ReturnsAsync(Result<List<DeviceMeasurement>>.CreateSuccess(response));

            var result = await controller.GetDataForDevice("dockan", new DateTime(2015, 01, 01));

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IActionResult>(result);
        }
    }
}
