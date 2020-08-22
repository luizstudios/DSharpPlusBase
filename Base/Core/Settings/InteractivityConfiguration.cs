using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using System;

namespace DiscordBotBase.Core.Settings
{
    public sealed class InteractivityConfiguration
    {
        public PaginationBehaviour PaginationBehaviour { get; set; } = PaginationBehaviour.WrapAround;
        public PaginationDeletion PaginationDeletion { get; set; } = PaginationDeletion.DeleteEmojis;
        public PaginationEmojis PaginationEmojis { get; set; } = null;
        public PollBehaviour PollBehaviour { get; set; } = PollBehaviour.DeleteEmojis;
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);
    }
}