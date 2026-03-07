using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class Wisp : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Chases enemies through walls");
        }

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 14;
            Item.height = 14;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1f;
            Item.value = Item.sellPrice(0, 0, 1, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.shoot = Mod.Find<ModProjectile>("Wisp").Type;
            Item.ammo = Item.type;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
