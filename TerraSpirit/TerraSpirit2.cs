using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Reflection;
using Bluemagic.PuritySpirit;

namespace Bluemagic.TerraSpirit
{
    public class TerraSpirit2 : ModNPC
    {
        private const int size = 120;
        private const int particleSize = 12;
        public const int arenaWidth = 2400;
        public const int arenaHeight = 1600;

        internal int Stage
        {
            get
            {
                return (int)NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = value;
            }
        }

        internal int Progress
        {
            get
            {
                return (int)NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spirit of Purity");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = false;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 750000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.width = size;
            NPC.height = size;
            NPC.npcSlots = 1337f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.chaseable = false;
            NPC.HitSound = SoundID.Roar;
            NPC.DeathSound = null;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Music = MusicID.LunarBoss;
        }

        private IList<Particle> particles = new List<Particle>();
        private float[,] aura = new float[size, size];
        internal List<Bullet> bullets = new List<Bullet>();

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 2f;
            return null;
        }

        public override void AI()
        {
            int numPlayers = CountPlayers();
            if (!Main.dedServ)
            {
                UpdateParticles();
            }
            NPC.timeLeft = NPC.activeTime;
            if (Stage >= 0 && numPlayers == 0)
            {
                Stage = -1;
                Progress = 0;
            }
            if (Stage == -1)
            {
                RunAway();
            }
            else if (Stage == 0)
            {
                Initialize();
            }
            else if (Stage == 1)
            {
                Recover();
            }
            else if (Stage == 2)
            {
                Attack();
            }
            else if (Stage == 3)
            {
                End();
            }
            Progress++;
            Rectangle bounds = new Rectangle((int)NPC.Center.X - arenaWidth / 2, (int)NPC.Center.Y - arenaHeight / 2, arenaWidth, arenaHeight);
            for (int k = 0; k < bullets.Count; k++)
            {
                if (bullets[k].Update(null, bounds))
                {
                    Player player = Main.player[Main.myPlayer];
                    if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && bullets[k].Collides(player.Hitbox))
                    {
                        player.GetModPlayer<BluemagicPlayer>().TerraKill();
                    }
                }
                else
                {
                    bullets.RemoveAt(k);
                    k--;
                }
            }
        }

        private int CountPlayers()
        {
            int count = 0;
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
                {
                    count++;
                }
            }
            return count;
        }

        private void UpdateParticles()
        {
            foreach (Particle particle in particles)
            {
                particle.Update();
            }
            Vector2 newPos = new Vector2(Main.rand.Next(3 * size / 8, 5 * size / 8), Main.rand.Next(3 * size / 8, 5 * size / 8));
            double newAngle = 2 * Math.PI * Main.rand.NextDouble();
            Vector2 newVel = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
            newVel *= 0.5f * (1f + (float)Main.rand.NextDouble());
            particles.Add(new Particle(newPos, newVel));
            if (particles[0].strength <= 0f)
            {
                particles.RemoveAt(0);
            }
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    aura[x, y] *= 0.97f;
                }
            }
            foreach (Particle particle in particles)
            {
                int minX = (int)particle.position.X - particleSize / 2;
                int minY = (int)particle.position.Y - particleSize / 2;
                int maxX = minX + particleSize;
                int maxY = minY + particleSize;
                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        if (x >= 0 && x < size && y >= 0 && y < size)
                        {
                            float strength = particle.strength;
                            float offX = particle.position.X - x;
                            float offY = particle.position.Y - y;
                            strength *= 1f - (float)Math.Sqrt(offX * offX + offY * offY) / particleSize * 2;
                            if (strength < 0f)
                            {
                                strength = 0f;
                            }
                            aura[x, y] = 1f - (1f - aura[x, y]) * (1f - strength);
                        }
                    }
                }
            }
        }

        public void RunAway()
        {
            if (Progress >= 180)
            {
                NPC.active = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    BluemagicWorld.terraDeaths++;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                }
            }
        }

        public void Initialize()
        {
            if (Progress == Main.netMode)
            {
                bullets.Clear();
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.NewText("The Spirit of Purity is losing control!");
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Bluemagic.TerraSpiritExpert"), Color.White);
                }
                NPC.life = 1;
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>().terraLives += 3;
                    Main.NewText("You have been granted 3 extra lives!");
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.Bluemagic.ExtraLives"), Color.White);
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)MessageType.ExtraLives);
                    packet.Send();
                }
            }
            if (Progress >= 120)
            {
                Stage++;
                Progress = -1;
            }
        }

        private void Recover()
        {
            NPC.life = 1 + 4321 * Progress;
            if (NPC.life >= NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
                Stage++;
                Progress = -1;
            }
        }

        private void Attack()
        {
            Progress %= 300;
            if (Progress == 0 || (Main.expertMode && Progress == 150))
            {
                /*Player target = null;
                float distance = 0f;
                for (int k = 0; k < 255; k++)
                {
                    if (Main.player[k].active && !Main.player[k].dead)
                    {
                        float temp = Vector2.Distance(Main.player[k].Center, npc.Center);
                        if (target == null || temp < distance)
                        {
                            target = Main.player[k];
                            distance = temp;
                        }
                    }
                }
                if (target != null)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height - 20, mod.NPCType("NegativeBlob"), 0, npc.whoAmI, (target.Center - npc.Center).ToRotation());
                }*/
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height - 20, Mod.Find<ModNPC>("NegativeBlob").Type, 0, NPC.whoAmI, Main.rand.NextFloat() * MathHelper.TwoPi);
            }
            NPC.life += Main.expertMode ? 5 : 1;
            if (NPC.life > NPC.lifeMax)
            {
                NPC.life = NPC.lifeMax;
            }
        }

        private void End()
        {
            NPC.dontTakeDamage = true;
            bullets.Clear();
            if (Progress == 1)
            {
                SoundEngine.PlaySound(SoundID.Zombie92);
            }
            if (Progress >= 420)
            {
                BluemagicWorld.downedTerraSpirit = true;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.WorldData);
                }
                // Todo: Port to item drop database
                Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("PuriumCoin").Type, Main.expertMode ? Main.rand.Next(20, 25) : Main.rand.Next(10, 13));
                Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("RainbowStar").Type);
                if (Main.expertMode)
                {
                    Item.NewItem(NPC.GetSource_Death(), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("BlushieCharm").Type);
                }
                NPC.active = false;
            }
        }

        public override bool CheckDead()
        {
            Stage = 3;
            Progress = 0;
            NPC.life = NPC.lifeMax;
            NPC.dontTakeDamage = true;
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (Stage != 3)
            {
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/NegativeCircle").Value, NPC.Center - Main.screenPosition, null, Color.White * 0.25f, 0f, new Vector2(120f, 120f), 1f, SpriteEffects.None, 0f);
            }
            float scale = 1f;
            if (Stage == 3)
            {
                scale += 9f * Progress / 400f;
            }
            float alpha = 1f;
            alpha -= 0.8f * (scale - 1f) / 9f;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 drawPos = NPC.Center - Main.screenPosition;
                    drawPos.X += scale * (x * 2 - size);
                    drawPos.Y += scale * (y * 2 - size);
                    Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/NegativeParticle").Value, drawPos, null, Color.White * aura[x, y] * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PurityEyes").Value, NPC.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            const int blockSize = 16;
            int centerX = (int)NPC.Center.X;
            int centerY = (int)NPC.Center.Y;
            Texture2D outlineTexture = ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/TerraBlockOutline").Value;
            Texture2D blockTexture = ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/TerraBlock").Value;
            Texture2D outlineSpike = ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/TerraSpikeOutline").Value;
            Texture2D spike = ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/TerraSpike").Value;
            float spikeAlpha = 1f;
            if (Stage == 0)
            {
                spikeAlpha = (Progress % 80f) / 40f;
                if (spikeAlpha > 1f)
                {
                    spikeAlpha = 2f - spikeAlpha;
                }
            }
            Vector2 half = new Vector2(8, 8);
            for (int x = centerX - (arenaWidth + blockSize) / 2; x <= centerX + (arenaWidth + blockSize) / 2; x += blockSize)
            {
                int y = centerY - (arenaHeight + blockSize) / 2;
                Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
                Main.spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                Main.spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                Main.spriteBatch.Draw(outlineSpike, drawPos + new Vector2(0f, blockSize) + half, null, Color.White * spikeAlpha, MathHelper.Pi, half, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(spike, drawPos + new Vector2(0f, blockSize) + half, null, Color.White * spikeAlpha * 0.75f, MathHelper.Pi, half, 1f, SpriteEffects.None, 0f);
                drawPos.Y += arenaHeight + blockSize;
                Main.spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                Main.spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                Main.spriteBatch.Draw(outlineSpike, drawPos + new Vector2(0f, -blockSize), Color.White * spikeAlpha);
                Main.spriteBatch.Draw(spike, drawPos + new Vector2(0f, -blockSize), Color.White * spikeAlpha * 0.75f);
            }
            for (int y = centerY - (arenaHeight + blockSize) / 2; y <= centerY + (arenaHeight + blockSize) / 2; y += blockSize)
            {
                int x = centerX - (arenaWidth + blockSize) / 2;
                Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
                Main.spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                Main.spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                Main.spriteBatch.Draw(outlineSpike, drawPos + new Vector2(blockSize, 0f) + half, null, Color.White * spikeAlpha, MathHelper.PiOver2, half, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(spike, drawPos + new Vector2(blockSize, 0f) + half, null, Color.White * spikeAlpha * 0.75f, MathHelper.PiOver2, half, 1f, SpriteEffects.None, 0f);
                drawPos.X += arenaWidth + blockSize;
                Main.spriteBatch.Draw(outlineTexture, drawPos, Color.White);
                Main.spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
                Main.spriteBatch.Draw(outlineSpike, drawPos + new Vector2(-blockSize, 0f) + half, null, Color.White * spikeAlpha, -MathHelper.PiOver2, half, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(spike, drawPos + new Vector2(-blockSize, 0f) + half, null, Color.White * spikeAlpha * 0.75f, -MathHelper.PiOver2, half, 1f, SpriteEffects.None, 0f);
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
