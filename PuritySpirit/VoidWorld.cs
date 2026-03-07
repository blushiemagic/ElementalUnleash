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
    public class VoidWorld : ModProjectile
    {
        private Random rand;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void World");
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 200;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            CooldownSlot = 1;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.localAI[0] = reader.ReadSingle();
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (!Main.dedServ && Projectile.localAI[0] >= 180f && Projectile.localAI[0] < 480f && Main.rand.Next(10) == 0)
            {
                BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
                if (modPlayer.heroLives > 0)
                {
                    SoundEngine.PlaySound(SoundID.Item14);
                }
                else
                {
                    SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                }
            }
            Projectile.position = NextPosition();
            if (Projectile.localAI[0] >= 500f)
            {
                Projectile.Kill();
            }
        }

        private Vector2 NextPosition()
        {
            if (rand == null)
            {
                rand = new Random((int)Projectile.ai[1]);
            }
            const int interval = 60;
            int arenaWidth = PuritySpirit.arenaWidth;
            int arenaHeight = PuritySpirit.arenaHeight;
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            PuritySpirit modNPC = (PuritySpirit)npc.ModNPC;
            Vector2 nextPos;
            if (Projectile.localAI[0] > 300f)
            {
                nextPos = npc.Center;
            }
            else if ((int)Projectile.localAI[0] % 100 == 0 || (Main.expertMode && (int)Projectile.localAI[0] % 50 == 0))
            {
                int k = modNPC.targets[rand.Next(modNPC.targets.Count)];
                nextPos = Main.player[k].Center;
            }
            else if (rand.Next(5) == 0)
            {
                int k = modNPC.targets[rand.Next(modNPC.targets.Count)];
                nextPos = Main.player[k].Center + interval * new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6));
                if (nextPos.X < npc.Center.X - arenaWidth / 2)
                {
                    nextPos.X += arenaWidth;
                }
                else if (nextPos.X > npc.Center.X + arenaWidth / 2)
                {
                    nextPos.X -= arenaWidth;
                }
                if (nextPos.Y < npc.Center.Y - arenaHeight / 2)
                {
                    nextPos.Y += arenaHeight;
                }
                else if (nextPos.Y > npc.Center.Y + arenaHeight / 2)
                {
                    nextPos.Y -= arenaHeight;
                }
            }
            else
            {
                int leftBound = (-arenaWidth / 2 + 40) / interval;
                int rightBound = (arenaWidth / 2 - 40) / interval + 1;
                int upperBound = (-arenaHeight / 2 + 40) / interval;
                int lowerBound = (arenaHeight / 2 - 40) / interval + 1;
                nextPos = npc.Center + interval * new Vector2(rand.Next(leftBound, rightBound), rand.Next(upperBound, lowerBound));
            }
            nextPos.X -= Projectile.width / 2;
            nextPos.Y -= Projectile.height / 2;
            return nextPos;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            if (target.hurtCooldowns[1] <= 0)
            {
                BluemagicPlayer modPlayer = target.GetModPlayer<BluemagicPlayer>();
                modPlayer.constantDamage = Projectile.damage;
                modPlayer.percentDamage = Main.expertMode ? 1.2f : 1f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            projHitbox.Width -= 16;
            projHitbox.Height -= 16;
            for (int k = Math.Max(180, (int)Projectile.localAI[0] - 301); k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] != Vector2.Zero)
                {
                    projHitbox.X = (int)Projectile.oldPos[k].X + 8;
                    projHitbox.Y = (int)Projectile.oldPos[k].Y + 8;
                    if (projHitbox.Intersects(targetHitbox))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int k = Math.Max(0, (int)Projectile.localAI[0] - 300); k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] != Vector2.Zero)
                {
                    Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition;
                    Rectangle frame = new Rectangle(0, 0, 80, 80);
                    frame.Y += 82 * (k * 7 / 180);
                    Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, frame, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}