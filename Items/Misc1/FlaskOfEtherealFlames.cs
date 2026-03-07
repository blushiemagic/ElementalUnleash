using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class FlaskOfEtherealFlames : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Melee attacks inflict ethereal flames");
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 24;
            Item.maxStack = 30;
            Item.rare = ItemRarityID.Yellow;
            Item.value = 5000;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useAnimation = 17;
            Item.useTime = 17;
            Item.UseSound = SoundID.Item3;
            Item.consumable = true;
            Item.buffType = Mod.Find<ModBuff>("EtherealFlamesEnchant").Type;
            Item.buffTime = 72000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Ectoplasm, 2);
            recipe.AddTile(TileID.ImbuingStation);
            recipe.Register();
        }
    }
}