using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System.Reflection;
using Bluemagic.PuritySpirit;

namespace Bluemagic.TerraSpirit
{
	public class TerraSpirit : ModNPC
	{
		private const int size = 120;
		private const int particleSize = 12;
		public const int arenaWidth = 2400;
		public const int arenaHeight = 1600;

		internal int Stage
		{
			get
			{
				return (int)npc.ai[0];
			}
			set
			{
				npc.ai[0] = value;
			}
		}

		internal int Progress
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Purity");
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
			NPCID.Sets.NeedsExpertScaling[npc.type] = false;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 750000;
			npc.damage = 0;
			npc.defense = 140;
			npc.knockBackResist = 0f;
			npc.width = size;
			npc.height = size;
			npc.value = Item.buyPrice(100, 0, 0, 0);
			npc.npcSlots = 1337f;
			npc.boss = true;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.chaseable = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			npc.alpha = 255;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Despicable beautiful");
		}

		private IList<Particle> particles = new List<Particle>();
		private float[,] aura = new float[size, size];
		internal List<Bullet> bullets = new List<Bullet>();

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 2f;
			return null;
		}

		public override void AI()
		{
			int numPlayers = CountPlayers();
			if (!Main.dedServ)
			{
				UpdateParticles();
			}
			npc.timeLeft = NPC.activeTime;
			if (Stage % 2 > 0 && numPlayers == 0)
			{
				Stage = -1;
				Progress = 0;
			}
			if (Stage == -1)
			{
				RunAway();
			}
			else if (Stage % 2 == 0)
			{
				Initialize();
			}
			else if (Stage == 1)
			{
				Stage1();
			}
			else if (Stage == 3)
			{
				Stage2();
			}
			else if (Stage == 5)
			{
				Stage3();
			}
			FixLife();
			Progress++;
			Rectangle bounds = new Rectangle((int)npc.Center.X - arenaWidth / 2, (int)npc.Center.Y - arenaHeight / 2, arenaWidth, arenaHeight);
			for (int k = 0; k < bullets.Count; k++)
			{
				if (bullets[k].Update(this, bounds))
				{
					Player player = Main.player[Main.myPlayer];
					if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && bullets[k].Collides(player.Hitbox))
					{
						player.GetModPlayer<BluemagicPlayer>().TerraKill();
					}
				}
				else
				{
					bullets.RemoveAt(k);
					k--;
				}
			}
		}

		private int CountPlayers()
		{
			int count = 0;
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0)
				{
					count++;
				}
			}
			return count;
		}

		public Player GetTarget()
		{
			Player player = Main.player[Main.myPlayer];
			if (!player.active || player.dead || player.GetModPlayer<BluemagicPlayer>().terraLives <= 0)
			{
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead && Main.player[k].GetModPlayer<BluemagicPlayer>().terraLives > 0)
					{
						player = Main.player[k];
						break;
					}
				}
			}
			return player;
		}

		private void UpdateParticles()
		{
			foreach (Particle particle in particles)
			{
				particle.Update();
			}
			Vector2 newPos = new Vector2(Main.rand.Next(3 * size / 8, 5 * size / 8), Main.rand.Next(3 * size / 8, 5 * size / 8));
			double newAngle = 2 * Math.PI * Main.rand.NextDouble();
			Vector2 newVel = new Vector2((float)Math.Cos(newAngle), (float)Math.Sin(newAngle));
			newVel *= 0.5f * (1f + (float)Main.rand.NextDouble());
			particles.Add(new Particle(newPos, newVel));
			if (particles[0].strength <= 0f)
			{
				particles.RemoveAt(0);
			}
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					aura[x, y] *= 0.97f;
				}
			}
			foreach (Particle particle in particles)
			{
				int minX = (int)particle.position.X - particleSize / 2;
				int minY = (int)particle.position.Y - particleSize / 2;
				int maxX = minX + particleSize;
				int maxY = minY + particleSize;
				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						if (x >= 0 && x < size && y >= 0 && y < size)
						{
							float strength = particle.strength;
							float offX = particle.position.X - x;
							float offY = particle.position.Y - y;
							strength *= 1f - (float)Math.Sqrt(offX * offX + offY * offY) / particleSize * 2;
							if (strength < 0f)
							{
								strength = 0f;
							}
							aura[x, y] = 1f - (1f - aura[x, y]) * (1f - strength);
						}
					}
				}
			}
		}

		public void RunAway()
		{
			if (Progress >= 180)
			{
				npc.active = false;
			}
		}

		public void Initialize()
		{
			if (Progress == 0)
			{
				bullets.Clear();
				Vector2 center = npc.Center;
				for (int k = 0; k < 255; k++)
				{
					Player player = Main.player[k];
					if (player.active & !player.dead && player.position.X > center.X - arenaWidth / 2 && player.position.X + player.width < center.X + arenaWidth / 2 && player.position.Y > center.Y - arenaHeight / 2 && player.position.Y + player.height < center.Y + arenaHeight / 2)
					{
						player.GetModPlayer<BluemagicPlayer>().terraLives = 10;
						if (Main.netMode == 2)
						{
							ModPacket netMessage = GetPacket(TerraSpiritMessageType.TerraPlayer);
							netMessage.Send(k);
						}
						else if (player.whoAmI == Main.myPlayer)
						{
							Main.NewText("You have " + 10 + " lives!");
						}
					}
				}
			}
			BluemagicPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
			if (Main.netMode != 2 && modPlayer.terraLives > 0)
			{
				if (Progress == 90)
				{
					Talk("You haved saved Terraria...");
				}
				if (Progress == 210)
				{
					Talk("You have passed my trial...");
				}
				if (Progress == 330)
				{
					Talk("You have slain the god responsible for the Elemental Unleash...");
				}
				if (Progress == 450)
				{
					Talk("And yet, you still desire more power, more challenge?");
				}
				if (Progress >= 570)
				{
					Main.PlaySound(15, -1, -1, 0);
					Talk("Your greed shall be your undoing.");
					Stage++;
					Progress = -1;
				}
			}
		}

		private void Stage1()
		{
			if (Progress == 0 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe1"), 0, npc.whoAmI);
			}
		}

		private void Stage2()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress == 60)
			{
				Vector2 center = npc.Center;
				const int xOffset = arenaWidth / 4;
				const int yOffset = arenaHeight / 4;
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, -yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, -yOffset)));
			}
			if (Progress >= 420 && Progress <= 660 && Progress % 60 == 0)
			{
				Vector2 center = GetTarget().Center;
				bullets.Add(new BulletCross(center + new Vector2(0f, 192f)));
				bullets.Add(new BulletCross(center + new Vector2(0f, -192f)));
				bullets.Add(new BulletCross(center + new Vector2(192f, 0f)));
				bullets.Add(new BulletCross(center + new Vector2(-192f, 0f)));
			}
			if (Progress == 720)
			{
				bullets.Add(new BulletChase(npc.Center, 30, 360, (position, spirit) => new BulletAccel(position, spirit.GetTarget().Center - position)));
			}
			if (Progress >= 1080 && Progress <= 1320 && Progress % 120 == 0)
			{
				Vector2 bulletPos = GetTarget().Center;
				bullets.Add(new BulletRingTimed(bulletPos, 6, 192f, 0.03f, 120));
				bullets.Add(new BulletBeamBig(bulletPos, 320, MathHelper.PiOver2, 120));
			}
			if (Progress == 1440 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe2"), 0, npc.whoAmI);
			}
		}

		private void Stage3()
		{
			if (Progress == 0)
			{
				Main.PlaySound(15, -1, -1, 0);
			}
			if (Progress >= 60 && Progress <= 300 && Progress % 60 == 0)
			{
				Vector2 center = GetTarget().Center;
				const float length = 192f;
				float rotation = 0f;
				if (Progress == 120)
				{
					rotation = MathHelper.PiOver4;
				}
				else if (Progress == 180)
				{
					rotation = MathHelper.Pi / 8f;
				}
				else if (Progress == 240)
				{
					rotation = MathHelper.Pi * 3f / 8f;
				}
				bullets.Add(new BulletCross(center + length * rotation.ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center + length * (rotation + MathHelper.PiOver2).ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center - length * rotation.ToRotationVector2(), rotation));
				bullets.Add(new BulletCross(center - length * (rotation + MathHelper.PiOver2).ToRotationVector2(), rotation));
			}
			if (Progress == 360)
			{
				Vector2 center = npc.Center;
				const int xOffset = arenaWidth / 4;
				const int yOffset = arenaHeight / 4;
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(xOffset, -yOffset)));
				bullets.Add(new BulletPortal(center, center + new Vector2(-xOffset, -yOffset)));
			}
			if (Progress >= 510 && Progress <= 690 && (Progress - 510) % 90 == 0)
			{
				bullets.Add(new BulletBeamBig(GetTarget().Center, 160, MathHelper.PiOver2, 60));
			}
			if (Progress >= 720 && Progress <= 1020 && Progress % 30 == 0)
			{
				bullets.Add(new BulletRingShrink(GetTarget().Center, 8f, 0.012f));
			}
			if (Progress == 1140)
			{
				bullets.Add(new BulletBeamBigRotate(npc.Center, 0.01f, 360, 120f, MathHelper.PiOver2, 60));
				bullets.Add(new BulletBeamBigRotate(npc.Center, 0.01f, 360, 120f, 0f, 60));
				bullets.Add(new BulletChase(npc.Center, 30, 360, (position, spirit) => new BulletAccel(position, spirit.GetTarget().Center - position)));
			}
			if (Progress >= 1500 && Progress <= 1800 && Progress % 4 == 0)
			{
				Vector2 bulletPos = new Vector2(npc.Center.X, npc.Center.Y - arenaHeight / 2);
				bulletPos.X += 80f * (float)Math.Sin(MathHelper.TwoPi * Progress / 120);
				bullets.Add(new BulletArray(bulletPos, 0f, 160f, new Vector2(0f, 8f), arenaHeight / 8));
			}
			if (Progress >= 1560 && Progress <= 1800 && Progress % 60 == 0)
			{
				bullets.Add(new BulletBeamBig(GetTarget().Center, 160, 0f, 60));
			}
			if (Progress == 1980 && Main.netMode != 1)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TerraProbe3"), 0, npc.whoAmI);
			}
		}

		private void FixLife()
		{
			switch (Stage)
			{
			case 0:
			case 1:
				npc.life = 750000;
				break;
			case 2:
			case 3:
				npc.life = 700000;
				break;
			case 4:
			case 5:
				npc.life = 600000;
				break;
			case 6:
			case 7:
				npc.life = 450000;
				break;
			case 8:
			case 9:
				npc.life = 250000;
				break;
			case 10:
			case 11:
				if (Main.expertMode)
				{
					npc.life = 1;
				}
				break;
			}
		}

		public override bool CheckDead()
		{
			FixLife();
			return false;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return false;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					Vector2 drawPos = npc.position - Main.screenPosition;
					drawPos.X += x * 2 - size / 2;
					drawPos.Y += y * 2 - size / 2;
					spriteBatch.Draw(mod.GetTexture("PuritySpirit/PurityParticle"), drawPos, null, Color.White * aura[x, y], 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(mod.GetTexture("PuritySpirit/PurityEyes"), npc.position - Main.screenPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			const int blockSize = 16;
			int centerX = (int)npc.Center.X;
			int centerY = (int)npc.Center.Y;
			Texture2D outlineTexture = mod.GetTexture("TerraSpirit/TerraBlockOutline");
			Texture2D blockTexture = mod.GetTexture("TerraSpirit/TerraBlock");
			for (int x = centerX - (arenaWidth + blockSize) / 2; x <= centerX + (arenaWidth + blockSize) / 2; x += blockSize)
			{
				int y = centerY - (arenaHeight + blockSize) / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.Y += arenaHeight + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}
			for (int y = centerY - (arenaHeight + blockSize) / 2; y <= centerY + (arenaHeight + blockSize) / 2; y += blockSize)
			{
				int x = centerX - (arenaWidth + blockSize) / 2;
				Vector2 drawPos = new Vector2(x - blockSize / 2, y - blockSize / 2) - Main.screenPosition;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
				drawPos.X += arenaWidth + blockSize;
				spriteBatch.Draw(outlineTexture, drawPos, Color.White);
				spriteBatch.Draw(blockTexture, drawPos, Color.White * 0.75f);
			}

			foreach (Bullet bullet in bullets)
			{
				bullet.Draw(spriteBatch);
			}
		}

		private void Talk(string message)
		{
			if (Main.netMode != 2)
			{
				string text = Language.GetTextValue("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(npc.type), message);
				Main.NewText(text, 150, 250, 150);
			}
			else
			{
				NetworkText text = NetworkText.FromKey("Mods.Bluemagic.NPCTalk", Lang.GetNPCNameValue(npc.type), message);
				NetMessage.BroadcastChatMessage(text, new Color(150, 250, 150));
			}
		}

		private ModPacket GetPacket(TerraSpiritMessageType type)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)MessageType.TerraSpirit);
			packet.Write(npc.whoAmI);
			packet.Write((byte)type);
			return packet;
		}

		public void HandlePacket(BinaryReader reader)
		{
			TerraSpiritMessageType type = (TerraSpiritMessageType)reader.ReadByte();
			if (type == TerraSpiritMessageType.TerraPlayer)
			{
				Player player = Main.player[Main.myPlayer];
				player.GetModPlayer<BluemagicPlayer>(mod).heroLives = 10;
			}
		}
	}

	enum TerraSpiritMessageType : byte
	{
		TerraPlayer
	}
}
