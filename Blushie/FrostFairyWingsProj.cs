using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Blushie
{
    public class FrostFairyWingsProj : Minion
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wings of the Frost Fairy");
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 62;
            Projectile.height = 52;
            Projectile.minion = true;
            Projectile.minionSlots = 1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.frostFairy = false;
            }
            if (modPlayer.frostFairy)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void Behavior()
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int k = 0; k < Projectile.whoAmI; k++)
                {
                    if (Main.projectile[k].active && Main.projectile[k].owner == Projectile.owner && Main.projectile[k].type == Projectile.type)
                    {
                        Projectile.Kill();
                        break;
                    }
                }
            }
            Projectile.Center = Main.player[Projectile.owner].Center;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] >= 10f)
            {
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 origin = Projectile.Center + new Vector2(-18f, -13f);
                    int target = GetTarget((NPC npc) => npc.Center.X <= Projectile.Center.X && npc.Center.Y <= Projectile.Center.Y, origin);
                    if (target >= 0)
                    {
                        Vector2 dir = Main.npc[target].Center - origin;
                        dir.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), origin, dir, Mod.Find<ModProjectile>("FrostFairyLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, Projectile.identity);
                    }
                    origin = Projectile.Center + new Vector2(18f, -13f);
                    target = GetTarget((NPC npc) => npc.Center.X >= Projectile.Center.X && npc.Center.Y <= Projectile.Center.Y, origin);
                    if (target >= 0)
                    {
                        Vector2 dir = Main.npc[target].Center - origin;
                        dir.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), origin, dir, Mod.Find<ModProjectile>("FrostFairyLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, Projectile.identity);
                    }
                    origin = Projectile.Center + new Vector2(-16f, 15f);
                    target = GetTarget((NPC npc) => npc.Center.X <= Projectile.Center.X && npc.Center.Y >= Projectile.Center.Y, origin);
                    if (target >= 0)
                    {
                        Vector2 dir = Main.npc[target].Center - origin;
                        dir.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), origin, dir, Mod.Find<ModProjectile>("FrostFairyLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 2f, Projectile.identity);
                    }
                    origin = Projectile.Center + new Vector2(16f, 15f);
                    target = GetTarget((NPC npc) => npc.Center.X >= Projectile.Center.X && npc.Center.Y >= Projectile.Center.Y, origin);
                    if (target >= 0)
                    {
                        Vector2 dir = Main.npc[target].Center - origin;
                        dir.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), origin, dir, Mod.Find<ModProjectile>("FrostFairyLaser").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 3f, Projectile.identity);
                    }
                    Projectile.ai[0] = 0f;
                }
            }
        }

        private int GetTarget(Func<NPC, bool> priority, Vector2 origin)
        {
            int index = -1;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].CanBeChasedBy(Projectile))
                {
                    float distance = Vector2.Distance(origin, Main.npc[k].Center);
                    if (distance < 800f)
                    {
                        if (index < 0)
                        {
                            index = k;
                        }
                        else if (!priority(Main.npc[index]) && priority(Main.npc[k]))
                        {
                            index = k;
                        }
                        else if (priority(Main.npc[index]) == priority(Main.npc[k]) && distance < Vector2.Distance(origin, Main.npc[index].Center))
                        {
                            index = k;
                        }
                    }
                }
            }
            return index;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}