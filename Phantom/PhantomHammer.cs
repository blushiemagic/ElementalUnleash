using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class PhantomHammer : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Phantom Hammer";
			projectile.width = 38;
			projectile.height = 38;
			projectile.alpha = 70;
			projectile.timeLeft = 600;
			projectile.maxPenetrate = -1;
			projectile.hostile = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}

		public override void AI()
		{
			projectile.rotation += 0.5f;
			projectile.ai[1] += 1f;
			NPC npc = Main.npc[(int)projectile.ai[0]];
			if (projectile.ai[1] <= 100f)
			{
				projectile.Center = npc.Center;
			}
			else if (projectile.ai[1] == 101f)
			{
				Vector2 move = Main.player[npc.target].Center - projectile.Center;
				float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
				if (Main.expertMode)
				{
					Vector2 prediction = Main.player[npc.target].velocity;
					prediction *= magnitude / 7f;
					prediction *= Main.rand.NextFloat();
					move += prediction;
				}
				if (magnitude > 0f)
				{
					move *= 7f / magnitude;
				}
				projectile.velocity = move;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 1);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.85f;
		}
	}
}