using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class VoidEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Emblem of the Void");
            /* Tooltip.SetDefault("Summons a void emissary to fight alongside you."
                + "\nDoes not interfere with your piercing projectiles");*/
        }

        public override void SetDefaults()
        {
            Item.damage = 450;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 6f;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item44;
            Item.shoot = Mod.Find<ModProjectile>("VoidEmissary").Type;
            Item.shootSpeed = 10f;
            Item.buffType = Mod.Find<ModBuff>("VoidEmissary").Type;
            Item.buffTime = 3600;
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "DanceOfBlades");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "CleanserBeam");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PrismaticShocker");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}