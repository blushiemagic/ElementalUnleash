using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Bluemagic.Tiles
{
	public class PuriumForge : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.InteractibleByNPCs[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			dustType = 128;
			animationFrameHeight = 38;
			adjTiles = new int[] { TileID.Furnaces, TileID.Hellforge, TileID.AdamantiteForge };
			AddMapEntry(new Color(100, 210, 100), "Purium Forge");
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 48, 32, mod.ItemType("PuriumForge"));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.83f;
			g = 0.6f;
			b = 0.5f;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frame = Main.tileFrame[TileID.AdamantiteForge];
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
		{
			offsetY = 2;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.frameX == 18 && tile.frameY == 18 && Main.rand.Next(40) == 0)
			{
				int dust = Dust.NewDust(new Vector2(i * 16 - 4, j * 16 - 6), 8, 6, 6, 0f, 0f, 100, default(Color), 1f);
				if (Main.rand.Next(3) != 0)
				{
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}