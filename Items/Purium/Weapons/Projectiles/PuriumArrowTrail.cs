using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
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
            // DisplayName.SetDefault("Purium Arrow");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 90;
        }

        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[0]);
            if (uuid >= 0 && Main.projectile[uuid].active && Main.projectile[uuid].type == Mod.Find<ModProjectile>("PuriumArrow").Type)
            {
                Projectile.position = Main.projectile[uuid].position;
            }
            else
            {
                Projectile.ai[0] = -1f;
            }
            if (Projectile.position == Projectile.oldPos[Projectile.oldPos.Length - 1])
            {
                Projectile.Kill();
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            foreach (Vector2 position in Projectile.oldPos)
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

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                float alpha = 1f - 0.65f * (float)k / (float)Projectile.oldPos.Length;
                if (k == Projectile.oldPos.Length - 1)
                {
                    Main.spriteBatch.Draw(texture, Projectile.oldPos[k] - Main.screenPosition, Color.White * alpha);
                }
                else if (Projectile.oldPos[k] != Projectile.oldPos[k + 1])
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition;
                    for (int l = 0; l < 4; l++)
                    {
                        Main.spriteBatch.Draw(texture, drawPos, Color.White * alpha);
                        drawPos += (Projectile.oldPos[k + 1] - Projectile.oldPos[k]) / 4f;
                    }
                }
            }
            return false;
        }
    }
}