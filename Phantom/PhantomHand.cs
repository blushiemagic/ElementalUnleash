using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomHand : ModNPC
    {
        private const float maxSpeed = 8f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Phantom");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.damage = 100;
            NPC.defense = 50;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.width = 32;
            NPC.height = 40;
            NPC.alpha = 70;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
            NPC.npcSlots = 0f;
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
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance);
            NPC.damage = (int)(NPC.damage * 0.7f);
        }

        public Phantom Head
        {
            get
            {
                return (Phantom)Main.npc[(int)NPC.ai[0]].ModNPC;
            }
        }

        public float Direction
        {
            get
            {
                return NPC.ai[1];
            }
        }

        public float AttackID
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

        public float AttackTimer
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

        public float MaxAttackTimer
        {
            get
            {
                return 60f + 120f * (float)Head.NPC.life / (float)Head.NPC.lifeMax;
            }
        }

        public override void AI()
        {
            NPC headNPC = Main.npc[(int)NPC.ai[0]];
            if (!headNPC.active || headNPC.type != Mod.Find<ModNPC>("Phantom").Type)
            {
                NPC.active = false;
                return;
            }
            NPC.timeLeft = headNPC.timeLeft;

            if (Head.Enraged)
            {
                NPC.damage = NPC.defDamage * 3;
                NPC.defense = NPC.defDefense * 3;
            }
            NPC.direction = (int)Direction;
            NPC.spriteDirection = (int)Direction;
            if (!NPC.HasValidTarget)
            {
                NPC.TargetClosest(false);
            }

            if (AttackTimer >= 0f)
            {
                IdleBehavior();
            }
            else if (AttackID == 1 || AttackID == 4)
            {
                if (Direction == -1f)
                {
                    HammerAttack();
                }
                else
                {
                    WispAttack();
                }
            }
            else if (AttackID == 2 || AttackID == 5)
            {
                if (Direction == -1f)
                {
                    BladeAttack();
                }
                else
                {
                    HammerAttack();
                }
            }
            else if (AttackID == 3)
            {
                ChargeAttack();
            }
            else
            {
                IdleBehavior();
            }
            AttackTimer += 1f;
            if (AttackTimer >= MaxAttackTimer)
            {
                ChooseAttack();
            }

            CreateDust();
        }

        private void ChooseAttack()
        {
            AttackID += 1f;
            if (AttackID >= 6f)
            {
                AttackID = 1f;
            }
            if (AttackID == 1f || AttackID == 4f)
            {
                if (Direction == -1f)
                {
                    AttackTimer = -210f;
                }
                else
                {
                    AttackTimer = -240f;
                }
            }
            else if (AttackID == 2f || AttackID == 5f)
            {
                if (Direction == -1f)
                {
                    AttackTimer = -240f;
                }
                else
                {
                    AttackTimer = -210f;
                }
            }
            else if (AttackID == 3f)
            {
                AttackTimer = -60f;
            }
            NPC.TargetClosest(false);
            NPC.netUpdate = true;
        }

        private void IdleBehavior()
        {
            Vector2 target = Head.NPC.Bottom + new Vector2(Direction * 128f, 64f);
            Vector2 change = target - NPC.Bottom;
            CapVelocity(ref change, maxSpeed * 2f);
            ModifyVelocity(change);
            CapVelocity(ref NPC.velocity, maxSpeed * 2f);
        }

        private void HammerAttack()
        {
            Vector2 target = Main.player[NPC.target].Center;
            Vector2 moveTarget = target + new Vector2(Direction * 240f, -240f);
            Vector2 offset = moveTarget - NPC.Center;
            CapVelocity(ref offset, maxSpeed);
            ModifyVelocity(offset);
            CapVelocity(ref NPC.velocity, maxSpeed);

            int attackTimer = (int)AttackTimer + 210;
            if (attackTimer % 20 == 0 && attackTimer < 100 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int damage = (NPC.damage - 20) / 2;
                if (Main.expertMode)
                {
                    damage /= 2;
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("PhantomHammer").Type, damage, 6f, Main.myPlayer, NPC.whoAmI);
            }
        }

        private void BladeAttack()
        {
            Vector2 target = Main.player[NPC.target].Center;
            Vector2 moveTarget = target + new Vector2(Direction * 240f, 0f);
            Vector2 offset = moveTarget - NPC.Center;
            CapVelocity(ref offset, maxSpeed);
            ModifyVelocity(offset);
            CapVelocity(ref NPC.velocity, maxSpeed);

            if (AttackTimer == -240f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("PhantomOrb").Type, 0, 2f, NPC.whoAmI, 0f, 0f, NPC.target);
            }
        }

        private void WispAttack()
        {
            Vector2 target = Main.player[NPC.target].Center;
            Vector2 moveTarget = target + new Vector2(Direction * 240f, 0f);
            Vector2 offset = moveTarget - NPC.Center;
            CapVelocity(ref offset, maxSpeed);
            ModifyVelocity(offset);
            CapVelocity(ref NPC.velocity, maxSpeed);

            if (AttackTimer == -240f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("PhantomOrb").Type, 0, 1f, NPC.whoAmI, 0f, 0f, NPC.target);
            }
        }

        private void ChargeAttack()
        {
            if (AttackTimer < -40f)
            {
                Vector2 offset = Main.player[NPC.target].Center - NPC.Center;
                CapVelocity(ref offset, maxSpeed);
                ModifyVelocity(offset, 0.1f);
                CapVelocity(ref NPC.velocity, maxSpeed);
            }
        }

        private void CreateDust()
        {
            Vector2 target = Head.NPC.Center;
            target += new Vector2(Direction * 60f, 60f);
            Vector2 offset = target - NPC.Center;
            float length = offset.Length();
            if (offset != Vector2.Zero)
            {
                offset.Normalize();
            }
            for (float k = 0f; k < length - 10f; k += 4f)
            {
                if (Main.rand.Next(10) == 0)
                {
                    int dust = Dust.NewDust(NPC.Center + offset * k, 0, 0, Mod.Find<ModDust>("Phantom").Type);
                    Main.dust[dust].alpha = 100;
                }
            }
        }

        private void ModifyVelocity(Vector2 modify, float weight = 0.05f)
        {
            NPC.velocity = Vector2.Lerp(NPC.velocity, modify, weight);
        }

        private void CapVelocity(ref Vector2 velocity, float max)
        {
            if (velocity.Length() > max)
            {
                velocity.Normalize();
                velocity *= max;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f;
        }
    }
}