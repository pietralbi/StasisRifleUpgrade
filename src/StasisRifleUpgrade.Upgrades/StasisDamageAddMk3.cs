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

internal class StasisDamageAddMk3
{
	public static UpgradeData Mk3DamageAddData = new UpgradeData(500f, 1f);

	public static CustomPrefab Mk3DamageAddPrefab;

	public static PrefabInfo Mk3DamageAddPrefabInfo;

	public static TechType TechType = (TechType)2101;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisRifleDamageAddMk3", "Stasis Rifle Damage Addition Module MK3", "Mk 3 Damage Addition to Stasis Rifle, makes the static field electrify it's victim, dealing massive damage.", "English", false, (Assembly)null);
		Mk3DamageAddPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteDamage);
		UpgradeData.UpgradeDataDict.Add(Mk3DamageAddPrefabInfo.TechType, Mk3DamageAddData);
		Mk3DamageAddPrefab = new CustomPrefab(Mk3DamageAddPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk3DamageAddPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk3DamageAddPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk3DamageAddPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(StasisDamageAddMk2.Mk2DamageAddPrefabInfo.TechType, 1),
				new Ingredient((TechType)54, 1),
				new Ingredient((TechType)56, 2),
				new Ingredient((TechType)66, 1),
				new Ingredient((TechType)34, 1)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk3DamageAddPrefab, (TechType)755, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk3DamageAddPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk3DamageAddPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk3DamageAddPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisRifleDamageAddMk3 successfully initialized.");
	}
}
