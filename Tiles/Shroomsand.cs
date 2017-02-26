using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
	public class Shroomsand : BaseMushroomTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileSand[Type] = true;
			dustType = 17;
			drop = mod.ItemType("Shroomsand");
			AddMapEntry(new Color(93, 127, 255));
			TileID.Sets.Conversion.Sand[Type] = true;
			TileID.Sets.ForAdvancedCollision.ForSandshark[Type] = true;
			TileID.Sets.TouchDamageSands[Type] = 15;
			TileID.Sets.Falling[Type] = true;
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			if (WorldGen.noTileActions)
			{
				return true;
			}
			Tile above = Main.tile[i, j - 1];
			Tile below = Main.tile[i, j + 1];
			bool canFall = true;
			if (below == null || below.active())
			{
				canFall = false;
			}
			if (above.active() && (TileLoader.IsChest(above.type) || above.type == TileID.FakeContainers || above.type == TileID.PalmTree || TileLoader.IsDresser(above.type)))
			{
				canFall = false;
			}
			if (canFall)
			{
				int type = mod.ProjectileType("ShroomsandBall");
				float posX = i * 16 + 8;
				float posY = j * 16 + 8;
				if (Main.netMode == 0)
				{
					Main.tile[i, j].ClearTile();
					int proj = Projectile.NewProjectile(posX, posY, 0f, 0.41f, type, 10, 0f, Main.myPlayer, 0f, 0f);
					Main.projectile[proj].ai[0] = 1f;
					WorldGen.SquareTileFrame(i, j, true);
				}
				else if (Main.netMode == 2)
				{
					Main.tile[i, j].active(false);
					bool spawnProj = true;
					for (int k = 0; k < 1000; k++)
					{
						Projectile otherProj = Main.projectile[k];
						if (otherProj.active && otherProj.owner == Main.myPlayer && otherProj.type == type && Math.Abs(otherProj.timeLeft - 3600) < 60 && otherProj.Distance(new Vector2(posX, posY)) < 4f)
						{
							spawnProj = false;
							break;
						}
					}
					if (spawnProj)
					{
						int proj = Projectile.NewProjectile(posX, posY, 0f, 2.5f, type, 10, 0f, Main.myPlayer, 0f, 0f);
						Main.projectile[proj].velocity.Y = 0.5f;
						Main.projectile[proj].position.Y += 2f;
						Main.projectile[proj].netUpdate = true;
					}
					NetMessage.SendTileSquare(-1, i, j, 1);
					WorldGen.SquareTileFrame(i, j, true);
				}
				return false;
			}
			return true;
		}
	}
}