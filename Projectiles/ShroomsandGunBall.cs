using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
	public class ShroomsandGunBall : ShroomsandBall
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = mod.Name + "/Projectiles/ShroomsandBall";
			return base.Autoload(ref name, ref texture);
		}

		public override void SetDefaults()
		{
			projectile.name = "Shroomsand Ball";
			projectile.knockBack = 6f;
			projectile.width = 10;
			projectile.height = 10;
			//projectile.aiStyle = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.ForcePlateDetection[projectile.type] = true;
			falling = false;
		}
	}
}