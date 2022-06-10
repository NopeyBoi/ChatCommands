using ChatCommands.Utils;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("health, hp", Usage = "health <value>", Description = "Sets your current Health", AdminOnly = true)]
    public static class Health
    {
        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length != 0)
            {
                if (float.Parse(ctx.Args[0]) <= 0)
                {
                    CommandOutput.InvalidArguments(ctx);
                }
                var component = ctx.EntityManager.GetComponentData<ProjectM.Health>(ctx.Event.SenderCharacterEntity);
                if (float.Parse(ctx.Args[0]) > component.MaxHealth.Value) component.Value = component.MaxHealth.Value;
                else component.Value = float.Parse(ctx.Args[0]);
                ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
                ctx.Event.User.SendSystemMessage($"Set Health to <color=#ffff00ff>{ctx.Args[0]}</color>");
            }
            else
            {
                CommandOutput.MissingArguments(ctx);
            }
        }
    }
}
