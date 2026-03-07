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
    public class HolySphere : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private int timer = 0;

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || (npc.type != Mod.Find<ModNPC>("ChaosSpirit").Type && npc.type != Mod.Find<ModNPC>("ChaosSpirit2").Type))
            {
                Projectile.Kill();
                return;
            }
            Vector2 offset = Projectile.Center - npc.Center;
            float distance = offset.Length();
            float rotation = offset.ToRotation() + 0.01f;
            offset = distance * rotation.ToRotationVector2();
            Projectile.Center = npc.Center + offset;

            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] == 300f)
            {
                List<int> targets = null;
                if (npc.ModNPC is ChaosSpirit)
                {
                    targets = ((ChaosSpirit)npc.ModNPC).targets;
                }
                else if (npc.ModNPC is ChaosSpirit2)
                {
                    targets = ((ChaosSpirit2)npc.ModNPC).targets;
                }
                if (targets != null && targets.Contains(Main.myPlayer))
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
            if (Main.netMode == NetmodeID.Server && Projectile.ai[1] > 360f)
            {
                Projectile.Kill();
            }

            if (Main.rand.Next(2) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Sparkle").Type, 0f, 0f, 0, default(Color), 1f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.position - Main.screenPosition, null, Color.White * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/ChaosSpirit/HolySphereBorder").Value, Projectile.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            if (Projectile.ai[1] > 260f)
            {
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * ((Projectile.ai[1] - 260f) / 40f));
            }
            return false;
        }
    }
}