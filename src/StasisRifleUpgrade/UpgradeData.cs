using System.Collections.Generic;
using UnityEngine;
using UpgradesLIB;

namespace StasisRifleUpgrade;

internal class UpgradeData
{
    public static readonly Dictionary<TechType, UpgradeData> UpgradeDataDict = new Dictionary<TechType, UpgradeData>();

    public float DamageRawAdd { get; }
    public float StasisDuration { get; }

    public static ModOptions Options { get; set; } = null!;

    public UpgradeData(float damageAdd = 0f, float stasisDur = 0f)
    {
        DamageRawAdd = damageAdd;
        StasisDuration = stasisDur;
    }

    public static float CalculateDamage(StasisRifle instance)
    {
        float damage = 0f;

        foreach (UpgradeData upgrade in GetEquippedUpgradeData(instance))
        {
            damage += upgrade.DamageRawAdd;
        }

        return damage * (Options?.DamageMultiplier ?? 1f);
    }

    public static float CalculateDuration(StasisRifle instance)
    {
        float durationMultiplier = 1f;

        foreach (UpgradeData upgrade in GetEquippedUpgradeData(instance))
        {
            durationMultiplier += upgrade.StasisDuration;
        }

        return durationMultiplier * (Options?.DurationMultiplier ?? 1f);
    }

    private static IEnumerable<UpgradeData> GetEquippedUpgradeData(StasisRifle instance)
    {
        if ((Object)(object)instance == (Object)null)
        {
            yield break;
        }

        ModdedUpgradeConsoleInput panel = Utilities.GetPanel(
            ((Component)instance).gameObject,
            StasisRifleUpgradePlugin.StorageName,
            StasisRifleUpgradePlugin.StorageClassId);

        if ((Object)(object)panel == (Object)null || panel.equipment == null)
        {
            yield break;
        }

        if (!DataTypes.Equipment.TryGetValue(StasisRifleUpgradePlugin.StasisRifleTechType, out string[] slots))
        {
            yield break;
        }

        foreach (string slot in slots)
        {
            InventoryItem item = panel.equipment.GetItemInSlot(slot);

            if (item == null || item.techType == TechType.None)
            {
                continue;
            }

            if (UpgradeDataDict.TryGetValue(item.techType, out UpgradeData upgrade))
            {
                yield return upgrade;
            }
        }
    }
}
