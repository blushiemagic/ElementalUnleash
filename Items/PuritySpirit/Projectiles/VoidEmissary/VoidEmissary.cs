using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.Projectiles;

namespace Bluemagic.Items.PuritySpirit.Projectiles.VoidEmissary
{
	public class VoidEmissary : Minion
	{
		private VoidEmissaryHand hand1;
		private VoidEmissaryHand hand2;
		private bool handsOpen = false;
		private const float voidPortalCooldown = 600f;

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.name = "Void Emissary";
			projectile.width = 32;
			projectile.height = 48;
			Main.projFrames[projectile.type] = 3;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			hand1.offset = new Vector2(-16f, 24f);
			hand2.offset = new Vector2(16f, 24f);
			ProjectileID.Sets.Homing[projectile.type] = true;
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
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>(mod);
			if (player.dead)
			{
				modPlayer.voidEmissary = false;
			}
			if (modPlayer.voidEmissary)
			{
				projectile.timeLeft = 2;
			}
		}

		public override void Behavior()
		{
			if (projectile.ai[0] == 0f)
			{
				ChooseAttack();
			}
			projectile.rotation = 0f;
			if (projectile.ai[0] == 0f)
			{
				IdleBehavior();
			}
			else if (projectile.ai[0] == 1f)
			{
				VoidPortalAttack();
			}
			else if (projectile.ai[0] == 2f)
			{
				LaserAttack();
			}
			else if (projectile.ai[0] == 3f)
			{
				ChargeAttack();
			}
			else
			{
				projectile.ai[0] = 0f;
				projectile.netUpdate = true;
			}
			if (projectile.localAI[0] > 0f)
			{
				projectile.localAI[0] -= 1f;
			}
			CreateDust();
			SelectFrame();
		}

		private void ChooseAttack()
		{
			List<NPC> targets = new List<NPC>();
			bool flag = false;
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].CanBeChasedBy(projectile) && NPCInRange(Main.npc[k]))
				{
					targets.Add(Main.npc[k]);
					if (Main.npc[k].boss)
					{
						flag = true;
					}
				}
			}
			if (targets.Count == 0)
			{
				return;
			}
			if ((flag || targets.Count > 1) && projectile.localAI[0] <= 0f)
			{
				projectile.ai[0] = 1f;
				projectile.ai[1] = 0f;
				projectile.netUpdate = true;
				return;
			}
			Player player = Main.player[projectile.owner];
			bool canHitLine = false;
			float distance = -1f;
			float rotation = 0f;
			foreach (NPC npc in targets)
			{
				bool testCanHitLine = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
				if (testCanHitLine)
				{
					canHitLine = true;
				}
				if (!canHitLine || testCanHitLine)
				{
					float testDistance = Vector2.Distance(player.Center, npc.Center);
					if (distance < 0f || testDistance < distance)
					{
						distance = testDistance;
						rotation = (npc.Center - projectile.Center).ToRotation();
					}
				}
			}
			if (distance == 0f)
			{
				rotation = -MathHelper.PiOver2;
			}
			projectile.ai[0] = canHitLine ? 2f : 3f;
			projectile.ai[1] = rotation;
			projectile.localAI[1] = 0f;
			projectile.netUpdate = true;
		}

		private void IdleBehavior()
		{
			Vector2 target = GetIdleTarget();
			Vector2 offset = target - projectile.Center;
			if (offset.Length() > 2000f)
			{
				projectile.Center = target;
			}
			else if (offset.Length() > 16f)
			{
				Vector2 velocityDir = projectile.velocity;
				Vector2 offsetDir = offset;
				velocityDir.Normalize();
				offsetDir.Normalize();
				float maxSpeed = 8f;
				if (offset.Length() > 160f)
				{
					maxSpeed *= 2f;
				}
				if (Vector2.Dot(velocityDir, offsetDir) <= 0)
				{
					projectile.velocity *= 0.8f;
				}
				if (projectile.velocity.Length() > maxSpeed)
				{
					projectile.velocity *= 0.8f;
				}
				projectile.velocity *= 0.9f;
				projectile.velocity += maxSpeed * 0.1f * offsetDir;
				if (offset.X > 0f)
				{
					projectile.direction = 1;
				}
				else if (offset.X < 0f)
				{
					projectile.direction = -1;
				}
				projectile.spriteDirection = -projectile.direction;
			}
			else
			{
				projectile.velocity *= 0.9f;
			}
			IdleUpdateHand(ref hand1, new Vector2(-16f, 24f));
			IdleUpdateHand(ref hand2, new Vector2(16f, 24f));
			handsOpen = false;
		}

		private Vector2 GetIdleTarget()
		{
			int myNum = 0;
			int num = 0;
			for (int k = 0; k < 1000; k++)
			{
				Projectile proj = Main.projectile[k];
				if (proj.active && proj.owner == projectile.owner && proj.type == projectile.type)
				{
					if (k < projectile.whoAmI)
					{
						myNum++;
					}
					num++;
				}
			}
			float rotation = (float)myNum / (float)num * MathHelper.TwoPi;
			rotation -= MathHelper.PiOver2;
			Vector2 offset = 96f * rotation.ToRotationVector2();
			return Main.player[projectile.owner].Center + offset;
		}

		private void IdleUpdateHand(ref VoidEmissaryHand hand, Vector2 target)
		{
			if (hand.velocity == Vector2.Zero)
			{
				float rotation = MathHelper.TwoPi * (float)Main.rand.NextDouble();
				hand.velocity = 0.5f * rotation.ToRotationVector2();
			}
			if (Vector2.Distance(hand.offset, target) >= 4f)
			{
				float dirToTarget = (target - hand.offset).ToRotation();
				float disturb = MathHelper.Pi * (float)Main.rand.NextDouble() - MathHelper.PiOver2;
				hand.velocity = (dirToTarget + disturb).ToRotationVector2();
				if (Vector2.Distance(hand.offset, target) < 5f)
				{
					hand.velocity *= 0.5f;
				}
			}
			hand.offset += hand.velocity;
			hand.rotation = 0f;
		}

		private void VoidPortalAttack()
		{
			projectile.ai[1] += 1f;
			if (projectile.ai[1] <= 60f)
			{
				Vector2 target = projectile.position + new Vector2(projectile.width / 2, projectile.height * 2 / 3);
				for (int k = 0; k < 3; k++)
				{
					CreateChargeDust(target, 48);
				}
			}
			else
			{
				if (projectile.owner == Main.myPlayer)
				{
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].CanBeChasedBy(projectile) && NPCInRange(Main.npc[k]))
						{
							Projectile.NewProjectile(Main.npc[k].Center.X, Main.npc[k].Center.Y, 0f, 0f, mod.ProjectileType("VoidPortal"), projectile.damage * 2, projectile.knockBack, projectile.owner);
						}
					}
				}
				projectile.ai[0] = 0f;
				projectile.localAI[0] = voidPortalCooldown;
				projectile.netUpdate = true;
			}
			projectile.velocity *= 0.95f;
			VoidPortalUpdateHand(ref hand1, new Vector2(-16f, 8f));
			VoidPortalUpdateHand(ref hand2, new Vector2(16f, 8f));
			handsOpen = false;
		}

		private void VoidPortalUpdateHand(ref VoidEmissaryHand hand, Vector2 target)
		{
			Vector2 offset = target - hand.offset;
			float distance = offset.Length();
			if (distance != 0f)
			{
				if (distance > 0.5f)
				{
					offset.Normalize();
					if (distance <= 4f)
					{
						offset *= 0.5f;
					}
				}
				hand.offset += offset;
			}
			hand.velocity = Vector2.Zero;
			hand.rotation = 0f;
		}

		private void LaserAttack()
		{
			projectile.velocity *= 0.8f;
			if (Math.Abs(projectile.ai[1]) > MathHelper.PiOver2)
			{
				projectile.direction = -1;
			}
			else
			{
				projectile.direction = 1;
			}
			projectile.spriteDirection = -projectile.direction;
			Vector2 direction = projectile.ai[1].ToRotationVector2();
			if (projectile.localAI[1] < 15f)
			{
				CreateChargeDust(projectile.Center + 30f * direction, 16);
			}
			if (projectile.localAI[1] == 15f && projectile.owner == Main.myPlayer)
			{
				int laser = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("VoidLaser"), projectile.damage, projectile.knockBack, projectile.owner, 0f, projectile.whoAmI);
			}
			projectile.localAI[1] += 1f;
			if (projectile.localAI[1] >= 60f)
			{
				projectile.ai[0] = 0f;
				projectile.netUpdate = true;
			}
			Vector2 normal = new Vector2(-direction.Y, direction.X);
			hand1.offset = 24 * direction + 16f * normal;
			hand2.offset = 24 * direction - 16f * normal;
			hand1.rotation = projectile.ai[1];
			hand2.rotation = projectile.ai[1];
			if (projectile.spriteDirection == -1)
			{
				hand1.offset.X *= -1f;
				hand2.offset.X *= -1f;
				hand1.rotation = MathHelper.Pi - hand1.rotation;
				hand2.rotation = MathHelper.Pi - hand2.rotation;
			}
			handsOpen = true;
		}

		private void ChargeAttack()
		{
			IdleBehavior();
			projectile.rotation = projectile.ai[1] + MathHelper.PiOver2;
			if (Math.Abs(projectile.ai[1]) > MathHelper.PiOver2)
			{
				projectile.direction = -1;
			}
			else
			{
				projectile.direction = 1;
			}
			projectile.spriteDirection = -projectile.direction;
			projectile.velocity = 12f * projectile.ai[1].ToRotationVector2();
			projectile.localAI[1] += 1f;
			if (projectile.localAI[1] >= 30f)
			{
				projectile.rotation = 0f;
				projectile.ai[0] = 0f;
				projectile.netUpdate = true;
			}
			hand1.offset = new Vector2(-16f, 24f);
			hand2.offset = new Vector2(16f, 24f);
			hand1.rotation = 0f;
			hand2.rotation = 0f;
			handsOpen = false;
		}

		private bool NPCInRange(NPC npc)
		{
			Vector2 playerCenter = Main.player[projectile.owner].Center;
			Vector2 npcCenter = npc.Center;
			return Math.Abs(playerCenter.X - npcCenter.X) <= 600f && Math.Abs(playerCenter.Y - npcCenter.Y) <= 350f;
		}

		private void CreateChargeDust(Vector2 center, int radius)
		{
			int dust = Dust.NewDust(center - new Vector2(radius, radius), 2 * radius, 2 * radius, mod.DustType("CleanserBeamCharge"), 0f, 0f, 70);
			Main.dust[dust].customData = center;
		}

		private void CreateDust()
		{
			if (Main.rand.Next(3) == 0)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height / 3, mod.DustType("PuriumFlame"));
				Main.dust[dust].velocity.Y -= 1.2f;
				Main.dust[dust].velocity += projectile.velocity * 0.5f;
			}
			Lighting.AddLight(projectile.Center, 0.6f, 0.9f, 0.3f);
		}

		private void SelectFrame()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 8)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 3;
			}
		}

		public override bool MinionContactDamage()
		{
			return projectile.ai[0] == 3f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawHand(hand1, spriteBatch);
			DrawHand(hand2, spriteBatch);
			if (projectile.ai[0] == 3f)
			{
				Texture2D texture = mod.GetTexture("Items/PuritySpirit/Projectiles/VoidEmissary/VoidEmissaryCharge");
				Vector2 position = projectile.Center - Main.screenPosition;
				float alpha = Math.Abs((projectile.localAI[1] % 10f) / 5f - 1f);
				alpha = 0.1f + 0.9f * alpha;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, position, null, Color.White * alpha, projectile.rotation, origin, 1f, SpriteEffects.None, 0f);
			}
		}

		private void DrawHand(VoidEmissaryHand hand, SpriteBatch spriteBatch)
		{
			Texture2D texture = mod.GetTexture("Items/PuritySpirit/Projectiles/VoidEmissary/VoidEmissaryHand");
			Vector2 position = hand.offset;
			Rectangle frame = new Rectangle(0, 0, texture.Width, texture.Height / 2 - 2);
			if (handsOpen)
			{
				frame.Y += texture.Height / 2;
			}
			float rotation = hand.rotation + projectile.rotation;
			SpriteEffects effects = SpriteEffects.None;
			if (projectile.spriteDirection == -1)
			{
				position.X = -hand.offset.X;
				rotation = MathHelper.Pi - rotation;
			}
			position = position.RotatedBy(projectile.rotation);
			position += projectile.Center - Main.screenPosition;
			Vector2 origin = new Vector2(frame.Width / 2, frame.Height / 2);
			spriteBatch.Draw(texture, position, frame, Color.White, rotation, origin, 1f, effects, 0f);
		}
	}

	struct VoidEmissaryHand
	{
		public Vector2 offset;
		public float rotation;
		public Vector2 velocity;
	}
}