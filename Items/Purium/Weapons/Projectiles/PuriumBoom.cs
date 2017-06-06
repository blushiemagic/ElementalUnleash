using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumBoom : ModProjectile
	{
		public override string Texture
		{
			get
			{
				return "Bluemagic/Items/Purium/Weapons/Projectiles/PuriumBullet";
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purium Breaker");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.width = 8;
			projectile.height = 8;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override void AI()
		{
			projectile.localAI[0] = (projectile.localAI[0] + 1f) % 4f;
			if (projectile.localAI[0] == 0f)
			{
				Dust.NewDust(projectile.position, 0, 0, mod.DustType("PuriumBullet"));
			}
			if (projectile.ai[0] <= 0f)
			{
				projectile.Kill();
			}
			projectile.ai[0] -= 1f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.penetrate >= 0)
			{
				projectile.penetrate++;
			}
		}

		public override void Kill(int timeLeft)
		{
			projectile.position = projectile.Center;
			for (int k = 0; k < 4; k++)
			{
				float scale = 0.4f * k;
				CreateGore(scale, 1f, 1f);
				CreateGore(scale, -1f, 1f);
				CreateGore(scale, 1f, -1f);
				CreateGore(scale, -1f, -1f);
			}
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			projectile.width = 160;
			projectile.height = 160;
			projectile.position -= projectile.Size / 2f;
			projectile.damage *= 2;
			projectile.penetrate = 1;
			projectile.maxPenetrate = 1;
			projectile.Damage();
		}

		private void CreateGore(float scale, float offX, float offY)
		{
			int gore = Gore.NewGore(projectile.position, Vector2.Zero, Main.rand.Next(435, 438), 1f);
			Main.gore[gore].velocity *= scale;
			Main.gore[gore].velocity.X += offX;
			Main.gore[gore].velocity.Y += offY;
		}
	}
}