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
            // DisplayName.SetDefault("Purium Infuser");
            // Tooltip.SetDefault("Creates pockets of concentrated purity underground");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Red;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                BluemagicWorld.GenPurium();
            }
            return true;
        }
    }
}