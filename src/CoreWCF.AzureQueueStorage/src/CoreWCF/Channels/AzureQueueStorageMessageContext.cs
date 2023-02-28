// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using CoreWCF.Queue.Common;

namespace CoreWCF.Channels
{
    internal class AzureQueueStorageMessageContext : QueueMessageContext
    {
        private IDictionary<string, object> _properties = new Dictionary<string, object>();

        public override IDictionary<string, object> Properties
        {
            get { return _properties; }
        }

        public void SetProperties(IDictionary<string, object> properties)
        {
            _properties = properties;
        }
    }
}
