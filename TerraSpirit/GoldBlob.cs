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
			DisplayName.SetDefault("Charged Negative Blob");
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 20000;
			npc.damage = 0;
			npc.defense = 100;
			npc.knockBackResist = 0f;
			npc.width = 80;
			npc.height = 80;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}

		public override void AI()
		{
			NPC npc1 = Main.npc[(int)npc.ai[0]];
			NPC npc2 = Main.npc[(int)npc.ai[1]];
			NPC npc3 = Main.npc[(int)npc.ai[2]];
			int checkType = mod.NPCType("NegativeBlob2");
			if (!npc1.active || !npc2.active || !npc3.active || npc1.type != checkType || npc2.type != checkType || npc3.type != checkType)
			{
				npc.active = false;
				return;
			}
			npc.rotation += 0.1f;
			if (npc.timeLeft < 600)
			{
				npc.timeLeft = 600;
			}
			Vector2 start;
			Vector2 end;
			if (npc.ai[3] == 0f)
			{
				start = npc1.position;
				end = npc2.position;
			}
			else if (npc.ai[3] == 1f)
			{
				start = npc2.position;
				end = npc3.position;
			}
			else
			{
				start = npc3.position;
				end = npc1.position;
			}
			Vector2 offset = npc.position - start;
			Vector2 unit = end - start;
			unit.Normalize();
			npc.position += unit * 8f;
			if (Vector2.Distance(npc.position, start) >= Vector2.Distance(end, start))
			{
				npc.position = end;
				npc.ai[3] += 1f;
				npc.ai[3] %= 3f;
			}
			Player player = Main.player[Main.myPlayer];
			if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(npc.Hitbox))
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

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			npc.localAI[0] = player.whoAmI;
			if (Main.netMode == 1)
			{
				ModPacket packet = Bluemagic.Instance.GetPacket();
				packet.Write((byte)MessageType.GoldBlob);
				packet.Write((byte)npc.localAI[0]);
				packet.Send();
			}
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			npc.localAI[0] = Main.player[projectile.owner].whoAmI;
			if (Main.netMode == 1)
			{
				ModPacket packet = Bluemagic.Instance.GetPacket();
				packet.Write((byte)MessageType.GoldBlob);
				packet.Write((byte)npc.whoAmI);
				packet.Write((byte)npc.localAI[0]);
				packet.Send();
			}
		}

		public override bool PreNPCLoot()
		{
			Main.npc[(int)npc.ai[0]].ai[3] = -1f;
			Main.npc[(int)npc.ai[1]].ai[3] = -1f;
			Main.npc[(int)npc.ai[2]].ai[3] = -1f;
			Main.npc[(int)npc.ai[0]].netUpdate = true;
			Main.npc[(int)npc.ai[1]].netUpdate = true;
			Main.npc[(int)npc.ai[2]].netUpdate = true;
			NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("GoldBlob2"), 0, npc.localAI[0]);
			return false;
		}
	}
}
