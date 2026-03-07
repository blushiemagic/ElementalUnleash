using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class ElementalYoyo : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = -1f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 400f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 17.5f;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 4f)
                {
                    float num3 = 480f;
                    Vector2 shootVel = Projectile.velocity;
                    Vector2 randOffset = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    randOffset.Normalize();
                    randOffset *= (float)Main.rand.Next(10, 61) * 0.1f;
                    if (Main.rand.Next(3) == 0)
                    {
                        randOffset *= 2f;
                    }
                    shootVel *= 0.25f;
                    shootVel += randOffset;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].CanBeChasedBy(Projectile, false))
                        {
                            Vector2 targetPos = Main.npc[i].Center;
                            float distance = Math.Abs(Projectile.Center.X - targetPos.X) + Math.Abs(Projectile.Center.Y - targetPos.Y);
                            if (distance < num3 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                            {
                                num3 = distance;
                                shootVel = targetPos - Projectile.Center;
                                shootVel.Normalize();
                                shootVel *= 12f;
                            }
                        }
                    }
                    shootVel *= 0.8f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - shootVel, shootVel, ModContent.ProjectileType<ElementalYoyoBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(6), 0f);
                    Projectile.localAI[1] = 0f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            int choice = Main.rand.Next(5);
            int debuff = GetDebuff(choice);
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime(choice));
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int choice = Main.rand.Next(5);
            int debuff = GetDebuff(choice);
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime(choice) / 2);
            }
        }

        public int GetDebuff(int type)
        {
            switch (type)
            {
            case 0:
                return BuffID.OnFire;
            case 1:
                return BuffID.Frostburn;
            case 2:
                return Mod.Find<ModBuff>("EtherealFlames").Type;
            case 3:
                return BuffID.Venom;
            case 4:
                return BuffID.Ichor;
            default:
                return 0;
            }
        }

        public int GetDebuffTime(int type)
        {
            switch (type)
            {
            case 0:
                return 600;
            case 1:
                return 400;
            case 2:
                return 300;
            case 3:
                return 400;
            case 4:
                return 900;
            default:
                return 0;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 200);
        }
    }
}
