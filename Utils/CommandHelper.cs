using ProjectM;
using ProjectM.Network;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Wetstone.API;

namespace ChatCommands.Utils
{
    public static class CommandHelper
    {
        public static PrefabGUID GetGUIDFromName(string name)
        {
            var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();
            var managed = gameDataSystem.ManagedDataRegistry;

            foreach (var entry in gameDataSystem.ItemHashLookupMap)
            {
                try
                {
                    var item = managed.GetOrDefault<ManagedItemData>(entry.Key);
                    if (item.PrefabName.StartsWith("Item_VBloodSource") || item.PrefabName.StartsWith("GM_Unit_Creature_Base") || item.PrefabName == "Item_Cloak_ShadowPriest") continue;
                    if (item.Name.ToString().ToLower() == name.ToLower())
                    {
                        return entry.Key;
                    }
                }
                catch { }
            }

            return new PrefabGUID(0);
        }

        public static Entity AddItemToInventory(Context ctx, PrefabGUID guid, int amount)
        {
            unsafe
            {
                var gameData = VWorld.Server.GetExistingSystem<GameDataSystem>();
                var bytes = stackalloc byte[Marshal.SizeOf<FakeNull>()];
                var bytePtr = new IntPtr(bytes);
                Marshal.StructureToPtr<FakeNull>(new()
                {
                    value = 7,
                    has_value = true
                }, bytePtr, false);
                var boxedBytePtr = IntPtr.Subtract(bytePtr, 0x10);
                var hack = new Il2CppSystem.Nullable<int>(boxedBytePtr);
                var hasAdded = InventoryUtilitiesServer.TryAddItem(ctx.EntityManager, gameData.ItemHashLookupMap, ctx.Event.SenderCharacterEntity, guid, amount, out _, out Entity e, default, hack);
                return e;
            }
        }

        public static BloodType GetBloodTypeFromName(string name)
        {
            BloodType type = BloodType.Frailed;
            if (Enum.IsDefined(typeof(BloodType), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)))
                Enum.TryParse(name, true, out type);
            return type;
        }

        public static void Spawn(Context ctx, string name, float2 position)
        {
            try
            {
                var bufferSystem = VWorld.Server.GetExistingSystem<EntityCommandBufferSystem>();
                var buffer = new EntityCommandBufferSafe(Unity.Collections.Allocator.Temp)
                {
                    Unsafe = bufferSystem.CreateCommandBuffer()
                };

                var prefabCollectionSystem = VWorld.Server.GetExistingSystem<PrefabCollectionSystem>();
                foreach (var kv in prefabCollectionSystem._PrefabGuidToNameMap)
                {
                    if (kv.Value.ToString() == name)
                    {
                        ctx.Event.User.SendSystemMessage($"Attempting to spawn: {name}...");
                        foreach (var pkv in prefabCollectionSystem._PrefabGuidToEntityMap)
                        {
                            if (pkv.Key == kv.Key)
                            {
                                var prefabEntity = pkv.Value;
                                ctx.Event.User.SendSystemMessage($"Found: {name} as {pkv.Value} - {pkv.Key}...");
                                Entity spawnedEntity = buffer.Instantiate(prefabEntity);

                                var translation = VWorld.Server.EntityManager.GetComponentData<Translation>(ctx.Event.SenderUserEntity);

                                var f3pos = new float3(position.x, translation.Value.y, position.y);

                                ctx.Event.User.SendSystemMessage($"Spawning unit: {name} <{f3pos.x},{f3pos.y}, {f3pos.z}> (command from <{translation.Value.x},{translation.Value.y},{translation.Value.z}>)");
                                buffer.SetComponent(spawnedEntity, new Translation() { Value = f3pos });
                                ctx.Event.User.SendSystemMessage($"Spawning Complete: {name}");

                                return;
                            }
                        }
                    }
                }
                ctx.Event.User.SendSystemMessage("Could not find specified unit: " + name);
                return;
            }
            catch (Exception e)
            {
                ctx.Event.User.SendSystemMessage($"ERROR: {e.Message}");
            }
        }

        public static void TeleportTo(Context ctx, float2 position)
        {
            var entity = ctx.EntityManager.CreateEntity(
                    ComponentType.ReadWrite<FromCharacter>(),
                    ComponentType.ReadWrite<PlayerTeleportDebugEvent>()
                );

            ctx.EntityManager.SetComponentData<FromCharacter>(entity, new()
            {
                User = ctx.Event.SenderUserEntity,
                Character = ctx.Event.SenderCharacterEntity
            });

            ctx.EntityManager.SetComponentData<PlayerTeleportDebugEvent>(entity, new()
            {
                Position = position,
                Target = PlayerTeleportDebugEvent.TeleportTarget.Self
            });
        }

        struct FakeNull
        {
            public int value;
            public bool has_value;
        }

        public enum BloodType
        {
            Frailed = -899826404,
            Creature = -77658840,
            Warrior = -1094467405,
            Rogue = 793735874,
            Brute = 581377887,
            Scholar = -586506765,
            Worker = -540707191
        }
    }
}
