﻿using ProjectM;
using HarmonyLib;
using OpenRPG.Utils;
using OpenRPG.Systems;
using OpenRPG.Configuration;
using Bloodstone.API;

namespace OpenRPG.Hooks
{
    [HarmonyPatch(typeof(UnitSpawnerReactSystem), nameof(UnitSpawnerReactSystem.OnUpdate))]
    public static class UnitSpawnerReactSystem_Patch
    {
        public static bool listen = false;
        public static void Prefix(UnitSpawnerReactSystem __instance)
        {
            //if (__instance.__OnUpdate_LambdaJob0_entityQuery != null)

            var entities = __instance.__OnUpdate_LambdaJob0_entityQuery.ToEntityArray(Unity.Collections.Allocator.Temp);
            foreach (var entity in entities)
            {
                if (!__instance.EntityManager.HasComponent<LifeTime>(entity)) return;

                var Duration = __instance.EntityManager.GetComponentData<LifeTime>(entity).Duration;
                if (Duration == HunterHuntedSystem.ambush_despawn_timer)
                {
                    var Faction = __instance.EntityManager.GetComponentData<FactionReference>(entity);
                    Faction.FactionGuid.ApplyModification(Helper.SGM, entity, entity, ModificationType.Set, new PrefabGUID(2120169232));
                    __instance.EntityManager.SetComponentData(entity, Faction);
                }

                if (listen)
                {
                    if (Cache.spawnNPC_Listen.TryGetValue(Duration, out var Content))
                    {
                        Content.EntityIndex = entity.Index;
                        Content.EntityVersion = entity.Version;
                        if (Content.Options.Process) Content.Process = true;

                        Cache.spawnNPC_Listen[Duration] = Content;
                        listen = false;
                    }
                }
                if(RandomEncountersConfig.Enabled.Value && Plugin.initServer)
                {
                    RandomEncountersSystem.ServerEvents_OnUnitSpawned(VWorld.Server, entity);
                }
                
            }

        }
    }
}
