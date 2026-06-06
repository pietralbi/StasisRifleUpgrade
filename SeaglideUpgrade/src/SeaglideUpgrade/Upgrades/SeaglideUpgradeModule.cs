using System;
using System.Collections.Generic;
using System.Reflection;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using UnityEngine;
using UpgradesLIB;
using UpgradesLIB.Items.Equipment;

namespace SeaglideUpgrade.Upgrades;

internal sealed class SeaglideUpgradeModule
{
    private readonly Func<IReadOnlyList<Ingredient>> ingredientsFactory;
    private readonly string[] fabricatorPath;
    private readonly Sprite icon;
    public string ClassId { get; }
    public string DisplayName { get; }
    public string Description { get; }
    public int Mk { get; }
    public UpgradeData Data { get; }
    public TechType TechType { get; private set; }
    public PrefabInfo Info { get; private set; }
    public CustomPrefab Prefab { get; private set; }

    public SeaglideUpgradeModule(
        string classId,
        string displayName,
        string description,
        int mk,
        UpgradeData data,
        Sprite icon,
        string[] fabricatorPath,
        Func<IReadOnlyList<Ingredient>> ingredientsFactory)
    {
        ClassId = classId;
        DisplayName = displayName;
        Description = description;
        Mk = mk;
        Data = data;
        this.icon = icon;
        this.fabricatorPath = fabricatorPath;
        this.ingredientsFactory = ingredientsFactory;
    }

    public void Register()
    {
        PrefabInfo info = PrefabInfo.WithTechType(ClassId, DisplayName, Description, "English", false, (Assembly)null);
        Info = info.WithIcon(icon);
        TechType = Info.TechType;
        UpgradeData.UpgradeDataByTechType[TechType] = Data;

        Prefab = new CustomPrefab(Info);
        CloneTemplate template = new CloneTemplate(Info, SeaglideUpgradePlugin.ModuleCloneTechType);
        template.ModifyPrefab += obj =>
        {
            GameObject gameObject = obj.gameObject;
            gameObject.transform.localScale = Vector3.one / 2f;
        };

        Prefab.SetGameObject(template);

        GadgetExtensions.SetRecipe((ICustomPrefab)(object)Prefab, new RecipeData
        {
            craftAmount = 1,
            Ingredients = new List<Ingredient>(ingredientsFactory())
        }).WithFabricatorType(Handheldprefab.HandheldfabTreeType)
            .WithStepsToFabricatorTab(fabricatorPath)
            .WithCraftingTime(5f);

        GadgetExtensions.SetUnlock((ICustomPrefab)(object)Prefab, SeaglideUpgradePlugin.SeaglideTechType, 1);
        GadgetExtensions.SetEquipment((ICustomPrefab)(object)Prefab, SeaglideUpgradePlugin.EquipmentType);
        GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Prefab, Plugin.toolupgrademodules, SeaglideUpgradePlugin.SeaglideUpgradeCategory);
        Prefab.Register();

        SeaglideUpgradePlugin.Log.LogInfo($"Prefab {ClassId} successfully initialized.");
    }
}
