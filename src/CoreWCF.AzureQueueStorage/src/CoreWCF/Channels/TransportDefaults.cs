// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace CoreWCF.Channels
{
    internal class TransportDefaults
    {
        //max size of Azure Queue message can be upto 64KB
        internal const long DefaultMaxMessageSize = 8000L;

        internal static readonly TimeSpan ReceiveMessagevisibilityTimeout = TimeSpan.FromMinutes(15);
    }
}
