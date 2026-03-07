using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class GoldBlob : ModNPC
    {
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
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            NPC npc1 = Main.npc[(int)NPC.ai[0]];
            NPC npc2 = Main.npc[(int)NPC.ai[1]];
            NPC npc3 = Main.npc[(int)NPC.ai[2]];
            int checkType = Mod.Find<ModNPC>("NegativeBlob2").Type;
            if (!npc1.active || !npc2.active || !npc3.active || npc1.type != checkType || npc2.type != checkType || npc3.type != checkType)
            {
                NPC.active = false;
                return;
            }
            NPC.rotation += 0.1f;
            if (NPC.timeLeft < 600)
            {
                NPC.timeLeft = 600;
            }
            Vector2 start;
            Vector2 end;
            if (NPC.ai[3] == 0f)
            {
                start = npc1.position;
                end = npc2.position;
            }
            else if (NPC.ai[3] == 1f)
            {
                start = npc2.position;
                end = npc3.position;
            }
            else
            {
                start = npc3.position;
                end = npc1.position;
            }
            Vector2 offset = NPC.position - start;
            Vector2 unit = end - start;
            unit.Normalize();
            NPC.position += unit * 8f;
            if (Vector2.Distance(NPC.position, start) >= Vector2.Distance(end, start))
            {
                NPC.position = end;
                NPC.ai[3] += 1f;
                NPC.ai[3] %= 3f;
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

        public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            NPC.localAI[0] = player.whoAmI;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Bluemagic.Instance.GetPacket();
                packet.Write((byte)MessageType.GoldBlob);
                packet.Write((byte)NPC.localAI[0]);
                packet.Send();
            }
        }

        public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            NPC.localAI[0] = Main.player[projectile.owner].whoAmI;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = Bluemagic.Instance.GetPacket();
                packet.Write((byte)MessageType.GoldBlob);
                packet.Write((byte)NPC.whoAmI);
                packet.Write((byte)NPC.localAI[0]);
                packet.Send();
            }
        }

        public override bool PreKill()
        {
            Main.npc[(int)NPC.ai[0]].ai[3] = -1f;
            Main.npc[(int)NPC.ai[1]].ai[3] = -1f;
            Main.npc[(int)NPC.ai[2]].ai[3] = -1f;
            Main.npc[(int)NPC.ai[0]].netUpdate = true;
            Main.npc[(int)NPC.ai[1]].netUpdate = true;
            Main.npc[(int)NPC.ai[2]].netUpdate = true;
            NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("GoldBlob2").Type, 0, NPC.localAI[0]);
            return false;
        }
    }
}
