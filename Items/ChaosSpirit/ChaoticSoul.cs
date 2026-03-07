using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
    public class ChaoticSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("'The essence of everything, mashed into chaos'");
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.maxStack = 999;
            Item.value = 125000;
            Item.rare = ItemRarityID.Pink;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            float strength = (float)Main.rand.Next(90, 111) * 0.01f * Main.essScale;
            Lighting.AddLight(Item.Center, 0.4f * strength, 0.4f * strength, 0.25f * strength);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.9f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SoulofNight);
            recipe.AddIngredient(ItemID.SoulofLight);
            recipe.AddIngredient(ItemID.SoulofFlight);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.Register();
        }
    }
}