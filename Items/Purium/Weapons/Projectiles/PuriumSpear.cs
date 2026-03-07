using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Purium.Weapons.Projectiles
{
    public class PuriumSpear : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purium Lightbeam");
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = ProjAIStyleID.Spear;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1.3f;
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.ownerHitCheck = true;
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.direction = player.direction;
            player.heldProj = Projectile.whoAmI;
            player.itemTime = player.itemAnimation;
            Projectile.position = center - Projectile.Size / 2f;
            bool forwards = true;
            if (!player.frozen)
            {
                if (Projectile.ai[0] == 0f)
                {
                    Projectile.ai[0] = 3f;
                    Projectile.netUpdate = true;
                }
                if (player.itemAnimation < player.itemAnimationMax / 3)
                {
                    Projectile.ai[0] -= 2.4f;
                    forwards = false;
                }
                else
                {
                    Projectile.ai[0] += 2.1f;
                }
            }
            Projectile.position += Projectile.velocity * Projectile.ai[0];
            float speed = (float)Math.Sqrt(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y);
            if (Projectile.owner == Main.myPlayer && Projectile.localAI[0] == 0f)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity * 2f / speed, Mod.Find<ModProjectile>("PuriumLightbeam").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.ai[1] = proj;
                Projectile.localAI[0] = 1f;
            }
            int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
            if (forwards && uuid >= 0 && Main.projectile[uuid].active && Main.projectile[uuid].type == Mod.Find<ModProjectile>("PuriumLightbeam").Type)
            {
                PuriumLightbeam beam = (PuriumLightbeam)Main.projectile[uuid].ModProjectile;
                beam.AddPosition(Projectile.Center);
            }
            if (player.itemAnimation == 0)
            {
                Projectile.Kill();
            }
            Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 2.355f;
            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation -= 1.57f;
            }
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (target.defense > 100)
            {
                modifiers.FinalDamage.Flat += (target.defense - 100) / 2;
            }
        }
    }
}