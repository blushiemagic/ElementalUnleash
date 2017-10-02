using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
	public class SaltBlockBall : SandBall
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Salt Ball");
			ProjectileID.Sets.ForcePlateDetection[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.knockBack = 6f;
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.hostile = true;
			projectile.penetrate = -1;
			tileType = mod.TileType("SaltBlock");
			dustType = 13;
		}
	}
}