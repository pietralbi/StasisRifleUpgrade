using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace StasisRifleUpgrade;

[Menu("Stasis Rifle Upgrades")]
public class ModOptions : ConfigFile
{
    [Slider("Damage Multiplier", 0.2f, 10f, DefaultValue = 1f, Step = 0.1f)]
    public float DamageMultiplier = 1f;

    [Slider("Duration Multiplier", 0.2f, 10f, DefaultValue = 1f, Step = 0.1f)]
    public float DurationMultiplier = 1f;
}
