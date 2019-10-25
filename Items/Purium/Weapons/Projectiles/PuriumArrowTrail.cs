using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumArrowTrail : ModProjectile
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
            DisplayName.SetDefault("Purium Arrow");
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 90;
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.ranged = true;
            projectile.penetrate = -1;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int uuid = Projectile.GetByUUID(projectile.owner, projectile.ai[0]);
            if (uuid >= 0 && Main.projectile[uuid].active && Main.projectile[uuid].type == mod.ProjectileType("PuriumArrow"))
            {
                projectile.position = Main.projectile[uuid].position;
            }
            else
            {
                projectile.ai[0] = -1f;
            }
            if (projectile.position == projectile.oldPos[projectile.oldPos.Length - 1])
            {
                projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            foreach (Vector2 position in projectile.oldPos)
            {
                projHitbox.X = (int)position.X;
                projHitbox.Y = (int)position.Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                float alpha = 1f - 0.65f * (float)k / (float)projectile.oldPos.Length;
                if (k == projectile.oldPos.Length - 1)
                {
                    spriteBatch.Draw(texture, projectile.oldPos[k] - Main.screenPosition, Color.White * alpha);
                }
                else if (projectile.oldPos[k] != projectile.oldPos[k + 1])
                {
                    Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition;
                    for (int l = 0; l < 4; l++)
                    {
                        spriteBatch.Draw(texture, drawPos, Color.White * alpha);
                        drawPos += (projectile.oldPos[k + 1] - projectile.oldPos[k]) / 4f;
                    }
                }
            }
            return false;
        }
    }
}