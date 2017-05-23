using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
	public class DimensionalChest : ModItem
	{
		public override void SetDefaults()
		{
			item.name = "Dimensional Chest";
			item.toolTip = "Steals loot from other dimensions";
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.rare = 8;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe;

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.Spear);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.WoodenBoomerang);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.Blowpipe);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.Aglet);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.ClimbingClaws);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.Umbrella);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.Radar);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.CordageGuide);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WoodKey"));
			recipe.SetResult(ItemID.WandofSparking);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.BandofRegeneration);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.MagicMirror);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.CloudinaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.HermesBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.EnchantedBoomerang);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.ShoeSpikes);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.FlareGun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("StoneKey"));
			recipe.SetResult(ItemID.Extractinator);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.IceBoomerang);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.IceBlade);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.IceSkates);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.SnowballCannon);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.BlizzardinaBottle);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.FlurryBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.IceMachine);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.IceMirror);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("IceKey"));
			recipe.SetResult(ItemID.Fish);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WaterKey"));
			recipe.SetResult(ItemID.Trident);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WaterKey"));
			recipe.SetResult(ItemID.BreathingReed);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WaterKey"));
			recipe.SetResult(ItemID.Flipper);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("WaterKey"));
			recipe.SetResult(ItemID.WaterWalkingBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("SkywareKey"));
			recipe.SetResult(ItemID.ShinyRedBalloon);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("SkywareKey"));
			recipe.SetResult(ItemID.Starfury);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("SkywareKey"));
			recipe.SetResult(ItemID.LuckyHorseshoe);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("SkywareKey"));
			recipe.SetResult(ItemID.SkyMill);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.AnkletoftheWind);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.FeralClaws);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.StaffofRegrowth);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.Boomstick);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.Seaweed);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.FiberglassFishingPole);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.FlowerBoots);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.LivingMahoganyWand);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.LivingMahoganyLeafWand);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(mod.ItemType("MahoganyKey"));
			recipe.SetResult(ItemID.HoneyDispenser);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.MagicMissile);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.Muramasa);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.CobaltShield);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.AquaScepter);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.BlueMoon);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.Handgun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.ShadowKey);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.BoneWelder);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.GoldenKey);
			recipe.SetResult(ItemID.Valor);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.ShadowKey);
			recipe.SetResult(ItemID.DarkLance);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.ShadowKey);
			recipe.SetResult(ItemID.Flamelash);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.ShadowKey);
			recipe.SetResult(ItemID.FlowerofFire);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.ShadowKey);
			recipe.SetResult(ItemID.Sunfury);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.ShadowKey);
			recipe.SetResult(ItemID.HellwingBow);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.CorruptionKey);
			recipe.SetResult(ItemID.ScourgeoftheCorruptor);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.CrimsonKey);
			recipe.SetResult(ItemID.VampireKnives);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.JungleKey);
			recipe.SetResult(ItemID.PiranhaGun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.HallowedKey);
			recipe.SetResult(ItemID.RainbowGun);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this);
			recipe.AddIngredient(ItemID.FrozenKey);
			recipe.SetResult(ItemID.StaffoftheFrostHydra);
			recipe.AddRecipe();
		}
	}
}