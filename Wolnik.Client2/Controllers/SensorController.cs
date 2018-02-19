using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Wolnik.Client2.Models;
using Wolnik.Client2.Services;
using Wolnik.Model;

namespace Wolnik.Client2.Controllers {
    [Authorize]
    public class SensorController : Controller {
        private ISensorDataHttpClient _client;

        public SensorController(ISensorDataHttpClient client) {
            _client = client;
        }
        public async Task<IActionResult> Index() {
            var idToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            return View(new TokensViewModel { IdToken = idToken, AccessToken = accessToken });
        }

        public IActionResult About() {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact() {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<IActionResult> FetchData() {
            var client = await _client.GetClientAsync();

            var response = await client.GetAsync("api/sensors");

            return await HandleApiResponse(response, async () => {
                var jsonContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var sensorData = JsonConvert.DeserializeObject<IEnumerable<SensorData>>(jsonContent)
                    .ToList();

                return View(sensorData);
            });
        }

        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<IActionResult> HandleApiResponse(HttpResponseMessage response, Func<Task<IActionResult>> onSuccess) {
            switch (response.StatusCode) {
                case HttpStatusCode.OK: {
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
