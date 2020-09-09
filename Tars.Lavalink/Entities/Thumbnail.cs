using System;
using System.Collections.Generic;
using System.Text;

namespace Tars.Lavalink.Entities
{
    /// <summary>
    /// Enum that indicates the qualities of the thumbnail versions of youtube.
    /// </summary>
    public enum Thumbnail
    {
        /// <summary>
        /// Thumbnail with standard youtube quality.
        /// </summary>
        Default,

        /// <summary>
        /// Thumbnail with low resolution quality within YouTube standards.
        /// </summary>
        SdDefault,

        /// <summary>
        /// Thumbnail with average resolution quality within YouTube standards.
        /// </summary>
        MqDefault,

        /// <summary>
        /// Thumbnail with maximum resolution quality within YouTube standards.
        /// </summary>
        HqDefault,

        /// <summary>
        /// Thumbnail with the highest possible quality.
        /// </summary>
        MaxResDefault
    }
}