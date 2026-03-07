using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class DayInvert : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Celestial Rune");
            // Tooltip.SetDefault("Manipulates the sun and moon");
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
                    Main.time = 54000;
                }
                else
                {
                    Main.time = 32400;
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
            recipe.AddIngredient(null, "SolarDrop", 5);
            recipe.AddIngredient(null, "LunarDrop", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}