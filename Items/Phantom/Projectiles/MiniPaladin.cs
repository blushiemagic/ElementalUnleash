using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Items.Phantom.Projectiles
{
    public class MiniPaladin : Minion
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.paladinMinion = false;
            }
            if (modPlayer.paladinMinion)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void Behavior()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (Projectile.ai[1] > 0)
            {
                Projectile.ai[1]--;
            }
            bool moveLeft = false;
            bool moveRight = false;
            int targetFollowDist = 40 * (Projectile.minionPos + 1) * player.direction;
            if (player.position.X + (float)(player.width / 2) < Projectile.position.X + (float)(Projectile.width / 2) + (float)targetFollowDist - 10f)
            {
                moveLeft = true;
            }
            else if (player.position.X + (float)(player.width / 2) > Projectile.position.X + (float)(Projectile.width / 2) + (float)targetFollowDist + 10f)
            {
                moveRight = true;
            }
            if (!Throwing())
            {
                int flyDistance = 500 + 40 * Projectile.minionPos;
                if (player.rocketDelay2 > 0)
                {
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
                Vector2 projCenter = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float xDist = player.position.X + (float)(player.width / 2) - projCenter.X;
                float yDist = player.position.Y + (float)(player.height / 2) - projCenter.Y;
                float distance = (float)System.Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                if (distance > 2000f)
                {
                    Projectile.position.X = player.position.X + (float)(player.width / 2) - (float)(Projectile.width / 2);
                    Projectile.position.Y = player.position.Y + (float)(player.height / 2) - (float)(Projectile.height / 2);
                }
                else if (distance > (float)flyDistance || System.Math.Abs(yDist) > 300f)
                {
                    if (yDist > 0f && Projectile.velocity.Y < 0f)
                    {
                        Projectile.velocity.Y = 0f;
                    }
                    if (yDist < 0f && Projectile.velocity.Y > 0f)
                    {
                        Projectile.velocity.Y = 0f;
                    }
                    Projectile.ai[0] = 1f;
                    Projectile.netUpdate = true;
                }
            }
            if (Throwing())
            {
                UpdateFrame();
                Projectile.velocity.X = 0f;
            }
            else if (Projectile.ai[0] != 0f)
            {
                Projectile.tileCollide = false;
                Vector2 projCenter = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float moveDistX = player.position.X + (float)(player.width / 2) - projCenter.X - (float)(40 * player.direction);
                float viewRange = 600f;
                bool aggro = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].CanBeChasedBy(this))
                    {
                        float monsterX = Main.npc[k].position.X + (float)(Main.npc[k].width / 2);
                        float monsterY = Main.npc[k].position.Y + (float)(Main.npc[k].height / 2);
                        float distance = System.Math.Abs(player.position.X + (float)(player.width / 2) - monsterX) + System.Math.Abs(player.position.Y + (float)(player.height / 2) - monsterY);
                        if (distance < viewRange)
                        {
                            aggro = true;
                            break;
                        }
                    }
                }
                if (!aggro)
                {
                    moveDistX -= (float)(40 * Projectile.minionPos * player.direction);
                }
                float moveDistY = player.position.Y + (float)(player.height / 2) - projCenter.Y;
                float moveDist = (float)System.Math.Sqrt((double)(moveDistX * moveDistX + moveDistY * moveDistY));
                float maxSpeed = 10f;
                if (moveDist < 200f && player.velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    if (Projectile.velocity.Y < -6f)
                    {
                        Projectile.velocity.Y = -6f;
                    }
                    Projectile.netUpdate = true;
                }
                if (moveDist < 60f)
                {
                    moveDistX = Projectile.velocity.X;
                    moveDistY = Projectile.velocity.Y;
                }
                else
                {
                    moveDist = maxSpeed / moveDist;
                    moveDistX *= moveDist;
                    moveDistY *= moveDist;
                }
                float acceleration = 0.2f;
                if (Projectile.velocity.X < moveDistX)
                {
                    Projectile.velocity.X += acceleration;
                    if (Projectile.velocity.X < 0f)
                    {
                        Projectile.velocity.X += acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.X > moveDistX)
                {
                    Projectile.velocity.X -= acceleration;
                    if (Projectile.velocity.X > 0f)
                    {
                        Projectile.velocity.X -= acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.Y < moveDistY)
                {
                    Projectile.velocity.Y += acceleration;
                    if (Projectile.velocity.Y < 0f)
                    {
                        Projectile.velocity.Y += acceleration * 1.5f;
                    }
                }
                if (Projectile.velocity.Y > moveDistY)
                {
                    Projectile.velocity.Y -= acceleration;
                    if (Projectile.velocity.Y > 0f)
                    {
                        Projectile.velocity.Y -= acceleration * 1.5f;
                    }
                }
                if ((double)Projectile.velocity.X > 0.5)
                {
                    Projectile.spriteDirection = -1;
                }
                else if ((double)Projectile.velocity.X < -0.5)
                {
                    Projectile.spriteDirection = 1;
                }
                UpdateFrame();
                if (Main.rand.Next(3) == 0)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("SpectreDust").Type);
                    Main.dust[dust].velocity /= 2f;
                }
            }
            else
            {
                float separation = (float)(40 * Projectile.minionPos);
                if (true)
                {
                    float moveToX = Projectile.position.X;
                    float moveToY = Projectile.position.Y;
                    float moveDist = 100000f;
                    int attacking = -1;
                    if (Projectile.OwnerMinionAttackTargetNPC != null && Projectile.OwnerMinionAttackTargetNPC.CanBeChasedBy(this))
                    {
                        NPC npc = Projectile.OwnerMinionAttackTargetNPC;
                        moveToX = npc.Center.X;
                        moveToY = npc.Center.Y;
                        moveDist = Vector2.Distance(npc.Center, Projectile.Center);
                        attacking = npc.whoAmI;
                    }
                    else
                    {
                        float closestDist = moveDist;
                        for (int k = 0; k < 200; k++)
                        {
                            if (Main.npc[k].CanBeChasedBy(this))
                            {
                                float monsterX = Main.npc[k].position.X + (float)(Main.npc[k].width / 2);
                                float monsterY = Main.npc[k].position.Y + (float)(Main.npc[k].height / 2);
                                float monsterDist = System.Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - monsterX) + System.Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - monsterY);
                                if (monsterDist < moveDist)
                                {
                                    if (attacking == -1 && monsterDist <= closestDist)
                                    {
                                        closestDist = monsterDist;
                                        moveToX = monsterX;
                                        moveToY = monsterY;
                                    }
                                    if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
                                    {
                                        moveDist = monsterDist;
                                        moveToX = monsterX;
                                        moveToY = monsterY;
                                        attacking = k;
                                    }
                                }
                            }
                        }
                        if (attacking == -1 && closestDist < moveDist)
                        {
                            moveDist = closestDist;
                        }
                    }
                    if (attacking >= 0 && moveDist < 1000f)
                    {
                        Projectile.friendly = true;
                        if (Projectile.ai[1] == 0 && Projectile.owner == Main.myPlayer)
                        {
                            Vector2 throwSpeed = new Vector2(moveToX, moveToY) - Projectile.Center;
                            if (throwSpeed != Vector2.Zero)
                            {
                                throwSpeed.Normalize();
                                throwSpeed *= 10f;
                            }
                            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, throwSpeed.X, throwSpeed.Y, Mod.Find<ModProjectile>("MiniHammer").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                            if (moveToX < Projectile.Center.X)
                            {
                                Projectile.direction = -1;
                            }
                            else if (moveToX > Projectile.Center.X)
                            {
                                Projectile.direction = 1;
                            }
                            Projectile.ai[1] = 100;
                            Projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        Projectile.friendly = false;
                    }
                }
                Projectile.rotation = 0f;
                Projectile.tileCollide = true;
                float increment = 0.2f;
                float maxSpeed = 3f;
                if (maxSpeed < System.Math.Abs(player.velocity.X) + System.Math.Abs(player.velocity.Y))
                {
                    increment = 0.3f;
                }
                if (moveLeft)
                {
                    Projectile.velocity.X -= increment;
                }
                else if (moveRight)
                {
                    Projectile.velocity.X += increment;
                }
                else
                {
                    Projectile.velocity.X *= 0.8f;
                    if (Projectile.velocity.X >= -increment && Projectile.velocity.X <= increment)
                    {
                        Projectile.velocity.X = 0f;
                    }
                }
                bool willCollide = false;
                if (moveLeft || moveRight)
                {
                    int checkX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                    int checkY = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
                    if (moveLeft)
                    {
                        checkX--;
                    }
                    if (moveRight)
                    {
                        checkX++;
                    }
                    checkX += (int)Projectile.velocity.X;
                    if (WorldGen.SolidTile(checkX, checkY))
                    {
                        willCollide = true;
                    }
                }
                bool playerBelow = player.position.Y + (float)player.height - 8f > Projectile.position.Y + (float)Projectile.height;
                Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY, 1, false);
                if (Projectile.velocity.Y == 0f)
                {
                    if (!playerBelow && (Projectile.velocity.X != 0f))
                    {
                        int checkX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                        int checkY = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16 + 1;
                        if (moveLeft)
                        {
                            checkX--;
                        }
                        if (moveRight)
                        {
                            checkX++;
                        }
                        WorldGen.SolidTile(checkX, checkY);
                    }
                    if (willCollide)
                    {
                        int checkX = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
                        int checkY = (int)(Projectile.position.Y + (float)Projectile.height) / 16 + 1;
                        if (WorldGen.SolidTile(checkX, checkY) || Main.tile[checkX, checkY].IsHalfBlock || Main.tile[checkX, checkY].Slope > 0)
                        {
                            try
                            {
                                checkY--;
                                if (moveLeft)
                                {
                                    checkX--;
                                }
                                if (moveRight)
                                {
                                    checkX++;
                                }
                                checkX += (int)Projectile.velocity.X;
                                if (!WorldGen.SolidTile(checkX, checkY - 1) && !WorldGen.SolidTile(checkX, checkY - 2))
                                {
                                    Projectile.velocity.Y = -5.1f;
                                }
                                else
                                {
                                    Projectile.velocity.Y = -7.1f;
                                }
                            }
                            catch
                            {
                                Projectile.velocity.Y = -7.1f;
                            }
                        }
                    }
                    else if (!Projectile.friendly && player.position.Y + player.height + 80f < Projectile.position.Y)
                    {
                        Projectile.velocity.Y = -7f;
                    }
                }
                if (Projectile.velocity.X > maxSpeed)
                {
                    Projectile.velocity.X = maxSpeed;
                }
                if (Projectile.velocity.X < -maxSpeed)
                {
                    Projectile.velocity.X = -maxSpeed;
                }
                if (Projectile.velocity.X < 0f)
                {
                    Projectile.direction = -1;
                }
                if (Projectile.velocity.X > 0f)
                {
                    Projectile.direction = 1;
                }
                if (Projectile.velocity.X != 0f || Throwing())
                {
                    Projectile.spriteDirection = -Projectile.direction;
                }
                UpdateFrame();
                if (Projectile.wet)
                {
                    Projectile.velocity *= 0.9f;
                    Projectile.velocity.Y += 0.2f;
                }
                else
                {
                    Projectile.velocity.Y += 0.4f;
                }
                if (Projectile.velocity.Y > 10f)
                {
                    Projectile.velocity.Y = 10f;
                }
            }
        }

        private bool Throwing()
        {
            return Projectile.ai[1] > 85;
        }

        private void UpdateFrame()
        {
            Projectile.alpha = 0;
            if (Projectile.ai[0] != 0)
            {
                Projectile.alpha = 70;
                Projectile.frame = 10;
                Projectile.rotation = -0.1f * Projectile.spriteDirection;
            }
            else if (Throwing())
            {
                if (Projectile.ai[1] > 93)
                {
                    Projectile.frame = 8;
                }
                else
                {
                    Projectile.frame = 9;
                }
            }
            else if (Projectile.velocity.Y != 0)
            {
                Projectile.frame = 1;
            }
            else if (Projectile.velocity.X != 0)
            {
                Projectile.frameCounter++;
                Projectile.frameCounter %= 18;
                if (Projectile.frameCounter < 3)
                {
                    Projectile.frame = 2;
                }
                else if (Projectile.frameCounter < 6)
                {
                    Projectile.frame = 3;
                }
                else if (Projectile.frameCounter < 9)
                {
                    Projectile.frame = 4;
                }
                else if (Projectile.frameCounter < 12)
                {
                    Projectile.frame = 5;
                }
                else if (Projectile.frameCounter < 15)
                {
                    Projectile.frame = 6;
                }
                else
                {
                    Projectile.frame = 7;
                }
            }
            else
            {
                Projectile.frame = 0;
            }
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }
}