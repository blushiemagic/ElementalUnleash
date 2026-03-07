using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public class BlushiemagicJ : BlushiemagicBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("blushiemagic (J)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.takenDamageMultiplier = 5f;
            this.Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Phyrnna - Return of the Snow Queen");
        }

        public override void AI()
        {
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (BlushieBoss.Phase3Attack > 8 || (BlushieBoss.Phase3Attack == 8 && BlushieBoss.Timer >= 2120))
            {
                return true;
            }
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull_Back").Value;
            Main.spriteBatch.Draw(texture, NPC.Bottom - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height), 1f, SpriteEffects.None, 0f);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture;
            Color color;
            Vector2 origin;
            if (BlushieBoss.Phase3Attack == 8 && BlushieBoss.Timer >= 2120 && BlushieBoss.Timer < 2420)
            {
                int timer = BlushieBoss.Timer - 2120;
                float angle = MathHelper.PiOver2;
                if (timer <= 30)
                {
                    angle *= timer * timer / 900f;
                }
                else
                {
                    angle += MathHelper.Pi / 6f * (float)Math.Sin(MathHelper.Pi * (timer - 30f) / 40f);
                }
                Vector2 hinge = new Vector2(13f, 111f);
                float glow = 0f;
                if (timer > 120)
                {
                    glow = (timer - 120) / 180f;
                    if (glow > 1f) {
                        glow = 1f;
                    }
                }
                color = Color.White * glow;
                texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull_Top").Value;
                Texture2D glowTexture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull_Top_Glow").Value;
                Vector2 drawPos = NPC.Bottom + new Vector2(0f, -texture.Height / 2);
                Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, drawPos - Main.screenPosition, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
                texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull_Bottom").Value;
                glowTexture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull_Bottom_Glow").Value;
                drawPos.X -= texture.Width / 2;
                drawPos.Y -= texture.Height / 2;
                drawPos += hinge;
                Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Color.White, angle, hinge, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, drawPos - Main.screenPosition, null, color, angle, hinge, 1f, SpriteEffects.None, 0f);
                texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Bone").Value;
                glowTexture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Bone_Glow").Value;
                origin = new Vector2(texture.Width / 2, texture.Height / 2);
                Main.spriteBatch.Draw(texture, BlushieBoss.BoneLTPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, BlushieBoss.BoneLBPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, BlushieBoss.BoneRTPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, BlushieBoss.BoneRBPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, BlushieBoss.BoneLTPos - Main.screenPosition, null, color, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, BlushieBoss.BoneLBPos - Main.screenPosition, null, color, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, BlushieBoss.BoneRTPos - Main.screenPosition, null, color, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(glowTexture, BlushieBoss.BoneRBPos - Main.screenPosition, null, color, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
                return;
            }
            else if (BlushieBoss.Phase3Attack > 8 || (BlushieBoss.Phase3Attack == 8 && BlushieBoss.Timer >= 2420))
            {
                return;
            }
            texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Skull").Value;
            Vector2 center = NPC.Bottom + new Vector2(0f, -texture.Height / 2);
            Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/Bone").Value;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            float alpha = 1f;
            if (BlushieBoss.Timer < 960)
            {
                alpha = (BlushieBoss.Timer - 900) / 60f;
                if (alpha < 0f)
                {
                    alpha = 0f;
                }
            }
            color = Color.White * alpha;
            Main.spriteBatch.Draw(texture, BlushieBoss.BoneLTPos - Main.screenPosition, null, color, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, BlushieBoss.BoneLBPos - Main.screenPosition, null, color, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, BlushieBoss.BoneRTPos - Main.screenPosition, null, color, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, BlushieBoss.BoneRBPos - Main.screenPosition, null, color, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
        }

        public override bool UseSpecialDamage()
        {
            return false;
        }
    }
}