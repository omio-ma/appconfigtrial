using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace appconfigtrial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] FunkySummaries = new[]
        {
           "Cats and dogs", "blimey", "windy", "wowza", "burning", "freezing"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _config;
        private readonly IFeatureManagerSnapshot _featureManager;
        private readonly FeatureTogglesResponse _response;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config, IFeatureManagerSnapshot featureManager, IOptions<FeatureTogglesResponse> response)
        {
            _logger = logger;
            _config = config;
            _featureManager = featureManager;
            _response = response.Value;
        }

        //[HttpGet]
        //public async Task<IEnumerable<WeatherForecast>> Get()
        //{
        //    var rng = new Random();
        //    var isEnabledFeature = await _featureManager.IsEnabledAsync("funky");
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = isEnabledFeature
        //            ? FunkySummaries[rng.Next(FunkySummaries.Length)]
        //            : Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("funky")]
        [FeatureGate("funky_summary")]
        public IEnumerable<WeatherForecast> GetWithFunkySummary()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = FunkySummaries[rng.Next(FunkySummaries.Length)]
                })
                .ToArray();
        }

        [HttpGet]
        [Route("featuretoggle")]
        public async Task<ActionResult<FeatureTogglesResponse>> GetFeatureToggle(string featureToggleName = "")
        {
            //var featureToggles = _featureManager.GetFeatureNamesAsync().GetAsyncEnumerator();
            //while (await featureToggles.MoveNextAsync())
            //{
            //    Console.Write(featureToggles.Current + " ");
            //}

            return Ok(_featureManager.IsEnabledAsync(featureToggleName).GetAwaiter().GetResult());
        }

        [HttpGet]
        [Route("featuretoggles")]
        public async Task<ActionResult<FeatureTogglesResponse>> GetFeatureToggles()
        {
            //var config = _config.GetSection("FeatureManagement").GetChildren().ToList();
            return Ok(_response);
        }

    }

    public class FeatureTogglesResponse
    {
        public bool FunkySummary { get; set; }
        public bool AnotherFeature { get; set; }
    }
}
