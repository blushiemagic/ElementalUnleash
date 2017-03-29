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

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}