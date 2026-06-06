using System.Collections.Generic;
using UnityEngine;
using UpgradesLIB;
using Object = UnityEngine.Object;

namespace SeaglideUpgrade;

internal sealed class UpgradeData
{
    public const float MaximumSpeedMultiplier = 1.45f;
    public const float MaximumEfficiencyBonus = 0.60f;
    public const float MinimumEffectScalar = 0.25f;
    public const float MaximumEffectScalar = 1f;

    private const float BaseEnergyCost = 0.1f;
    private const float MinimumEnergyCost = BaseEnergyCost * (1f - MaximumEfficiencyBonus);

    public static readonly Dictionary<TechType, UpgradeData> UpgradeDataByTechType = new Dictionary<TechType, UpgradeData>();

    public static ModOptions Options { get; set; }

    public float SpeedMultiplier { get; }
    public float EfficiencyBonus { get; }

    public UpgradeData(float speedMultiplier = 1f, float efficiencyBonus = 0f)
    {
        SpeedMultiplier = speedMultiplier;
        EfficiencyBonus = efficiencyBonus;
    }

    public static float CalculateSpeedMultiplier(Seaglide instance)
    {
        float effectScalar = ClampEffectScalar(Options?.SpeedEffectMultiplier ?? MaximumEffectScalar);
        float strongestMultiplier = 1f;

        foreach (UpgradeData upgrade in GetEquippedUpgradeData(instance))
        {
            strongestMultiplier = Mathf.Max(
                strongestMultiplier,
                ScaleSpeedMultiplier(upgrade.SpeedMultiplier, effectScalar));
        }

        return strongestMultiplier;
    }

    public static float CalculateEnergyCost(Seaglide instance)
    {
        float effectScalar = ClampEffectScalar(Options?.EfficiencyEffectMultiplier ?? MaximumEffectScalar);
        float strongestBonus = 0f;

        foreach (UpgradeData upgrade in GetEquippedUpgradeData(instance))
        {
            strongestBonus = Mathf.Max(
                strongestBonus,
                ScaleEfficiencyBonus(upgrade.EfficiencyBonus, effectScalar));
        }

        return Mathf.Clamp(BaseEnergyCost * (1f - strongestBonus), MinimumEnergyCost, BaseEnergyCost);
    }

    private static float ClampEffectScalar(float value)
    {
        return Mathf.Clamp(value, MinimumEffectScalar, MaximumEffectScalar);
    }

    private static float ScaleSpeedMultiplier(float speedMultiplier, float effectScalar)
    {
        float scaledMultiplier = 1f + ((speedMultiplier - 1f) * effectScalar);
        return Mathf.Clamp(scaledMultiplier, 1f, MaximumSpeedMultiplier);
    }

    private static float ScaleEfficiencyBonus(float efficiencyBonus, float effectScalar)
    {
        return Mathf.Clamp(efficiencyBonus * effectScalar, 0f, MaximumEfficiencyBonus);
    }

    private static IEnumerable<UpgradeData> GetEquippedUpgradeData(Seaglide instance)
    {
        if ((Object)(object)instance == (Object)null)
        {
            yield break;
        }

        ModdedUpgradeConsoleInput panel = Utilities.GetPanel(
            ((Component)instance).gameObject,
            SeaglideUpgradePlugin.StorageName,
            SeaglideUpgradePlugin.StorageClassId);

        if ((Object)(object)panel == (Object)null || panel.equipment == null)
        {
            yield break;
        }

        if (!DataTypes.Equipment.TryGetValue(SeaglideUpgradePlugin.SeaglideTechType, out string[] slots))
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

            if (UpgradeDataByTechType.TryGetValue(item.techType, out UpgradeData upgrade))
            {
                yield return upgrade;
            }
        }
    }
}
