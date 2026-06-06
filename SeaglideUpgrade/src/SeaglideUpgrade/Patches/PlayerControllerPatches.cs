using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SeaglideUpgrade.Patches;

[HarmonyPatch(typeof(PlayerController))]
internal static class PlayerControllerPatches
{
    [HarmonyPatch(nameof(PlayerController.SetMotorMode))]
    [HarmonyPostfix]
    public static void SetMotorModePostfix(PlayerController __instance, Player.MotorMode newMotorMode)
    {
        if ((int)newMotorMode != 2 || __instance == null)
        {
            return;
        }

        if (!(Inventory.main?.GetHeldTool() is Seaglide seaglide) || (Object)(object)seaglide == (Object)null)
        {
            return;
        }

        float speedMultiplier = UpgradeData.CalculateSpeedMultiplier(seaglide);
        if (speedMultiplier <= 1f)
        {
            return;
        }

        PlayerMotor motor = __instance.underWaterController;
        motor.forwardMaxSpeed *= speedMultiplier;
        motor.backwardMaxSpeed *= speedMultiplier;
        motor.strafeMaxSpeed *= speedMultiplier;
        motor.verticalMaxSpeed *= speedMultiplier;
        motor.waterAcceleration *= Mathf.Sqrt(speedMultiplier);
    }
}
