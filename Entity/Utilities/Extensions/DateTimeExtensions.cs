using Entity.Base.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Base.Entity.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string FormatWithBaseSettings(this DateTime dateTime) => dateTime.ToString(EntityBase._entityBaseDiscordConfiguration.DateTimeFormat);
    }
}