using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
	public class PuriumLightbeam : ModProjectile
	{
		private List<Vector2> positions = new List<Vector2>();

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Bluemagic/Items/Purium/Weapons/Projectiles/PuriumBullet";
			return base.Autoload(ref name, ref texture);
		}

		public override void SetDefaults()
		{
			projectile.name = "Purium Lightbeam";
			projectile.width = 8;
			projectile.height = 8;
			projectile.hide = true;
			//projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.MaxUpdates = 16;
			projectile.melee = true;
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void AI()
		{
			for (int k = 0; k < positions.Count; k++)
			{
				if (Collision.TileCollision(positions[k], projectile.velocity, projectile.width, projectile.height, true, true) != projectile.velocity)
				{
					positions.RemoveAt(k);
					k--;
					if (positions.Count == 0)
					{
						projectile.Kill();
					}
				}
				else
				{
					positions[k] += projectile.velocity;
					int dust = Dust.NewDust(positions[k] + projectile.Size / 2f, 0, 0, mod.DustType("PuriumBullet"));
					Main.dust[dust].customData = 4;
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int k = 0; k < positions.Count; k++)
			{
				projHitbox.X = (int)positions[k].X;
				projHitbox.Y = (int)positions[k].Y;
				if (projHitbox.Intersects(targetHitbox))
				{
					positions.RemoveAt(k);
					if (positions.Count == 0)
					{
						projectile.Kill();
					}
					else
					{
						projectile.penetrate++;
					}
					return true;
				}
			}
			return false;
		}

		public void AddPosition(Vector2 pos)
		{
			positions.Add(pos - projectile.Size / 2f);
		}
	}
}