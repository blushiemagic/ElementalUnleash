using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Items.Abomination.Projectiles
{
	public class MiniCaptiveElement : Minion
	{
		private static int[] elementToType = new int[6];
		private int element;

		public MiniCaptiveElement() : this(1) { }

		public MiniCaptiveElement(int element)
		{
			this.element = element;
		}

		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}

		public override bool Autoload(ref string name)
		{
			if (mod.Properties.Autoload)
			{
				for (int k = 0; k <= 5; k++)
				{
					ModProjectile next = new MiniCaptiveElement(k);
					mod.AddProjectile(name + k, next);
					elementToType[k] = next.projectile.type;
				}
			}
			return false;
		}

		public override string Texture
		{
			get
			{
				return "Bluemagic/Items/Abomination/Projectiles/MiniCaptiveElement";
			}
		}

		private bool Charging
		{
			get
			{
				return element == 2 || element == 5;
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Captive Element");
			Main.projFrames[projectile.type] = 6;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 32;
			projectile.height = 32;
			projectile.penetrate = -1;
			projectile.timeLeft *= 5;
			projectile.minion = true;
			projectile.minionSlots = 0.333f;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.friendly = true;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}

		public override void CheckActive()
		{
			Player player = Main.player[projectile.owner];
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			if (player.dead)
			{
				modPlayer.elementMinion = false;
			}
			if (modPlayer.elementMinion)
			{
				projectile.timeLeft = 2;
			}
		}

		public override void Behavior()
		{
			if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner && element == 0)
			{
				for (int k = 1; k <= 5; k++)
				{
					Projectile.NewProjectile(projectile.Center, projectile.velocity, elementToType[k], projectile.damage, projectile.knockBack, projectile.owner);
				}
				projectile.localAI[0] = 1f;
			}
			Player player = Main.player[projectile.owner];
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			projectile.frame = element;
			if (Main.rand.Next(4) == 0)
			{
				CreateDust();
			}
			for (int k = 0; k < 1000; k++)
			{
				Projectile other = Main.projectile[k];
				if (k != projectile.whoAmI && other.active && other.owner == projectile.owner && elementToType.Contains(other.type) && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
				{
					const float pushAway = 0.05f;
					if (projectile.position.X < other.position.X)
					{
						projectile.velocity.X -= pushAway;
					}
					else
					{
						projectile.velocity.X += pushAway;
					}
					if (projectile.position.Y < other.position.Y)
					{
						projectile.velocity.Y -= pushAway;
					}
					else
					{
						projectile.velocity.Y += pushAway;
					}
				}
			}

			if (projectile.ai[0] == 2f && Charging)
			{
				projectile.ai[1] += 1f;
				projectile.extraUpdates = 1;
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
				if (projectile.ai[1] > 40f)
				{
					projectile.ai[1] = 1f;
					projectile.ai[0] = 0f;
					projectile.extraUpdates = 0;
					projectile.numUpdates = 0;
					projectile.netUpdate = true;
				}
				else
				{
					return;
				}
			}

			if (projectile.ai[0] != 1f)
			{
				projectile.tileCollide = true;
			}
			if (projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)))
			{
				projectile.tileCollide = false;
			}

			float targetDist = 700f;
			Vector2 targetPos = projectile.position;
			bool hasTarget = false;
			NPC ownerTarget = projectile.OwnerMinionAttackTargetNPC;
			if (ownerTarget != null && ownerTarget.CanBeChasedBy(projectile))
			{
				if (Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerTarget.position, ownerTarget.width, ownerTarget.height))
				{
					targetDist = Vector2.Distance(ownerTarget.Center, projectile.Center);
					targetPos = ownerTarget.Center;
					hasTarget = true;
				}
			}
			if (!hasTarget)
			{
				for (int k = 0; k < 200; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.CanBeChasedBy(projectile))
					{
						float distance = Vector2.Distance(npc.Center, projectile.Center);
						if (((distance < Vector2.Distance(projectile.Center, targetPos) && distance < targetDist) || !hasTarget) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height))
						{
							targetDist = distance;
							targetPos = npc.Center;
							hasTarget = true;
						}
					}
				}
			}
			float leashLength = hasTarget ? 1200f : 800f;
			if (Vector2.Distance(player.Center, projectile.Center) > leashLength)
			{
				projectile.ai[0] = 1f;
				projectile.tileCollide = false;
				projectile.netUpdate = true;
			}

			if (hasTarget && projectile.ai[0] == 0f)
			{
				Vector2 offset = targetPos - projectile.Center;
				offset.Normalize();
				if (targetDist > 200f)
				{
					offset *= Charging ? 8f : 6f;
				}
				else
				{
					offset *= -4f;
				}
				projectile.velocity = (projectile.velocity * 40f + offset) / 41f;
			}
			else
			{
				float speed = projectile.ai[0] == 1f ? 15f : 6f;
				Vector2 offset = player.Center - projectile.Center + new Vector2(0f, -60f);
				float distance = offset.Length();
				if (distance > 200f && speed < 8f)
				{
					speed = 8f;
				}
				if (distance < 150f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					projectile.netUpdate = true;
				}
				if (distance > 2000f)
				{
					projectile.position = player.Center - new Vector2(projectile.width / 2, projectile.height / 2);
					projectile.netUpdate = true;
				}
				if (distance > 70f)
				{
					offset.Normalize();
					offset *= speed;
					projectile.velocity = (projectile.velocity * 40f + offset) / 41f;
				}
				else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
				{
					projectile.velocity = new Vector2(-0.15f, -0.05f);
				}
			}

			if (Charging || !hasTarget)
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.Pi;
			}
			else
			{
				projectile.rotation = (targetPos - projectile.Center).ToRotation() + MathHelper.Pi;
			}
			if (projectile.ai[1] > 0f)
			{
				projectile.ai[1] += (float)Main.rand.Next(1, 4);
			}
			if (projectile.ai[1] > 90f && !Charging)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}
			if (projectile.ai[1] > 40f && Charging)
			{
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
			}

			if (projectile.ai[0] == 0f && projectile.ai[1] == 0f && hasTarget)
			{
				projectile.ai[1] += 1f;
				if (!Charging)
				{
					if (Main.myPlayer == projectile.owner && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, targetPos, 0, 0))
					{
						Vector2 offset = targetPos - projectile.Center;
						offset.Normalize();
						offset *= 8f;
						Projectile.NewProjectile(projectile.Center, offset, mod.ProjectileType("MiniPixelBall"), projectile.damage, 0f, Main.myPlayer, element, 0f);
						projectile.netUpdate = true;
					}
				}
				else if (targetDist < 500f)
				{
					if (Main.myPlayer == projectile.owner)
					{
						projectile.ai[0] = 2f;
						Vector2 offset = targetPos - projectile.Center;
						offset.Normalize();
						projectile.velocity = offset * 8f;
						projectile.netUpdate = true;
					}
				}
			}
		}

		private void CreateDust()
		{
			Color? color = GetColor();
			if (color.HasValue)
			{
				Vector2 unit = -new Vector2((float)Math.Cos(projectile.rotation), (float)Math.Sin(projectile.rotation));
				Vector2 center = projectile.Center;
				for (int k = 0; k < 4; k++)
				{
					int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("Pixel"), 0f, 0f, 0, color.Value);
					Vector2 offset = Main.dust[dust].position - center;
					offset.X = (offset.X - (float)projectile.width / 2f) / 2f;
					Main.dust[dust].position = center + new Vector2(unit.X * offset.X - unit.Y * offset.Y, unit.Y * offset.X + unit.X * offset.Y);
					Main.dust[dust].velocity += -3f * unit;
					Main.dust[dust].rotation = projectile.rotation - MathHelper.Pi;
					Main.dust[dust].velocity += projectile.velocity;
					Main.dust[dust].scale = 0.9f;
				}
			}
		}

		public Color? GetColor()
		{
			switch (element)
			{
			case 0:
				return new Color(250, 10, 0);
			case 1:
				return new Color(0, 230, 230);
			case 2:
				return new Color(0, 153, 230);
			case 3:
				return null;
			case 4:
				return new Color(0, 178, 0);
			case 5:
				return new Color(230, 192, 0);
			default:
				return null;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true;
			return true;
		}

		public override bool MinionContactDamage()
		{
			return Charging && projectile.ai[0] == 2f;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (projectile.ai[0] == 3f)
			{
				damage += 20;
			}
		}

		public override void ModifyHitPvp(Player target, ref int damage, ref bool crit)
		{
			if (projectile.ai[0] == 3f)
			{
				damage += 20;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int debuff = GetDebuff();
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime());
			}
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			int debuff = GetDebuff();
			if (debuff > 0)
			{
				target.AddBuff(debuff, GetDebuffTime() / 2);
			}
		}

		public int GetDebuff()
		{
			switch (element)
			{
			case 0:
				return BuffID.OnFire;
			case 1:
				return BuffID.Frostburn;
			case 2:
				return mod.BuffType("EtherealFlames");
			case 3:
				return 0;
			case 4:
				return BuffID.Venom;
			case 5:
				return BuffID.Ichor;
			default:
				return 0;
			}
		}

		public int GetDebuffTime()
		{
			switch (element)
			{
			case 0:
				return 600;
			case 1:
				return 400;
			case 2:
				return 300;
			case 3:
				return 0;
			case 4:
				return 400;
			case 5:
				return 900;
			default:
				return 0;
			}
		}
	}
}