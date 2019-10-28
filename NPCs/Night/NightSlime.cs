using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.NPCs.Night
{
    public class NightSlime : ModNPC
    {
        private static Random sparkleRand = new Random();
        private int sparkleFrame = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = 1;
            npc.width = 36;
            npc.height = 24;
            npc.lifeMax = 600;
            npc.damage = 80;
            npc.defense = 50;
            npc.knockBackResist = 0.6f;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.value = Item.buyPrice(0, 0, 8, 0);
            npc.npcSlots = 1;
            npc.color = new Color(0, 0, 0, 50);
            npc.alpha = 120;
            animationType = 16;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            banner = npc.type;
            bannerItem = mod.ItemType("NightSlimeBanner");
        }

        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (npc.type == mod.NPCType("NightSlime") && Main.rand.Next(20) == 0)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.White, 1.5f);
            }
            npc.ai[0] += 2f;
            if (npc.localAI[0] > 0f)
            {
                npc.localAI[0] -= 1f;
            }
            Vector2 center = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
            float shootToX = player.position.X + (float)player.width * 0.5f - center.X;
            float shootToY = player.position.Y - center.Y;
            float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
            if (distance < 480f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
            {
                if (npc.velocity.Y == 0f)
                {
                    npc.velocity.X *= 0.9f;
                }
                if (Main.netMode != 1 && npc.localAI[0] == 0f)
                {
                    int projType = mod.ProjectileType("StarGel");
                    int damage = 35;
                    if (npc.type == mod.NPCType("DirtySlime"))
                    {
                        projType = mod.ProjectileType("DirtGel");
                        damage = 45;
                    }
                    distance = 3f / distance;
                    shootToX *= distance;
                    shootToY *= distance;
                    npc.localAI[0] = 120f;
                    Projectile.NewProjectile(center.X, center.Y, shootToX, shootToY, projType, damage, 0f, Main.myPlayer, 0f, 0f);
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            int chance = 10;
            int time = 240;
            if (npc.type == mod.NPCType("DirtySlime"))
            {
                chance = 9;
                time = 300;
            }
            if (Main.rand.Next(chance) == 0)
            {
                target.AddBuff(mod.BuffType("Liquified"), time, true);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.type == mod.NPCType("NightSlime"))
            {
                Texture2D sparkleTexture = mod.GetTexture("NPCs/Night/NightSlimeSparkle");
                frameHeight = sparkleTexture.Height / 3;
                sparkleFrame = (sparkleRand.Next(2) * frameHeight + sparkleFrame) % (3 * frameHeight);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            Color color = npc.color;
            if (npc.type == mod.NPCType("DirtySlime"))
            {
                color = new Color(178, 152, 71);
            }
            if (npc.life <= 0)
            {
                for (int k = 0; k < 50; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, color, 1f);
                    if (k % 5 == 0 && npc.type == mod.NPCType("NightSlime"))
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.White, 1.5f);
                    }
                }
            }
            else
            {
                int count = 0;
                while (count < damage / npc.lifeMax * 100)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, color, 1f);
                    if (count % 5 == 0 && npc.type == mod.NPCType("NightSlime"))
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"), 0f, 0f, 0, Color.White, 1.5f);
                        Main.dust[dust].velocity.X += hitDirection * 3;
                    }
                    count++;
                }
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(4, 7));
            if (Main.rand.Next(Main.expertMode ? 4200 : 6000) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlimeStaff);
            }
            if (Main.rand.Next(Main.expertMode ? 3 : 2) != 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SuspiciousGel"));
            }
        }

        public override float SpawnChance(NPCSpawnInfo info)
        {
            if (!SpawnHelper.NoBiomeNormalSpawn(info) || !BluemagicWorld.elementalUnleash)
            {
                return 0f;
            }
            if (npc.type == mod.NPCType("NightSlime"))
            {
                return info.spawnTileY <= Main.worldSurface && !Main.dayTime ? 1f / 6f : 0f;
            }
            if (npc.type == mod.NPCType("DirtySlime"))
            {
                return info.spawnTileY > Main.worldSurface && info.spawnTileY < Main.maxTilesY - 190 ? 1f / 6f : 0f;
            }
            return 0f;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.type == mod.NPCType("NightSlime"))
            {
                Texture2D sparkleTexture = mod.GetTexture("NPCs/Night/NightSlimeSparkle");
                spriteBatch.Draw(sparkleTexture, npc.position - Main.screenPosition, new Rectangle(0, sparkleFrame, sparkleTexture.Width, sparkleTexture.Height / 3), Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
        }
    }
}
