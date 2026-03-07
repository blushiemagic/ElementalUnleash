using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Abomination
{
    [AutoloadBossHead]
    public class AbominationRun : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/Abomination/Abomination";
            }
        }

        public override string BossHeadTexture
        {
            get
            {
                return "Bluemagic/Abomination/Abomination_Head_Boss";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Abomination");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 40000;
            NPC.damage = 100;
            NPC.defense = 55;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 15f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.buffImmune[24] = true;
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
                SoundEngine.PlaySound(SoundID.Roar, NPC.position);
                NPC.localAI[0] = 1f;
            }
            NPC.velocity.Y += 1f;
            if (NPC.timeLeft > 10)
            {
                NPC.timeLeft = 10;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.rand.Next(2) == 0)
            {
                target.AddBuff(BuffID.OnFire, 600, true);
            }
        }
    }
}