using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Items.Abomination.Projectiles
{

    public class ElementSystem : ModSystem
    {
        public static int[] elementToType = new int[6];
    }


    public class MiniCaptiveElement0 : MiniCaptiveElement
    {
        public override int element => 0;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[0] = Projectile.type;
        }
    }
    public class MiniCaptiveElement1 : MiniCaptiveElement
    {
        public override int element => 1;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[1] = Projectile.type;
        }
    }
    public class MiniCaptiveElement2 : MiniCaptiveElement
    {
        public override int element => 2;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[2] = Projectile.type;
        }
    }
    public class MiniCaptiveElement3 : MiniCaptiveElement
    {
        public override int element => 3;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[3] = Projectile.type;
        }
    }
    public class MiniCaptiveElement4 : MiniCaptiveElement
    {
        public override int element => 4;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[4] = Projectile.type;
        }
    }
    public class MiniCaptiveElement5 : MiniCaptiveElement
    {
        public override int element => 5;
        public override void Load()
        {
            base.Load();
            ElementSystem.elementToType[5] = Projectile.type;
        }
    }

    public abstract class MiniCaptiveElement : Minion
    {
        public virtual int element => -1;

        protected override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override string Texture
        {
            get
            {
                return "Bluemagic/Items/Abomination/Projectiles/MiniCaptiveElement";
            }
        }

        private bool Charging
        {
            get
            {
                return element == 2 || element == 5;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mini Captive Element");
            Main.projFrames[Projectile.type] = 6;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.minion = true;
            Projectile.minionSlots = 0.333f;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (player.dead)
            {
                modPlayer.elementMinion = false;
            }
            if (modPlayer.elementMinion)
            {
                Projectile.timeLeft = 2;
            }
        }

        public override void Behavior()
        {
            if (Projectile.localAI[0] == 0f && Main.myPlayer == Projectile.owner && element == 0)
            {
                for (int k = 1; k <= 5; k++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ElementSystem.elementToType[k], Projectile.damage, Projectile.knockBack, Projectile.owner);
                }
                Projectile.localAI[0] = 1f;
            }
            Player player = Main.player[Projectile.owner];
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            Projectile.frame = element;
            if (Main.rand.Next(4) == 0)
            {
                CreateDust();
            }
            for (int k = 0; k < 1000; k++)
            {
                Projectile other = Main.projectile[k];
                if (k != Projectile.whoAmI && other.active && other.owner == Projectile.owner && ElementSystem.elementToType.Contains(other.type) && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width)
                {
                    const float pushAway = 0.05f;
                    if (Projectile.position.X < other.position.X)
                    {
                        Projectile.velocity.X -= pushAway;
                    }
                    else
                    {
                        Projectile.velocity.X += pushAway;
                    }
                    if (Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= pushAway;
                    }
                    else
                    {
                        Projectile.velocity.Y += pushAway;
                    }
                }
            }

            if (Projectile.ai[0] == 2f && Charging)
            {
                Projectile.ai[1] += 1f;
                Projectile.extraUpdates = 1;
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
                if (Projectile.ai[1] > 40f)
                {
                    Projectile.ai[1] = 1f;
                    Projectile.ai[0] = 0f;
                    Projectile.extraUpdates = 0;
                    Projectile.numUpdates = 0;
                    Projectile.netUpdate = true;
                }
                else
                {
                    return;
                }
            }

            if (Projectile.ai[0] != 1f)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16)))
            {
                Projectile.tileCollide = false;
            }

            float targetDist = 700f;
            Vector2 targetPos = Projectile.position;
            bool hasTarget = false;
            NPC ownerTarget = Projectile.OwnerMinionAttackTargetNPC;
            if (ownerTarget != null && ownerTarget.CanBeChasedBy(Projectile))
            {
                if (Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, ownerTarget.position, ownerTarget.width, ownerTarget.height))
                {
                    targetDist = Vector2.Distance(ownerTarget.Center, Projectile.Center);
                    targetPos = ownerTarget.Center;
                    hasTarget = true;
                }
            }
            if (!hasTarget)
            {
                for (int k = 0; k < 200; k++)
                {
                    NPC npc = Main.npc[k];
                    if (npc.CanBeChasedBy(Projectile))
                    {
                        float distance = Vector2.Distance(npc.Center, Projectile.Center);
                        if (((distance < Vector2.Distance(Projectile.Center, targetPos) && distance < targetDist) || !hasTarget) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height))
                        {
                            targetDist = distance;
                            targetPos = npc.Center;
                            hasTarget = true;
                        }
                    }
                }
            }
            float leashLength = hasTarget ? 1200f : 800f;
            if (Vector2.Distance(player.Center, Projectile.Center) > leashLength)
            {
                Projectile.ai[0] = 1f;
                Projectile.tileCollide = false;
                Projectile.netUpdate = true;
            }

            if (hasTarget && Projectile.ai[0] == 0f)
            {
                Vector2 offset = targetPos - Projectile.Center;
                offset.Normalize();
                if (targetDist > 200f)
                {
                    offset *= Charging ? 8f : 6f;
                }
                else
                {
                    offset *= -4f;
                }
                Projectile.velocity = (Projectile.velocity * 40f + offset) / 41f;
            }
            else
            {
                float speed = Projectile.ai[0] == 1f ? 15f : 6f;
                Vector2 offset = player.Center - Projectile.Center + new Vector2(0f, -60f);
                float distance = offset.Length();
                if (distance > 200f && speed < 8f)
                {
                    speed = 8f;
                }
                if (distance < 150f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
                {
                    Projectile.ai[0] = 0f;
                    Projectile.netUpdate = true;
                }
                if (distance > 2000f)
                {
                    Projectile.position = player.Center - new Vector2(Projectile.width / 2, Projectile.height / 2);
                    Projectile.netUpdate = true;
                }
                if (distance > 70f)
                {
                    offset.Normalize();
                    offset *= speed;
                    Projectile.velocity = (Projectile.velocity * 40f + offset) / 41f;
                }
                else if (Projectile.velocity.X == 0f && Projectile.velocity.Y == 0f)
                {
                    Projectile.velocity = new Vector2(-0.15f, -0.05f);
                }
            }

            if (Charging || !hasTarget)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.Pi;
            }
            else
            {
                Projectile.rotation = (targetPos - Projectile.Center).ToRotation() + MathHelper.Pi;
            }
            if (Projectile.ai[1] > 0f)
            {
                Projectile.ai[1] += (float)Main.rand.Next(1, 4);
            }
            if (Projectile.ai[1] > 90f && !Charging)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] > 40f && Charging)
            {
                Projectile.ai[1] = 0f;
                Projectile.netUpdate = true;
            }

            if (Projectile.ai[0] == 0f && Projectile.ai[1] == 0f && hasTarget)
            {
                Projectile.ai[1] += 1f;
                if (!Charging)
                {
                    if (Main.myPlayer == Projectile.owner && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, targetPos, 0, 0))
                    {
                        Vector2 offset = targetPos - Projectile.Center;
                        offset.Normalize();
                        offset *= 8f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, offset, Mod.Find<ModProjectile>("MiniPixelBall").Type, Projectile.damage, 0f, Main.myPlayer, element, 0f);
                        Projectile.netUpdate = true;
                    }
                }
                else if (targetDist < 500f)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.ai[0] = 2f;
                        Vector2 offset = targetPos - Projectile.Center;
                        offset.Normalize();
                        Projectile.velocity = offset * 8f;
                        Projectile.netUpdate = true;
                    }
                }
            }
        }

        private void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                Vector2 unit = -new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation));
                Vector2 center = Projectile.Center;
                for (int k = 0; k < 4; k++)
                {
                    int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                    Vector2 offset = Main.dust[dust].position - center;
                    offset.X = (offset.X - (float)Projectile.width / 2f) / 2f;
                    Main.dust[dust].position = center + new Vector2(unit.X * offset.X - unit.Y * offset.Y, unit.Y * offset.X + unit.X * offset.Y);
                    Main.dust[dust].velocity += -3f * unit;
                    Main.dust[dust].rotation = Projectile.rotation - MathHelper.Pi;
                    Main.dust[dust].velocity += Projectile.velocity;
                    Main.dust[dust].scale = 0.9f;
                }
            }
        }

        public Color? GetColor()
        {
            switch (element)
            {
            case 0:
                return new Color(250, 10, 0);
            case 1:
                return new Color(0, 230, 230);
            case 2:
                return new Color(0, 153, 230);
            case 3:
                return null;
            case 4:
                return new Color(0, 178, 0);
            case 5:
                return new Color(230, 192, 0);
            default:
                return null;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = true;
            return true;
        }

        public override bool MinionContactDamage()
        {
            return Charging && Projectile.ai[0] == 2f;
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
            switch (element)
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
            switch (element)
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
    }
}