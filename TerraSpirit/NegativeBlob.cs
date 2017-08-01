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
				return Main.npc[(int)npc.ai[0]];
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Negative Blob");
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
			NPC spirit = Spirit;
			if (!spirit.active || !(spirit.modNPC is TerraSpirit2) || spirit.ai[0] == 3f)
			{
				npc.active = false;
			}
			if (npc.velocity == Vector2.Zero)
			{
				npc.velocity = 8f * npc.ai[1].ToRotationVector2();
			}
			if (npc.position.X <= spirit.Center.X - TerraSpirit.arenaWidth / 2)
			{
				npc.velocity.X *= -1f;
			}
			else if (npc.position.X + npc.width >= spirit.Center.X + TerraSpirit.arenaWidth / 2)
			{
				npc.velocity.X *= -1f;
			}
			if (npc.position.Y <= spirit.Center.Y - TerraSpirit.arenaHeight / 2)
			{
				npc.velocity.Y *= -1f;
			}
			else if (npc.position.Y + npc.height >= spirit.Center.Y + TerraSpirit.arenaHeight / 2)
			{
				npc.velocity.Y *= -1f;
			}
			npc.rotation += 0.1f;
			if (npc.timeLeft < 600)
			{
				npc.timeLeft = 600;
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

		public override bool PreNPCLoot()
		{
			bool success = true;
			for (int k = 0; k < 200; k++)
			{
				if (k != npc.whoAmI && Main.npc[k].active && (Main.npc[k].type == mod.NPCType("TerraSpirit2") || Main.npc[k].type == mod.NPCType("NegativeBlob2")) && Vector2.Distance(Main.npc[k].Center, npc.Center) < 160f)
				{
					success = false;
					break;
				}
			}
			if (success)
			{
				NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("NegativeBlob2"), 1, npc.ai[0]);
			}
			else
			{
				Vector2 normal = new Vector2(-npc.velocity.Y, npc.velocity.X);
				if (Main.netMode == 0)
				{
					var bullets = ((TerraSpirit2)Spirit.modNPC).bullets;
					bullets.Add(new BulletNegative(npc.Center, npc.velocity));
					bullets.Add(new BulletNegative(npc.Center, -npc.velocity));
					bullets.Add(new BulletNegative(npc.Center, normal));
					bullets.Add(new BulletNegative(npc.Center, -normal));
				}
				else
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)MessageType.BulletNegative);
					packet.Write((byte)npc.ai[0]);
					packet.Write((byte)4);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(npc.velocity);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(-npc.velocity);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(normal);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(-normal);
					packet.Send();
				}
			}
			return false;
		}
	}
}
