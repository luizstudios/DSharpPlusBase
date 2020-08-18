using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlusBase.Core.Settings
{
    public sealed class BotBaseConfiguration
    {
        public bool AutoReconnect { get; set; } = true;
        public bool ConnectAfterInitialize { get; set; } = true;
    }
}