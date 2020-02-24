﻿using Advert.API.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace Advert.API.HealthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAdvertStorageService _advertStorageService;
        public StorageHealthCheck(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var isStorageOk = await _advertStorageService.CheckHeathAsync();

            return isStorageOk ? HealthCheckResult.Healthy("Storage Is Fine") : HealthCheckResult.Unhealthy("Storage is not working");
        }
    }
}
