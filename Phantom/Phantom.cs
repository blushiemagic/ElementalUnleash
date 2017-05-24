using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
	public class Phantom : ModNPC
	{
		private const float maxSpeed = 8f;

		public override void SetDefaults()
		{
			npc.name = "Phantom";
			npc.displayName = "The Phantom";
			npc.aiStyle = -1;
			npc.lifeMax = 50000;
			npc.damage = 120;
			npc.defense = 50;
			npc.knockBackResist = 0f;
			npc.width = 80;
			npc.height = 80;
			npc.alpha = 70;
			npc.value = Item.buyPrice(0, 15, 0, 0);
			npc.npcSlots = 12f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath6;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = MusicID.Boss3;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.7f);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.5f;
			return null;
		}

		public bool Enraged
		{
			get
			{
				return npc.ai[0] != 0f;
			}
			set
			{
				npc.ai[0] = value ? 1f : 0f;
			}
		}

		public float AttackID
		{
			get
			{
				return npc.ai[2];
			}
			set
			{
				npc.ai[2] = value;
			}
		}

		public float AttackTimer
		{
			get
			{
				return npc.ai[3];
			}
			set
			{
				npc.ai[3] = value;
			}
		}

		public float MaxAttackTimer
		{
			get
			{
				return 60f + 120f * (float)npc.life / (float)npc.lifeMax;
			}
		}

		public float PaladinTimer
		{
			get
			{
				return npc.localAI[1];
			}
			set
			{
				npc.localAI[1] = value;
			}
		}

		public float MaxPaladinTimer
		{
			get
			{
				float maxValue = Main.expertMode ? 2f / 3f : 0.5f;
				return 120f + 180f * (float)npc.life / (npc.lifeMax * maxValue);
			}
		}

		public override void AI()
		{
			Initialize();

			if (!npc.HasValidTarget || !Main.player[npc.target].ZoneDungeon)
			{
				npc.TargetClosest(false);
			}
			if (!npc.HasValidTarget || AttackID == 100)
			{
				npc.velocity = new Vector2(0f, maxSpeed);
				if (npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
				AttackID = 100;
				npc.netUpdate = true;
				return;
			}
			if (Main.netMode != 1 && !Enraged && (!npc.HasValidTarget || !Main.player[npc.target].ZoneDungeon))
			{
				Enraged = true;
				npc.netUpdate = true;
				Talk("You thought you could escape...");
				Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
			}
			if (Enraged)
			{
				npc.damage = npc.defDamage * 2;
				npc.defense = npc.defDefense * 2;
			}

			if (AttackTimer >= 0f)
			{
				IdleBehavior();
			}
			else if (AttackID == 1f || AttackID == 2f)
			{
				ChargeAttack();
			}
			else if (AttackID == 3f)
			{
				SphereAttack();
			}
			else
			{
				IdleBehavior();
			}
			AttackTimer += 1f;
			if (AttackTimer >= MaxAttackTimer)
			{
				ChooseAttack();
			}

			if (Main.netMode != 1 && (npc.life <= npc.lifeMax / 2 || (Main.expertMode && npc.life <= npc.lifeMax * 2 / 3)))
			{
				PaladinTimer += 1f;
				if (PaladinTimer >= MaxPaladinTimer)
				{
					SpawnPaladin();
					PaladinTimer = 0f;
					npc.netUpdate = true;
				}
			}
		}

		private void Initialize()
		{
			if (Main.netMode != 1 && npc.localAI[0] == 0f)
			{
				int spawnX = (int)npc.Bottom.X;
				int spawnY = (int)npc.Bottom.Y + 64;
				int left = NPC.NewNPC(spawnX - 128, spawnY, mod.NPCType("PhantomHand"), 0, npc.whoAmI, -1f, 0f, -30f);
				int right = NPC.NewNPC(spawnX + 128, spawnY, mod.NPCType("PhantomHand"), 0, npc.whoAmI, 1f, 0f, -60f);
				npc.netUpdate = true;
				Main.npc[left].netUpdate = true;
				Main.npc[right].netUpdate = true;
			}
			if (Main.netMode != 2 && npc.localAI[0] == 0f)
			{
				Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
				Main.NewText("The Phantom has awoken!", 50, 150, 200);
			}
			npc.localAI[0] = 1f;
		}

		private void ChooseAttack()
		{
			AttackID += 1f;
			if (AttackID >= 4f)
			{
				AttackID = 1f;
			}
			if (AttackID == 3f)
			{
				AttackTimer = -300f;
			}
			else
			{
				AttackTimer = -120f;
			}
			npc.TargetClosest(false);
			npc.netUpdate = true;
		}

		private void IdleBehavior()
		{
			Vector2 offset = npc.Center - Main.player[npc.target].Center;
			offset *= 0.9f;
			Vector2 target = offset.RotatedBy(Main.expertMode ? 0.03f : 0.02f);
			CapVelocity(ref target, 320f);
			Vector2 change = target - offset;
			CapVelocity(ref change, maxSpeed);
			ModifyVelocity(change);
			CapVelocity(ref npc.velocity, maxSpeed);
		}

		private void ChargeAttack()
		{
			Vector2 offset = Main.player[npc.target].Center - npc.Center;
			if (AttackTimer < -90f || offset.Length() > 320f)
			{
				CapVelocity(ref offset, maxSpeed);
				ModifyVelocity(offset, 0.1f);
				CapVelocity(ref npc.velocity, maxSpeed);
			}
		}

		private void SphereAttack()
		{
			IdleBehavior();

			int attackTimer = (int)AttackTimer + 300;
			if (attackTimer % 30 == 0 && attackTimer < 150)
			{
				int damage = (npc.damage - 10) / 2;
				if (Main.expertMode)
				{
					damage /= 2;
				}
				Vector2 offset = npc.Center - Main.player[npc.target].Center;
				if (offset != Vector2.Zero)
				{
					offset.Normalize();
					offset *= 320f;
				}
				Projectile.NewProjectile(Main.player[npc.target].Center + offset, Vector2.Zero, mod.ProjectileType("PhantomSphereHostile"), damage, 6f, Main.myPlayer, npc.whoAmI);
			}
		}

		private void SpawnPaladin()
		{
			int x = Main.rand.Next(2) == 0 ? -160 : 160;
			NPC.NewNPC((int)npc.Bottom.X + x, (int)npc.Bottom.Y + 80, mod.NPCType("PhantomOrb"), 0, 3f, npc.whoAmI, x, 80f, npc.target);
		}

		private void ModifyVelocity(Vector2 modify, float weight = 0.05f)
		{
			npc.velocity = Vector2.Lerp(npc.velocity, modify, weight);
		}

		private void CapVelocity(ref Vector2 velocity, float max)
		{
			if (velocity.Length() > max)
			{
				velocity.Normalize();
				velocity *= max;
			}
		}

		private void Talk(string message)
		{
			message = "<" + npc.displayName + "> " + message;
			if (Main.netMode == 0)
			{
				Main.NewText(message, 50, 150, 200);
			}
			else
			{
				NetMessage.SendData(MessageID.ChatText, -1, -1, message, 255, 50, 150, 200);
			}
		}

		public override void NPCLoot()
		{
			
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Phantom/PhantomBody");
			spriteBatch.Draw(texture, npc.Bottom - new Vector2(0f, 20f) - Main.screenPosition, null, Color.White * 0.6f, 0f, new Vector2(texture.Width / 2, 0f), 1f, SpriteEffects.None, 0f);
			texture = Main.npcTexture[npc.type];
			spriteBatch.Draw(texture, npc.position - Main.screenPosition, Color.White * 0.8f);
			return false;
		}
	}
}