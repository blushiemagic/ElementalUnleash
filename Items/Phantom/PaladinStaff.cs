using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PaladinStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a mini paladin to fight for you.");
        }

        public override void SetDefaults()
        {
            Item.damage = 62;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item44;
            Item.shoot = Mod.Find<ModProjectile>("MiniPaladin").Type;
            Item.shootSpeed = 5f;
            Item.buffType = Mod.Find<ModBuff>("MiniPaladin").Type;
            Item.buffTime = 3600;
        }
        
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.altFunctionUse != 2;
        }
        
        public override bool? UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim(false);
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                Recipe recipe;

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PhantomBlade");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "SpectreGun");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "PhantomSphere");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}
