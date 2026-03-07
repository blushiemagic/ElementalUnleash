using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class ElementalEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Permanently increases the number of accessory slots to 7"
                + "\nCan only be used if the Demon Heart has been used");*/
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            return player.extraAccessory && !modPlayer.extraAccessory2;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<BluemagicPlayer>().extraAccessory2 = true;
            return true;
        }
    }
}