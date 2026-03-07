using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class PurityShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Mount - Encases you inside a shield of purity"
                + "\nInfinite flight and +10% purity shield fill rate"
                + "\nHold the jump key to move more slowly");*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.alpha = 75;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 30, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item25;
            Item.noMelee = true;
            Item.mountType = Mod.Find<ModMount>("PurityShield").Type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CosmicCarKey);
            recipe.AddIngredient(null, "InfinityCrystal", 3);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}