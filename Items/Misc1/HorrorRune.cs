using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class HorrorRune : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Toggles the horrors from above");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.dayTime)
                {
                    Main.eclipse = !Main.eclipse;
                }
                else
                {
                    Main.bloodMoon = !Main.bloodMoon;
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "HorrorDrop", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}