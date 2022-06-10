using ChatCommands.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("help, h", Usage = "help", Description = "Shows a list of commands")]
    public static class Help
    {
        public static void Initialize(Context ctx)
        {
            List<string> commands = new List<string>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0).ToArray();
            try
            {
                if (types.Any(x => x.GetAttributeValue((CommandAttribute cmd) => cmd.Aliases.First() == ctx.Args[0].ToLower())))
                {
                    var type = types.First(x => x.GetAttributeValue((CommandAttribute cmd) => cmd.Aliases.First() == ctx.Args[0].ToLower()));

                    List<string> aliases = type.GetAttributeValue((CommandAttribute cmd) => cmd.Aliases);
                    if (ctx.DisabledCommands.Any(x => x.ToLower() == aliases.First().ToLower())) return;
                    string usage = type.GetAttributeValue((CommandAttribute cmd) => cmd.Usage);
                    string description = type.GetAttributeValue((CommandAttribute cmd) => cmd.Description);
                    bool adminOnly = type.GetAttributeValue((CommandAttribute cmd) => cmd.AdminOnly);

                    ctx.Event.User.SendSystemMessage($"Help for <color=#00ff00ff>{ctx.Prefix}{aliases.First()}</color>");
                    ctx.Event.User.SendSystemMessage($"<color=#ffffffff>Aliases: {string.Join(", ", aliases)}</color>");
                    if (adminOnly) ctx.Event.User.SendSystemMessage($"<color=#ffffffff>Description: <color=#ff0000ff>[ADMIN]</color> {description}</color>");
                    else ctx.Event.User.SendSystemMessage($"<color=#ffffffff>Description: {description}</color>");
                    ctx.Event.User.SendSystemMessage($"<color=#ffffffff>Usage: {ctx.Prefix}{usage}</color>");
                }
            }
            catch
            {
                ctx.Event.User.SendSystemMessage("List of all commands:");
                foreach (Type type in types)
                {
                    List<string> aliases = type.GetAttributeValue((CommandAttribute cmd) => cmd.Aliases);
                    if (ctx.DisabledCommands.Any(x => x.ToLower() == aliases.First().ToLower())) continue;
                    string description = type.GetAttributeValue((CommandAttribute cmd) => cmd.Description);
                    bool adminOnly = type.GetAttributeValue((CommandAttribute cmd) => cmd.AdminOnly);

                    string s = "";
                    if (adminOnly) s = $"<color=#00ff00ff>{ctx.Prefix}{aliases.First()}</color> - <color=#ff0000ff>[ADMIN]</color> <color=#ffffffff>{description}</color>";
                    else s = $"<color=#00ff00ff>{ctx.Prefix}{aliases.First()}</color> - <color=#ffffffff>{description}</color>";
                    ctx.Event.User.SendSystemMessage(s);
                }
            }
        }
    }
}
