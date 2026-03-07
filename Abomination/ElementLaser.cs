using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Abomination
{
    public class ElementLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Elemental Laser");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.timeLeft = 630;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (!npc.active || npc.type != Mod.Find<ModNPC>("CaptiveElement").Type)
            {
                Projectile.Kill();
                return;
            }
            if (Projectile.localAI[0] == 0f)
            {
                if (npc.type == Mod.Find<ModNPC>("CaptiveElement").Type && npc.ai[1] == 2f && (Main.expertMode || NPC.downedMoonlord))
                {
                    CooldownSlot = 1;
                }
                Projectile.Name = GetName();
            }
            Projectile.Center = npc.Center;
            Projectile.localAI[0] += 1f;
            float maxTime = Main.expertMode && NPC.downedMoonlord ? 450f : 300f;
            if (Projectile.localAI[0] > maxTime)
            {
                Projectile.damage = 0;
                Projectile.alpha = (int)((Projectile.localAI[0] - maxTime) / 30f);
            }
            if (Projectile.localAI[0] > maxTime + 30f)
            {
                Projectile.Kill();
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.Next(2) == 0 && Projectile.ai[0] >= 0f)
            {
                int debuff = GetDebuff();
                if (debuff >= 0)
                {
                    target.AddBuff(debuff, GetDebuffTime(), true);
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPoint = Main.npc[(int)Projectile.ai[1]].Center;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPoint, 4f, ref point);
        }

        public string GetName()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type == Mod.Find<ModNPC>("Abomination").Type)
            {
                return "Fire Beam";
            }
            if (npc.ai[1] == 0f)
            {
                return "Frost Beam";
            }
            if (npc.ai[1] == 1f)
            {
                return "Spirit Beam";
            }
            if (npc.ai[1] == 2f)
            {
                return "Water Beam";
            }
            if (npc.ai[1] == 3f)
            {
                return "Venom Beam";
            }
            if (npc.ai[1] == 4f)
            {
                return "Ichor Beam";
            }
            return "Elemental Laser";
        }

        public Color GetColor()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type == Mod.Find<ModNPC>("Abomination").Type)
            {
                return new Color(250, 10, 0);
            }
            if (npc.ai[1] == 0f)
            {
                return new Color(0, 230, 230);
            }
            if (npc.ai[1] == 1f)
            {
                return new Color(0, 153, 230);
            }
            if (npc.ai[1] == 3f)
            {
                return new Color(0, 178, 0);
            }
            if (npc.ai[1] == 4f)
            {
                return new Color(230, 192, 0);
            }
            return Color.White;
        }

        public int GetDebuff()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type == Mod.Find<ModNPC>("Abomination").Type)
            {
                return BuffID.OnFire;
            }
            if (npc.ai[1] == 0f)
            {
                return BuffID.Frostburn;
            }
            if (npc.ai[1] == 1f)
            {
                return Mod.Find<ModBuff>("EtherealFlames").Type;
            }
            if (npc.ai[1] == 3f)
            {
                return BuffID.Venom;
            }
            if (npc.ai[1] == 4f)
            {
                return BuffID.Ichor;
            }
            return -1;
        }

        public int GetDebuffTime()
        {
            NPC npc = Main.npc[(int)Projectile.ai[0]];
            if (npc.type == Mod.Find<ModNPC>("Abomination").Type)
            {
                return 600;
            }
            if (npc.ai[1] == 0f)
            {
                return 400;
            }
            if (npc.ai[1] == 1f)
            {
                return 300;
            }
            if (npc.ai[1] == 3f)
            {
                return 400;
            }
            if (npc.ai[1] == 4f)
            {
                return 900;
            }
            return -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 endPoint = Main.npc[(int)Projectile.ai[1]].Center;
            Vector2 unit = endPoint - Projectile.Center;
            float length = unit.Length();
            unit.Normalize();
            Color alpha = GetColor() * ((255 - Projectile.alpha) / 255f);
            for (float k = 0; k <= length; k += 4f)
            {
                Vector2 drawPos = Projectile.Center + unit * k - Main.screenPosition;
                Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, alpha, k, new Vector2(2, 2), 1f, SpriteEffects.None, 0f);
            }
            return false;
        }
    }
}