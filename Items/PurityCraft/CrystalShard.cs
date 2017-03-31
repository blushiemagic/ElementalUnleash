using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
	public class CrystalShard : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.name = "Crystal Shard";
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.alpha = 50;
			projectile.tileCollide = true;
			projectile.magic = true;
		}

		public override void AI()
		{
			projectile.velocity.Y += 0.2f;
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - (float)Math.PI / 4f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int k = 0; k < 6; k++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CrystalStar"), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 1.2f);
			}
			for (int k = 0; k < 2; k++)
			{
				int goreType = mod.GetGoreSlot(Main.rand.Next(2) == 0 ? "Gores/GreenStar" : "Gores/WhiteStar");
				Gore.NewGore(projectile.position, 0.05f * projectile.velocity, goreType, 1f);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 0);
		}
	}
}