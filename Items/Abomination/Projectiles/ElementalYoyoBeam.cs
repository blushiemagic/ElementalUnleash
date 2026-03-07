using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Abomination.Projectiles
{
    public class ElementalYoyoBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Yoyo");
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];
            switch ((int)Projectile.ai[0])
            {
            case 0:
                Lighting.AddLight(Projectile.position, 0.25f, 0.05f, 0.05f);
                break;
            case 1:
                Lighting.AddLight(Projectile.position, 0.05f, 0.2f, 0.25f);
                break;
            case 2:
                Lighting.AddLight(Projectile.position, 0.05f, 0.05f, 0.25f);
                break;
            case 3:
                Lighting.AddLight(Projectile.position, 0.15f, 0.2f, 0.2f);
                break;
            case 4:
                Lighting.AddLight(Projectile.position, 0.05f, 0.2f, 0.05f);
                break;
            case 5:
                Lighting.AddLight(Projectile.position, 0.25f, 0.25f, 0.05f);
                break;
            }
            Projectile.velocity *= 0.985f;
            Projectile.rotation += Projectile.velocity.X * 0.2f;
            if (Projectile.velocity.X > 0f)
            {
                Projectile.rotation += 0.08f;
            }
            else
            {
                Projectile.rotation -= 0.08f;
            }
            Projectile.ai[1] += 1f;
            if (Projectile.ai[1] > 45f)
            {
                Projectile.alpha += 10;
                if (Projectile.alpha >= 255)
                {
                    Projectile.alpha = 255;
                    Projectile.Kill();
                }
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Projectile.ai[0] == 3f)
            {
                modifiers.FinalDamage.Flat += 20;
            }
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (Projectile.ai[0] == 3f)
            {
                modifiers.FinalDamage.Flat += 20;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].Counterweight(target.Center, Projectile.damage, Projectile.knockBack);
            Projectile.friendly = false;
            Projectile.ai[1] = 1000f;
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime());
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            int debuff = GetDebuff();
            if (debuff > 0)
            {
                target.AddBuff(debuff, GetDebuffTime() / 2);
            }
        }

        public int GetDebuff()
        {
            switch ((int)Projectile.ai[0])
            {
            case 0:
                return BuffID.OnFire;
            case 1:
                return BuffID.Frostburn;
            case 2:
                return Mod.Find<ModBuff>("EtherealFlames").Type;
            case 3:
                return 0;
            case 4:
                return BuffID.Venom;
            case 5:
                return BuffID.Ichor;
            default:
                return 0;
            }
        }

        public int GetDebuffTime()
        {
            switch ((int)Projectile.ai[0])
            {
            case 0:
                return 600;
            case 1:
                return 400;
            case 2:
                return 300;
            case 3:
                return 0;
            case 4:
                return 400;
            case 5:
                return 900;
            default:
                return 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            int value = 255 - Projectile.alpha;
            return new Color(value, value, value, 0);
        }

        public override void PostDraw(Color lightColor)
        {
            int count = (int)Projectile.ai[1] + 1;
            if (count > 7)
            {
                count = 7;
            }
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height / 2);
            Vector2 drawPos = Projectile.position - Main.screenPosition + origin;
            drawPos.Y += Projectile.gfxOffY;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Color color = GetAlpha(lightColor).Value;
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            for (int k = 1; k < count; k++)
            {
                Vector2 drawOff = Projectile.velocity * k * 1.5f;
                float strength = 0.4f - k * 0.06f;
                strength *= 1f - Projectile.alpha / 255f;
                Color drawColor = color;
                drawColor.R = (byte)(color.R * strength);
                drawColor.G = (byte)(color.G * strength);
                drawColor.B = (byte)(color.B * strength);
                drawColor.A = (byte)(color.A * strength / 2f);
                float scale = Projectile.scale;
                scale -= k * 0.1f;
                Main.spriteBatch.Draw(texture, drawPos - drawOff, frame, drawColor, Projectile.rotation, origin, scale, spriteEffects, 0f);
            }
        }
    }
}
