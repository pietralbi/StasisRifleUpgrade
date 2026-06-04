using HarmonyLib;
using UnityEngine;
using UpgradesLIB;

namespace StasisRifleUpgrade.Patches;

[HarmonyPatch(typeof(StasisRifle))]
internal class StasisRiflePatches
{
    internal class StasisRifleUpdater : MonoBehaviour
    {
        private StasisRifle rifle;

        private void Awake()
        {
            rifle = GetComponent<StasisRifle>();
        }

        private void Update()
        {
            if ((Object)(object)rifle == (Object)null || !IsHeldRifle())
            {
                return;
            }

            if (!GameInput.GetButtonDown(StasisRifleUpgradePlugin.OpenUpgradesButton))
            {
                return;
            }

            if (Player.main == null
                || !Player.main.IsFreeToInteract()
                || ((uGUI_InputGroup)uGUI.main.craftingMenu).selected)
            {
                return;
            }

            ModdedUpgradeConsoleInput panel = Utilities.GetPanel(
                gameObject,
                StasisRifleUpgradePlugin.StorageName,
                StasisRifleUpgradePlugin.StorageClassId);

            if ((Object)(object)panel == (Object)null)
            {
                StasisRifleUpgradePlugin.Log.LogWarning("Stasis rifle upgrade panel was not found.");
                return;
            }

            panel.OpenPDA();
        }

        private bool IsHeldRifle()
        {
            PlayerTool heldTool = Inventory.main?.GetHeldTool();
            return ReferenceEquals(heldTool, rifle);
        }
    }

    [HarmonyPatch(nameof(StasisRifle.Awake))]
    [HarmonyPostfix]
    public static void AwakePostfix(StasisRifle __instance)
    {
        if ((Object)(object)__instance == (Object)null)
        {
            return;
        }

        if ((Object)(object)((Component)__instance).GetComponent<StasisRifleUpdater>() == (Object)null)
        {
            ((Component)__instance).gameObject.AddComponent<StasisRifleUpdater>();
        }
    }
}
