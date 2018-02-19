using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolnik.Model;

namespace Wolnik.API.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    public class SensorsController : Controller {
        [HttpGet]
        public IEnumerable<SensorData> Get() {
            return new List<SensorData>
            {
                new SensorData {SensorId = "1", Name = "L1", Type = "temperature", Value = "23.1"},
                new SensorData {SensorId = "2", Name = "L2", Type = "temperature", Value = "23.2"},
                new SensorData {SensorId = "3", Name = "L3", Type = "temperature", Value = "23.5"},
                new SensorData {SensorId = "4", Name = "H", Type = "humidity", Value = "40"},
                new SensorData {SensorId = "5", Name = "Out", Type = "temperature", Value = "12"},
                new SensorData {SensorId = "6", Name = "In", Type = "temperature", Value = "23.5"},
            };

        }

    }
}
