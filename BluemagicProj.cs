using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Projectiles
{
    public class BluemagicProj : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (projectile.type != ProjectileID.NorthPoleSnowflake && !projectile.npcProj && projectile.CountsAsClass(DamageClass.Melee) && !projectile.noEnchantments && Main.player[projectile.owner].active)
            {
                int meleeEnchant = Main.player[projectile.owner].GetModPlayer<BluemagicPlayer>().customMeleeEnchant;
                if (meleeEnchant == 1)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, Mod.Find<ModDust>("EtherealFlame").Type, projectile.velocity.X * 0.2f + projectile.direction * 3, projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.7f;
                        Main.dust[dust].velocity.Y -= 0.5f;
                    }
                }
                else if (meleeEnchant == 2)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.IceTorch, projectile.velocity.X * 0.2f + projectile.direction * 3, projectile.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].velocity *= 0.7f;
                        Main.dust[dust].velocity.Y -= 0.5f;
                    }
                }
            }
        }
    }
}