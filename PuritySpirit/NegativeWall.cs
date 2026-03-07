using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Bluemagic.PuritySpirit
{
    public class NegativeWall : ModProjectile
    {
        public const float speed = 2f;
        private static readonly Color color1 = new Color(100, 0, 100);
        private static readonly Color color2 = new Color(100, 0, 0);

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.alpha = 127;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            Projectile.localAI[0] %= 120f;
            if (Projectile.ai[1] > 0f && Projectile.height != (int)Projectile.ai[1])
            {
                Vector2 center = Projectile.Center;
                Projectile.height = (int)Projectile.ai[1];
                Projectile.Center = center;
            }
            if (Projectile.ai[1] < 0f && Projectile.width != (int)-Projectile.ai[1])
            {
                Vector2 center = Projectile.Center;
                Projectile.width = (int)-Projectile.ai[1];
                Projectile.Center = center;
            }
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            int arenaWidth = PuritySpirit.arenaWidth;
            int arenaHeight = PuritySpirit.arenaHeight;
            if (Projectile.Center.X >= npc.Center.X + arenaWidth / 2)
            {
                Projectile.velocity.X = -speed;
            }
            else if (Projectile.Center.X <= npc.Center.X - arenaWidth / 2)
            {
                Projectile.velocity.X = speed;
            }
            if (Projectile.Center.Y >= npc.Center.Y + arenaHeight / 2)
            {
                Projectile.velocity.Y = -speed;
            }
            else if (Projectile.Center.Y <= npc.Center.Y - arenaHeight / 2)
            {
                Projectile.velocity.Y = speed;
            }
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active && !player.dead && player.Hitbox.Intersects(Projectile.Hitbox))
                {
                    BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
                    if (modPlayer.purityDebuffCooldown <= 0)
                    {
                        modPlayer.PuritySpiritDebuff();
                        modPlayer.purityDebuffCooldown = Main.expertMode ? 60 : 90;
                    }
                }
            }
            Projectile.timeLeft = 2;
            if (!npc.active || npc.type != Mod.Find<ModNPC>("PuritySpirit").Type)
            {
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Color color;
            if (Projectile.localAI[0] < 60f)
            {
                color = Color.Lerp(color1, color2, Projectile.localAI[0] / 60f);
            }
            else
            {
                color = Color.Lerp(color2, color1, (Projectile.localAI[0] - 60f) / 30f);
            }
            color *= 0.85f;
            Vector2 drawPos = Projectile.position - Main.screenPosition;
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, 0f, Vector2.Zero, Projectile.Size, SpriteEffects.None, 0f);
            return false;
        }
    }
}