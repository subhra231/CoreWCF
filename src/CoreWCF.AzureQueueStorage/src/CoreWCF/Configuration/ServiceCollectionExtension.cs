// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Storage.Queues;
using CoreWCF.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace CoreWCF.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceModelAzureQueueStorageSupport(this IServiceCollection services)
        {
            services.AddSingleton<QueueClient, QueueClient>();
            return services;
        }
    }
}
