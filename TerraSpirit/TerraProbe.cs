using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
	public abstract class TerraProbe : ModNPC
	{
		protected NPC Spirit
		{
			get
			{
				return Main.npc[(int)npc.ai[0]];
			}
		}

		protected int Timer
		{
			get
			{
				return (int)npc.ai[1];
			}
			set
			{
				npc.ai[1] = value;
			}
		}

		public override string Texture
		{
			get
			{
				return "Bluemagic/TerraSpirit/TerraProbe";
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purity Probe");
			NPCID.Sets.NeedsExpertScaling[npc.type] = false;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 50000;
			npc.damage = 0;
			npc.defense = 140;
			npc.knockBackResist = 0f;
			npc.width = 64;
			npc.height = 96;
			npc.boss = true;
			npc.npcSlots = 42f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Despicable beautiful");
		}

		public override void AI()
		{
			NPC spirit = Spirit;
			if (!spirit.active || !(spirit.modNPC is TerraSpirit))
			{
				npc.active = false;
			}
			Behavior();
			Player target = null;
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
				{
					target = player;
					break;
				}
			}
			if (target != null)
			{
				Vector2 offset = target.Center - npc.Center;
				float distance = offset.Length();
				if (distance == 0f)
				{
					offset = new Vector2(-1f, 0f);
				}
				offset.Normalize();
				npc.velocity = 0.05f * offset * (distance - 320f);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public abstract void Behavior();

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
			NPC spirit = Spirit;
			if (spirit.active && spirit.modNPC is TerraSpirit)
			{
				TerraSpirit modSpirit = (TerraSpirit)spirit.modNPC;
				modSpirit.Stage += 2;
				modSpirit.Progress = 0;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, spirit.whoAmI);
				}
			}
			return false;
		}
	}
}
