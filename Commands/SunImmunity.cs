using ChatCommands.Utils;
using ProjectM;
using Wetstone.API;

namespace ChatCommands.Commands
{
    [Command("sunimmunity, sun", Usage = "sunimmunity", Description = "Makes you immune to the sun")]
    public static class SunImmunity
    {
        public static void Initialize(Context ctx)
        {
            var component = ctx.EntityManager.GetComponentData<TakeDamageInSun>(ctx.Event.SenderCharacterEntity);
            if (component.SunDamageDisabled) component.ImmuneToSun.Value = false;
            else component.SunDamageDisabled = true;
            ctx.EntityManager.SetComponentData(ctx.Event.SenderCharacterEntity, component);
            string s = component.ImmuneToSun.Value ? "Activated" : "Deactivated";
            ctx.Event.User.SendSystemMessage($"Sun Immunity <color=#ffff00ff>{s}</color>");
        }
    }
}
