using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;

namespace Bluemagic.Abomination
{
    public class FreedElement : ModNPC
    {
        private int elementType
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

        private int chargeTimer
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

        private float chargeX
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

        private float chargeY
        {
            get
            {
                return NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        public override string Texture
        {
            get
            {
                return "Bluemagic/Abomination/CaptiveElement2";
            }
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
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
            NPC.alpha = 255;
            NPC.width = 50;
            NPC.height = 50;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 5f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            Music = MusicID.Boss2;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }

        public override void AI()
        {
            if (NPC.localAI[0] == 0f)
            {
                if (elementType == 0)
                {
                    NPC.coldDamage = true;
                }
                if (elementType == 2)
                {
                    NPC.damage += 20;
                    if (Main.expertMode)
                    {
                        NPC.damage += 20;
                    }
                }
                NPC.localAI[0] = 1f;
            }
            if (NPC.AnyNPCs(Mod.Find<ModNPC>("CaptiveElement2").Type))
            {
                if (NPC.timeLeft < 750)
                {
                    NPC.timeLeft = 750;
                }
            }
            else
            {
                NPC.life = -1;
                NPC.active = false;
                return;
            }
            chargeTimer--;
            if (chargeTimer <= 0)
            {
                NPC.TargetClosest(false);
                Player player = Main.player[NPC.target];
                Vector2 offset = player.Center - NPC.Center;
                if (offset != Vector2.Zero)
                {
                    offset.Normalize();
                }
                offset *= 12f;
                chargeX = offset.X;
                chargeY = offset.Y;
                chargeTimer = 150;
                NPC.netUpdate = true;
            }
            else if (chargeTimer <= 30)
            {
                chargeX = 0;
                chargeY = 0;
            }
            NPC.velocity = (99f * NPC.velocity + new Vector2(chargeX, chargeY)) / 100f;
            CreateDust();
        }

        private void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                for (int k = 0; k < 5; k++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                    double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                    Main.dust[dust].velocity = 3f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                }
            }
            else
            {
                for (int k = 0; k < 1; k++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Bubble").Type, 0f, 0f, 0);
                    double angle = Main.rand.NextDouble() * 2.0 * Math.PI;
                    Main.dust[dust].velocity = 2f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                }
            }
        }

        public override bool PreKill()
        {
            return false;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (elementType == 2 && (Main.expertMode || NPC.downedMoonlord))
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
            switch (elementType)
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
            switch (elementType)
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
                    time = 600;
                    break;
                default:
                    return -1;
            }
            return time;
        }

        public Color? GetColor()
        {
            switch (elementType)
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
    }
}