// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Contracts;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.AzureQueueStorage.Tests.Helpers;
using CoreWCF.Queue.Common.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace CoreWCF.AzureQueueStorage.Tests
{
    public class IntegrationTests
    {
        private readonly ITestOutputHelper _output;
        public const string QueueName = "AzureQueue";


        [Fact(Skip = "Need AzureQueue")]
        public void ReceiveMessage_ServiceCall_Success()
        {
            IWebHost host = ServiceHelper.CreateWebHostBuilder<Startup>(_output).Build();
            using (host)
            {
                host.Start();
                MessageQueueHelper.SendMessageInQueue(QueueName);

                var resolver = new DependencyResolverHelper(host);
                var testService = resolver.GetService<TestService>();
                Assert.True(testService.ManualResetEvent.Wait(System.TimeSpan.FromSeconds(5)));
            }
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TestService>();
            services.AddServiceModelServices();
            services.AddQueueTransport(x => { x.ConcurrencyLevel = 1; });
            services.AddServiceModelAzureQueueStorageSupport();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseServiceModel(services =>
            {
                services.AddService<TestService>();
                services.AddServiceEndpoint<TestService, ITestContract>(new AzureQueueStorageBinding(),
                    $"net.aqs://localhost/private/{IntegrationTests.QueueName}");
            });
        }
    }
}
