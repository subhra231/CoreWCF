// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace CoreWCF.Channels
{
    internal class DeadLetterQueue : IQueueBase
    {
        private QueueClient _client;

        public DeadLetterQueue(string connectionString, string queueName)
        {
            _client = new QueueClient(connectionString, queueName);
        }

        public QueueClient queueClient { get => _client; set => _client = value; }

        public Response DeleteMessage(string messageId, string popReceipt, CancellationToken cancellationToken = default)
        {
            return _client.DeleteMessage(messageId, popReceipt, cancellationToken);
        }

        public Task<Response<QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = null, CancellationToken cancellationToken = default)
        {
            return _client.ReceiveMessageAsync(visibilityTimeout, cancellationToken);
        }

        public async Task SendMessageAsync(BinaryData binaryData, TimeSpan timeSpan)
        {
            CancellationTokenSource cts = new(timeSpan);

            try
            {
                await _client.SendMessageAsync(binaryData, default, default, cts.Token).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw AzureQueueStorageChannelHelpers.ConvertTransferException(e);
            }
            finally
            {
                cts.Dispose();
            }
        }
    }
}
