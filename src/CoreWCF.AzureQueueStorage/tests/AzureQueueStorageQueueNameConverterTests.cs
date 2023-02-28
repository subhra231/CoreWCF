// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using CoreWCF.Channels;
using Xunit;

namespace CoreWCF.AzureQueueStorage.Tests
{
    public class AzureQueueStorageQueueNameConverterTests
    {
        [Fact]
        public void GetAzureQueueStorageFormatQueueNameTest()
        {
            var uri = new Uri("net.aqs://localhost/private/QueueName");
            string result = AzureQueueStorageQueueNameConverter.GetAzureQueueStorageQueueName(uri);
            Assert.Equal(".\\Private$\\QueueName", result);
        }

        [Fact]
        public void GetEndpointUrlTest()
        {
            string result = AzureQueueStorageQueueNameConverter.GetEndpointUrl("StorageAccountName","QueueName");
            Assert.Equal("net.aqs://localhost/private/StorageAccountName/QueueName", result);
        }
    }
}
