// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace CoreWCF.RabbitMQ.Tests
{
    public static class ServiceHelper
    {
        public static IWebHostBuilder CreateWebHostBuilder<TStartup>(ITestOutputHelper outputHelper)
            where TStartup : class
        {
            return WebHost.CreateDefaultBuilder(Array.Empty<string>())
#if DEBUG
                .ConfigureLogging((ILoggingBuilder logging) =>
                {
                    logging.AddProvider(new XunitLoggerProvider(outputHelper));
                    logging.AddFilter("Default", LogLevel.Debug);
                    logging.AddFilter("Microsoft", LogLevel.Debug);
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
#endif // DEBUG
                .UseStartup<TStartup>();
        }
    }
}
