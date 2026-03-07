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

        public override string Texture
        {
            get
            {
                return "Bluemagic/Items/Purium/Weapons/Projectiles/PuriumBullet";
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.NeedsUUID[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hide = true;
            //projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 16;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            for (int k = 0; k < positions.Count; k++)
            {
                if (Collision.TileCollision(positions[k], Projectile.velocity, Projectile.width, Projectile.height, true, true) != Projectile.velocity)
                {
                    positions.RemoveAt(k);
                    k--;
                    if (positions.Count == 0)
                    {
                        Projectile.Kill();
                    }
                }
                else
                {
                    positions[k] += Projectile.velocity;
                    int dust = Dust.NewDust(positions[k] + Projectile.Size / 2f, 0, 0, Mod.Find<ModDust>("PuriumBullet").Type);
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
                        Projectile.Kill();
                    }
                    else
                    {
                        Projectile.penetrate++;
                    }
                    return true;
                }
            }
            return false;
        }

        public void AddPosition(Vector2 pos)
        {
            positions.Add(pos - Projectile.Size / 2f);
        }
    }
}