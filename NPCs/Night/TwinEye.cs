using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.NPCs.Night
{
    public class TwinEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.width = 30;
            npc.height = 32;
            npc.lifeMax = 600;
            npc.damage = 90;
            npc.defense = 50;
            npc.knockBackResist = 0.75f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = Item.buyPrice(0, 0, 8, 0);
            npc.netAlways = true;
            banner = npc.type;
            bannerItem = mod.ItemType("TwinEyeBanner");
        }

        public override void AI()
        {
            if (npc.ai[0] == 0f)
            {
                if (Main.netMode == 1)
                {
                    return;
                }
                npc.ai[0] = 1 + Main.rand.Next(2);
                npc.netUpdate = true;
            }
            if (npc.ai[0] == 1f)
            {
                npc.GivenName = Language.GetTextValue("Mods.Bluemagic.NPCName.Retineye");
            }
            else
            {
                npc.GivenName = Language.GetTextValue("Mods.Bluemagic.NPCName.Spazmateye");
            }

            if (npc.collideX)
            {
                npc.velocity.X = npc.oldVelocity.X * -0.5f;
                if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                {
                    npc.velocity.X = 2f;
                }
                if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                {
                    npc.velocity.X = -2f;
                }
            }
            if (npc.collideY)
            {
                npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                {
                    npc.velocity.Y = 1f;
                }
                if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                {
                    npc.velocity.Y = -1f;
                }
            }
            if (Main.dayTime && (double)npc.position.Y <= Main.worldSurface * 16.0)
            {
                if (npc.timeLeft > 10)
                {
                    npc.timeLeft = 10;
                }
                npc.directionY = -1;
                if (npc.velocity.Y > 0f)
                {
                    npc.directionY = 1;
                }
                npc.direction = -1;
                if (npc.velocity.X > 0f)
                {
                    npc.direction = 1;
                }
            }
            else
            {
                npc.TargetClosest(true);
            }

            if (npc.ai[0] == 2f)
            {
                if (npc.direction == -1 && npc.velocity.X > -6f)
                {
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 6f)
                    {
                        npc.velocity.X -= 0.1f;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.velocity.X < -6f)
                    {
                        npc.velocity.X = -6f;
                    }
                }
                else if (npc.direction == 1 && npc.velocity.X < 6f)
                {
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -6f)
                    {
                        npc.velocity.X += 0.1f;
                    }
                    else if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    if (npc.velocity.X > 6f)
                    {
                        npc.velocity.X = 6f;
                    }
                }
                if (npc.directionY == -1 && npc.velocity.Y > -4f)
                {
                    npc.velocity.Y -= 0.1f;
                    if (npc.velocity.Y > 4f)
                    {
                        npc.velocity.Y = npc.velocity.Y - 0.1f;
                    }
                    else if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y += 0.05f;
                    }
                    if (npc.velocity.Y < -4f)
                    {
                        npc.velocity.Y = -4f;
                    }
                }
                else if (npc.directionY == 1 && npc.velocity.Y < 4f)
                {
                    npc.velocity.Y += 0.1f;
                    if (npc.velocity.Y < -4f)
                    {
                        npc.velocity.Y += 0.1f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    if (npc.velocity.Y > 4f)
                    {
                        npc.velocity.Y = 4f;
                    }
                }
            }
            else
            {
                if (npc.direction == -1 && npc.velocity.X > -4f)
                {
                    npc.velocity.X -= 0.1f;
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X -= 0.1f;
                    }
                    else if (npc.velocity.X > 0f)
                    {
                        npc.velocity.X += 0.05f;
                    }
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X = -4f;
                    }
                }
                else if (npc.direction == 1 && npc.velocity.X < 4f)
                {
                    npc.velocity.X += 0.1f;
                    if (npc.velocity.X < -4f)
                    {
                        npc.velocity.X += 0.1f;
                    }
                    else if (npc.velocity.X < 0f)
                    {
                        npc.velocity.X -= 0.05f;
                    }
                    if (npc.velocity.X > 4f)
                    {
                        npc.velocity.X = 4f;
                    }
                }
                if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5)
                {
                    npc.velocity.Y -= 0.04f;
                    if ((double)npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y -= 0.05f;
                    }
                    else if (npc.velocity.Y > 0f)
                    {
                        npc.velocity.Y += 0.03f;
                    }
                    if ((double)npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y = -1.5f;
                    }
                }
                else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5)
                {
                    npc.velocity.Y += 0.04f;
                    if ((double)npc.velocity.Y < -1.5)
                    {
                        npc.velocity.Y += 0.05f;
                    }
                    else if (npc.velocity.Y < 0f)
                    {
                        npc.velocity.Y -= 0.03f;
                    }
                    if ((double)npc.velocity.Y > 1.5)
                    {
                        npc.velocity.Y = 1.5f;
                    }
                }
            }

            if (Main.rand.Next(40) == 0)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f, 0, default(Color), 1f);
                Main.dust[dust].velocity.X *= 0.5f;
                Main.dust[dust].velocity.Y *= 0.1f;
            }
            if (npc.wet)
            {
                if (npc.velocity.Y > 0f)
                {
                    npc.velocity.Y *= 0.95f;
                }
                npc.velocity.Y -= 0.5f;
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
                npc.TargetClosest(true);
            }

            npc.ai[1] += 1f;
            if (npc.ai[1] >= 180f / npc.ai[0])
            {
                Player player = Main.player[npc.target];
                if (player.active && !player.dead)
                {
                    Vector2 distanceTo = player.Center - npc.Center;
                    float angleTo = (float)Math.Atan2(distanceTo.Y, distanceTo.X);
                    if (npc.spriteDirection == -1)
                    {
                        angleTo += (float)Math.PI;
                        angleTo %= 2f * (float)Math.PI;
                    }
                    float distance = (float)Math.Sqrt(distanceTo.X * distanceTo.X + distanceTo.Y * distanceTo.Y);
                    float toleration = (float)Math.PI;
                    if (distance > 0f)
                    {
                        toleration = 1f / distance;
                    }
                    if (toleration < 0.1f)
                    {
                        toleration = 0.1f;
                    }
                    if (Math.Abs(angleTo - npc.rotation) < toleration && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                    {
                        Vector2 unit = new Vector2((float)Math.Cos(npc.rotation), (float)Math.Sin(npc.rotation));
                        unit *= (float)npc.spriteDirection;
                        float speed = 9f;
                        int type = 83;
                        if (npc.ai[0] == 2f)
                        {
                            speed = 6f;
                            type = 96;
                        }
                        Projectile.NewProjectile(npc.Center.X + unit.X, npc.Center.Y + unit.Y, speed * unit.X, speed * unit.Y, type, 40, 0f, Main.myPlayer, 0f, 0f);
                        npc.ai[1] = 0f;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.X > 0f)
            {
                npc.spriteDirection = 1;
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
            }
            if (npc.velocity.X < 0f)
            {
                npc.spriteDirection = -1;
                npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + MathHelper.Pi;
            }
            npc.frameCounter += 1.0;
            npc.frameCounter %= 16.0;
            if (npc.frameCounter < 8.0)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.Y = frameHeight;
            }
            if (npc.ai[0] == 2f)
            {
                npc.frame.Y += frameHeight * 2;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int x = 0; x < 50; x++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)(2 * hitDirection), -2f, 0, default(Color), 1f);
                }
                int type = mod.GetGoreSlot(npc.ai[0] == 1f ? "Gores/Retineye" : "Gores/Spazmateye");
                Gore.NewGore(npc.position, npc.velocity, type, 1f);
                Gore.NewGore(new Vector2(npc.position.X, npc.position.Y + 14f), npc.velocity, type, 1f);
            }
            else
            {
                for (int x = 0; x < (double)damage / (double)npc.lifeMax * 100.0; x++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(2) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Lens);
            }
            if (Main.rand.Next(50) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BlackLens);
            }
            if (Main.rand.Next(Main.expertMode ? 80 : 100) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.MechanicalEye);
            }
        }

        public override float SpawnChance(NPCSpawnInfo info)
        {
            if (!SpawnHelper.NoBiomeNormalSpawn(info) || !BluemagicWorld.elementalUnleash)
            {
                return 0f;
            }
            return info.spawnTileY <= Main.worldSurface && !Main.dayTime ? 1f / 6f : 0f;
        }
    }
}
