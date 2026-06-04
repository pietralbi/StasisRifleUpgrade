using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StasisRifleUpgrade.Patches;

[HarmonyPatch(typeof(StasisSphere))]
internal class StasisSpherePatch
{
    private static readonly ConditionalWeakTable<StasisSphere, HashSet<LiveMixin>> DamagedTargets = new ConditionalWeakTable<StasisSphere, HashSet<LiveMixin>>();
    private static readonly FieldInfo TimeField = typeof(StasisSphere).GetField("time", BindingFlags.Instance | BindingFlags.NonPublic);

    [HarmonyPatch("Shoot")]
    [HarmonyPostfix]
    public static void ShootPostfix(StasisSphere __instance)
    {
        try
        {
            if (!(Inventory.main?.GetHeldTool() is StasisRifle rifle))
            {
                return;
            }

            if (TimeField == null)
            {
                StasisRifleUpgradePlugin.Log.LogError("Could not find private StasisSphere.time field.");
                return;
            }

            float baseTime = (float)TimeField.GetValue(__instance);
            float upgradedTime = baseTime * UpgradeData.CalculateDuration(rifle);
            TimeField.SetValue(__instance, upgradedTime);

            StasisRifleUpgradePlugin.Log.LogDebug($"Stasis sphere time updated to {upgradedTime}.");
        }
        catch (Exception ex)
        {
            StasisRifleUpgradePlugin.Log.LogError($"Error in StasisSphere Shoot postfix: {ex}");
        }
    }

    [HarmonyPatch("Freeze")]
    [HarmonyPostfix]
    private static void FreezePostfix(StasisSphere __instance, Collider other, ref Rigidbody target, bool __result)
    {
        try
        {
            if (!__result || (Object)(object)target == (Object)null)
            {
                return;
            }

            LiveMixin liveMixin = ((Component)target).GetComponentInParent<LiveMixin>();
            if ((Object)(object)liveMixin == (Object)null
                || (Object)(object)((Component)target).GetComponentInParent<Vehicle>() != (Object)null)
            {
                return;
            }

            if (!(Inventory.main?.GetHeldTool() is StasisRifle rifle))
            {
                return;
            }

            float damage = UpgradeData.CalculateDamage(rifle);
            if (damage <= 0f)
            {
                return;
            }

            HashSet<LiveMixin> damagedTargets = DamagedTargets.GetOrCreateValue(__instance);
            if (damagedTargets.Add(liveMixin))
            {
                liveMixin.TakeDamage(damage, target.position, (DamageType)8, null);
            }
        }
        catch (Exception ex)
        {
            StasisRifleUpgradePlugin.Log.LogError($"Error in StasisSphere Freeze postfix: {ex}");
        }
    }
}
