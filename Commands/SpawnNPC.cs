using ChatCommands.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Unity.Transforms;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("spawnnpc, spn", "spawnnpc <Prefab Name> [<Waypoint>]", "Spawns a NPC to a previously created waypoint.")]
    public static class SpawnNPC
    {
        public static List<WaypointMarker> waypoints;

        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length != 0)
            {
                string name = "";
                string waypoint = "";

                if (ctx.Args.Length == 1)
                {
                    name = string.Join(' ', ctx.Args);
                    var pos = ctx.EntityManager.GetComponentData<LocalToWorld>(ctx.Event.SenderCharacterEntity).Position;
                    CommandHelper.SpawnAtPosition(ctx, name, new(pos.x, pos.z));
                }
                else
                {
                    name = string.Join(' ', ctx.Args.SkipLast(1));
                    waypoint = ctx.Args.Last();

                    LoadWaypoints();

                    if (waypoints.Any(x => x.Name.ToLower() == waypoint.ToLower()))
                    {
                        var wp = waypoints.First(x => x.Name.ToLower() == waypoint.ToLower());
                        CommandHelper.SpawnAtPosition(ctx, name, new(wp.X, wp.Y));
                    }
                    else
                    {
                        CommandOutput.CustomErrorMessage(ctx, "This waypoint doesn't exist.");
                    }
                }
            }
            else
            {
                CommandOutput.MissingArguments(ctx);
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