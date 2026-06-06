using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace SeaglideUpgrade;

[Menu("Seaglide Upgrade")]
public class ModOptions : ConfigFile
{
    [Slider("Speed Effect", UpgradeData.MinimumEffectScalar, UpgradeData.MaximumEffectScalar, DefaultValue = UpgradeData.MaximumEffectScalar, Step = 0.05f)]
    public float SpeedEffectMultiplier = 1f;

    [Slider("Efficiency Effect", UpgradeData.MinimumEffectScalar, UpgradeData.MaximumEffectScalar, DefaultValue = UpgradeData.MaximumEffectScalar, Step = 0.05f)]
    public float EfficiencyEffectMultiplier = 1f;
}
