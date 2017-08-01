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
			npc.dontTakeDamage = true;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}

		public override void AI()
		{
			NPC spirit = null;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && Main.npc[k].type == mod.NPCType("TerraSpirit2") && Main.npc[k].ai[0] != 3f)
				{
					spirit = Main.npc[k];
					break;
				}
			}
			if (spirit == null)
			{
				npc.active = false;
				return;
			}
			Player player;
			if (npc.velocity == Vector2.Zero)
			{
				player = Main.player[(int)npc.ai[0]];
				Vector2 offset = npc.Center - player.Center;
				offset.Normalize();
				npc.velocity = 8f * offset;
			}
			if (npc.position.X <= spirit.Center.X - TerraSpirit.arenaWidth / 2)
			{
				npc.velocity.X *= -1f;
				npc.ai[1] += 1f;
			}
			else if (npc.position.X + npc.width >= spirit.Center.X + TerraSpirit.arenaWidth / 2)
			{
				npc.velocity.X *= -1f;
				npc.ai[1] += 1f;
			}
			if (npc.position.Y <= spirit.Center.Y - TerraSpirit.arenaHeight / 2)
			{
				npc.velocity.Y *= -1f;
				npc.ai[1] += 1f;
			}
			else if (npc.position.Y + npc.height >= spirit.Center.Y + TerraSpirit.arenaHeight / 2)
			{
				npc.velocity.Y *= -1f;
				npc.ai[1] += 1f;
			}
			if (npc.ai[1] >= 3f)
			{
				if (Main.netMode == 0)
				{
					var bullets = ((TerraSpirit2)spirit.modNPC).bullets;
					bullets.Add(new BulletNegative(npc.Center, npc.velocity));
					bullets.Add(new BulletNegative(npc.Center, npc.velocity.RotatedBy(MathHelper.Pi / 4f)));
					bullets.Add(new BulletNegative(npc.Center, npc.velocity.RotatedBy(-MathHelper.Pi / 4f)));
				}
				else if (Main.netMode == 2)
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)MessageType.BulletNegative);
					packet.Write((byte)spirit.whoAmI);
					packet.Write((byte)3);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(npc.velocity);
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(npc.velocity.RotatedBy(MathHelper.Pi / 4f));
					packet.WriteVector2(npc.Center);
					packet.WriteVector2(npc.velocity.RotatedBy(-MathHelper.Pi / 4f));
					packet.Send();
				}
				npc.active = false;
				return;
			}
			npc.rotation += 0.1f;
			if (npc.timeLeft < 600)
			{
				npc.timeLeft = 600;
			}
			player = Main.player[Main.myPlayer];
			if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(npc.Hitbox))
			{
				player.GetModPlayer<BluemagicPlayer>().TerraKill();
			}
			if (spirit.Hitbox.Intersects(npc.Hitbox))
			{
				npc.active = false;
				spirit.StrikeNPCNoInteraction(200000, 0f, 0);
				if (Main.netMode == 2)
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
