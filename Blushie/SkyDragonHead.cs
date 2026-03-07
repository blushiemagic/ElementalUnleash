using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;
using Bluemagic.BlushieBoss;

namespace Bluemagic.Blushie
{
    public class SkyDragonHead : Minion
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/BlushieBoss/DragonHead";
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 200;
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
                modPlayer.skyDragon = false;
            }
            if (modPlayer.skyDragon)
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
                        return;
                    }
                }
            }
            if (Projectile.owner == Main.myPlayer && Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
                int proj1 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(400f, 0f), Vector2.Zero, Mod.Find<ModProjectile>("SkyDragonArm").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.identity, 1f);
                int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + new Vector2(-400f, 0f), Vector2.Zero, Mod.Find<ModProjectile>("SkyDragonArm").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.identity, -1f);
                Projectile.ai[0] = Main.projectile[proj1].identity;
                Projectile.ai[1] = Main.projectile[proj2].identity;
                Projectile.netUpdate = true;
            }
            if (Projectile.owner == Main.myPlayer)
            {
                int leftUuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[0]);
                int rightUuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
                if (leftUuid < 0 || rightUuid < 0)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile left = Main.projectile[leftUuid];
                Projectile right = Main.projectile[rightUuid];
                if (!left.active || !right.active || left.type != Mod.Find<ModProjectile>("SkyDragonArm").Type || right.type != Mod.Find<ModProjectile>("SkyDragonArm").Type)
                {
                    Projectile.Kill();
                    return;
                }
            }

            Vector2 target = Main.player[Projectile.owner].Center + new Vector2(0f, -240f);
            Projectile.Center = Vector2.Lerp(Projectile.Center, target, 0.1f);
            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] < 60f && Projectile.localAI[1] % 5 == 0)
                {
                    int index = -1;
                    float distance = 1600f;
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && Main.npc[k].CanBeChasedBy(Projectile))
                        {
                            float check = Vector2.Distance(Projectile.Center, Main.npc[k].Center);
                            if (check < distance)
                            {
                                index = k;
                                distance = check;
                            }
                        }
                    }
                    if (index >= 0)
                    {
                        Vector2 offset = Main.npc[index].Center - Projectile.Center;
                        if (distance == 0f)
                        {
                            offset = -Vector2.UnitY;
                        }
                        offset.Normalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 32f * offset, Mod.Find<ModProjectile>("SkyDragonBullet").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, index);
                    }
                }
                if (Projectile.localAI[1] >= 120f)
                {
                    Projectile.localAI[1] = 0f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 origin = Main.player[Projectile.owner].Center;
            int leftUuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[0]);
            int rightUuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[1]);
            if (leftUuid >= 0 && rightUuid >= 0)
            {
                BlushiemagicM.DrawDragonArms(Main.spriteBatch, origin, Main.projectile[leftUuid].Center, Main.projectile[rightUuid].Center, true);
            }
            BlushiemagicM.DrawDragonHead(Main.spriteBatch, origin, Projectile.Center);
            return false;
        }
    }
}