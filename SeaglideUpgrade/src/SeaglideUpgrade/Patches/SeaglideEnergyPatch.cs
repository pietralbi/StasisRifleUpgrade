using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace SeaglideUpgrade.Patches;

[HarmonyPatch(typeof(Seaglide))]
[HarmonyPatch(nameof(Seaglide.UpdateEnergy))]
internal static class SeaglideEnergyPatch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = instructions.ToList();
        bool patched = false;

        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Ldc_R4 && codes[i].operand is float value && Math.Abs(value - 0.1f) < 0.0001f)
            {
                codes[i] = new CodeInstruction(OpCodes.Ldarg_0);
                codes.Insert(i + 1, new CodeInstruction(
                    OpCodes.Call,
                    AccessTools.Method(typeof(UpgradeData), nameof(UpgradeData.CalculateEnergyCost), new[] { typeof(Seaglide) })));
                patched = true;
                break;
            }
        }

        if (!patched)
        {
            SeaglideUpgradePlugin.Log.LogWarning("Could not find Seaglide.UpdateEnergy base energy cost; efficiency modules will not affect energy use.");
        }

        return codes;
    }
}
