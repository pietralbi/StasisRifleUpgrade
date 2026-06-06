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

namespace StasisRifleUpgrade.Upgrades;

internal class StasisDurationMk1
{
	public static UpgradeData Mk1DurationData = new UpgradeData(0f, 2f);

	public static CustomPrefab Mk1DurationPrefab;

	public static PrefabInfo Mk1DurationPrefabInfo;

	public static TechType TechType = TechType.VehiclePowerUpgradeModule;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisDurationMk1", "Stasis Rifle Static Duration Module MK1", "Mk 1 Duration Module to Stasis Rifle, makes the static field stands for more time and hold foes longer.", "English", false, (Assembly)null);
		Mk1DurationPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteTime);
		UpgradeData.UpgradeDataDict.Add(Mk1DurationPrefabInfo.TechType, Mk1DurationData);
		Mk1DurationPrefab = new CustomPrefab(Mk1DurationPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk1DurationPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk1DurationPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk1DurationPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(TechType.CopperWire, 2),
				new Ingredient(TechType.Lead, 2),
				new Ingredient(TechType.Benzene, 1)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk1DurationPrefab, StasisRifleUpgradePlugin.StasisRifleTechType, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk1DurationPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk1DurationPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk1DurationPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisDurationMk1 successfully initialized.");
	}
}
