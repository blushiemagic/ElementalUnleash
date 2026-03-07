using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PurityShield : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shield of Purity");
            Main.npcFrameCount[NPC.type] = 4;
            NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = PuritySpirit.dpsCap;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax / Main.GameModeInfo.EnemyMaxLifeMultiplier * balance);
            NPC.defense = 102;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void AI()
        {
            NPC owner = Main.npc[(int)NPC.ai[0]];
            if (!owner.active || owner.type != Mod.Find<ModNPC>("PuritySpirit").Type)
            {
                NPC.active = false;
                return;
            }
            PuritySpirit modOwner = (PuritySpirit)owner.ModNPC;
            if (NPC.localAI[0] == 0f)
            {
                if (modOwner.targets.Contains(Main.myPlayer))
                {
                    SoundEngine.PlaySound(SoundID.Item2);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item2, NPC.position);
                }
                NPC.localAI[0] = 1f;
            }
            Vector2 targetPos = new Vector2(NPC.ai[1], NPC.ai[2]);
            Vector2 direction = targetPos - NPC.Center;
            if (direction != Vector2.Zero)
            {
                float speed = direction.Length();
                if (speed > 2f)
                {
                    speed = 2f;
                }
                direction.Normalize();
                direction *= speed;
                NPC.position += direction;
            }
            else
            {
                NPC.localAI[1] = 1f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 1.0;
            NPC.frameCounter %= 120.0;
            NPC.frame.Y = frameHeight * (((int)NPC.frameCounter % 20) / 5);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                int gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(10f, 0f), Vector2.Zero, Main.rand.Next(435, 438), 2f);
                Main.gore[gore].velocity *= 0.3f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(50f, 10f), Vector2.Zero, Main.rand.Next(435, 438), 2f);
                Main.gore[gore].velocity *= 0.3f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0f, 60f), Vector2.Zero, Main.rand.Next(435, 438), 2f);
                Main.gore[gore].velocity *= 0.3f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(40f, 50f), Vector2.Zero, Main.rand.Next(435, 438), 2f);
                Main.gore[gore].velocity *= 0.3f;
                gore = Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(30f, 30f), Vector2.Zero, Main.rand.Next(435, 438), 2f);
                Main.gore[gore].velocity *= 0.3f;
            }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return CanBeHitByPlayer(player);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return CanBeHitByPlayer(Main.player[projectile.owner]);
        }

        private bool? CanBeHitByPlayer(Player player)
        {
            NPC owner = Main.npc[(int)NPC.ai[0]];
            PuritySpirit modOwner = owner.ModNPC == null ? null : owner.ModNPC as PuritySpirit;
            if (modOwner != null && !modOwner.targets.Contains(player.whoAmI))
            {
                return false;
            }
            return null;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Vector2 end1 = NPC.Center;
            Vector2 end2 = Main.npc[(int)NPC.ai[0]].Center;
            Texture2D texture;
            if (end1 != end2)
            {
                float length = Vector2.Distance(end1, end2);
                Vector2 direction = end2 - end1;
                direction.Normalize();
                float start = (float)NPC.frameCounter % 8f;
                start *= 2f;
                if (NPC.localAI[1] == 0f)
                {
                    start *= 2f;
                    start %= 16f;
                }
                texture = ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PurityShieldChain").Value;
                for (float k = start; k <= length; k += 16f)
                {
                    Main.spriteBatch.Draw(texture, end1 + k * direction - Main.screenPosition, null, Color.White, 0f, new Vector2(16f, 16f), 1f, SpriteEffects.None, 0f);
                }
            }
            texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 drawPos = NPC.Center - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2 / Main.npcFrameCount[NPC.type]);
            Main.spriteBatch.Draw(texture, drawPos, NPC.frame, Color.White, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/PuritySpirit/PurityShieldGlow").Value;
            Vector2 drawPos = NPC.position - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 25);
            frame.Y = (int)NPC.frameCounter % 60;
            if (frame.Y > 24)
            {
                frame.Y = 24;
            }
            frame.Y *= NPC.height;
            Main.spriteBatch.Draw(texture, drawPos, frame, Color.White * 0.7f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}