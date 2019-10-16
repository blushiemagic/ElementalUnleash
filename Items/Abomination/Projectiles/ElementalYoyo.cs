using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
	public class ElementalYoyo : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 400f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17.5f;
		}

		public override void SetDefaults()
		{
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}

		public override void AI()
		{
			if (projectile.owner == Main.myPlayer)
			{
				projectile.localAI[1] += 1f;
				if (projectile.localAI[1] >= 4f)
				{
					float num3 = 480f;
					Vector2 shootVel = projectile.velocity;
					Vector2 randOffset = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					randOffset.Normalize();
					randOffset *= (float)Main.rand.Next(10, 61) * 0.1f;
					if (Main.rand.Next(3) == 0)
					{
						randOffset *= 2f;
					}
					shootVel *= 0.25f;
					shootVel += randOffset;
					for (int i = 0; i < 200; i++)
					{
						if (Main.npc[i].CanBeChasedBy(projectile, false))
						{
							Vector2 targetPos = Main.npc[i].Center;
							float distance = Math.Abs(projectile.Center.X - targetPos.X) + Math.Abs(projectile.Center.Y - targetPos.Y);
							if (distance < num3 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
							{
								num3 = distance;
								shootVel = targetPos - projectile.Center;
								shootVel.Normalize();
								shootVel *= 12f;
							}
						}
					}
					shootVel *= 0.8f;
					Projectile.NewProjectile(projectile.Center - shootVel, shootVel, ModContent.ProjectileType<ElementalYoyoBeam>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.Next(6), 0f);
					projectile.localAI[1] = 0f;
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int choice = Main.rand.Next(5);
			int debuff = GetDebuff(choice);
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime(choice));
			}
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			int choice = Main.rand.Next(5);
			int debuff = GetDebuff(choice);
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime(choice) / 2);
			}
		}

		public int GetDebuff(int type)
		{
			switch (type)
			{
			case 0:
				return BuffID.OnFire;
			case 1:
				return BuffID.Frostburn;
			case 2:
				return mod.BuffType("EtherealFlames");
			case 3:
				return BuffID.Venom;
			case 4:
				return BuffID.Ichor;
			default:
				return 0;
			}
		}

		public int GetDebuffTime(int type)
		{
			switch (type)
			{
			case 0:
				return 600;
			case 1:
				return 400;
			case 2:
				return 300;
			case 3:
				return 400;
			case 4:
				return 900;
			default:
				return 0;
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 200);
		}
	}
}
