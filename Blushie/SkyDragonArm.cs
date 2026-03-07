using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;
using Bluemagic.BlushieBoss;

namespace Bluemagic.Blushie
{
    public class SkyDragonArm : Minion
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/BlushieBoss/DragonClaw";
            }
        }

        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.minion = true;
            Projectile.minionSlots = 0.5f;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
            writer.Write(Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
            Projectile.localAI[1] = reader.ReadSingle();
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
                int uuid = Projectile.GetByUUID(Projectile.owner, Projectile.ai[0]);
                if (uuid < 0)
                {
                    Projectile.Kill();
                    return;
                }
                Projectile head = Main.projectile[uuid];
                if (!head.active || head.type != Mod.Find<ModProjectile>("SkyDragonHead").Type)
                {
                    Projectile.Kill();
                    return;
                }
            }

            if (Projectile.localAI[0] == 1f)
            {
                Projectile.velocity *= 0.99f;
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] % 30f == 0f)
                {
                    int index = -1;
                    float distance = 1600f;
                    for (int k = 0; k < 200; k++)
                    {
                        NPC check = Main.npc[k];
                        if (check.CanBeChasedBy(Projectile) && Vector2.Distance(check.Center, Projectile.Center) < distance)
                        {
                            index = k;
                            distance = Vector2.Distance(check.Center, Projectile.Center);
                        }
                    }
                    if (index >= 0)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, Mod.Find<ModProjectile>("SkyDragonBullet").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 1f, index);
                    }
                }
                if (Projectile.localAI[1] >= 60)
                {
                    Projectile.localAI[0] = 0f;
                    Projectile.localAI[1] = 0f;
                }
            }
            else
            {
                Player player = Main.player[Projectile.owner];
                int index = -1;
                float distance = 1600f;
                float playerDistance = 3200f;
                for (int k = 0; k < 200; k++)
                {
                    NPC check = Main.npc[k];
                    if (check.CanBeChasedBy(Projectile) && Vector2.Distance(check.Center, player.Center) < playerDistance && Vector2.Distance(check.Center, Projectile.Center) < distance)
                    {
                        index = k;
                        distance = Vector2.Distance(check.Center, Projectile.Center);
                    }
                }
                if (index < 0)
                {
                    Vector2 target = player.Center + new Vector2(Projectile.ai[1] * 400f, 0f);
                    Projectile.Center = Vector2.Lerp(Projectile.Center, target, 0.1f);
                }
                else
                {
                    Vector2 target = Main.npc[index].Center;
                    Vector2 offset = target - Projectile.Center;
                    if (offset == Vector2.Zero)
                    {
                        offset = -Vector2.UnitY;
                    }
                    offset.Normalize();
                    Projectile.velocity = 16f * offset;
                    Projectile.localAI[0] = 1f;
                    Projectile.localAI[1] = 0f;
                }
            }
        }

        public override bool MinionContactDamage()
        {
            return true;
        }
    }
}