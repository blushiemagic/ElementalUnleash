using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;
using Bluemagic.BlushieBoss;

namespace Bluemagic.Blushie
{
	public class SkyDragonArm : Minion
	{
		public override string Texture
		{
			get
			{
				return "Bluemagic/BlushieBoss/DragonClaw";
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.minion = true;
			projectile.minionSlots = 0.5f;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}

		public override void CheckActive()
		{
			Player player = Main.player[projectile.owner];
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			if (player.dead)
			{
				modPlayer.skyDragon = false;
			}
			if (modPlayer.skyDragon)
			{
				projectile.timeLeft = 2;
			}
		}

		public override void Behavior()
		{
			if (projectile.owner == Main.myPlayer)
			{
				int uuid = Projectile.GetByUUID(projectile.owner, projectile.ai[0]);
				if (uuid < 0)
				{
					projectile.Kill();
					return;
				}
				Projectile head = Main.projectile[uuid];
				if (!head.active || head.type != mod.ProjectileType("SkyDragonHead"))
				{
					projectile.Kill();
					return;
				}
			}

			if (projectile.localAI[0] == 1f)
			{
				projectile.velocity *= 0.99f;
				projectile.localAI[1] += 1f;
				if (projectile.localAI[1] % 30f == 0f)
				{
					int index = -1;
					float distance = 1600f;
					for (int k = 0; k < 200; k++)
					{
						NPC check = Main.npc[k];
						if (check.CanBeChasedBy(projectile) && Vector2.Distance(check.Center, projectile.Center) < distance)
						{
							index = k;
							distance = Vector2.Distance(check.Center, projectile.Center);
						}
					}
					if (index >= 0)
					{
						Projectile.NewProjectile(projectile.Center, Vector2.Zero, mod.ProjectileType("SkyDragonBullet"), projectile.damage, projectile.knockBack, projectile.owner, 1f, index);
					}
				}
				if (projectile.localAI[1] >= 60)
				{
					projectile.localAI[0] = 0f;
					projectile.localAI[1] = 0f;
				}
			}
			else
			{
				Player player = Main.player[projectile.owner];
				int index = -1;
				float distance = 1600f;
				float playerDistance = 3200f;
				for (int k = 0; k < 200; k++)
				{
					NPC check = Main.npc[k];
					if (check.CanBeChasedBy(projectile) && Vector2.Distance(check.Center, player.Center) < playerDistance && Vector2.Distance(check.Center, projectile.Center) < distance)
					{
						index = k;
						distance = Vector2.Distance(check.Center, projectile.Center);
					}
				}
				if (index < 0)
				{
					Vector2 target = player.Center + new Vector2(projectile.ai[1] * 400f, 0f);
					projectile.Center = Vector2.Lerp(projectile.Center, target, 0.1f);
				}
				else
				{
					Vector2 target = Main.npc[index].Center;
					Vector2 offset = target - projectile.Center;
					if (offset == Vector2.Zero)
					{
						offset = -Vector2.UnitY;
					}
					offset.Normalize();
					projectile.velocity = 16f * offset;
					projectile.localAI[0] = 1f;
					projectile.localAI[1] = 0f;
				}
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}
	}
}