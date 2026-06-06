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

internal class StasisDurationMk2
{
	public static UpgradeData Mk2DurationData = new UpgradeData(0f, 3.5f);

	public static CustomPrefab Mk2DurationPrefab;

	public static PrefabInfo Mk2DurationPrefabInfo;

	public static TechType TechType = TechType.VehiclePowerUpgradeModule;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisDurationMk2", "Stasis Rifle Static Duration Module Mk2", "Mk 2 Duration Module to Stasis Rifle, makes the static field stands for more time and hold foes even longer.", "English", false, (Assembly)null);
		Mk2DurationPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteTime);
		UpgradeData.UpgradeDataDict.Add(Mk2DurationPrefabInfo.TechType, Mk2DurationData);
		Mk2DurationPrefab = new CustomPrefab(Mk2DurationPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk2DurationPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk2DurationPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk2DurationPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(StasisDurationMk1.Mk1DurationPrefabInfo.TechType, 1),
				new Ingredient(TechType.ComputerChip, 1),
				new Ingredient(TechType.Lead, 3),
				new Ingredient(TechType.Aerogel, 1)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk2DurationPrefab, StasisRifleUpgradePlugin.StasisRifleTechType, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk2DurationPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk2DurationPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk2DurationPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisDurationMk2 successfully initialized.");
	}
}
