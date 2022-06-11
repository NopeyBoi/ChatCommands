using ChatCommands.Utils;
using ProjectM;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("speed", Usage = "speed <value|reset>", Description = "Sets your movement speed")]
    public static class Speed
    {
        public static void Initialize(Context ctx)
        {
            if (ctx.Args.Length != 0)
            {
                var component = ctx.EntityManager.GetComponentData<Movement>(ctx.Event.SenderCharacterEntity);
                if (float.TryParse(ctx.Args[0], out float speed))
                {
                    component.Speed = ModifiableFloat.Create(ctx.Event.SenderCharacterEntity, ctx.EntityManager, speed);
                    ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
                    ctx.Event.User.SendSystemMessage($"Set speed to <color=#ffff00ff>{float.Parse(ctx.Args[0])}</color>");
                }
                else if (ctx.Args[0].ToLower() == "reset")
                {
                    component.Speed = ModifiableFloat.Create(ctx.Event.SenderCharacterEntity, ctx.EntityManager, (float)4.4);
                    ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
                    ctx.Event.User.SendSystemMessage($"Set speed to <color=#ffff00ff>4.4 (default)</color>");
                }
                else
                {
                    CommandOutput.InvalidArguments(ctx);
                }
            }
            else
            {
                CommandOutput.MissingArguments(ctx);
            }
        }
    }
}
