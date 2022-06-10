using ChatCommands.Utils;
using ProjectM;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("blood", Usage = "blood <Type> [<Quality>] [<Value>]", Description = "Sets your current Blood Type, Quality and Value", AdminOnly = true)]
    public static class Blood
    {
        public static void Initialize(Context ctx)
        {
            CommandHelper.BloodType type = CommandHelper.BloodType.Frailed;
            float quality = 100;
            float value = 100;

            if (ctx.Args.Length >= 1)
            {
                type = CommandHelper.GetBloodTypeFromName(ctx.Args[0]);
            }

            if (ctx.Args.Length >= 2)
            {
                quality = float.Parse(ctx.Args[1]);
                if (float.Parse(ctx.Args[1]) < 0) quality = 0;
                if (float.Parse(ctx.Args[1]) > 100) quality = 100;
            }
            if (ctx.Args.Length >= 3) value = float.Parse(ctx.Args[2]);

            var component = ctx.EntityManager.GetComponentData<ProjectM.Blood>(ctx.Event.SenderCharacterEntity);
            component.BloodType = new PrefabGUID((int)type);
            component.Quality = quality;
            component.Value = value;
            if (component.ShowBloodHUD.Value) component.ShowBloodHUD.Value = false;
            ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
            ctx.Event.User.SendSystemMessage($"Changed Blood Type to <color=#ffff00ff>{type}</color> with <color=#ffff00ff>{quality}</color>% quality");
        }
    }
}
