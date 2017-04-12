using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
	public class PuriumOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine[Type] = 800;
			Main.tileShine2[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileValue[Type] = 750;
			TileID.Sets.Ore[Type] = true;
			soundType = 21;
			soundStyle = 1;
			dustType = 128;
			drop = mod.ItemType("PuriumOre");
			minPick = 225;
			mineResist = 5f;
			AddMapEntry(new Color(100, 210, 100), "Purium");
		}

		public override void RandomUpdate(int i, int j)
		{
			for (int x = -5; x <= 5; x++)
			{
				for (int y = -5; y <= 5; y++)
				{
					WorldGen.Convert(i + x, j + y, 0, 0);
					Tile tile = Main.tile[i + x, j + y];
					if (tile.active() && (tile.type == TileID.Demonite || tile.type == TileID.Crimtane))
					{
						tile.type = (ushort)mod.TileType("PuriumOre");
						NetMessage.SendTileRange(Main.myPlayer, i + x, j + y, 1, 1);
					}
				}
			}
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}