using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination
{
    public class PuriumOreGen : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purium Infuser");
            Tooltip.SetDefault("Creates pockets of concentrated purity underground");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.maxStack = 20;
            item.rare = 10;
            item.useStyle = 4;
            item.useAnimation = 45;
            item.useTime = 45;
            item.UseSound = SoundID.Item4;
            item.consumable = true;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != 1)
            {
                BluemagicWorld.GenPurium();
            }
            return true;
        }
    }
}