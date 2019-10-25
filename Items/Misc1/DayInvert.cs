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
            DisplayName.SetDefault("Celestial Rune");
            Tooltip.SetDefault("Manipulates the sun and moon");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 20;
            item.maxStack = 20;
            item.rare = 4;
            item.useStyle = 4;
            item.useAnimation = 45;
            item.useTime = 45;
            item.UseSound = SoundID.Item44;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != 1)
            {
                if (Main.dayTime)
                {
                    Main.time = 54000;
                }
                else
                {
                    Main.time = 32400;
                }
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SolarDrop", 5);
            recipe.AddIngredient(null, "LunarDrop", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}