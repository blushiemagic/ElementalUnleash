using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Abomination
{
    [AutoloadBossHead]
    public class CaptiveElement : ModNPC
    {
        private int difficulty
        {
            get
            {
                int result = 0;
                if (Main.expertMode)
                {
                    result++;
                }
                if (NPC.downedMoonlord)
                {
                    result++;
                }
                return result;
            }
        }

        private int center
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

        private int captiveType
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

        private float attackCool
        {
            get
            {
                return NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        private int change
        {
            get
            {
                return (int)NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 10;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 15000;
            NPC.damage = 100;
            NPC.defense = 55;
            if (NPC.downedMoonlord)
            {
                NPC.lifeMax = 30000;
                NPC.damage = 120;
                NPC.defense = 80;
            }
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 10f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss2;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }

        public override void AI()
        {
            NPC abomination = Main.npc[center];
            if (!abomination.active || abomination.type != Mod.Find<ModNPC>("Abomination").Type)
            {
                if (change > 0 || NPC.AnyNPCs(Mod.Find<ModNPC>("AbominationRun").Type))
                {
                    if (change == 0)
                    {
                        NPC.netUpdate = true;
                    }
                    change++;
                }
                else
                {
                    NPC.life = -1;
                    NPC.active = false;
                    return;
                }
            }
            if (change > 0)
            {
                Color? color = GetColor();
                if (color.HasValue)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                        double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                        Main.dust[dust].velocity = 3f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient && change >= 100f)
                {
                    int next = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.position.Y + NPC.height, Mod.Find<ModNPC>("CaptiveElement2").Type);
                    Main.npc[next].ai[0] = captiveType;
                    if (captiveType != 4)
                    {
                        Main.npc[next].ai[1] = 300f + (float)Main.rand.Next(100);
                    }
                    NPC.life = -1;
                    NPC.active = false;
                }
                return;
            }
            else if (NPC.timeLeft < 750)
            {
                NPC.timeLeft = 750;
            }
            if (NPC.localAI[0] == 0f)
            {
                if (GetDebuff() >= 0f)
                {
                    NPC.buffImmune[GetDebuff()] = true;
                }
                if (captiveType == 2f)
                {
                    NPC.damage += 20;
                    if (Main.expertMode)
                    {
                        NPC.damage += 20;
                    }
                }
                if (captiveType == 3f)
                {
                    NPC.buffImmune[20] = true;
                }
                if (captiveType == 0f)
                {
                    NPC.coldDamage = true;
                }
                NPC.localAI[0] = 1f;
            }
            SetPosition(NPC);
            attackCool -= 1f;
            if (Main.netMode != NetmodeID.MultiplayerClient && difficulty > 1 && attackCool > 0f && attackCool <= 60f && (int)Math.Ceiling(attackCool) % 30 == 0)
            {
                Shoot(abomination);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && attackCool <= 0f)
            {
                attackCool = 200f + 200f * (float)abomination.life / (float)abomination.lifeMax + (float)Main.rand.Next(200);
                Shoot(abomination);
                NPC.netUpdate = true;
            }
        }

        private void Shoot(NPC abomination)
        {
            Vector2 delta = Main.player[abomination.target].Center - NPC.Center;
            float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
            if (magnitude > 0)
            {
                delta *= 5f / magnitude;
            }
            else
            {
                delta = new Vector2(0f, 5f);
            }
            int damage = (NPC.damage - 30) / 2;
            if (Main.expertMode)
            {
                damage = (int)(damage / Main.GameModeInfo.EnemyDamageMultiplier);
            }
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, delta.X, delta.Y, Mod.Find<ModProjectile>("ElementBall").Type, damage, 3f, Main.myPlayer, GetDebuff(), GetDebuffTime());
        }

        public static void SetPosition(NPC npc)
        {
            CaptiveElement modNPC = npc.ModNPC as CaptiveElement;
            if (modNPC != null)
            {
                Vector2 center = Main.npc[modNPC.center].Center;
                double angle = Main.npc[modNPC.center].ai[3] + 2.0 * Math.PI * modNPC.captiveType / 5.0;
                npc.position = center + 300f * (new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) - npc.Size / 2f;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = captiveType * frameHeight;
            if (captiveType == 1)
            {
                NPC.alpha = 100;
            }
            if (attackCool < 50f)
            {
                NPC.frame.Y += 5 * frameHeight;
            }
            else if (difficulty > 1 && attackCool < 110f)
            {
                NPC.frame.Y += 5 * frameHeight;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (captiveType == 2 && difficulty > 0)
            {
                cooldownSlot = 1;
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.rand.Next(2) == 0)
            {
                int debuff = GetDebuff();
                if (debuff >= 0)
                {
                    target.AddBuff(debuff, GetDebuffTime(), true);
                }
            }
        }

        public int GetDebuff()
        {
            switch (captiveType)
            {
                case 0:
                    return BuffID.Frostburn;
                case 1:
                    return Mod.Find<ModBuff>("EtherealFlames").Type;
                case 3:
                    return BuffID.Venom;
                case 4:
                    return BuffID.Ichor;
                default:
                    return -1;
            }
        }

        public int GetDebuffTime()
        {
            int time;
            switch (captiveType)
            {
                case 0:
                    time = 400;
                    break;
                case 1:
                    time = 300;
                    break;
                case 3:
                    time = 400;
                    break;
                case 4:
                    time = 900;
                    break;
                default:
                    return -1;
            }
            return time;
        }

        public Color? GetColor()
        {
            switch (captiveType)
            {
                case 0:
                    return new Color(0, 230, 230);
                case 1:
                    return new Color(0, 153, 230);
                case 3:
                    return new Color(0, 178, 0);
                case 4:
                    return new Color(230, 192, 0);
                default:
                    return null;
            }
        }

        public override void BossHeadSlot(ref int index)
        {
            if (captiveType > 0)
            {
                index = NPCHeadLoader.GetBossHeadSlot(Bluemagic.captiveElementHead + captiveType);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Abomination abomination = Main.npc[center].ModNPC as Abomination;
            if (difficulty > 0 && abomination != null && abomination.NPC.active && abomination.laserTimer <= 60 && (abomination.laser1 == captiveType || abomination.laser2 == captiveType))
            {
                Color? color = GetColor();
                if (!color.HasValue)
                {
                    color = Color.White;
                }
                float rotation = abomination.laserTimer / 30f;
                if (abomination.laser1 == captiveType)
                {
                    rotation *= -1f;
                }
                Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/Abomination/Rune").Value, NPC.Center - Main.screenPosition, null, color.Value, rotation, new Vector2(64, 64), 1f, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}