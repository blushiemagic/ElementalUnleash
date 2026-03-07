using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium
{
    public class PuriumForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Used to smelt elemental ores");
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 30;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 13, 0, 0);
            Item.createTile = Mod.Find<ModTile>("PuriumForge").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteForge);
            recipe.AddIngredient(null, "PuriumOre", 60);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumForge);
            recipe.AddIngredient(null, "PuriumOre", 60);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}