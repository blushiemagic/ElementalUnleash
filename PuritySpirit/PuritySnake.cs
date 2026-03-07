using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PuritySnake : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void Trail");
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 180;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.hide = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            NPC source = Main.npc[(int)Projectile.ai[0]];
            if (!source.active || source.type != Mod.Find<ModNPC>("PuritySpirit").Type)
            {
                Projectile.Kill();
                return;
            }
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 240f)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.localAI[0] % 10 == 0)
            {
                BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
                if (modPlayer.heroLives > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item20);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                }
            }
            if (Projectile.localAI[0] > 60f)
            {
                IList<int> targets = ((PuritySpirit)source.ModNPC).targets;
                Vector2 offset = Vector2.Zero;
                bool flag = false;
                foreach (int player in targets)
                {
                    Vector2 newOffset = Main.player[player].Center - Projectile.Center;
                    if (!flag || offset.Length() > newOffset.Length())
                    {
                        offset = newOffset;
                        flag = true;
                    }
                }
                if (offset != Vector2.Zero)
                {
                    offset.Normalize();
                }
                offset *= 7f + 3f * (1 - Projectile.ai[1]);
                Projectile.velocity = offset;
            }
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                CreateDust(Projectile.oldPos[k]);
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.defenseEffect = 1f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero)
                {
                    return null;
                }
                projHitbox.X = (int)Projectile.oldPos[k].X;
                projHitbox.Y = (int)Projectile.oldPos[k].Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return null;
        }

        public void CreateDust(Vector2 pos)
        {
            if (Main.rand.Next(5) == 0)
            {
                int dust = Dust.NewDust(pos, Projectile.width, Projectile.height, Mod.Find<ModDust>("Smoke").Type, 0f, 0f, 0, new Color(0, 180, 0));
                Main.dust[dust].scale = 2f;
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].noLight = true;
            }
        }
    }
}