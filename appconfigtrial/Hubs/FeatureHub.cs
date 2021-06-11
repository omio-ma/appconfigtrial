using Microsoft.AspNetCore.SignalR;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace appconfigtrial.Hubs
{
    public class FeatureHub : Hub
    {
        private readonly IFeatureManagerSnapshot feature;

        public FeatureHub(IFeatureManagerSnapshot feature)
        {
            this.feature = feature;
        }

        public async Task HealthCheck()
        {
            await Clients.All.SendAsync("FeatureHubHealth", "Feature hub is up and running");
        }

        public async Task CheckFeature(string featureName)
        {
            var isEnabled = await feature.IsEnabledAsync(featureName);
            await Clients.All.SendAsync("CheckFeature", isEnabled);
        }
    }
}
