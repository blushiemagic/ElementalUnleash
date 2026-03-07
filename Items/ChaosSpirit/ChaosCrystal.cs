using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.ChaosSpirit
{
    public class ChaosCrystal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Grants one chaos point");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(1, 50, 0, 0);
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item29;
        }

        public override bool CanUseItem(Player player)
        {
            CustomStats stats = player.GetModPlayer<BluemagicPlayer>().chaosStats;
            return stats.CanUpgrade();
        }

        public override bool? UseItem(Player player)
        {
            CustomStats stats = player.GetModPlayer<BluemagicPlayer>().chaosStats;
            stats.Points++;
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}