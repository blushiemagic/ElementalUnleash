using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementalStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons mini captive elements to fight for you."
                + "\nUses 2 minion slots in total"
                + "\nYo I heard you like debuffs, so I...");
            ItemID.Sets.StaffMinionSlotsRequired[item.type] = 2;
        }

        public override void SetDefaults()
        {
            item.mana = 10;
            item.damage = 215;
            item.useStyle = 1;
            item.shootSpeed = 10f;
            item.shoot = mod.ProjectileType("MiniCaptiveElement0");
            item.width = 26;
            item.height = 28;
            item.UseSound = SoundID.Item82;
            item.useAnimation = 36;
            item.useTime = 36;
            item.rare = 5;
            item.noMelee = true;
            item.knockBack = 2f;
            item.buffType = mod.BuffType("MiniCaptiveElement");
            item.buffTime = 3600;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.summon = true;
        }
        
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return player.altFunctionUse != 2;
        }
        
        public override bool UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }

        public override void AddRecipes()
        {
            if (Bluemagic.Sushi != null)
            {
                ModRecipe recipe;

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "ElementalYoyo");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "ElementalSprayer");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "EyeballTome");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();

                recipe = new ModRecipe(mod);
                recipe.AddIngredient(null, "EyeballGlove");
                recipe.AddIngredient(Bluemagic.Sushi.ItemType("SwapToken"));
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
        }
    }
}
