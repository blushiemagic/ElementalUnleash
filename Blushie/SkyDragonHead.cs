using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;
using Bluemagic.BlushieBoss;

namespace Bluemagic.Blushie
{
	public class SkyDragonHead : Minion
	{
		public override string Texture
		{
			get
			{
				return "Bluemagic/BlushieBoss/DragonHead";
			}
		}

		public override void SetStaticDefaults()
		{
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 100;
			projectile.height = 200;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
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
				for (int k = 0; k < projectile.whoAmI; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == projectile.owner && Main.projectile[k].type == projectile.type)
					{
						projectile.Kill();
						return;
					}
				}
			}
			if (projectile.owner == Main.myPlayer && projectile.localAI[0] == 0f)
			{
				projectile.localAI[0] = 1f;
				int proj1 = Projectile.NewProjectile(projectile.Center + new Vector2(400f, 0f), Vector2.Zero, mod.ProjectileType("SkyDragonArm"), projectile.damage, projectile.knockBack, projectile.owner, projectile.identity, 1f);
				int proj2 = Projectile.NewProjectile(projectile.Center + new Vector2(-400f, 0f), Vector2.Zero, mod.ProjectileType("SkyDragonArm"), projectile.damage, projectile.knockBack, projectile.owner, projectile.identity, -1f);
				projectile.ai[0] = Main.projectile[proj1].identity;
				projectile.ai[1] = Main.projectile[proj2].identity;
				projectile.netUpdate = true;
			}
			if (projectile.owner == Main.myPlayer)
			{
				int leftUuid = Projectile.GetByUUID(projectile.owner, projectile.ai[0]);
				int rightUuid = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
				if (leftUuid < 0 || rightUuid < 0)
				{
					projectile.Kill();
					return;
				}
				Projectile left = Main.projectile[leftUuid];
				Projectile right = Main.projectile[rightUuid];
				if (!left.active || !right.active || left.type != mod.ProjectileType("SkyDragonArm") || right.type != mod.ProjectileType("SkyDragonArm"))
				{
					projectile.Kill();
					return;
				}
			}

			Vector2 target = Main.player[projectile.owner].Center + new Vector2(0f, -240f);
			projectile.Center = Vector2.Lerp(projectile.Center, target, 0.1f);
			if (projectile.owner == Main.myPlayer)
			{
				projectile.localAI[1] += 1f;
				if (projectile.localAI[1] < 60f && projectile.localAI[1] % 5 == 0)
				{
					int index = -1;
					float distance = 1600f;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && Main.npc[k].CanBeChasedBy(projectile))
						{
							float check = Vector2.Distance(projectile.Center, Main.npc[k].Center);
							if (check < distance)
							{
								index = k;
								distance = check;
							}
						}
					}
					if (index >= 0)
					{
						Vector2 offset = Main.npc[index].Center - projectile.Center;
						if (distance == 0f)
						{
							offset = -Vector2.UnitY;
						}
						offset.Normalize();
						Projectile.NewProjectile(projectile.Center, 32f * offset, mod.ProjectileType("SkyDragonBullet"), projectile.damage, projectile.knockBack, projectile.owner, 0f, index);
					}
				}
				if (projectile.localAI[1] >= 120f)
				{
					projectile.localAI[1] = 0f;
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color color)
		{
			Vector2 origin = Main.player[projectile.owner].Center;
			int leftUuid = Projectile.GetByUUID(projectile.owner, projectile.ai[0]);
			int rightUuid = Projectile.GetByUUID(projectile.owner, projectile.ai[1]);
			if (leftUuid >= 0 && rightUuid >= 0)
			{
				BlushiemagicM.DrawDragonArms(spriteBatch, origin, Main.projectile[leftUuid].Center, Main.projectile[rightUuid].Center, true);
			}
			BlushiemagicM.DrawDragonHead(spriteBatch, origin, projectile.Center);
			return false;
		}
	}
}