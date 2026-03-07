using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public static class TileUtils
    {
        public static bool TileFrame_Sand(int i, int j, int projType)
        {
            if (WorldGen.noTileActions)
            {
                return true;
            }
            Tile above = Main.tile[i, j - 1];
            Tile below = Main.tile[i, j + 1];
            bool canFall = true;
            if (below == null || below.HasTile)
            {
                canFall = false;
            }
            if (above.HasTile && (TileID.Sets.BasicChest[above.TileType] || TileID.Sets.BasicChestFake[above.TileType] || above.TileType == TileID.PalmTree || TileID.Sets.BasicDresser[above.TileType]))
            {
                canFall = false;
            }
            if (canFall)
            {
                int type = projType;
                float posX = i * 16 + 8;
                float posY = j * 16 + 8;
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.tile[i, j].ClearTile();
                    int proj = Projectile.NewProjectile(new EntitySource_TileBreak(i, j), posX, posY, 0f, 0.41f, type, 10, 0f, Main.myPlayer, 0f, 0f);
                    Main.projectile[proj].ai[0] = 1f;
                    WorldGen.SquareTileFrame(i, j, true);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    Main.tile[i, j].ClearTile();
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
                        int proj = Projectile.NewProjectile(new EntitySource_TileBreak(i,j), posX, posY, 0f, 2.5f, type, 10, 0f, Main.myPlayer, 0f, 0f);
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