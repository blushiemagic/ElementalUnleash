using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class SixColorShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Six-Color Shield");
            // Tooltip.SetDefault("Creates elemental energy to protect you when damaged.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(10, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            Item.accessory = true;
            Item.damage = 120;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 2f;
            Item.defense = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BluemagicPlayer>().elementShield = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}