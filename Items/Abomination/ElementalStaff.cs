using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Summons mini captive elements to fight for you."
                + "\nUses 2 minion slots in total"
                + "\nYo I heard you like debuffs, so I...");*/
            ItemID.Sets.StaffMinionSlotsRequired[Item.type] = 2;
        }

        public override void SetDefaults()
        {
            Item.mana = 10;
            Item.damage = 215;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shootSpeed = 10f;
            Item.shoot = Mod.Find<ModProjectile>("MiniCaptiveElement0").Type;
            Item.width = 26;
            Item.height = 28;
            Item.UseSound = SoundID.Item82;
            Item.useAnimation = 36;
            Item.useTime = 36;
            Item.rare = ItemRarityID.Pink;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.buffType = Mod.Find<ModBuff>("MiniCaptiveElement").Type;
            Item.buffTime = 3600;
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.DamageType = DamageClass.Summon;
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
                recipe.AddIngredient(null, "ElementalYoyo");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "ElementalSprayer");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "EyeballTome");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();

                recipe = CreateRecipe();
                recipe.AddIngredient(null, "EyeballGlove");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}
