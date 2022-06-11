using ChatCommands.Utils;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("health, hp", Usage = "health <value>", Description = "Sets your current Health")]
    public static class Health
    {
        public static void Initialize(Context ctx)
        {
            var component = ctx.EntityManager.GetComponentData<ProjectM.Health>(ctx.Event.SenderCharacterEntity);
            if (ctx.Args.Length != 0)
            {
                if (float.Parse(ctx.Args[0]) <= 0)
                {
                    CommandOutput.InvalidArguments(ctx);
                }
                if (float.Parse(ctx.Args[0]) > component.MaxHealth.Value) component.Value = component.MaxHealth.Value;
                else component.Value = float.Parse(ctx.Args[0]);
            }
            else component.Value = component.MaxHealth.Value;
            component.MaxRecoveryHealth = component.Value;
            ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
            ctx.Event.User.SendSystemMessage($"Set Health to <color=#ffff00ff>{component.Value}</color>");
        }
    }
}
