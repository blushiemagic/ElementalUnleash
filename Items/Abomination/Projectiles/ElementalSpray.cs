using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
	public class ElementalSpray : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = 4;
			projectile.extraUpdates = 2;
			projectile.ranged = true;
		}

		public override void AI()
		{
			if (projectile.timeLeft > 60)
			{
				projectile.timeLeft = 60;
			}
			if (projectile.ai[1] > 6f)
			{
				projectile.ai[1] += 1f;
				if (Main.rand.Next(2) == 0)
				{
					int dustType = DustType();
					Dust dust;
					if (dustType == 171)
					{
						int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
						dust = Main.dust[dustIndex];
						if (Main.rand.Next(3) != 0)
						{
							dust.scale *= 3f;
							dust.noGravity = true;
							dust.velocity *= 2f;
						}
						dust.scale *= 1.15f;
						dust.velocity *= 1.2f;
					}
					else if (dustType == mod.DustType("Bubble"))
					{
						int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default(Color), 0.75f);
						dust = Main.dust[dustIndex];
						if (Main.rand.Next(3) != 0)
						{
							dust.scale *= 1.5f;
							dust.velocity *= 2f;
						}
						dust.velocity *= 1.2f;
					}
					else
					{
						int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100);
						dust = Main.dust[dustIndex];
						if (Main.rand.Next(3) != 0)
						{
							dust.noGravity = true;
							dust.scale *= 3f;
							dust.velocity *= 2f;
						}
						dust.scale *= 1.5f;
						dust.velocity *= 1.2f;
					}
					if (projectile.ai[1] == 7f)
					{
						dust.scale *= 0.25f;
					}
					else if (projectile.ai[1] == 8f)
					{
						dust.scale *= 0.5f;
					}
					else if (projectile.ai[1] == 9f)
					{
						dust.scale *= 0.75f;
					}
				}
			}
			else
			{
				projectile.ai[1] += 1f;
			}
			projectile.rotation += 0.3f * projectile.direction;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.X -= 30;
			hitbox.Y -= 30;
			hitbox.Width += 60;
			hitbox.Height += 60;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.ai[0] == 3f)
			{
				damage += 20;
			}
		}

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			if (projectile.ai[0] == 3f)
			{
				damage += 20;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int debuff = GetDebuff();
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime());
			}
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			int debuff = GetDebuff();
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime() / 2);
			}
		}

		public int DustType()
		{
			switch ((int)projectile.ai[0])
			{
			case 0:
				return 6;
			case 1:
				return 135;
			case 2:
				return mod.DustType("EtherealFlame");
			case 3:
				return mod.DustType("Bubble");
			case 4:
				return 171;
			case 5:
				return 169;
			default:
				return 6;
			}
		}

		public int GetDebuff()
		{
			switch ((int)projectile.ai[0])
			{
			case 0:
				return BuffID.OnFire;
			case 1:
				return BuffID.Frostburn;
			case 2:
				return mod.BuffType("EtherealFlames");
			case 3:
				return 0;
			case 4:
				return BuffID.Venom;
			case 5:
				return BuffID.Ichor;
			default:
				return 0;
			}
		}

		public int GetDebuffTime()
		{
			switch ((int)projectile.ai[0])
			{
			case 0:
				return 600;
			case 1:
				return 400;
			case 2:
				return 300;
			case 3:
				return 0;
			case 4:
				return 400;
			case 5:
				return 900;
			default:
				return 0;
			}
		}
	}
}
