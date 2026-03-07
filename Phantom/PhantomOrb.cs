using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomOrb : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 500;
            NPC.defense = 50;
            NPC.knockBackResist = 0f;
            NPC.width = 40;
            NPC.height = 40;
            NPC.alpha = 70;
            NPC.npcSlots = 0f;
            NPC.netAlways = true;
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

        public override void AI()
        {
            NPC.rotation += 0.1f;
            if (NPC.rotation > 2f * (float)Math.PI)
            {
                NPC.rotation -= 2f * (float)Math.PI;
            }
            NPC follow = Main.npc[(int)NPC.ai[1]];
            NPC.Center = follow.Center + new Vector2(NPC.ai[2], NPC.ai[3]);
            NPC.localAI[0] += 1f;
            if (NPC.localAI[0] >= 180f)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] == 1f)
                {
                    WispAttack();
                }
                else if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] == 2f)
                {
                    BladeAttack();
                }
                else if (Main.netMode != NetmodeID.MultiplayerClient && NPC.ai[0] == 3f)
                {
                    SpawnPaladin();
                }
                NPC.active = false;
            }
        }

        private void WispAttack()
        {
            NPC hand = Main.npc[(int)NPC.ai[1]];
            int damage = hand.damage / 6;
            if (Main.expertMode)
            {
                damage /= 2;
            }
            for (int k = 0; k < 4; k++)
            {
                Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("WispHostile").Type, damage, 3f, Main.myPlayer, NPC.ai[1], k * 15);
            }
        }

        private void BladeAttack()
        {
            NPC hand = Main.npc[(int)NPC.ai[1]];
            int damage = (hand.damage - 10) / 2;
            if (Main.expertMode)
            {
                damage /= 2;
            }
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Vector2.Zero, Mod.Find<ModProjectile>("PhantomBladeHostile").Type, damage, 6f, Main.myPlayer, NPC.ai[1]);
        }

        private void SpawnPaladin()
        {
            if (NPC.CountNPCS(NPCID.Paladin) < 5)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 32, NPCID.Paladin);
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int x = 0; x < 50; x++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Phantom").Type);
                    Main.dust[dust].velocity *= 2f;
                }
            }
        }

        public override bool PreKill()
        {
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.85f;
        }
    }
}