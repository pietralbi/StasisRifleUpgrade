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

internal class StasisDamageAddMk2
{
	public static UpgradeData Mk2DamageAddData = new UpgradeData(300f, 1f);

	public static CustomPrefab Mk2DamageAddPrefab;

	public static PrefabInfo Mk2DamageAddPrefabInfo;

	public static TechType TechType = (TechType)2101;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisRifleDamageAddMk2", "Stasis Rifle Damage Addition Module MK2", "Mk 2 Damage Addition to Stasis Rifle, makes the static field electrify it's victim, dealing heavier damage.", "English", false, (Assembly)null);
		Mk2DamageAddPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteDamage);
		UpgradeData.UpgradeDataDict.Add(Mk2DamageAddPrefabInfo.TechType, Mk2DamageAddData);
		Mk2DamageAddPrefab = new CustomPrefab(Mk2DamageAddPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk2DamageAddPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk2DamageAddPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk2DamageAddPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(StasisDamageAddMk1.Mk1DamageAddPrefabInfo.TechType, 1),
				new Ingredient((TechType)23, 2),
				new Ingredient((TechType)63, 2),
				new Ingredient((TechType)44, 2)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk2DamageAddPrefab, (TechType)755, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk2DamageAddPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk2DamageAddPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk2DamageAddPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisRifleDamageAddMk2 successfully initialized.");
	}
}
