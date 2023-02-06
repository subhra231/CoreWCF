// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Contracts;
using CoreWCF.Channels;
using CoreWCF.Configuration;
using CoreWCF.Queue.Common.Configuration;
using CoreWCF.RabbitMQ.Tests.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace CoreWCF.RabbitMQ.Tests
{
    public class QueueDeclareConfigurationFixture
    {
        public AzureQueueStorageBinding azureQueueStorageBinding;


        public QueueDeclareConfigurationFixture()
        {
            azureQueueStorageBinding = new AzureQueueStorageBinding();

        }
    }

    public class QueueDeclareConfigurationTests : IClassFixture<QueueDeclareConfigurationFixture>
    {

    }
}
