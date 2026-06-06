using System;
using System.Collections.Generic;
using Nautilus.Crafting;

namespace SeaglideUpgrade.Upgrades;

internal static class UpgradeRegistry
{
    private static readonly string[] MainPath = { "Tools", "SeaglideTab" };
    private static readonly List<SeaglideUpgradeModule> Modules = new List<SeaglideUpgradeModule>();

    private static bool registered;

    public static void RegisterAll()
    {
        if (registered)
        {
            return;
        }

        BuildDefinitions();

        foreach (SeaglideUpgradeModule module in Modules)
        {
            module.Register();
        }

        registered = true;
    }

    private static void BuildDefinitions()
    {
        Modules.Clear();

        SeaglideUpgradeModule speedMk1 = SpeedModule(1, 1.15f, () => Ingredients(
            Item(TechType.Lubricant, 1),
            Item(TechType.WiringKit, 1)));

        SeaglideUpgradeModule speedMk2 = SpeedModule(2, 1.30f, () => Ingredients(
            Item(TechType.Lubricant, 2),
            Item(TechType.AdvancedWiringKit, 1),
            Item(speedMk1.TechType, 1)));

        SeaglideUpgradeModule speedMk3 = SpeedModule(3, UpgradeData.MaximumSpeedMultiplier, () => Ingredients(
            Item(TechType.Lubricant, 1),
            Item(TechType.AdvancedWiringKit, 1),
            Item(TechType.Battery, 1),
            Item(speedMk2.TechType, 1)));

        Modules.Add(speedMk1);
        Modules.Add(speedMk2);
        Modules.Add(speedMk3);

        SeaglideUpgradeModule efficiencyMk1 = EfficiencyModule(1, 0.20f, () => Ingredients(
            Item(TechType.Battery, 1),
            Item(TechType.WiringKit, 1)));

        SeaglideUpgradeModule efficiencyMk2 = EfficiencyModule(2, 0.40f, () => Ingredients(
            Item(TechType.ComputerChip, 1),
            Item(TechType.WiringKit, 1),
            Item(efficiencyMk1.TechType, 1)));

        SeaglideUpgradeModule efficiencyMk3 = EfficiencyModule(3, UpgradeData.MaximumEfficiencyBonus, () => Ingredients(
            Item(TechType.ComputerChip, 1),
            Item(TechType.AdvancedWiringKit, 1),
            Item(efficiencyMk2.TechType, 1)));

        Modules.Add(efficiencyMk1);
        Modules.Add(efficiencyMk2);
        Modules.Add(efficiencyMk3);
    }

    private static SeaglideUpgradeModule SpeedModule(
        int mk,
        float speedMultiplier,
        Func<IReadOnlyList<Ingredient>> ingredientsFactory)
    {
        int percent = (int)Math.Round((speedMultiplier - 1f) * 100f);
        return new SeaglideUpgradeModule(
            $"SeaglideSpeedUpgradeMk{mk}",
            $"Seaglide Speed Upgrade Module Mk {mk}",
            $"Mk {mk} speed module for the Seaglide. Increases swimming speed by {percent} percent while equipped.",
            mk,
            new UpgradeData(speedMultiplier),
            SeaglideUpgradePlugin.SpeedIcons[mk],
            MainPath,
            ingredientsFactory);
    }

    private static SeaglideUpgradeModule EfficiencyModule(
        int mk,
        float efficiencyBonus,
        Func<IReadOnlyList<Ingredient>> ingredientsFactory)
    {
        int percent = (int)Math.Round(efficiencyBonus * 100f);
        return new SeaglideUpgradeModule(
            $"SeaglideEfficiencyUpgradeMk{mk}",
            $"Seaglide Efficiency Upgrade Module Mk {mk}",
            $"Mk {mk} efficiency module for the Seaglide. Reduces Seaglide energy use by {percent} percent while equipped.",
            mk,
            new UpgradeData(1f, efficiencyBonus),
            SeaglideUpgradePlugin.EfficiencyIcons[mk],
            MainPath,
            ingredientsFactory);
    }

    private static IReadOnlyList<Ingredient> Ingredients(params Ingredient[] ingredients)
    {
        return new List<Ingredient>(ingredients);
    }

    private static Ingredient Item(TechType techType, int amount)
    {
        return new Ingredient(techType, amount);
    }
}
