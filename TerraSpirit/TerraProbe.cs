using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public abstract class TerraProbe : ModNPC
    {
        protected NPC Spirit
        {
            get
            {
                return Main.npc[(int)NPC.ai[0]];
            }
        }

        protected int Timer
        {
            get
            {
                return (int)NPC.localAI[1];
            }
            set
            {
                NPC.localAI[1] = value;
            }
        }

        public override string Texture
        {
            get
            {
                return "Bluemagic/TerraSpirit/TerraProbe";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purity Probe");
            NPCID.Sets.NeedsExpertScaling[NPC.type] = false;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 50000;
            NPC.damage = 0;
            NPC.defense = 140;
            NPC.knockBackResist = 0f;
            NPC.width = 64;
            NPC.height = 96;
            NPC.boss = true;
            NPC.npcSlots = 42f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Despicable beautiful");
        }

        public override void AI()
        {
            NPC spirit = Spirit;
            if (!spirit.active || !(spirit.ModNPC is TerraSpirit))
            {
                NPC.active = false;
            }
            Behavior();
            Player target = null;
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
                {
                    target = player;
                    break;
                }
            }
            if (target != null)
            {
                Vector2 offset = target.Center - NPC.Center;
                float distance = offset.Length();
                if (distance == 0f)
                {
                    offset = new Vector2(-1f, 0f);
                }
                offset.Normalize();
                NPC.velocity = 0.05f * offset * (distance - 320f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public abstract void Behavior();

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
            {
                return null;
            }
            return false;
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode)
            {
                modifiers.SourceDamage *= 0.8f;
            }
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (!projectile.npcProj && !projectile.trap && Main.player[projectile.owner].GetModPlayer<BluemagicPlayer>().terraLives > 0)
            {
                return null;
            }
            return false;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode)
            {
                modifiers.SourceDamage *= 0.8f;
            }
        }

        public override bool PreKill()
        {
            NPC spirit = Spirit;
            if (spirit.active && spirit.ModNPC is TerraSpirit)
            {
                TerraSpirit modSpirit = (TerraSpirit)spirit.ModNPC;
                modSpirit.Stage += 2;
                modSpirit.Progress = 0;
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spirit.whoAmI);
                }
            }
            return false;
        }

        protected int FindHighestLife(int start = 0)
        {
            int max = start;
            for (int k = 0; k < 255; k++)
            {
                if (Main.player[k].active && !Main.player[k].dead)
                {
                    BluemagicPlayer modPlayer = Main.player[k].GetModPlayer<BluemagicPlayer>();
                    if (modPlayer.terraLives > max)
                    {
                        max = modPlayer.terraLives;
                    }
                }
            }
            return max;
        }
    }
}
