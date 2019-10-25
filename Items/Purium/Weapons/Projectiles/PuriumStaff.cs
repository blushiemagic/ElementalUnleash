using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumStaff : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 255;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0 && System.Math.Abs(projectile.velocity.X) + System.Math.Abs(projectile.velocity.Y) > 2f)
            {
                projectile.soundDelay = 10;
                Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 9);
            }
            for (int num143 = 0; num143 < 1; num143++)
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 66, 0f, 0f, 100, Bluemagic.PureColor, 2.5f);
                Main.dust[dust].velocity *= 0.1f;
                Main.dust[dust].velocity += projectile.velocity * 0.2f;
                Main.dust[dust].position.X = projectile.Center.X + 4f + (float)Main.rand.Next(-2, 3);
                Main.dust[dust].position.Y = projectile.Center.Y + (float)Main.rand.Next(-2, 3);
                Main.dust[dust].noGravity = true;
            }
            Lighting.AddLight(projectile.Center, Bluemagic.PureColor.ToVector3());
            if (Main.myPlayer == projectile.owner && projectile.ai[0] == 0f)
            {
                if (Main.player[projectile.owner].channel)
                {
                    float num146 = 16f;
                    Vector2 vector10 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num147 = (float)Main.mouseX + Main.screenPosition.X - vector10.X;
                    float num148 = (float)Main.mouseY + Main.screenPosition.Y - vector10.Y;
                    if (Main.player[projectile.owner].gravDir == -1f)
                    {
                        num148 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector10.Y;
                    }
                    float num149 = (float)System.Math.Sqrt((double)(num147 * num147 + num148 * num148));
                    num149 = (float)System.Math.Sqrt((double)(num147 * num147 + num148 * num148));
                    if (num149 > num146)
                    {
                        num149 = num146 / num149;
                        num147 *= num149;
                        num148 *= num149;
                        int num150 = (int)(num147 * 1000f);
                        int num151 = (int)(projectile.velocity.X * 1000f);
                        int num152 = (int)(num148 * 1000f);
                        int num153 = (int)(projectile.velocity.Y * 1000f);
                        if (num150 != num151 || num152 != num153)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num147;
                        projectile.velocity.Y = num148;
                    }
                    else
                    {
                        int num154 = (int)(num147 * 1000f);
                        int num155 = (int)(projectile.velocity.X * 1000f);
                        int num156 = (int)(num148 * 1000f);
                        int num157 = (int)(projectile.velocity.Y * 1000f);
                        if (num154 != num155 || num156 != num157)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.velocity.X = num147;
                        projectile.velocity.Y = num148;
                    }
                }
                else if (projectile.ai[0] == 0f)
                {
                    projectile.ai[0] = 1f;
                    projectile.netUpdate = true;
                    float num158 = 12f;
                    Vector2 vector11 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    float num159 = (float)Main.mouseX + Main.screenPosition.X - vector11.X;
                    float num160 = (float)Main.mouseY + Main.screenPosition.Y - vector11.Y;
                    if (Main.player[projectile.owner].gravDir == -1f)
                    {
                        num160 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector11.Y;
                    }
                    float num161 = (float)System.Math.Sqrt((double)(num159 * num159 + num160 * num160));
                    if (num161 == 0f)
                    {
                        vector11 = new Vector2(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2), Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2));
                        num159 = projectile.position.X + (float)projectile.width * 0.5f - vector11.X;
                        num160 = projectile.position.Y + (float)projectile.height * 0.5f - vector11.Y;
                        num161 = (float)System.Math.Sqrt((double)(num159 * num159 + num160 * num160));
                    }
                    num161 = num158 / num161;
                    num159 *= num161;
                    num160 *= num161;
                    projectile.velocity.X = num159;
                    projectile.velocity.Y = num160;
                    if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                    {
                        projectile.Kill();
                    }
                }
            }
            if (projectile.velocity.X != 0f || projectile.velocity.Y != 0f)
            {
                projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - 2.355f;
            }
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            for (int k = 0; k < 20; k++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 66, 0f, 0f, 100, Bluemagic.PureColor, 2f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 4f;
            }
        }
    }
}