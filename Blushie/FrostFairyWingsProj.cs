using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Blushie
{
	public class FrostFairyWingsProj : Minion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wings of the Frost Fairy");
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 62;
			projectile.height = 52;
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
				modPlayer.frostFairy = false;
			}
			if (modPlayer.frostFairy)
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
						break;
					}
				}
			}
			projectile.Center = Main.player[projectile.owner].Center;

			projectile.ai[0] += 1f;
			if (projectile.ai[0] >= 10f)
			{
				if (projectile.owner == Main.myPlayer)
				{
					Vector2 origin = projectile.Center + new Vector2(-18f, -13f);
					int target = GetTarget((NPC npc) => npc.Center.X <= projectile.Center.X && npc.Center.Y <= projectile.Center.Y, origin);
					if (target >= 0)
					{
						Vector2 dir = Main.npc[target].Center - origin;
						dir.Normalize();
						Projectile.NewProjectile(origin, dir, mod.ProjectileType("FrostFairyLaser"), projectile.damage, projectile.knockBack, projectile.owner, 0f, projectile.identity);
					}
					origin = projectile.Center + new Vector2(18f, -13f);
					target = GetTarget((NPC npc) => npc.Center.X >= projectile.Center.X && npc.Center.Y <= projectile.Center.Y, origin);
					if (target >= 0)
					{
						Vector2 dir = Main.npc[target].Center - origin;
						dir.Normalize();
						Projectile.NewProjectile(origin, dir, mod.ProjectileType("FrostFairyLaser"), projectile.damage, projectile.knockBack, projectile.owner, 1f, projectile.identity);
					}
					origin = projectile.Center + new Vector2(-16f, 15f);
					target = GetTarget((NPC npc) => npc.Center.X <= projectile.Center.X && npc.Center.Y >= projectile.Center.Y, origin);
					if (target >= 0)
					{
						Vector2 dir = Main.npc[target].Center - origin;
						dir.Normalize();
						Projectile.NewProjectile(origin, dir, mod.ProjectileType("FrostFairyLaser"), projectile.damage, projectile.knockBack, projectile.owner, 2f, projectile.identity);
					}
					origin = projectile.Center + new Vector2(16f, 15f);
					target = GetTarget((NPC npc) => npc.Center.X >= projectile.Center.X && npc.Center.Y >= projectile.Center.Y, origin);
					if (target >= 0)
					{
						Vector2 dir = Main.npc[target].Center - origin;
						dir.Normalize();
						Projectile.NewProjectile(origin, dir, mod.ProjectileType("FrostFairyLaser"), projectile.damage, projectile.knockBack, projectile.owner, 3f, projectile.identity);
					}
					projectile.ai[0] = 0f;
				}
			}
		}

		private int GetTarget(Func<NPC, bool> priority, Vector2 origin)
		{
			int index = -1;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].CanBeChasedBy(projectile))
				{
					float distance = Vector2.Distance(origin, Main.npc[k].Center);
					if (distance < 800f)
					{
						if (index < 0)
						{
							index = k;
						}
						else if (!priority(Main.npc[index]) && priority(Main.npc[k]))
						{
							index = k;
						}
						else if (priority(Main.npc[index]) == priority(Main.npc[k]) && distance < Vector2.Distance(origin, Main.npc[index].Center))
						{
							index = k;
						}
					}
				}
			}
			return index;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
	}
}