using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
    public abstract class SandBall : ModProjectile
    {
        protected bool falling = true;
        protected int tileType;
        protected int dustType;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.ForcePlateDetection[Projectile.type] = true;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0f, 0f, 0, default(Color), 1f);
                Main.dust[dust].velocity.X *= 0.4f;
            }
            Projectile.tileCollide = true;
            Projectile.localAI[1] = 0f;
            if (Projectile.ai[0] == 1f)
            {
                if (!falling)
                {
                    Projectile.ai[1] += 1f;
                    if (Projectile.ai[1] >= 60f)
                    {
                        Projectile.ai[1] = 60f;
                        Projectile.velocity.Y += 0.2f;
                    }
                }
                else
                {
                    Projectile.velocity.Y += 0.41f;
                }
            }
            else if (Projectile.ai[0] == 2f)
            {
                Projectile.velocity.Y += 0.2f;
                if (Projectile.velocity.X < -0.04f)
                {
                    Projectile.velocity.X += 0.04f;
                }
                else if (Projectile.velocity.X > 0.04f)
                {
                    Projectile.velocity.X -= 0.04f;
                }
                else
                {
                    Projectile.velocity.X = 0f;
                }
            }
            Projectile.rotation += 0.1f;
            if (Projectile.velocity.Y > 10f)
            {
                Projectile.velocity.Y = 10f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            if (falling)
            {
                Projectile.velocity = Collision.AnyCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, true);
            }
            else
            {
                Projectile.velocity = Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height, fallThrough, fallThrough, 1);
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer && !Projectile.noDropItem)
            {
                int tileX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                int tileY = (int)(Projectile.position.Y + (float)(Projectile.width / 2)) / 16;
                if (Main.tile[tileX, tileY].IsHalfBlock && Projectile.velocity.Y > 0f && System.Math.Abs(Projectile.velocity.Y) > System.Math.Abs(Projectile.velocity.X))
                {
                    tileY--;
                }
                if (!Main.tile[tileX, tileY].HasTile)
                {
                    bool onMinecartTrack = tileY < Main.maxTilesY - 2 && Main.tile[tileX, tileY + 1] != null && Main.tile[tileX, tileY + 1].HasTile && Main.tile[tileX, tileY + 1].TileType == TileID.MinecartTrack;
                    if (!onMinecartTrack)
                    {
                        WorldGen.PlaceTile(tileX, tileY, tileType, false, true, -1, 0);
                    }
                    if (!onMinecartTrack && Main.tile[tileX, tileY].HasTile && Main.tile[tileX, tileY].TileType == tileType)
                    {
                        if (Main.tile[tileX, tileY + 1].IsHalfBlock || Main.tile[tileX, tileY + 1].Slope != 0)
                        {
                            WorldGen.SlopeTile(tileX, tileY + 1, 0);
                            if (Main.netMode == NetmodeID.Server)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 14, tileX, tileY + 1, 0f, 0, 0, 0);
                            }
                        }
                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 1, tileX, tileY, tileType, 0, 0, 0);
                        }
                    }
                }
            }
        }

        public override bool? CanDamage()
        {
            return Projectile.localAI[1] != -1f;
        }
    }
}