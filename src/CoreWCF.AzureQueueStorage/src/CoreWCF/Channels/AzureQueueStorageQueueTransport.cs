// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using CoreWCF.Configuration;
using CoreWCF.Queue;
using CoreWCF.Queue.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoreWCF.Channels
{
    internal class AzureQueueStorageQueueTransport : IQueueTransport
    {
        private MessageQueue _queueClient;
        private DeadLetterQueue _deadLetterQueueClient;
        private TimeSpan _queueReceiveTimeOut;
        private TimeSpan _receiveMessagevisibilityTimeout;
        private ILogger<AzureQueueStorageQueueTransport> _logger;
        private Uri _baseAddress;

        public AzureQueueStorageQueueTransport(IServiceDispatcher serviceDispatcher, IServiceProvider serviceProvider)
        {
            _queueClient = serviceProvider.GetRequiredService<MessageQueue>();
            _deadLetterQueueClient = serviceProvider.GetRequiredService<DeadLetterQueue>();
            _queueReceiveTimeOut = serviceDispatcher.Binding.ReceiveTimeout;
            _logger = serviceProvider.GetRequiredService<ILogger<AzureQueueStorageQueueTransport>>();
            _baseAddress = serviceDispatcher.BaseAddress;
        }

        public int ConcurrencyLevel => 1;

        public async ValueTask<QueueMessageContext> ReceiveQueueMessageContextAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Receiving message from Azure queue storage");

            QueueMessage queueMessage = await _queueClient.ReceiveMessageAsync(_receiveMessagevisibilityTimeout, cancellationToken);
            if(queueMessage == null)
            {
                return null;
            }
            _queueClient.DeleteMessage(queueMessage.MessageId, queueMessage.PopReceipt);
            var reader = PipeReader.Create(new ReadOnlySequence<byte>(queueMessage.Body.ToMemory()));
            return GetContext(reader, new EndpointAddress(_baseAddress));
        }

        private QueueMessageContext GetContext(PipeReader reader, EndpointAddress endpointAddress)
        {
            var context = new QueueMessageContext
            {
                QueueMessageReader = reader,
                LocalAddress = endpointAddress,
                DispatchResultHandler = NotifyError,
            };
            return context;
        }

        private async Task NotifyError(QueueDispatchResult dispatchResult, QueueMessageContext context)
        {
            if (dispatchResult == QueueDispatchResult.Failed)
            {
                //send message to dead letter queue
                await _deadLetterQueueClient.SendMessageAsync(context.RequestMessage);
                
            }
        }
    }
}
