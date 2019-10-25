using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.PurityCraft
{
    public class CrystalStar : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 5;
            aiType = ProjectileID.HallowStar;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 50;
            projectile.tileCollide = false;
            projectile.magic = true;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CrystalStar"), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default(Color), 1.2f);
            }
            for (int k = 0; k < 3; k++)
            {
                int goreType = mod.GetGoreSlot(Main.rand.Next(2) == 0 ? "Gores/GreenStar" : "Gores/WhiteStar");
                Gore.NewGore(projectile.position, 0.05f * projectile.velocity, goreType, 1f);
            }
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 center = projectile.Center;
                float offset = 6f;
                int type = mod.ProjectileType("CrystalShard");
                int damage = projectile.damage - 10;
                Projectile.NewProjectile(center.X - offset, center.Y - offset, -8f, -6f, type, damage, projectile.knockBack, projectile.owner);
                Projectile.NewProjectile(center.X + offset, center.Y - offset, 8f, -6f, type, damage, projectile.knockBack, projectile.owner);
                Projectile.NewProjectile(center.X - offset, center.Y + offset, -10f, -2f, type, damage, projectile.knockBack, projectile.owner);
                Projectile.NewProjectile(center.X + offset, center.Y + offset, 10f, -2f, type, damage, projectile.knockBack, projectile.owner);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 0);
        }
    }
}