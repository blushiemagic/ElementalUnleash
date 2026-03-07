using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public abstract class BlushiemagicBase : ModNPC
    {
        private Player hitBy;

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NeedsExpertScaling[NPC.type] = false;
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 1000000;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.knockBackResist = 0f;
            NPC.dontTakeDamage = true;
            NPC.width = 24;
            NPC.height = 48;
            NPC.npcSlots = 9001f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = null;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public virtual bool UseSpecialDamage()
        {
            return true;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (!BlushieBoss.Players[player.whoAmI])
            {
                return false;
            }
            if (UseSpecialDamage() && BlushieBoss.Immune > 0)
            {
                return false;
            }
            hitBy = player;
            return null;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (!BlushieBoss.Players[projectile.owner])
            {
                return false;
            }
            if (UseSpecialDamage() && BlushieBoss.Immune > 0)
            {
                return false;
            }
            return null;
        }

        public override void ModifyHitByItem(Player player, Item item, ref NPC.HitModifiers modifiers)
        {
            hitBy = player;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref NPC.HitModifiers modifiers)
        {
            if (!projectile.trap && !projectile.npcProj)
            {
                hitBy = Main.player[projectile.owner];
            }
        }

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            if (!UseSpecialDamage())
            {
                hitBy = null;
                return;
            }
            if (hitBy == null || !hitBy.active || BlushieBoss.Immune > 0)
            {
                modifiers.SetMaxDamage(1);
            }
            else
            {
                modifiers.FinalDamage.Base = (float)CalculateDamage(hitBy, modifiers.FinalDamage.Base);
                SetHealth(modifiers.FinalDamage.Base);
                BlushieBoss.Immune = 60;
            }
            hitBy = null;
        }

        public virtual double CalculateDamage(Player player, double damage)
        {
            return damage;
        }

        public virtual void SetHealth(double damage)
        {
        }
    }
}