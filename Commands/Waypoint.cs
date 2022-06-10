using ChatCommands.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Unity.Transforms;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("waypoint, wp", "waypoint <Name|Set|Remove> [<Name>] [global]", "Teleports you to previously created waypoints.")]
    public static class Waypoint
    {
        public static List<WaypointMarker> waypoints;

        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length < 1)
            {
                ctx.Event.User.SendSystemMessage("Missing parameters.");
                return;
            }

            LoadWaypoints();

            if (ctx.Args.Length > 1)
            {
                bool global = false;
                if (ctx.Args.Length > 2 && ctx.Args[2].ToLower() == "global" && ctx.Event.User.IsAdmin) global = true;
                if (ctx.Args[0].ToLower() == "set")
                {
                    if (waypoints.Count(x => x.User == ctx.Event.User.CharacterName.ToString()) >= (int)ctx.Config["Config", "Waypoint Limit"].BoxedValue)
                    {
                        ctx.Event.User.SendSystemMessage("You have reached your waypoint limit already.");
                        return;
                    }
                        
                    if (AddWaypoint(ctx, ctx.Args[1], global)) ctx.Event.User.SendSystemMessage("Successfully added Waypoint.");
                    else ctx.Event.User.SendSystemMessage("Waypoint already exists or you don't have permission to add that waypoint.");
                }
                if (ctx.Args[0].ToLower() == "remove")
                {
                    if (RemoveWaypoint(ctx, ctx.Args[1])) ctx.Event.User.SendSystemMessage("Successfully removed Waypoint.");
                    else ctx.Event.User.SendSystemMessage("Waypoint doesn't exist or you don't have permission to remove that waypoint.");
                }
                return;
            }

            if (ctx.Args[0].ToLower() == "list")
            {
                ctx.Event.User.SendSystemMessage("List of all accessible Waypoints:");
                foreach (WaypointMarker wp in waypoints)
                {
                    if (wp.IsGlobal || wp.User == ctx.Event.User.CharacterName.ToString())
                    {
                        string g = wp.IsGlobal ? "Global" : "Local";
                        string gColor = wp.IsGlobal ? "#00dd00ff" : "#ffff00ff";
                        ctx.Event.User.SendSystemMessage($"<color=#ffff00ff>{wp.Name}</color> - by <color=#ffff00ff>{wp.User}</color> - <color={gColor}>{g}</color>");
                    }
                }
                return;
            }

            if (waypoints.Any(x => x.Name.ToLower() == ctx.Args[0].ToLower()))
            {
                var wp = waypoints.First(x => x.Name.ToLower() == ctx.Args[0].ToLower());
                if (!wp.IsGlobal && wp.User != ctx.Event.User.CharacterName.ToString())
                {
                    ctx.Event.User.SendSystemMessage("You are not allowed to use this waypoint!");
                    return;
                }
                CommandHelper.TeleportTo(ctx, new(wp.X, wp.Y));
            }
            else
            {
                ctx.Event.User.SendSystemMessage("This waypoint doesn't exist.");
            }
        }

        public static bool AddWaypoint(Context ctx, string name, bool isGlobal = false)
        {
            if (isGlobal && !ctx.Event.User.IsAdmin) return false;
            if (waypoints.Any(x => x.Name.ToLower() == name.ToLower())) return false;
            var component = ctx.EntityManager.GetComponentData<LocalToWorld>(ctx.Event.SenderCharacterEntity);
            waypoints.Add(new WaypointMarker(name, component.Position.x, component.Position.z, ctx.Event.User.CharacterName.ToString(), isGlobal));
            SaveWaypoints();
            return true;
        }

        public static bool RemoveWaypoint(Context ctx, string name)
        {
            if (waypoints.Any(x => x.Name.ToLower() == name.ToLower()))
            {
                var wp = waypoints.First(x => x.Name.ToLower() == name.ToLower());
                if (wp.User != ctx.Event.User.CharacterName.ToString()) return false;
                if (wp.IsGlobal && !ctx.Event.User.IsAdmin) return false;
                waypoints.Remove(wp);
                SaveWaypoints();
                return true;
            }

            return false;
        }

        public static void LoadWaypoints()
        {
            if (!File.Exists("BepInEx/config/ChatCommands/waypoints.json"))File.Create("BepInEx/config/ChatCommands/waypoints.json");
            string json = File.ReadAllText("BepInEx/config/ChatCommands/waypoints.json");
            try
            {
                waypoints = JsonSerializer.Deserialize<List<WaypointMarker>>(json);
            }
            catch
            {
                waypoints = new List<WaypointMarker>();
            }
        }

        public static void SaveWaypoints()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                IncludeFields = true
            };
            File.WriteAllText("BepInEx/config/ChatCommands/waypoints.json", JsonSerializer.Serialize(waypoints, options));
        }
    }
}
