using System;
using System.Collections.Generic;
using System.Text;
using Wetstone.API;

namespace ChatCommands.Utils
{
    public static class CommandOutput
    {
        public static void InvalidArguments(Context ctx)
        {
            ctx.Event.User.SendSystemMessage($"<color=#ff0000ff>Invalid command parameters. Check {ctx.Prefix}help for more information.</color>");
        }

        public static void MissingArguments(Context ctx)
        {
            ctx.Event.User.SendSystemMessage($"<color=#ff0000ff>Missing command parameters. Check {ctx.Prefix}help for more information.</color>");
        }
    }
}
