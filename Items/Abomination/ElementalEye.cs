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
            Tooltip.SetDefault("Permanently increases the number of accessory slots to 7"
                + "\nCan only be used if the Demon Heart has been used");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 99;
            item.rare = 11;
            item.expert = true;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.consumable = true;
            item.useStyle = 4;
            item.useTime = 30;
            item.useAnimation = 30;
            item.UseSound = SoundID.Item4;
        }

        public override bool CanUseItem(Player player)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            return player.extraAccessory && !modPlayer.extraAccessory2;
        }

        public override bool UseItem(Player player)
        {
            player.GetModPlayer<BluemagicPlayer>().extraAccessory2 = true;
            return true;
        }
    }
}