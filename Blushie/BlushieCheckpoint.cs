using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class BlushieCheckpoint : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Use this item to die"
                + "\nStarts the fight at phase 3"
                + "\nCan be reused infinitely"
                + "\nEach player starts at {0}% max health"
                + "\nWARNING: Use this in the middle of a large open area (eg. the sky)"
                + "\nIt is highly recommended that you use the Purity Shield [i:" + mod.ItemType("PurityShield") + "] mount"
                + "\nRight-click to focus the camera on the entire boss arena"
                + "\nRight-click mid-fight to toggle the camera focus"
                + "\nYour hitbox becomes a single pixel");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 1;
            item.rare = 12;
            item.value = Item.sellPrice(2, 0, 0, 0);
            item.useAnimation = 45;
            item.useTime = 45;
            item.useStyle = 4;
            item.UseSound = SoundID.Item44;
            item.noUseGraphic = true;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (BluemagicWorld.blushieCheckpoint <= 0f)
            {
                return false;
            }
            return !BlushieBoss.BlushieBoss.Active || player.altFunctionUse == 2;
        }

        public override bool UseItem(Player player)
        {
            if (BlushieBoss.BlushieBoss.Active)
            {
                if (player.altFunctionUse == 2 && player.whoAmI == Main.myPlayer)
                {
                    BlushieBoss.BlushieBoss.CameraFocus = !BlushieBoss.BlushieBoss.CameraFocus;
                }
                return true;
            }
            if (Main.netMode != 1)
            {
                NPC.NewNPC((int)player.Center.X, (int)player.Center.Y + 24, mod.NPCType("BlushiemagicM"));
                BlushieBoss.BlushieBoss.InitializeCheckpoint();
                BlushieBoss.BlushieBoss.CameraFocus = player.altFunctionUse == 2;
            }
            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> lines)
        {
            for (int k = 0; k < lines.Count; k++)
            {
                if (lines[k].mod == "Terraria" && lines[k].Name == "Tooltip3")
                {
                    lines[k].text = string.Format(lines[k].text, (int)(BluemagicWorld.blushieCheckpoint * 100f));
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}