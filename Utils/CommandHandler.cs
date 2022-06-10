using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using Wetstone.API;
using Wetstone.Hooks;

namespace ChatCommands.Utils
{
    public class CommandHandler
    {
        public string Prefix { get; set; }
        public string DisabledCommands { get; set; }

        public CommandHandler(string prefix, string disabledCommands)
        {
            this.Prefix = prefix;
            this.DisabledCommands = disabledCommands;
        }

        public void HandleCommands(VChatEvent ev, ManualLogSource Log, ConfigFile config)
        {
            if (!ev.Message.StartsWith(Prefix)) return;
            if (!VWorld.IsServer) return;

            string[] args = { };
            if (ev.Message.Contains(' '))
                args = ev.Message.Split(' ').Skip(1).ToArray();

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0).ToArray();
            foreach (Type type in types)
            {
                ev.Cancel();

                string command = ev.Message.Split(' ')[0].Remove(0, 1);
                if (DisabledCommands.Split(',').Any(x => x.ToLower() == command.ToLower())) continue;
                if (!NameExists(type, command)) continue;
                if (IsNotAdmin(type, ev))
                {
                    ev.User.SendSystemMessage($"You do not have the required permissions to use that.");
                    return;
                }

                var cmd = type.GetMethod("Initialize");
                cmd.Invoke(null, new[] { new Context(Prefix, ev, Log, config, args, DisabledCommands) });

                Log.LogInfo($"[CommandHandler] {ev.User.CharacterName} used command: {command.ToLower()}");
            }
        }

        private static bool NameExists(Type type, string command)
        {
            List<string> aliases = type.GetAttributeValue((CommandAttribute cmd) => cmd.Aliases);
            if (aliases.Any(x => x.ToLower() == command.ToLower())) return true;
            return false;
        }

        private static bool IsNotAdmin(Type type, VChatEvent ev)
        {
            return type.GetAttributeValue((CommandAttribute cmd) => cmd.AdminOnly) && !ev.User.IsAdmin;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public List<string> Aliases;

        public string Name { get; set; }
        public string Usage { get; set; }
        public string Description { get; set; }
        public bool AdminOnly { get; set; }

        public CommandAttribute(string name, string usage = "", string description = "None", bool adminOnly = false)
        {
            this.Name = name;
            this.Usage = usage;
            this.Description = description;
            this.AdminOnly = adminOnly;

            this.Aliases = new List<string>();
            Aliases.AddRange(this.Name.ToLower().Split(", "));
        }
    }

    public class Context
    {
        public string Prefix { get; set; }
        public VChatEvent Event { get; set; }
        public ManualLogSource Log { get; set; }
        public string[] Args { get; set; }
        public ConfigFile Config { get; set; }
        public EntityManager EntityManager { get; set; }

        public string[] DisabledCommands;

        public Context(string prefix, VChatEvent ev, ManualLogSource log, ConfigFile config, string[] args, string disabledCommands)
        {
            this.Prefix = prefix;
            this.Event = ev;
            this.Log = log;
            this.Args = args;
            this.Config = config;

            EntityManager = VWorld.Server.EntityManager;
            DisabledCommands = disabledCommands.Split(',');
        }
    }

    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }
    }
}
