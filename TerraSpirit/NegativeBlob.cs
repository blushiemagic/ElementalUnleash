using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class NegativeBlob : ModNPC
    {
        protected NPC Spirit
        {
            get
            {
                return Main.npc[(int)NPC.ai[0]];
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Negative Blob");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 20000;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = null;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            NPC spirit = Spirit;
            if (!spirit.active || !(spirit.ModNPC is TerraSpirit2) || spirit.ai[0] == 3f)
            {
                NPC.active = false;
            }
            if (NPC.velocity == Vector2.Zero)
            {
                NPC.velocity = 8f * NPC.ai[1].ToRotationVector2();
            }
            if (NPC.position.X <= spirit.Center.X - TerraSpirit.arenaWidth / 2)
            {
                NPC.velocity.X *= -1f;
            }
            else if (NPC.position.X + NPC.width >= spirit.Center.X + TerraSpirit.arenaWidth / 2)
            {
                NPC.velocity.X *= -1f;
            }
            if (NPC.position.Y <= spirit.Center.Y - TerraSpirit.arenaHeight / 2)
            {
                NPC.velocity.Y *= -1f;
            }
            else if (NPC.position.Y + NPC.height >= spirit.Center.Y + TerraSpirit.arenaHeight / 2)
            {
                NPC.velocity.Y *= -1f;
            }
            NPC.rotation += 0.1f;
            if (NPC.timeLeft < 600)
            {
                NPC.timeLeft = 600;
            }
            Player player = Main.player[Main.myPlayer];
            if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(NPC.Hitbox))
            {
                player.GetModPlayer<BluemagicPlayer>().TerraKill();
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            if (player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
            {
                return null;
            }
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            if (!projectile.npcProj && !projectile.trap && Main.player[projectile.owner].GetModPlayer<BluemagicPlayer>().terraLives > 0)
            {
                return null;
            }
            return false;
        }

        public override bool PreKill()
        {
            bool success = true;
            for (int k = 0; k < 200; k++)
            {
                if (k != NPC.whoAmI && Main.npc[k].active && (Main.npc[k].type == Mod.Find<ModNPC>("TerraSpirit2").Type || Main.npc[k].type == Mod.Find<ModNPC>("NegativeBlob2").Type) && Vector2.Distance(Main.npc[k].Center, NPC.Center) < 160f)
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("NegativeBlob2").Type, 1, NPC.ai[0]);
            }
            else
            {
                Vector2 normal = new Vector2(-NPC.velocity.Y, NPC.velocity.X);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    var bullets = ((TerraSpirit2)Spirit.ModNPC).bullets;
                    bullets.Add(new BulletNegative(NPC.Center, NPC.velocity));
                    bullets.Add(new BulletNegative(NPC.Center, -NPC.velocity));
                    bullets.Add(new BulletNegative(NPC.Center, normal));
                    bullets.Add(new BulletNegative(NPC.Center, -normal));
                }
                else
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)MessageType.BulletNegative);
                    packet.Write((byte)NPC.ai[0]);
                    packet.Write((byte)4);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(NPC.velocity);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(-NPC.velocity);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(normal);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(-normal);
                    packet.Send();
                }
            }
            return false;
        }
    }
}
