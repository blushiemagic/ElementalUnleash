using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class DimensionalChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Steals loot from other dimensions");
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 22;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Yellow;
        }

        public override void AddRecipes()
        {
            Recipe recipe;

            recipe = Recipe.Create(ItemID.Spear);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.WoodenBoomerang);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Blowpipe);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Aglet);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.ClimbingClaws);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Umbrella);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Radar);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.CordageGuide);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.WandofSparking);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WoodKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.BandofRegeneration);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.MagicMirror);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.CloudinaBottle);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.HermesBoots);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.EnchantedBoomerang);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.ShoeSpikes);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FlareGun);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Extractinator);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("StoneKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.IceBoomerang);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.IceBlade);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.IceSkates);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.SnowballCannon);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.BlizzardinaBottle);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FlurryBoots);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.IceMachine);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.IceMirror);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Fish);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("IceKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Trident);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WaterKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.BreathingReed);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WaterKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Flipper);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WaterKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.WaterWalkingBoots);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("WaterKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.ShinyRedBalloon);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("SkywareKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Starfury);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("SkywareKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.LuckyHorseshoe);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("SkywareKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.SkyMill);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("SkywareKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.AnkletoftheWind);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FeralClaws);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.StaffofRegrowth);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Boomstick);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Seaweed);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FiberglassFishingPole);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FlowerBoots);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.LivingMahoganyWand);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.LivingMahoganyLeafWand);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.HoneyDispenser);
            recipe.AddIngredient(this);
            recipe.AddIngredient(Mod.Find<ModItem>("MahoganyKey").Type);
            recipe.Register();

            recipe = Recipe.Create(ItemID.MagicMissile);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Muramasa);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.CobaltShield);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.AquaScepter);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.BlueMoon);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Handgun);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.ShadowKey);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.BoneWelder);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Valor);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.GoldenKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.DarkLance);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Flamelash);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.FlowerofFire);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.Sunfury);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.HellwingBow);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.ShadowKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.ScourgeoftheCorruptor);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.CorruptionKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.VampireKnives);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.CrimsonKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.PiranhaGun);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.JungleKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.RainbowGun);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.HallowedKey);
            recipe.Register();

            recipe = Recipe.Create(ItemID.StaffoftheFrostHydra);
            recipe.AddIngredient(this);
            recipe.AddIngredient(ItemID.FrozenKey);
            recipe.Register();
        }
    }
}