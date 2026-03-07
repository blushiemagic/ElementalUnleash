using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class BlushieCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Use this item to die"
                + "\nCan be reused infinitely"
                + "\nWARNING: Use this in the middle of a large open area (eg. the sky)"
                + "\nIt is highly recommended that you use the Purity Shield [i:" + Mod.Find<ModItem>("PurityShield").Type + "] mount"
                + "\nRight-click to focus the camera on the entire boss arena"
                + "\nRight-click mid-fight to toggle the camera focus"
                + "\nYour hitbox becomes a single pixel");*/
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.sellPrice(2, 0, 0, 0);
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.noUseGraphic = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            return !BlushieBoss.BlushieBoss.Active || player.altFunctionUse == 2;
        }

        public override bool? UseItem(Player player)
        {
            if (BlushieBoss.BlushieBoss.Active)
            {
                if (player.altFunctionUse == 2 && player.whoAmI == Main.myPlayer)
                {
                    BlushieBoss.BlushieBoss.CameraFocus = !BlushieBoss.BlushieBoss.CameraFocus;
                }
                return true;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y + 24, Mod.Find<ModNPC>("Blushiemagic").Type);
                BlushieBoss.BlushieBoss.CameraFocus = player.altFunctionUse == 2;
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddIngredient(null, "ChaosCrystal");
            recipe.AddIngredient(null, "PureSalt", 10);
            recipe.Register();
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }
    }
}