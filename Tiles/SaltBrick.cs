using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
	public class SaltBrick : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileLighted[Type] = true;
			soundType = 21;
			soundStyle = 1;
			dustType = 13;
			drop = mod.ItemType("SaltBrick");
			AddMapEntry(new Color(200, 200, 255));
		}
	}
}