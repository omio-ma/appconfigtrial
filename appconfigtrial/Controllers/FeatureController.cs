using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;
using System.Threading.Tasks;

namespace appconfigtrial.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeatureController : ControllerBase
    {
        private IFeatureManagerSnapshot _featureManager;

        public FeatureController(ILogger<FeatureController> logger, IConfiguration config, IFeatureManagerSnapshot featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet]
        [Route("GetFeatureOneDependantData")]
        [FeatureGate(nameof(Features.FeatureOne))]
        public ActionResult GetFeatureOneDependantData()
        {
            return Ok(new { Data = "Hello. If you see me then Feature One is active." });
        }

        [HttpGet]
        [Route("GetFeature/{featureToggleName}")]
        public async Task<ActionResult<bool>> GetFeatureToggle(string featureToggleName = "")
        {
            return Ok(await _featureManager.IsEnabledAsync(featureToggleName));
        }
    }

    public class Features
    {
        public bool FeatureOne { get; set; }
        public bool FeatureTwo { get; set; }
    }
}
