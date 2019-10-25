using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.TerraSpirit
{
    public class RitualOfBunnies : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Enrages the Spirit of Purity"
                + "\nCan be reused infinitely"
                + "\nEach player starts with 10 lives"
                + "\nSpirit of Chaos must be defeated first"
                + "\nWARNING: Use this in the middle of a large open area (eg. the sky)"
                + "\nIt is highly recommended that you use the Purity Shield [i:" + mod.ItemType("PurityShield") + "] mount");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 50, 0, 0);
            item.rare = 12;
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(mod.NPCType("TerraSpirit"));
        }

        public override bool UseItem(Player player)
        {
            string message = null;
            if (!NPC.downedBoss1)
            {
                message = "The Eye of Cthulhu must first be defeated";
            }
            else if (!NPC.downedSlimeKing)
            {
                message = "King Slime must first be defeated";
            }
            else if (!NPC.downedBoss2)
            {
                message = "The Eater of Worlds or Brain of Cthulhu must first be defeated";
            }
            else if (!NPC.downedBoss3)
            {
                message = "Skeletron must first be defeated";
            }
            else if (!NPC.downedQueenBee)
            {
                message = "Queen Bee must first be defeated";
            }
            else if (!Main.hardMode)
            {
                message = "The Wall of Flesh must first be defeated";
            }
            else if (!NPC.downedMechBoss1)
            {
                message = "The Destroyer must first be defeated";
            }
            else if (!NPC.downedMechBoss2)
            {
                message = "The Twins must first be defeated";
            }
            else if (!NPC.downedMechBoss3)
            {
                message = "Skeletron Prime must first be defeated";
            }
            else if (!NPC.downedPlantBoss)
            {
                message = "Plantera must first be defeated";
            }
            else if (!NPC.downedGolemBoss)
            {
                message = "Golem must first be defeated";
            }
            else if (!NPC.downedFishron)
            {
                message = "Duke Fishron must first be defeated";
            }
            else if (!BluemagicWorld.downedPhantom)
            {
                message = "The Phantom must first be defeated";
            }
            else if (!BluemagicWorld.downedAbomination)
            {
                message = "The Abomination must first be defeated";
            }
            else if (!NPC.downedAncientCultist)
            {
                message = "The Lunatic Cultist must first be defeated";
            }
            else if (!NPC.downedMoonlord)
            {
                message = "Moon Lord must first be defeated";
            }
            else if (!BluemagicWorld.elementalUnleash)
            {
                message = "You must unleash the elements by defeating the Abomination again";
            }
            else if (!BluemagicWorld.downedPuritySpirit)
            {
                message = "You must first pass the Spirit of Purity's trial";
            }
            else if (!BluemagicWorld.downedChaosSpirit)
            {
                message = "You must first overcome and vanquish the Spirit of Chaos";
            }
            if (player.whoAmI == Main.myPlayer && message != null)
            {
                Main.NewText(message);
            }
            if (Main.netMode != 1 && message == null)
            {
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, mod.NPCType("TerraSpirit"));
            }
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "InfinityCrystal");
            recipe.AddIngredient(ItemID.Bunny);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}