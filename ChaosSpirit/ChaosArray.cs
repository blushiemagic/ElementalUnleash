using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
	public class ChaosArray : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Array of Chaos");
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			cooldownSlot = 1;
		}

		public override void AI()
		{
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (!npc.active || npc.type != mod.NPCType("ChaosSpirit"))
			{
				projectile.Kill();
				return;
			}
			projectile.Center = npc.Center;
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 255f)
			{
				projectile.Kill();
			}
		}

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
		{
			if (target.hurtCooldowns[1] <= 0)
			{
				BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>(mod);
				modPlayer.constantDamage = 200;
				modPlayer.percentDamage = 1f / 3f;
				if (Main.expertMode)
				{
					modPlayer.constantDamage = (int)(modPlayer.constantDamage * 1.5f);
					modPlayer.percentDamage *= 1.5f;
				}
				modPlayer.chaosDefense = true;
			}
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(mod.BuffType("Undead"), 300, false);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Vector2.Distance(target.Center, projectile.Center) >= 600f)
			{
				return false;
			}
			return base.CanHitNPC(target);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[1] >= 90f && projectile.ai[1] < 240f)
			{
				for (int x = -2400; x <= 2400; x += 160)
				{
					for (int y = -2400; y <= 2400; y += 160)
					{
						if (x == 0 && y == 0)
						{
							continue;
						}
						Vector2 testPos = projectile.position + new Vector2(x, y);
						if (Collision.CheckAABBvAABBCollision(testPos, new Vector2(projectile.width, projectile.height), new Vector2(targetHitbox.X, targetHitbox.Y), new Vector2(targetHitbox.Width, targetHitbox.Height)))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawCenter = projectile.Center - Main.screenPosition;
			float alpha = 1f;
			if (projectile.ai[1] < 60f)
			{
				alpha = 0.5f - (Math.Abs(projectile.ai[1] - 30f) / 30f);
			}
			else if (projectile.ai[1] < 90f)
			{
				alpha = (projectile.ai[1] - 60f) / 30f;
			}
			else if (projectile.ai[1] > 240f)
			{
				alpha = (255f - projectile.ai[1]) / 15f;
			}
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			for (int x = -2400; x <= 2400; x += 160)
			{
				for (int y = -2400; y <= 2400; y += 160)
				{
					if (x == 0 && y == 0)
					{
						continue;
					}
					int hash = (7 * x + 13 * y) / 200;
					float hue = (hash % 24) / 24f;
					Color color = Main.hslToRgb(hue, 1f, 0.5f) * alpha;
					spriteBatch.Draw(texture, drawCenter + new Vector2(x, y), null, color, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}