using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Wolnik.Client.Models;
using Wolnik.Client.Services;
using Wolnik.Model;

namespace Wolnik.Client.Controllers
{
    public class SensorController : Controller
    {
        private ISensorDataHttpClient _client;

        public SensorController(ISensorDataHttpClient client)
        {
            _client = client;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<IActionResult> FetchData()
        {
            var client = await _client.GetClientAsync();

            var response = await client.GetAsync("api/sensors");

            return await HandleApiResponse(response, async () =>
            {
                var imagesAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var sensorData = JsonConvert.DeserializeObject<IEnumerable<SensorData>>(imagesAsString)
                    .ToList();

                return View(sensorData);
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IActionResult> HandleApiResponse(HttpResponseMessage response, Func<Task<IActionResult>> onSuccess)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                {
                    return await onSuccess();
                }
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return RedirectToAction("AccessDenied", "Authorization");
                default:
                    throw new Exception($"A problem happened while calling the API: {response.ReasonPhrase}");
            }
        }
    }
}
