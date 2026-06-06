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

internal class StasisDamageAddMk1
{
	public static UpgradeData Mk1DamageAddData = new UpgradeData(150f, 1f);

	public static CustomPrefab Mk1DamageAddPrefab;

	public static PrefabInfo Mk1DamageAddPrefabInfo;

	public static TechType TechType = TechType.VehiclePowerUpgradeModule;

	public static void Register()
	{
		PrefabInfo val = PrefabInfo.WithTechType("StasisRifleDamageAddMk1", "Stasis Rifle Damage Addition Module MK1", "Mk 1 Damage Addition to Stasis Rifle, makes the static field electrify it's victim, dealing heavy damage.", "English", false, (Assembly)null);
		Mk1DamageAddPrefabInfo = val.WithIcon(StasisRifleUpgradePlugin.spriteDamage);
		UpgradeData.UpgradeDataDict.Add(Mk1DamageAddPrefabInfo.TechType, Mk1DamageAddData);
		Mk1DamageAddPrefab = new CustomPrefab(Mk1DamageAddPrefabInfo);
		CloneTemplate val2 = new CloneTemplate(Mk1DamageAddPrefabInfo, TechType);
		CloneTemplate val3 = val2;
		val3.ModifyPrefab = (Action<GameObject>)Delegate.Combine(val3.ModifyPrefab, (Action<GameObject>)delegate(GameObject obj)
		{
			GameObject gameObject = obj.gameObject;
			gameObject.transform.localScale = Vector3.one / 1f;
		});
		Mk1DamageAddPrefab.SetGameObject((PrefabTemplate)(object)val2);
		GadgetExtensions.SetRecipe((ICustomPrefab)(object)Mk1DamageAddPrefab, new RecipeData
		{
			craftAmount = 1,
			Ingredients = new List<Ingredient>
			{
				new Ingredient(TechType.Magnetite, 3),
				new Ingredient(TechType.HydrochloricAcid, 1),
				new Ingredient(TechType.CopperWire, 2)
			}
		}).WithFabricatorType(Handheldprefab.HandheldfabTreeType).WithStepsToFabricatorTab(new string[2] { "Tools", "StasisRifleTab" })
			.WithCraftingTime(5f);
		GadgetExtensions.SetUnlock((ICustomPrefab)(object)Mk1DamageAddPrefab, StasisRifleUpgradePlugin.StasisRifleTechType, 1);
		GadgetExtensions.SetEquipment((ICustomPrefab)(object)Mk1DamageAddPrefab, StasisRifleUpgradePlugin.EquipmentType);
		GadgetExtensions.SetPdaGroupCategory((ICustomPrefab)(object)Mk1DamageAddPrefab, Plugin.toolupgrademodules, StasisRifleUpgradePlugin.StasisRifleUpgrades);
		Mk1DamageAddPrefab.Register();
		StasisRifleUpgradePlugin.Log.LogInfo("Prefab StasisRifleDamageAddMk1 successfully initialized.");
	}
}
