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
            Main.projFrames[projectile.type] = 11;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.penetrate = -1;
            projectile.timeLeft = 18000;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
        }

        public override void CheckActive()
        {
            Player player = Main.player[projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.paladinMinion = false;
            }
            if (modPlayer.paladinMinion)
            {
                projectile.timeLeft = 2;
            }
        }

        public override void Behavior()
        {
            Player player = Main.player[projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (projectile.ai[1] > 0)
            {
                projectile.ai[1]--;
            }
            bool moveLeft = false;
            bool moveRight = false;
            int targetFollowDist = 40 * (projectile.minionPos + 1) * player.direction;
            if (player.position.X + (float)(player.width / 2) < projectile.position.X + (float)(projectile.width / 2) + (float)targetFollowDist - 10f)
            {
                moveLeft = true;
            }
            else if (player.position.X + (float)(player.width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)targetFollowDist + 10f)
            {
                moveRight = true;
            }
            if (!Throwing())
            {
                int flyDistance = 500 + 40 * projectile.minionPos;
                if (player.rocketDelay2 > 0)
                {
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
                Vector2 projCenter = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float xDist = player.position.X + (float)(player.width / 2) - projCenter.X;
                float yDist = player.position.Y + (float)(player.height / 2) - projCenter.Y;
                float distance = (float)System.Math.Sqrt((double)(xDist * xDist + yDist * yDist));
                if (distance > 2000f)
                {
                    projectile.position.X = player.position.X + (float)(player.width / 2) - (float)(projectile.width / 2);
                    projectile.position.Y = player.position.Y + (float)(player.height / 2) - (float)(projectile.height / 2);
                }
                else if (distance > (float)flyDistance || System.Math.Abs(yDist) > 300f)
                {
                    if (yDist > 0f && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    if (yDist < 0f && projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y = 0f;
                    }
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                }
            }
            if (Throwing())
            {
                UpdateFrame();
                projectile.velocity.X = 0f;
            }
            else if (projectile.ai[0] != 0f)
            {
                projectile.tileCollide = false;
                Vector2 projCenter = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
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
                    moveDistX -= (float)(40 * projectile.minionPos * player.direction);
                }
                float moveDistY = player.position.Y + (float)(player.height / 2) - projCenter.Y;
                float moveDist = (float)System.Math.Sqrt((double)(moveDistX * moveDistX + moveDistY * moveDistY));
                float maxSpeed = 10f;
                if (moveDist < 200f && player.velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= player.position.Y + (float)player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
                {
                    projectile.ai[0] = 0f;
                    if (projectile.velocity.Y < -6f)
                    {
                        projectile.velocity.Y = -6f;
                    }
                    projectile.netUpdate = true;
                }
                if (moveDist < 60f)
                {
                    moveDistX = projectile.velocity.X;
                    moveDistY = projectile.velocity.Y;
                }
                else
                {
                    moveDist = maxSpeed / moveDist;
                    moveDistX *= moveDist;
                    moveDistY *= moveDist;
                }
                float acceleration = 0.2f;
                if (projectile.velocity.X < moveDistX)
                {
                    projectile.velocity.X += acceleration;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.velocity.X += acceleration * 1.5f;
                    }
                }
                if (projectile.velocity.X > moveDistX)
                {
                    projectile.velocity.X -= acceleration;
                    if (projectile.velocity.X > 0f)
                    {
                        projectile.velocity.X -= acceleration * 1.5f;
                    }
                }
                if (projectile.velocity.Y < moveDistY)
                {
                    projectile.velocity.Y += acceleration;
                    if (projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y += acceleration * 1.5f;
                    }
                }
                if (projectile.velocity.Y > moveDistY)
                {
                    projectile.velocity.Y -= acceleration;
                    if (projectile.velocity.Y > 0f)
                    {
                        projectile.velocity.Y -= acceleration * 1.5f;
                    }
                }
                if ((double)projectile.velocity.X > 0.5)
                {
                    projectile.spriteDirection = -1;
                }
                else if ((double)projectile.velocity.X < -0.5)
                {
                    projectile.spriteDirection = 1;
                }
                UpdateFrame();
                if (Main.rand.Next(3) == 0)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("SpectreDust"));
                    Main.dust[dust].velocity /= 2f;
                }
            }
            else
            {
                float separation = (float)(40 * projectile.minionPos);
                if (true)
                {
                    float moveToX = projectile.position.X;
                    float moveToY = projectile.position.Y;
                    float moveDist = 100000f;
                    int attacking = -1;
                    if (projectile.OwnerMinionAttackTargetNPC != null && projectile.OwnerMinionAttackTargetNPC.CanBeChasedBy(this))
                    {
                        NPC npc = projectile.OwnerMinionAttackTargetNPC;
                        moveToX = npc.Center.X;
                        moveToY = npc.Center.Y;
                        moveDist = Vector2.Distance(npc.Center, projectile.Center);
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
                                float monsterDist = System.Math.Abs(projectile.position.X + (float)(projectile.width / 2) - monsterX) + System.Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - monsterY);
                                if (monsterDist < moveDist)
                                {
                                    if (attacking == -1 && monsterDist <= closestDist)
                                    {
                                        closestDist = monsterDist;
                                        moveToX = monsterX;
                                        moveToY = monsterY;
                                    }
                                    if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[k].position, Main.npc[k].width, Main.npc[k].height))
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
                        projectile.friendly = true;
                        if (projectile.ai[1] == 0 && projectile.owner == Main.myPlayer)
                        {
                            Vector2 throwSpeed = new Vector2(moveToX, moveToY) - projectile.Center;
                            if (throwSpeed != Vector2.Zero)
                            {
                                throwSpeed.Normalize();
                                throwSpeed *= 10f;
                            }
                            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, throwSpeed.X, throwSpeed.Y, mod.ProjectileType("MiniHammer"), projectile.damage, projectile.knockBack, projectile.owner);
                            if (moveToX < projectile.Center.X)
                            {
                                projectile.direction = -1;
                            }
                            else if (moveToX > projectile.Center.X)
                            {
                                projectile.direction = 1;
                            }
                            projectile.ai[1] = 100;
                            projectile.netUpdate = true;
                        }
                    }
                    else
                    {
                        projectile.friendly = false;
                    }
                }
                projectile.rotation = 0f;
                projectile.tileCollide = true;
                float increment = 0.2f;
                float maxSpeed = 3f;
                if (maxSpeed < System.Math.Abs(player.velocity.X) + System.Math.Abs(player.velocity.Y))
                {
                    increment = 0.3f;
                }
                if (moveLeft)
                {
                    projectile.velocity.X -= increment;
                }
                else if (moveRight)
                {
                    projectile.velocity.X += increment;
                }
                else
                {
                    projectile.velocity.X *= 0.8f;
                    if (projectile.velocity.X >= -increment && projectile.velocity.X <= increment)
                    {
                        projectile.velocity.X = 0f;
                    }
                }
                bool willCollide = false;
                if (moveLeft || moveRight)
                {
                    int checkX = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                    int checkY = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
                    if (moveLeft)
                    {
                        checkX--;
                    }
                    if (moveRight)
                    {
                        checkX++;
                    }
                    checkX += (int)projectile.velocity.X;
                    if (WorldGen.SolidTile(checkX, checkY))
                    {
                        willCollide = true;
                    }
                }
                bool playerBelow = player.position.Y + (float)player.height - 8f > projectile.position.Y + (float)projectile.height;
                Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY, 1, false);
                if (projectile.velocity.Y == 0f)
                {
                    if (!playerBelow && (projectile.velocity.X != 0f))
                    {
                        int checkX = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                        int checkY = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16 + 1;
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
                        int checkX = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
                        int checkY = (int)(projectile.position.Y + (float)projectile.height) / 16 + 1;
                        if (WorldGen.SolidTile(checkX, checkY) || Main.tile[checkX, checkY].halfBrick() || Main.tile[checkX, checkY].slope() > 0)
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
                                checkX += (int)projectile.velocity.X;
                                if (!WorldGen.SolidTile(checkX, checkY - 1) && !WorldGen.SolidTile(checkX, checkY - 2))
                                {
                                    projectile.velocity.Y = -5.1f;
                                }
                                else
                                {
                                    projectile.velocity.Y = -7.1f;
                                }
                            }
                            catch
                            {
                                projectile.velocity.Y = -7.1f;
                            }
                        }
                    }
                    else if (!projectile.friendly && player.position.Y + player.height + 80f < projectile.position.Y)
                    {
                        projectile.velocity.Y = -7f;
                    }
                }
                if (projectile.velocity.X > maxSpeed)
                {
                    projectile.velocity.X = maxSpeed;
                }
                if (projectile.velocity.X < -maxSpeed)
                {
                    projectile.velocity.X = -maxSpeed;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.direction = -1;
                }
                if (projectile.velocity.X > 0f)
                {
                    projectile.direction = 1;
                }
                if (projectile.velocity.X != 0f || Throwing())
                {
                    projectile.spriteDirection = -projectile.direction;
                }
                UpdateFrame();
                if (projectile.wet)
                {
                    projectile.velocity *= 0.9f;
                    projectile.velocity.Y += 0.2f;
                }
                else
                {
                    projectile.velocity.Y += 0.4f;
                }
                if (projectile.velocity.Y > 10f)
                {
                    projectile.velocity.Y = 10f;
                }
            }
        }

        private bool Throwing()
        {
            return projectile.ai[1] > 85;
        }

        private void UpdateFrame()
        {
            projectile.alpha = 0;
            if (projectile.ai[0] != 0)
            {
                projectile.alpha = 70;
                projectile.frame = 10;
                projectile.rotation = -0.1f * projectile.spriteDirection;
            }
            else if (Throwing())
            {
                if (projectile.ai[1] > 93)
                {
                    projectile.frame = 8;
                }
                else
                {
                    projectile.frame = 9;
                }
            }
            else if (projectile.velocity.Y != 0)
            {
                projectile.frame = 1;
            }
            else if (projectile.velocity.X != 0)
            {
                projectile.frameCounter++;
                projectile.frameCounter %= 18;
                if (projectile.frameCounter < 3)
                {
                    projectile.frame = 2;
                }
                else if (projectile.frameCounter < 6)
                {
                    projectile.frame = 3;
                }
                else if (projectile.frameCounter < 9)
                {
                    projectile.frame = 4;
                }
                else if (projectile.frameCounter < 12)
                {
                    projectile.frame = 5;
                }
                else if (projectile.frameCounter < 15)
                {
                    projectile.frame = 6;
                }
                else
                {
                    projectile.frame = 7;
                }
            }
            else
            {
                projectile.frame = 0;
            }
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }
}