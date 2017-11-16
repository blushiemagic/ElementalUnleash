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
			NPCID.Sets.NeedsExpertScaling[npc.type] = false;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 1000000;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0f;
			npc.dontTakeDamage = true;
			npc.width = 24;
			npc.height = 48;
			npc.npcSlots = 9001f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
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

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			hitBy = player;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (!projectile.trap && !projectile.npcProj)
			{
				hitBy = Main.player[projectile.owner];
			}
		}

		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (!UseSpecialDamage())
			{
				hitBy = null;
				return true;
			}
			if (hitBy == null || !hitBy.active || BlushieBoss.Immune > 0)
			{
				damage = 0;
			}
			else
			{
				damage = CalculateDamage(hitBy, damage);
				SetHealth(damage);
				BlushieBoss.Immune = 60;
			}
			hitBy = null;
			return false;
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