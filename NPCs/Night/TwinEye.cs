using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.NPCs.Night
{
    public class TwinEye : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 30;
            NPC.height = 32;
            NPC.lifeMax = 600;
            NPC.damage = 90;
            NPC.defense = 50;
            NPC.knockBackResist = 0.75f;
            NPC.noGravity = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = Item.buyPrice(0, 0, 8, 0);
            NPC.netAlways = true;
            Banner = NPC.type;
            BannerItem = Mod.Find<ModItem>("TwinEyeBanner").Type;
        }

        public override void AI()
        {
            if (NPC.ai[0] == 0f)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    return;
                }
                NPC.ai[0] = 1 + Main.rand.Next(2);
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] == 1f)
            {
                NPC.GivenName = Language.GetTextValue("Mods.Bluemagic.NPCName.Retineye");
            }
            else
            {
                NPC.GivenName = Language.GetTextValue("Mods.Bluemagic.NPCName.Spazmateye");
            }

            if (NPC.collideX)
            {
                NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                {
                    NPC.velocity.X = 2f;
                }
                if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                {
                    NPC.velocity.X = -2f;
                }
            }
            if (NPC.collideY)
            {
                NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                {
                    NPC.velocity.Y = 1f;
                }
                if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                {
                    NPC.velocity.Y = -1f;
                }
            }
            if (Main.dayTime && (double)NPC.position.Y <= Main.worldSurface * 16.0)
            {
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                NPC.directionY = -1;
                if (NPC.velocity.Y > 0f)
                {
                    NPC.directionY = 1;
                }
                NPC.direction = -1;
                if (NPC.velocity.X > 0f)
                {
                    NPC.direction = 1;
                }
            }
            else
            {
                NPC.TargetClosest(true);
            }

            if (NPC.ai[0] == 2f)
            {
                if (NPC.direction == -1 && NPC.velocity.X > -6f)
                {
                    NPC.velocity.X -= 0.1f;
                    if (NPC.velocity.X > 6f)
                    {
                        NPC.velocity.X -= 0.1f;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X += 0.05f;
                    }
                    if (NPC.velocity.X < -6f)
                    {
                        NPC.velocity.X = -6f;
                    }
                }
                else if (NPC.direction == 1 && NPC.velocity.X < 6f)
                {
                    NPC.velocity.X += 0.1f;
                    if (NPC.velocity.X < -6f)
                    {
                        NPC.velocity.X += 0.1f;
                    }
                    else if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X -= 0.05f;
                    }
                    if (NPC.velocity.X > 6f)
                    {
                        NPC.velocity.X = 6f;
                    }
                }
                if (NPC.directionY == -1 && NPC.velocity.Y > -4f)
                {
                    NPC.velocity.Y -= 0.1f;
                    if (NPC.velocity.Y > 4f)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - 0.1f;
                    }
                    else if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y += 0.05f;
                    }
                    if (NPC.velocity.Y < -4f)
                    {
                        NPC.velocity.Y = -4f;
                    }
                }
                else if (NPC.directionY == 1 && NPC.velocity.Y < 4f)
                {
                    NPC.velocity.Y += 0.1f;
                    if (NPC.velocity.Y < -4f)
                    {
                        NPC.velocity.Y += 0.1f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y -= 0.05f;
                    }
                    if (NPC.velocity.Y > 4f)
                    {
                        NPC.velocity.Y = 4f;
                    }
                }
            }
            else
            {
                if (NPC.direction == -1 && NPC.velocity.X > -4f)
                {
                    NPC.velocity.X -= 0.1f;
                    if (NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X -= 0.1f;
                    }
                    else if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X += 0.05f;
                    }
                    if (NPC.velocity.X < -4f)
                    {
                        NPC.velocity.X = -4f;
                    }
                }
                else if (NPC.direction == 1 && NPC.velocity.X < 4f)
                {
                    NPC.velocity.X += 0.1f;
                    if (NPC.velocity.X < -4f)
                    {
                        NPC.velocity.X += 0.1f;
                    }
                    else if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X -= 0.05f;
                    }
                    if (NPC.velocity.X > 4f)
                    {
                        NPC.velocity.X = 4f;
                    }
                }
                if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5)
                {
                    NPC.velocity.Y -= 0.04f;
                    if ((double)NPC.velocity.Y > 1.5)
                    {
                        NPC.velocity.Y -= 0.05f;
                    }
                    else if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y += 0.03f;
                    }
                    if ((double)NPC.velocity.Y < -1.5)
                    {
                        NPC.velocity.Y = -1.5f;
                    }
                }
                else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5)
                {
                    NPC.velocity.Y += 0.04f;
                    if ((double)NPC.velocity.Y < -1.5)
                    {
                        NPC.velocity.Y += 0.05f;
                    }
                    else if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y -= 0.03f;
                    }
                    if ((double)NPC.velocity.Y > 1.5)
                    {
                        NPC.velocity.Y = 1.5f;
                    }
                }
            }

            if (Main.rand.Next(40) == 0)
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f), NPC.width, (int)((float)NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f, 0, default(Color), 1f);
                Main.dust[dust].velocity.X *= 0.5f;
                Main.dust[dust].velocity.Y *= 0.1f;
            }
            if (NPC.wet)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.95f;
                }
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -4f)
                {
                    NPC.velocity.Y = -4f;
                }
                NPC.TargetClosest(true);
            }

            NPC.ai[1] += 1f;
            if (NPC.ai[1] >= 180f / NPC.ai[0])
            {
                Player player = Main.player[NPC.target];
                if (player.active && !player.dead)
                {
                    Vector2 distanceTo = player.Center - NPC.Center;
                    float angleTo = (float)Math.Atan2(distanceTo.Y, distanceTo.X);
                    if (NPC.spriteDirection == -1)
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
                    if (Math.Abs(angleTo - NPC.rotation) < toleration && Collision.CanHit(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
                    {
                        Vector2 unit = new Vector2((float)Math.Cos(NPC.rotation), (float)Math.Sin(NPC.rotation));
                        unit *= (float)NPC.spriteDirection;
                        float speed = 9f;
                        int type = 83;
                        if (NPC.ai[0] == 2f)
                        {
                            speed = 6f;
                            type = 96;
                        }
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X + unit.X, NPC.Center.Y + unit.Y, speed * unit.X, speed * unit.Y, type, 40, 0f, Main.myPlayer, 0f, 0f);
                        NPC.ai[1] = 0f;
                    }
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.velocity.X > 0f)
            {
                NPC.spriteDirection = 1;
                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
            }
            if (NPC.velocity.X < 0f)
            {
                NPC.spriteDirection = -1;
                NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + MathHelper.Pi;
            }
            NPC.frameCounter += 1.0;
            NPC.frameCounter %= 16.0;
            if (NPC.frameCounter < 8.0)
            {
                NPC.frame.Y = 0;
            }
            else
            {
                NPC.frame.Y = frameHeight;
            }
            if (NPC.ai[0] == 2f)
            {
                NPC.frame.Y += frameHeight * 2;
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int x = 0; x < 50; x++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)(2 * hit.HitDirection), -2f, 0, default(Color), 1f);
                }
                int type = Mod.Find<ModGore>(NPC.ai[0] == 1f ? "Retineye" : "Spazmateye").Type;
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), new Vector2(NPC.position.X, NPC.position.Y + 14f), NPC.velocity, type, 1f);
            }
            else
            {
                for (int x = 0; x < (double)hit.Damage / (double)NPC.lifeMax * 100.0; x++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)hit.HitDirection, -1f, 0, default(Color), 1f);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Lens, 2));
            npcLoot.Add(ItemDropRule.Common(ItemID.BlackLens, 50));
            npcLoot.Add(ItemDropRule.NormalvsExpert(ItemID.MechanicalEye, 100, 80));
        }

        public override float SpawnChance(NPCSpawnInfo info)
        {
            if (!SpawnHelper.NoBiomeNormalSpawn(info) || !BluemagicWorld.elementalUnleash)
            {
                return 0f;
            }
            return info.SpawnTileY <= Main.worldSurface && !Main.dayTime ? 1f / 6f : 0f;
        }
    }
}
