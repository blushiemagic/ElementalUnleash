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
            DisplayName.SetDefault("blushiemagic (J)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.takenDamageMultiplier = 5f;
            this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
        }

        public override void AI()
        {
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (BlushieBoss.Phase3Attack > 8 || (BlushieBoss.Phase3Attack == 8 && BlushieBoss.Timer >= 2120))
            {
                return true;
            }
            Texture2D texture = mod.GetTexture("BlushieBoss/Skull_Back");
            spriteBatch.Draw(texture, npc.Bottom - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height), 1f, SpriteEffects.None, 0f);
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
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
                texture = mod.GetTexture("BlushieBoss/Skull_Top");
                Texture2D glowTexture = mod.GetTexture("BlushieBoss/Skull_Top_Glow");
                Vector2 drawPos = npc.Bottom + new Vector2(0f, -texture.Height / 2);
                spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, drawPos - Main.screenPosition, null, color, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
                texture = mod.GetTexture("BlushieBoss/Skull_Bottom");
                glowTexture = mod.GetTexture("BlushieBoss/Skull_Bottom_Glow");
                drawPos.X -= texture.Width / 2;
                drawPos.Y -= texture.Height / 2;
                drawPos += hinge;
                spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Color.White, angle, hinge, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, drawPos - Main.screenPosition, null, color, angle, hinge, 1f, SpriteEffects.None, 0f);
                texture = mod.GetTexture("BlushieBoss/Bone");
                glowTexture = mod.GetTexture("BlushieBoss/Bone_Glow");
                origin = new Vector2(texture.Width / 2, texture.Height / 2);
                spriteBatch.Draw(texture, BlushieBoss.BoneLTPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, BlushieBoss.BoneLBPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, BlushieBoss.BoneRTPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(texture, BlushieBoss.BoneRBPos - Main.screenPosition, null, Color.White, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, BlushieBoss.BoneLTPos - Main.screenPosition, null, color, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, BlushieBoss.BoneLBPos - Main.screenPosition, null, color, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, BlushieBoss.BoneRTPos - Main.screenPosition, null, color, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(glowTexture, BlushieBoss.BoneRBPos - Main.screenPosition, null, color, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
                return;
            }
            else if (BlushieBoss.Phase3Attack > 8 || (BlushieBoss.Phase3Attack == 8 && BlushieBoss.Timer >= 2420))
            {
                return;
            }
            texture = mod.GetTexture("BlushieBoss/Skull");
            Vector2 center = npc.Bottom + new Vector2(0f, -texture.Height / 2);
            spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            texture = mod.GetTexture("BlushieBoss/Bone");
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
            spriteBatch.Draw(texture, BlushieBoss.BoneLTPos - Main.screenPosition, null, color, BlushieBoss.BoneLTRot, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, BlushieBoss.BoneLBPos - Main.screenPosition, null, color, BlushieBoss.BoneLBRot, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, BlushieBoss.BoneRTPos - Main.screenPosition, null, color, BlushieBoss.BoneRTRot, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, BlushieBoss.BoneRBPos - Main.screenPosition, null, color, BlushieBoss.BoneRBRot, origin, 1f, SpriteEffects.None, 0f);
        }

        public override bool UseSpecialDamage()
        {
            return false;
        }
    }
}