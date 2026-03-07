using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class PurityBeam : ModProjectile
    {
        internal const float charge = 60f;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Purifying Column");
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 14;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void AI()
        {
            if (Projectile.height != (int)Projectile.ai[0])
            {
                Vector2 center = Projectile.Center;
                Projectile.height = (int)Projectile.ai[0];
                Projectile.Center = center;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == charge)
            {
                BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
                if (modPlayer.heroLives > 0)
                {
                    SoundEngine.PlaySound(SoundID.Zombie104);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Zombie104, Projectile.Center);
                }
                Projectile.hostile = true;
            }
            if (Projectile.ai[1] >= charge + 60f)
            {
                Projectile.Kill();
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.constantDamage = Projectile.damage;
                modPlayer.percentDamage = Main.expertMode ? 0.6f : 0.5f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color = Color.White * 0.8f;
            Vector2 drawPos = Projectile.Top - Main.screenPosition;
            Rectangle frame = new Rectangle(0, 0, 100, 14);
            Vector2 drawCenter = new Vector2(50f, 0f);
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frame, color, 0f, drawCenter, 1f, SpriteEffects.None, 0f);
            drawPos.Y += Projectile.height / 2;
            frame.Y += 14;
            drawCenter.Y += 7f;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frame, color, 0f, drawCenter, new Vector2(Math.Min(Projectile.ai[1], charge) / charge, (Projectile.height - 28) / 14f), SpriteEffects.None, 0f);
            drawPos.Y += Projectile.height / 2;
            frame.Y += 14;
            drawCenter.Y += 7f;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frame, color, 0f, drawCenter, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}