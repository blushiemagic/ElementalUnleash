using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PhantomHammer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.damage = 78;
            Item.knockBack = 7f;
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Throwing;
            Item.value = Item.sellPrice(0, 0, 0, 10);
            Item.UseSound = SoundID.Item1;
            Item.shoot = Mod.Find<ModProjectile>("PhantomHammerFriendly").Type;
            Item.shootSpeed = 10f;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(333);
            recipe.AddIngredient(null, "PhantomPlate");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}