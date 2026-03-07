using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class MoltenBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("'Pulsing with heat energy'"
                + "\nThe first item bluemagic123 ever made");*/
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Red;
            Item.value = 30000;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = Mod.Find<ModTile>("MoltenBar").Type;
            Item.holdStyle = 4;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight((int)((Item.position.X + (float)Item.width*0.5f) / 16f), (int)((Item.position.Y + (float)Item.height*0.5f) / 16f), 0.7f, 0.4f, 0f);
        }

        public override void HoldStyle(Player player, Rectangle heldItemFrame)
        {
            player.itemLocation.X = player.Center.X + 6f * player.direction;
            player.itemLocation.Y = player.Center.Y + 10f;
            if(player.gravDir == -1)
            {
                player.itemLocation.Y = player.position.Y + player.height + (player.position.Y - player.itemLocation.Y);
            }
            player.itemRotation = 0;
        }
        public override void UseItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height * 3;
        }
    }
}