using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PuritySpirit
{
    public class PrismaticShocker : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Dissolve your foes in a dazzling display."
                + "\n<right> to turn off the lights.");*/
        }

        public override void SetDefaults()
        {
            Item.damage = 409;
            Item.DamageType = DamageClass.Magic;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item44;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 50, 0, 0);
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
            Item.autoReuse = false;
            Item.shoot = Mod.Find<ModProjectile>("PrismaticShocker").Type;
            Item.shootSpeed = 0f;
            Item.mana = 26;
        }

        public override bool AltFunctionUse(Player player)
        {
            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Mod.Find<ModProjectile>("PrismaticShocker").Type)
                {
                    proj.Kill();
                }
            }
            return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int count = 0;
            for (int k = 0; k < 1000; k++)
            {
                Projectile proj = Main.projectile[k];
                if (proj.active && proj.owner == player.whoAmI && proj.type == Mod.Find<ModProjectile>("PrismaticShocker").Type)
                {
                    if (count < 4)
                    {
                        count++;
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
            }
            position = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
            return true;
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
                recipe.AddIngredient(null, "VoidEmblem");
                recipe.AddIngredient(Bluemagic.Sushi.Find<ModItem>("SwapToken").Type);
                recipe.AddTile(TileID.TinkerersWorkbench);
                recipe.Register();
            }
        }
    }
}