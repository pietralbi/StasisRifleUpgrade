using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using StasisRifleUpgrade.Upgrades;
using UnityEngine;
using UnityEngine.InputSystem;
using UpgradesLIB;
using UpgradesLIB.Items.Equipment;

namespace StasisRifleUpgrade;

[BepInPlugin(MyGuid, PluginName, VersionString)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("com.lawabidingmodder.upgradeslib")]
[BepInProcess("Subnautica.exe")]
public class StasisRifleUpgradePlugin : BaseUnityPlugin
{
    private const string MyGuid = "com.digaoness.StasisRifleUpgrade";
    private const string PluginName = "StasisRifleUpgrade";
    private const string VersionString = "1.1.0";

    public const TechType StasisRifleTechType = TechType.StasisRifle;
    public const string EquipmentTypeName = "StasisRifleUpgrade";
    public const string StorageName = "StasisUpgradeStorage";
    public const string StorageClassId = "StasisUpgradeStorageChild";
    public const string PanelLabel = "STASIS RIFLE";

    public static readonly GameInput.Button OpenUpgradesButton =
        (GameInput.Button)EnumHandler.AddEntry<GameInput.Button>("OpenStasisRifleUpgrades")
            .CreateInput("Open Stasis Rifle Upgrades", string.Empty, "English", InputActionType.Button)
            .WithKeyboardBinding("<Keyboard>/b")
            .WithCategory("Tools Upgrades")
            .AvoidConflicts(GameInput.Device.Keyboard);

    private static readonly Harmony Harmony = new Harmony(MyGuid);
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    private static readonly string ModPath = Path.GetDirectoryName(Assembly.Location);
    private static readonly string SpriteTimePath = Path.Combine(ModPath, "Assets", "RifleUpgradeTime.png");
    private static readonly string SpriteDamagePath = Path.Combine(ModPath, "Assets", "RifleUpgradeShock.png");

    public static ManualLogSource Log { get; private set; } = new ManualLogSource(PluginName);
    public static Sprite spriteDamage = ImageUtils.LoadSpriteFromFile(SpriteDamagePath, (TextureFormat)25);
    public static Sprite spriteTime = ImageUtils.LoadSpriteFromFile(SpriteTimePath, (TextureFormat)25);
    public static ModOptions ModOptions { get; private set; }
    public static TechCategory StasisRifleUpgrades { get; private set; }
    public static EquipmentType EquipmentType { get; private set; }

    private void Awake()
    {
        Log = Logger;
        Logger.LogInfo($"{PluginName} {VersionString} is loading...");

        ModOptions = OptionsPanelHandler.RegisterModOptions<ModOptions>();
        UpgradeData.Options = ModOptions;

        StasisRifleUpgrades = (TechCategory)EnumHandler.AddEntry<TechCategory>("StasisRifleUpgrades")
            .WithPdaInfo("Stasis Rifle Upgrades", "English")
            .RegisterToTechGroup(UpgradesLIB.Plugin.toolupgrademodules);

        StartCoroutine(Utilities.CreateUpgradesContainer(
            StasisRifleTechType,
            EquipmentTypeName,
            StorageName,
            StorageClassId,
            PanelLabel,
            4,
            this));

        EquipmentType = Utilities.ClaimEquipmentTypes(this)[0];

        CraftTreeHandler.AddTabNode(
            Handheldprefab.HandheldfabTreeType,
            "StasisRifleTab",
            "Stasis Rifle",
            SpriteManager.Get(StasisRifleTechType),
            "Tools");

        InitializePrefabs();
        Harmony.PatchAll(Assembly);

        Logger.LogInfo($"{PluginName} {VersionString} is loaded.");
    }

    private void InitializePrefabs()
    {
        StasisDamageAddMk1.Register();
        StasisDamageAddMk2.Register();
        StasisDamageAddMk3.Register();
        StasisDurationMk1.Register();
        StasisDurationMk2.Register();
        StasisDurationMk3.Register();
    }
}
