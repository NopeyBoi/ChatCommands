using ChatCommands.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("spawnatwp, spwp", "spawnatwp <Prefab Name> <Waypoint>", "Spawns a NPC to a previously created waypoint.", AdminOnly = true)]
    public static class Spawn
    {
        public static List<WaypointMarker> waypoints;

        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length < 2)
            {
                ctx.Event.User.SendSystemMessage("Missing parameters.");
                return;
            }

            LoadWaypoints();

            string name = string.Join(' ', ctx.Args.SkipLast(1));
            string waypoint = ctx.Args.Last();

            if (waypoints.Any(x => x.Name.ToLower() == waypoint.ToLower()))
            {
                var wp = waypoints.First(x => x.Name.ToLower() == waypoint.ToLower());
                CommandHelper.SpawnAtWaypoint(ctx, name, new(wp.X, wp.Y));
            }
            else
            {
                ctx.Event.User.SendSystemMessage("This waypoint doesn't exist.");
            }
        }

        public static void LoadWaypoints()
        {
            if (!File.Exists("BepInEx/config/ChatCommands/waypoints.json")) File.Create("BepInEx/config/ChatCommands/waypoints.json");
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
    }
}