﻿using ChatCommands.Utils;
using ProjectM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("kit", "kit <Name>", "Gives you a previously specified set of items.")]
    public static class Kit
    {
        private static List<ItemKit> kits;

        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length < 1)
            {
                ctx.Event.User.SendSystemMessage($"Kit name missing.");
                return;
            }

            LoadKits();
            string name = string.Join(' ', ctx.Args);

            try
            {
                ItemKit kit = kits.First(x => x.Name.ToLower() == name.ToLower());
                foreach (var guid in kit.PrefabGUIDs)
                {
                    CommandHelper.AddItemToInventory(ctx, new PrefabGUID(guid.Key), guid.Value);
                }
                ctx.Event.User.SendSystemMessage($"You got the kit: <color=#ffff00ff>{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)}</color>");
            }
            catch
            {
                ctx.Event.User.SendSystemMessage($"Kit doesn't exist.");
                return;
            }
        }

        public static void LoadKits()
        {
            if (!File.Exists("BepInEx/config/ChatCommands/kits.json")) File.Create("BepInEx/config/ChatCommands/kits.json");
            string json = File.ReadAllText("BepInEx/config/ChatCommands/kits.json");
            try
            {
                kits = JsonSerializer.Deserialize<List<ItemKit>>(json);
            }
            catch
            {
                kits = new List<ItemKit>();
            }
        }

        public static void SaveKits()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true
            };
            File.WriteAllText("BepInEx/config/ChatCommands/kits.json", JsonSerializer.Serialize(kits, options));
        }
    }
}
