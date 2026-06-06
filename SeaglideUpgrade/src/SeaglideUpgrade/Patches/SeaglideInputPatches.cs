using HarmonyLib;
using UnityEngine;
using UpgradesLIB;
using Object = UnityEngine.Object;

namespace SeaglideUpgrade.Patches;

[HarmonyPatch(typeof(Seaglide))]
internal static class SeaglideInputPatches
{
    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void UpdatePostfix(Seaglide __instance)
    {
        if ((Object)(object)__instance == (Object)null || !IsHeldSeaglide(__instance))
        {
            return;
        }

        if (!GameInput.GetButtonDown(SeaglideUpgradePlugin.OpenUpgradesButton))
        {
            return;
        }

        ModdedUpgradeConsoleInput panel = Utilities.GetPanel(
            ((Component)__instance).gameObject,
            SeaglideUpgradePlugin.StorageName,
            SeaglideUpgradePlugin.StorageClassId);

        if ((Object)(object)panel == (Object)null)
        {
            SeaglideUpgradePlugin.Log.LogWarning("Seaglide upgrade panel was not found.");
            return;
        }

        panel.OpenPDA();
    }

    private static bool IsHeldSeaglide(Seaglide seaglide)
    {
        PlayerTool heldTool = Inventory.main?.GetHeldTool();
        return (Object)(object)heldTool == (Object)(object)seaglide;
    }
}
