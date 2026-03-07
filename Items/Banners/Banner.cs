using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Banners
{
    public abstract class Banner : ModItem
    {
        public virtual int placeStyle => -1;

        protected override bool CloneNewInstances => true;

        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("{$CommonItemTooltip.BannerBonus}{$Mods.Bluemagic.NPCName." + Name + "}");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = Mod.Find<ModTile>("Banner").Type;
            Item.placeStyle = this.placeStyle;
        }
    }

    public class NightSlimeBanner : Banner
    {
        public override int placeStyle => 0;
    }
    public class TwinEyeBanner : Banner
    {
        public override int placeStyle => 1;
    }
}
