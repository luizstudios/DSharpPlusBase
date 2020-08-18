using System;
using System.Collections.Generic;
using System.Text;

namespace DSharpPlusBase.Exceptions
{
    public sealed class RoleNotFoundException : Exception
    {
        public string RoleName { get; private set; }

        internal RoleNotFoundException(string roleName) : base($"This {roleName} role was not found!") => this.RoleName = roleName;
    }
}