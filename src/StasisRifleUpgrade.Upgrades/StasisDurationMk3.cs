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

internal class StasisDurationMk3
{
	public static UpgradeData Mk3DurationData = new UpgradeData(0f, 3.5f);

	public static CustomPrefab Mk3DurationPrefab;

	public static PrefabInfo Mk3DurationPrefabInfo;

	public static TechType TechType = (TechType)2101;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisDurationMk3", "Stasis Rifle Static Duration Module Mk3", "Mk 3 Duration Module to Stasis Rifle, makes the static field stands for more time and hold foes even longer.", "English", false, (Assembly)null);
		Mk3DurationPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteTime);
		UpgradeData.UpgradeDataDict.Add(Mk3DurationPrefabInfo.TechType, Mk3DurationData);
		Mk3DurationPrefab = new CustomPrefab(Mk3DurationPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk3DurationPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk3DurationPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk3DurationPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(StasisDurationMk2.Mk2DurationPrefabInfo.TechType, 1),
				new Ingredient((TechType)23, 2),
				new Ingredient((TechType)34, 1),
				new Ingredient((TechType)68, 2),
				new Ingredient((TechType)59, 2)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk3DurationPrefab, (TechType)755, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk3DurationPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk3DurationPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk3DurationPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisDurationMk3 successfully initialized.");
	}
}
