// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Azure.Storage.Queues;
using CoreWCF.Channels;
using Xunit;

namespace CoreWCF.AzureQueueStorage.Tests.Helpers
{
    public class QueueDeclareConfigurationFixture
    {
        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;QueueEndpoint=https://127.0.0.1:10001/devstoreaccount1;";
        private readonly string _queueName = "TestQueue";

        public AzureQueueStorageBinding azureQueueStorageBinding;
        public QueueClient queueClient;
        public AzureQueueStorageQueueNameConverter azureQueueStorageQueueNameConverter;

        public QueueDeclareConfigurationFixture()
        {
            azureQueueStorageBinding = new AzureQueueStorageBinding();
            queueClient = AzureQueueStorageConnectionSettings.GetQueueClientFromConnectionString(_connectionString, _queueName);
        }
    }

    public class AzureStorageQueueTests : IClassFixture<QueueDeclareConfigurationFixture>
    {
        private QueueDeclareConfigurationFixture _fixture;

        public AzureStorageQueueTests(QueueDeclareConfigurationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AzureQueueStorageBinding_MessageEncoding_IsText()
        {
            Assert.Equal(AzureQueueStorageMessageEncoding.Text, _fixture.azureQueueStorageBinding.MessageEncoding);
        }

        [Fact]
        public void AzureQueueStorageBinding_Scheme()
        {
            Assert.Equal("soap.aqs", _fixture.azureQueueStorageBinding.Scheme);
        }

        [Fact]
        public void AzureQueueStorageBinding_MessageSize()
        {
            Assert.Equal(8000L, _fixture.azureQueueStorageBinding.MaxMessageSize);
        }

        [Fact]
        public void AzureQueueStorageConnectionSettings_GetQueueClientFromConnectionString_AccountName()
        {
            Assert.Equal("devstoreaccount1", _fixture.queueClient.AccountName);
        }

        [Fact]
        public void AzureQueueStorageConnectionSettings_GetQueueClientFromConnectionString_QueueName()
        {
            Assert.Equal("TestQueue", _fixture.queueClient.Name);
        }

        [Fact]
        public void AzureQueueStorageQueueNameConverter_GetEndpointUrl()
        {
            Assert.Equal("net.aqs://account.queue.core.windows.net/Queue", AzureQueueStorageQueueNameConverter.GetEndpointUrl("account", "Queue");
        }
    }
}
