using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Banners
{
    public class Banner : ModItem
    {
        private string name;
        private int placeStyle;

        public Banner()
        {
            this.name = "";
            this.placeStyle = -1;
        }

        public Banner(string name, int placeStyle)
        {
            this.name = name;
            this.placeStyle = placeStyle;
        }

        public override bool Autoload(ref string name)
        {
            AddBanner("NightSlime", 0);
            return false;
        }

        public override bool CloneNewInstances => true;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("{$CommonItemTooltip.BannerBonus}{$Mods.Bluemagic.NPCName." + this.name + "}");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 24;
            item.maxStack = 99;
            item.rare = 1;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = mod.TileType("Banner");
            item.placeStyle = this.placeStyle;
        }

        private void AddBanner(string name, int placeStyle)
        {
            mod.AddItem(name + "Banner", new Banner(name, placeStyle));
        }
    }
}
