using System;
using Bluemagic.Items.Phantom;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    [AutoloadBossHead]
    public class Phantom : ModNPC
    {
        private const float maxSpeed = 8f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Phantom");
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.damage = 100;
            NPC.defense = 50;
            NPC.knockBackResist = 0f;
            NPC.width = 80;
            NPC.height = 80;
            NPC.alpha = 70;
            NPC.value = Item.buyPrice(0, 15, 0, 0);
            NPC.npcSlots = 12f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Music = MusicID.Boss3;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.7f * balance);
            NPC.damage = (int)(NPC.damage * 0.7f);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public bool Enraged
        {
            get
            {
                return NPC.ai[0] != 0f;
            }
            set
            {
                NPC.ai[0] = value ? 1f : 0f;
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
                return 60f + 120f * (float)NPC.life / (float)NPC.lifeMax;
            }
        }

        public float PaladinTimer
        {
            get
            {
                return NPC.localAI[1];
            }
            set
            {
                NPC.localAI[1] = value;
            }
        }

        public float MaxPaladinTimer
        {
            get
            {
                float maxValue = Main.expertMode ? 2f / 3f : 0.5f;
                return 120f + 180f * (float)NPC.life / (NPC.lifeMax * maxValue);
            }
        }

        public override void AI()
        {
            Initialize();

            if (!NPC.HasValidTarget || !Main.player[NPC.target].ZoneDungeon)
            {
                NPC.TargetClosest(false);
            }
            if (!NPC.HasValidTarget || AttackID == 100 || (!Main.player[NPC.target].ZoneDungeon && Vector2.Distance(Main.player[NPC.target].Center, NPC.Center) >= 1600f))
            {
                NPC.velocity = new Vector2(0f, maxSpeed);
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
                AttackID = 100;
                NPC.netUpdate = true;
                return;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && !Enraged && (!NPC.HasValidTarget || !Main.player[NPC.target].ZoneDungeon))
            {
                Enraged = true;
                NPC.netUpdate = true;
                Talk("You thought you could escape...");
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
            }
            if (Enraged)
            {
                NPC.damage = NPC.defDamage * 3;
                NPC.defense = NPC.defDefense * 3;
            }

            if (AttackTimer >= 0f)
            {
                IdleBehavior();
            }
            else if (AttackID == 1f || AttackID == 2f)
            {
                ChargeAttack();
            }
            else if (AttackID == 3f)
            {
                SphereAttack();
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

            if (Main.netMode != NetmodeID.MultiplayerClient && (NPC.life <= NPC.lifeMax / 2 || (Main.expertMode && NPC.life <= NPC.lifeMax * 2 / 3)))
            {
                PaladinTimer += 1f;
                if (PaladinTimer >= MaxPaladinTimer)
                {
                    SpawnPaladin();
                    PaladinTimer = 0f;
                    NPC.netUpdate = true;
                }
            }
        }

        private void Initialize()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.localAI[0] == 0f)
            {
                int spawnX = (int)NPC.Bottom.X;
                int spawnY = (int)NPC.Bottom.Y + 64;
                int left = NPC.NewNPC(NPC.GetSource_FromThis(), spawnX - 128, spawnY, Mod.Find<ModNPC>("PhantomHand").Type, 0, NPC.whoAmI, -1f, 0f, -30f);
                int right = NPC.NewNPC(NPC.GetSource_FromThis(), spawnX + 128, spawnY, Mod.Find<ModNPC>("PhantomHand").Type, 0, NPC.whoAmI, 1f, 0f, -60f);
                NPC.netUpdate = true;
                Main.npc[left].netUpdate = true;
                Main.npc[right].netUpdate = true;
            }
            if (Main.netMode != NetmodeID.Server && NPC.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                Main.NewText("The Phantom has awoken!", 50, 150, 200);
            }
            NPC.localAI[0] = 1f;
        }

        private void ChooseAttack()
        {
            AttackID += 1f;
            if (AttackID >= 4f)
            {
                AttackID = 1f;
            }
            if (AttackID == 3f)
            {
                AttackTimer = -300f;
            }
            else
            {
                AttackTimer = -120f;
            }
            NPC.TargetClosest(false);
            NPC.netUpdate = true;
        }

        private void IdleBehavior()
        {
            Vector2 offset = NPC.Center - Main.player[NPC.target].Center;
            offset *= 0.9f;
            Vector2 target = offset.RotatedBy(Main.expertMode ? 0.03f : 0.02f);
            CapVelocity(ref target, 320f);
            Vector2 change = target - offset;
            CapVelocity(ref change, maxSpeed);
            ModifyVelocity(change);
            CapVelocity(ref NPC.velocity, maxSpeed);
        }

        private void ChargeAttack()
        {
            Vector2 offset = Main.player[NPC.target].Center - NPC.Center;
            if (AttackTimer < -90f || offset.Length() > 320f)
            {
                CapVelocity(ref offset, maxSpeed);
                ModifyVelocity(offset, 0.1f);
                CapVelocity(ref NPC.velocity, maxSpeed);
            }
        }

        private void SphereAttack()
        {
            IdleBehavior();

            int attackTimer = (int)AttackTimer + 300;
            if (attackTimer % 30 == 0 && attackTimer < 150 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int damage = (NPC.damage - 10) / 2;
                if (Main.expertMode)
                {
                    damage /= 2;
                }
                Vector2 offset = NPC.Center - Main.player[NPC.target].Center;
                if (offset != Vector2.Zero)
                {
                    offset.Normalize();
                    offset *= 320f;
                }
                Projectile.NewProjectile(NPC.GetSource_FromThis(), Main.player[NPC.target].Center + offset, Vector2.Zero, Mod.Find<ModProjectile>("PhantomSphereHostile").Type, damage, 6f, Main.myPlayer, NPC.whoAmI);
            }
        }

        private void SpawnPaladin()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int x = Main.rand.Next(2) == 0 ? -160 : 160;
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X + x, (int)NPC.Bottom.Y + 80, Mod.Find<ModNPC>("PhantomOrb").Type, 0, 3f, NPC.whoAmI, x, 80f, NPC.target);
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

        private void Talk(string message)
        {
            message = "<" + NPC.TypeName + "> " + message;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(NPC.type), message);
                Main.NewText(text, 50, 150, 200);
            }
            else
            {
                NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(NPC.type), message);
                ChatHelper.BroadcastChatMessage(text, new Color(50, 150, 200));
            }
        }

        public override void OnKill()
        {
            BluemagicWorld.downedPhantom = true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PhantomBag>()));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantomTrophy>(), 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("PhantomMask").Type, 7));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("PhantomPlate").Type, 1, 5, 7));
            npcLoot.Add(new LeadingConditionRule(new Conditions.NotExpert()).OnSuccess(ItemDropRule.OneFromOptions(1, Mod.Find<ModItem>("PhantomBlade").Type, Mod.Find<ModItem>("SpectreGun").Type, Mod.Find<ModItem>("PhantomSphere").Type, Mod.Find<ModItem>("PaladinStaff").Type)));

        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/Phantom/PhantomBody").Value;
            Main.spriteBatch.Draw(texture, NPC.Bottom - new Vector2(0f, 20f) - Main.screenPosition, null, Color.White * 0.6f, 0f, new Vector2(texture.Width / 2, 0f), 1f, SpriteEffects.None, 0f);
            texture = TextureAssets.Npc[NPC.type].Value;
            Main.spriteBatch.Draw(texture, NPC.position - Main.screenPosition, Color.White * 0.8f);
            return false;
        }
    }
}