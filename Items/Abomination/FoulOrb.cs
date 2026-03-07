using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class FoulOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("The underworld would like this.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Cyan;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            return NPC.downedPlantBoss && player.position.Y / 16f > Main.maxTilesY - 200 && !NPC.AnyNPCs(Mod.Find<ModNPC>("Abomination").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("CaptiveElement").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("CaptiveElement2").Type);
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, Mod.Find<ModNPC>("Abomination").Type);
            SoundEngine.PlaySound(SoundID.Roar, player.position);
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.BeetleHusk);
            recipe.AddIngredient(null, "ScytheBlade");
            recipe.AddIngredient(null, "Icicle");
            recipe.AddIngredient(null, "Bubble");
            recipe.AddIngredient(null, "PhantomPlate");
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}