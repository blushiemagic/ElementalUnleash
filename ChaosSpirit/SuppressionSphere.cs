using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class SuppressionSphere : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private int timer = 0;

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("ChaosSpirit3").Type)
            {
                Projectile.Kill();
                return;
            }
            for (int k = 0; k < 255; k++)
            {
                Player player = Main.player[k];
                if (player.active && !player.dead && Ellipse.Collides(Projectile.position, new Vector2(Projectile.width, Projectile.height), player.position, new Vector2(player.width, player.height)))
                {
                    bool flag = true;
                    for (int i = 0; i < Player.MaxBuffs; i++)
                    {
                        if (player.buffType[i] == Mod.Find<ModBuff>("Suppression1").Type)
                        {
                            player.buffType[i] = Mod.Find<ModBuff>("Suppression2").Type;
                            flag = false;
                        }
                        else if (player.buffType[i] == Mod.Find<ModBuff>("Suppression2").Type)
                        {
                            player.buffType[i] = Mod.Find<ModBuff>("Suppression3").Type;
                            flag = false;
                        }
                        else if (player.buffType[i] == Mod.Find<ModBuff>("Suppression3").Type)
                        {
                            player.buffType[i] = Mod.Find<ModBuff>("Suppression4").Type;
                            flag = false;
                        }
                        else if (player.buffType[i] == Mod.Find<ModBuff>("Suppression4").Type)
                        {
                            player.buffTime[i] = 300;
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        player.AddBuff(Mod.Find<ModBuff>("Suppression1").Type, 300);
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, Color.Red, 1.1f);
                    }
                    Projectile.Kill();
                }
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] >= 600f)
            {
                Projectile.Kill();
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                Projectile.frame %= 3;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.position - Main.screenPosition, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Rectangle frame = new Rectangle(0, 120 * Projectile.frame, 120, 120);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/SuppressionSphereBorder").Value, Projectile.position - Main.screenPosition, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}