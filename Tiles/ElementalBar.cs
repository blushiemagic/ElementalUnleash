using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Bluemagic.Tiles
{
    public class ElementalBar : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileShine[Type] = 1100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Purium Bar");
            AddMapEntry(new Color(100, 210, 100), name);
        }

        public override ushort GetMapOption(int i, int j)
        {
            int style = Main.tile[i, j].frameX / 18;
            return 0;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            int style = Main.tile[i, j].frameX / 18;
            if (style == 0)
            {
                type = 128;
            }
            return true;
        }

        public override bool Drop(int i, int j)
        {
            int style = Main.tile[i, j].frameX / 18;
            if (style == 0)
            {
                Item.NewItem(i * 16, j * 16, 16, 16, mod.ItemType("PuriumBar"));
            }
            return false;
        }
    }
}