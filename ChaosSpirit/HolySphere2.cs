using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.ChaosSpirit
{
    public class HolySphere2 : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("ChaosSpirit3").Type)
            {
                Projectile.Kill();
                return;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 180f)
            {
                if (npc.ModNPC is ChaosSpirit3 && ((ChaosSpirit3)npc.ModNPC).targets.Contains(Main.myPlayer))
                {
                    Player player = Main.player[Main.myPlayer];
                    if (!Ellipse.Collides(Projectile.position, new Vector2(Projectile.width, Projectile.height), player.position, new Vector2(player.width, player.height)))
                    {
                        player.GetModPlayer<BluemagicPlayer>().ChaosKill();
                        player.AddBuff(Mod.Find<ModBuff>("Undead").Type, 300, false);
                    }
                }
                if (Main.netMode != NetmodeID.Server)
                {
                    Projectile.Kill();
                }
            }
            if (Main.netMode == NetmodeID.Server && Projectile.ai[1] >= 240f)
            {
                Projectile.Kill();
            }

            if (Main.rand.Next(4) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, default(Color), 1f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.position - Main.screenPosition, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/HolySphereBorder2").Value, Projectile.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (Projectile.ai[1] > 140f)
            {
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * ((Projectile.ai[1] - 140f) / 40f));
            }
            return false;
        }
    }
}