using System;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SeaglideUpgrade.Upgrades;
using Nautilus.Handlers;
using Nautilus.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UpgradesLIB;
using UpgradesLIB.Items.Equipment;

namespace SeaglideUpgrade;

[BepInPlugin(MyGuid, PluginName, VersionString)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("com.lawabidingmodder.upgradeslib")]
[BepInProcess("Subnautica.exe")]
public class SeaglideUpgradePlugin : BaseUnityPlugin
{
    private const string MyGuid = "com.lawabidingtroller.literalseaglideupgrades";
    private const string PluginName = "Seaglide Upgrade";
    private const string VersionString = "0.4.1";

    public const TechType SeaglideTechType = (TechType)751;
    public const TechType ModuleCloneTechType = (TechType)2101;
    public const string EquipmentTypeName = "SeaglideUpgrade";
    public const string StorageName = "SeaglideContainer";
    public const string StorageClassId = "SeaglideContainerClassID";
    public const string PanelLabel = "SEAGLIDE";

    public static readonly GameInput.Button OpenUpgradesButton =
        (GameInput.Button)EnumHandler.AddEntry<GameInput.Button>("OpenSeaglideUpgrades")
            .CreateInput("Open Seaglide Upgrades", string.Empty, "English", InputActionType.Button)
            .WithKeyboardBinding("<Keyboard>/b")
            .WithCategory("Tools Upgrades")
            .AvoidConflicts(GameInput.Device.Keyboard);

    private static readonly Harmony Harmony = new Harmony(MyGuid);
    private static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    private static readonly string ModPath = Path.GetDirectoryName(Assembly.Location);

    public static ManualLogSource Log { get; private set; } = new ManualLogSource(PluginName);
    public static ModOptions ModOptions { get; private set; }
    public static TechCategory SeaglideUpgradeCategory { get; private set; }
    public static EquipmentType EquipmentType { get; private set; }
    public static Sprite SpeedIcon1 { get; private set; }
    public static Sprite SpeedIcon2 { get; private set; }
    public static Sprite SpeedIcon3 { get; private set; }
    public static Sprite EfficiencyIcon1 { get; private set; }
    public static Sprite EfficiencyIcon2 { get; private set; }
    public static Sprite EfficiencyIcon3 { get; private set; }

    public static readonly Sprite[] SpeedIcons = new Sprite[4];
    public static readonly Sprite[] EfficiencyIcons = new Sprite[4];

    private void Awake()
    {
        Log = Logger;
        Logger.LogInfo($"{PluginName} {VersionString} is loading...");

        LoadIcons();
        ModOptions = OptionsPanelHandler.RegisterModOptions<ModOptions>();
        UpgradeData.Options = ModOptions;

        SeaglideUpgradeCategory = (TechCategory)EnumHandler.AddEntry<TechCategory>("SeaglideUpgrades")
            .WithPdaInfo("Seaglide Upgrades", "English")
            .RegisterToTechGroup(UpgradesLIB.Plugin.toolupgrademodules);

        StartCoroutine(Utilities.CreateUpgradesContainer(
            SeaglideTechType,
            EquipmentTypeName,
            StorageName,
            StorageClassId,
            PanelLabel,
            2,
            this));

        EquipmentType = Utilities.ClaimEquipmentTypes(this)[0];

        CraftTreeHandler.AddTabNode(
            Handheldprefab.HandheldfabTreeType,
            "SeaglideTab",
            "Seaglide",
            SpriteManager.Get(SeaglideTechType),
            "Tools");

        UpgradeRegistry.RegisterAll();
        Harmony.PatchAll(Assembly);

        Logger.LogInfo($"{PluginName} {VersionString} is loaded.");
    }

    private static void LoadIcons()
    {
        SpeedIcon1 = LoadSprite("sg_speed1.png", SeaglideTechType);
        SpeedIcon2 = LoadSprite("sg_speed2.png", SeaglideTechType);
        SpeedIcon3 = LoadSprite("sg_speed3.png", SeaglideTechType);
        EfficiencyIcon1 = LoadSprite("sg_eff1.png", TechType.PowerUpgradeModule);
        EfficiencyIcon2 = LoadSprite("sg_eff2.png", TechType.PowerUpgradeModule);
        EfficiencyIcon3 = LoadSprite("sg_eff3.png", TechType.PowerUpgradeModule);

        SpeedIcons[1] = SpeedIcon1;
        SpeedIcons[2] = SpeedIcon2;
        SpeedIcons[3] = SpeedIcon3;
        EfficiencyIcons[1] = EfficiencyIcon1;
        EfficiencyIcons[2] = EfficiencyIcon2;
        EfficiencyIcons[3] = EfficiencyIcon3;
    }

    private static Sprite LoadSprite(string fileName, TechType fallback)
    {
        string path = Path.Combine(ModPath, "Assets", fileName);
        if (File.Exists(path))
        {
            return ImageUtils.LoadSpriteFromFile(path, (TextureFormat)25);
        }

        Log.LogWarning($"Missing icon asset {path}; using fallback icon.");
        return SpriteManager.Get(fallback);
    }
}
