using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class GoldBlob2 : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "Bluemagic/TerraSpirit/GoldBlob";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Charged Negative Blob");
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
            NPC.dontTakeDamage = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            NPC spirit = null;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("TerraSpirit2").Type && Main.npc[k].ai[0] != 3f)
                {
                    spirit = Main.npc[k];
                    break;
                }
            }
            if (spirit == null)
            {
                NPC.active = false;
                return;
            }
            Player player;
            if (NPC.velocity == Vector2.Zero)
            {
                player = Main.player[(int)NPC.ai[0]];
                Vector2 offset = NPC.Center - player.Center;
                offset.Normalize();
                NPC.velocity = 8f * offset;
            }
            if (NPC.position.X <= spirit.Center.X - TerraSpirit.arenaWidth / 2)
            {
                NPC.velocity.X *= -1f;
                NPC.ai[1] += 1f;
            }
            else if (NPC.position.X + NPC.width >= spirit.Center.X + TerraSpirit.arenaWidth / 2)
            {
                NPC.velocity.X *= -1f;
                NPC.ai[1] += 1f;
            }
            if (NPC.position.Y <= spirit.Center.Y - TerraSpirit.arenaHeight / 2)
            {
                NPC.velocity.Y *= -1f;
                NPC.ai[1] += 1f;
            }
            else if (NPC.position.Y + NPC.height >= spirit.Center.Y + TerraSpirit.arenaHeight / 2)
            {
                NPC.velocity.Y *= -1f;
                NPC.ai[1] += 1f;
            }
            if (NPC.ai[1] >= 3f)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    var bullets = ((TerraSpirit2)spirit.ModNPC).bullets;
                    bullets.Add(new BulletNegative(NPC.Center, NPC.velocity));
                    bullets.Add(new BulletNegative(NPC.Center, NPC.velocity.RotatedBy(MathHelper.Pi / 4f)));
                    bullets.Add(new BulletNegative(NPC.Center, NPC.velocity.RotatedBy(-MathHelper.Pi / 4f)));
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)MessageType.BulletNegative);
                    packet.Write((byte)spirit.whoAmI);
                    packet.Write((byte)3);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(NPC.velocity);
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(NPC.velocity.RotatedBy(MathHelper.Pi / 4f));
                    packet.WriteVector2(NPC.Center);
                    packet.WriteVector2(NPC.velocity.RotatedBy(-MathHelper.Pi / 4f));
                    packet.Send();
                }
                NPC.active = false;
                return;
            }
            NPC.rotation += 0.1f;
            if (NPC.timeLeft < 600)
            {
                NPC.timeLeft = 600;
            }
            player = Main.player[Main.myPlayer];
            if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(NPC.Hitbox))
            {
                player.GetModPlayer<BluemagicPlayer>().TerraKill();
            }
            if (spirit.Hitbox.Intersects(NPC.Hitbox))
            {
                NPC.active = false;
                spirit.SimpleStrikeNPC(200000, 0, false, 0);
                if (Main.netMode == NetmodeID.Server)
                {
                    spirit.netUpdate = true;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
