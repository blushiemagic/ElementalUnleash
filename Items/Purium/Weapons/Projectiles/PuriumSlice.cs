using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Dusts = Bluemagic.Dusts;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumSlice : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purium Slicer");
		}

		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.scale = 1.4f;
			projectile.alpha = 100;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override void AI()
		{
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.3f, 0.6f, 0.2f);
			projectile.rotation += (float)projectile.direction * 0.5f;
			projectile.ai[0] += 1f;
			if (projectile.ai[0] >= 30f)
			{
				if (projectile.ai[0] < 100f && projectile.velocity.Length() < 32f)
				{
					projectile.velocity *= 1.06f;
				}
				else
				{
					projectile.ai[0] = 200f;
				}
			}
			if (Main.rand.Next(2) == 0)
			{
				int dust = Dusts.PuriumSlice.Create(projectile.position, projectile.width, projectile.height);
				Main.dust[dust].noGravity = true;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 12;
			height = 12;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int k = 0; k < 15; k++)
			{
				int dust = Dusts.PuriumSlice.Create(projectile.position, projectile.width, projectile.height);
				Main.dust[dust].noGravity = true;
				Dusts.PuriumSlice.Create(projectile.position, projectile.width, projectile.height);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}