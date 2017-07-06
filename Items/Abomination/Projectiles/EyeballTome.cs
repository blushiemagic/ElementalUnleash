using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
	public class EyeballTome : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 6;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 28;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.MaxUpdates = 4;
		}

		public override void AI()
		{
			projectile.frame = (int)projectile.ai[0];
			projectile.ai[1] += 0.25f;
			if (projectile.ai[1] >= 120f && projectile.velocity == Vector2.Zero)
			{
				float distance = 800f;
				NPC npc = null;
				for (int k = 0; k < 200; k++)
				{
					NPC check = Main.npc[k];
					if (check.active && check.CanBeChasedBy(this))
					{
						float checkDist = Vector2.Distance(projectile.Center, check.Center);
						if (checkDist < distance)
						{
							npc = check;
							distance = checkDist;
						}
					}
				}
				if (npc != null)
				{
					Vector2 offset = npc.Center - projectile.Center;
					if (distance > 0f)
					{
						offset.Normalize();
						offset *= 8f;
						projectile.velocity = offset;
					}
				}
			}
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

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(127 + lightColor.R / 2, 127 + lightColor.G / 2, 127 + lightColor.B / 2, lightColor.A);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, texture.Width, frameHeight);
			Color color = GetAlpha(lightColor).Value;
			for (int k = projectile.oldPos.Length - 1; k >= 0; k -= 2)
			{
				float alpha = (float)(projectile.oldPos.Length - k + 1) / (float)(projectile.oldPos.Length + 2);
				spriteBatch.Draw(texture, projectile.oldPos[k] - Main.screenPosition, frame, color * alpha, projectile.rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}
