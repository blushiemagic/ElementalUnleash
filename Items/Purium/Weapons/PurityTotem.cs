using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons
{
    public class PurityTotem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons a purity wisp to fight for you.");
        }

        public override void SetDefaults()
        {
            Item.damage = 442;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.UseSound = SoundID.Item44;
            Item.shoot = Mod.Find<ModProjectile>("PurityWisp").Type;
            Item.shootSpeed = 10f;
            Item.buffType = Mod.Find<ModBuff>("PurityWisp").Type;
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
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(null, "PuriumBar", 12);
            recipe.AddTile(null, "PuriumAnvil");
            recipe.Register();
        }
    }
}
