﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using CoreWCF.Configuration;
using CoreWCF.Queue.Common;
using CoreWCF.Queue.Common.Configuration;

namespace CoreWCF.Channels
{
    public class AzureQueueStorageTransportBindingElement : QueueBaseTransportBindingElement
    {
        private long _maxReceivedMessageSize;
        private TimeSpan _receiveMessagevisibilityTimeout = TransportDefaults.ReceiveMessagevisibilityTimeout;

        /// <summary>
        /// Creates a new instance of the AzureQueueStorageTransportBindingElement Class using the default protocol.
        /// </summary>
        public AzureQueueStorageTransportBindingElement()
        {
        }

        protected AzureQueueStorageTransportBindingElement(AzureQueueStorageTransportBindingElement other) : base(other) 
        {
            MaxReceivedMessageSize = other.MaxReceivedMessageSize;
        }

        public override BindingElement Clone()
        {
            return new AzureQueueStorageTransportBindingElement();
        }


        public override T GetProperty<T>(BindingContext context)
        {
            if (context == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(context));
            }

            return base.GetProperty<T>(context);
        }

        public override QueueTransportPump BuildQueueTransportPump(BindingContext context)
        {
            IQueueTransport queueTransport = CreateMyQueueTransport(context);
            return QueueTransportPump.CreateDefaultPump(queueTransport);
        }

        private IQueueTransport CreateMyQueueTransport(BindingContext context)
        {
            var serviceDispatcher = context.BindingParameters.Find<IServiceDispatcher>();
            var serviceProvider = context.BindingParameters.Find<IServiceProvider>();
            return new AzureQueueStorageQueueTransport(serviceDispatcher, serviceProvider, this);
        }

        /// <summary>
        /// Gets the scheme used by the binding, soap.amqp
        /// </summary>
        public override string Scheme
        {
            get { return CurrentVersion.Scheme; }
        }

        /// <summary>
        /// The largest receivable encoded message
        /// </summary>
        public override long MaxReceivedMessageSize
        {
            get { return _maxReceivedMessageSize;  }
            set
            {
                if (value < 0 || value > 8000L)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxReceivedMessageSize), "MaxReceivedMessageSize must not be more than 64K.");
                }
                _maxReceivedMessageSize = value;
            }
        }

        public TimeSpan MaxReceivedTimeout
        {
            get { return _receiveMessagevisibilityTimeout; }
            set { _receiveMessagevisibilityTimeout = value; }
        }
    }
}
