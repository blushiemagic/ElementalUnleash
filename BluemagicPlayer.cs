using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Bluemagic.BlushieBoss;
using Bluemagic.PuritySpirit;

namespace Bluemagic
{
	public class BluemagicPlayer : ModPlayer
	{
		public bool eFlames = false;
		public int customMeleeEnchant = 0;
		public bool paladinMinion = false;
		public bool elementShield = false;
		public int elementShields = 0;
		private int elementShieldTimer = 0;
		public int elementShieldPos = 0;
		public bool voidMonolith = false;
		public bool extraAccessory2 = false;
		public bool elementMinion = false;
		public bool saltLamp = false;

		public float puriumShieldChargeMax = 0f;
		public float puriumShieldChargeRate = 1f;
		public float puriumShieldEnduranceMult = 1f;
		public const float puriumShieldDamageEffectiveness = 0.002f;
		public int[] buffImmuneCounter;
		public static List<int> buffImmuneBlacklist = new List<int>(new int[]
		{
			BuffID.Wet,
			BuffID.Lovestruck,
			BuffID.Stinky,
			BuffID.Slimed,
			BuffID.Sunflower,
			BuffID.MonsterBanner,
			BuffID.PeaceCandle
		});
		public const float buffImmuneCost = 50f;
		public const float reviveCost = 1000f;
		private int miscTimer = 0;
		public bool purityMinion = false;

		public int heroLives = 0;
		public int reviveTime = 0;
		public int constantDamage = 0;
		public float percentDamage = 0f;
		public float defenseEffect = -1f;
		public bool chaosDefense = false;
		public bool badHeal = false;
		public int prevLife = -1;
		public int healHurt = 0;
		public bool nullified = false;
		public int purityDebuffCooldown = 0;

		public bool manaMagnet2 = false;
		public bool crystalCloak = false;
		public bool lifeMagnet2 = false;
		public bool voidEmissary = false;
		public bool purityShieldMount = false;

		private int chaosWarningCooldown = 0;
		public int chaosPressure = 0;
		public float suppression = 0f;
		public float ammoCost = 0f;
		public float thrownCost = 0f;
		public int cancelBadRegen = 0;

		internal int terraLives = 0;
		private int terraKill = 0;
		private int terraImmune = 0;
		private Vector2 lastPos;
		public bool triedGodmode = false;
		public bool godmode = false;
		public bool noGodmode = false;
		internal float blushieHealth = 0f;
		internal int origHealth;
		private int blushieImmune = 0;
		public bool frostFairy = false;
		public bool skyDragon = false;
		public int worldReaverCooldown = 0;

		//permanent data
		public float puriumShieldCharge = 0f;
		public CustomStats chaosStats;
		public CustomStats cataclysmStats;

		public override void Initialize()
		{
			buffImmuneCounter = new int[player.buffImmune.Length];
			chaosStats = CustomStats.CreateChaosStats();
			cataclysmStats = CustomStats.CreateCataclysmStats();
		}

		public override void ResetEffects()
		{
			if (lifeMagnet2)
			{
				player.potionDelayTime = (int)(player.potionDelayTime * 0.8f);
				player.restorationDelayTime = (int)(player.restorationDelayTime * 0.8f);
			}
			eFlames = false;
			customMeleeEnchant = 0;
			paladinMinion = false;
			elementShield = false;
			elementMinion = false;
			puriumShieldChargeMax = 0f;
			puriumShieldChargeRate = 1f;
			puriumShieldEnduranceMult = 1f;
			purityMinion = false;
			constantDamage = 0;
			percentDamage = 0f;
			defenseEffect = -1f;
			chaosDefense = false;
			badHeal = false;
			healHurt = 0;
			nullified = false;
			manaMagnet2 = false;
			crystalCloak = false;
			lifeMagnet2 = false;
			voidEmissary = false;
			purityShieldMount = false;
			chaosPressure = 0;
			suppression = 0f;
			ammoCost = 0f;
			thrownCost = 0f;
			cancelBadRegen = 0;
			triedGodmode = false;
			godmode = false;
			noGodmode = false;
			frostFairy = false;
			skyDragon = false;
			if (extraAccessory2)
			{
				player.extraAccessorySlots = 2;
			}
		}

		public override void UpdateDead()
		{
			eFlames = false;
			badHeal = false;
			puriumShieldCharge = 0f;
			reviveTime = 0;
			terraLives = 0;
			terraKill = 0;
			blushieHealth = 0f;
			blushieImmune = 0;
			if (heroLives == 1)
			{
				heroLives = 0;
				if (Main.netMode == 1)
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)MessageType.HeroLives);
					packet.Write(player.whoAmI);
					packet.Write(heroLives);
					packet.Send();
				}
			}
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["version"] = 0;
			tag["extraAccessory2"] = extraAccessory2;
			tag["puriumShieldCharge"] = puriumShieldCharge;
			tag["chaosStats"] = chaosStats.Save();
			tag["cataclysmStats"] = cataclysmStats.Save();
			return tag;
		}

		public override void Load(TagCompound tag)
		{
			extraAccessory2 = tag.GetBool("extraAccessory2");
			puriumShieldCharge = tag.GetFloat("puriumShieldCharge");
			TagCompound tagStats = tag.GetCompound("chaosStats");
			if (tagStats != null)
			{
				chaosStats.Load(tagStats);
			}
			tagStats = tag.GetCompound("cataclysmStats");
			if (tagStats != null)
			{
				cataclysmStats.Load(tagStats);
			}
		}

		public override void clientClone(ModPlayer clientClone)
		{
			BluemagicPlayer clone = clientClone as BluemagicPlayer;
			clone.chaosStats = chaosStats.Clone();
			clone.cataclysmStats = cataclysmStats.Clone();
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket(512);
			packet.Write((byte)MessageType.CustomStats);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)0);
			chaosStats.NetSend(packet);
			packet.Send(toWho, fromWho);
			packet = mod.GetPacket(512);
			packet.Write((byte)MessageType.CustomStats);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)1);
			cataclysmStats.NetSend(packet);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			BluemagicPlayer clone = clientPlayer as BluemagicPlayer;
			if (!chaosStats.Equals(clone.chaosStats))
			{
				ModPacket packet = mod.GetPacket(512);
				packet.Write((byte)MessageType.CustomStats);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)0);
				chaosStats.NetSend(packet);
				packet.Send();
			}
			if (!cataclysmStats.Equals(clone.cataclysmStats))
			{
				ModPacket packet = mod.GetPacket(512);
				packet.Write((byte)MessageType.CustomStats);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)1);
				cataclysmStats.NetSend(packet);
				packet.Send();
			}
		}

		private bool AnyChaosSpirit()
		{
			return NPC.AnyNPCs(mod.NPCType("ChaosSpirit")) || NPC.AnyNPCs(mod.NPCType("ChaosSpirit2")) || NPC.AnyNPCs(mod.NPCType("ChaosSpirit3"));
		}

		private bool AnyTerraSpirit()
		{
			return NPC.AnyNPCs(mod.NPCType("TerraSpirit")) || NPC.AnyNPCs(mod.NPCType("TerraSpirit2"));
		}

		private bool IsChaosSpirit(int type)
		{
			return type == mod.NPCType("ChaosSpirit") || type == mod.NPCType("ChaosSpirit2") || type == mod.NPCType("ChaosSpirit3");
		}

		private bool IsTerraSpirit(int type)
		{
			return type == mod.NPCType("TerraSpirit") || type == mod.NPCType("TerraSpirit2");
		}

		public override void UpdateBiomeVisuals()
		{
			bool useTerra = false;
			bool useChaos = false;
			bool usePurity = false;
			bool useVoidMonolith = false;
			if (BlushieBoss.BlushieBoss.Active)
			{
			}
			else if (AnyTerraSpirit())
			{
				useTerra = true;
			}
			else if (AnyChaosSpirit())
			{
				useChaos = true;
			}
			else if (NPC.AnyNPCs(mod.NPCType("PuritySpirit")))
			{
				usePurity = true;
			}
			else if (voidMonolith && !NPC.AnyNPCs(NPCID.MoonLordCore))
			{
				useVoidMonolith = true;
			}
			player.ManageSpecialBiomeVisuals("Bluemagic:TerraSpirit", useTerra);
			player.ManageSpecialBiomeVisuals("Bluemagic:ChaosSpirit", useChaos);
			player.ManageSpecialBiomeVisuals("Bluemagic:PuritySpirit", usePurity);
			player.ManageSpecialBiomeVisuals("Bluemagic:MonolithVoid", useVoidMonolith);
		}

		public override void UpdateBadLifeRegen()
		{
			if (eFlames)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 16;
			}
			if (healHurt > 0)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 120 * healHurt;
			}
			if (chaosPressure > 0)
			{
				player.lifeRegenTime -= chaosPressure;
				player.lifeRegen -= chaosPressure;
				if (player.lifeRegenTime < 0)
				{
					player.lifeRegenTime = 0;
				}
			}
			if (blushieImmune > 0)
			{
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				if (!Bluemagic.testing || player.statLife > 1)
				{
					player.lifeRegen -= 32;
				}
			}
		}

		public override void UpdateLifeRegen()
		{
			if (player.lifeRegen < 0 && cancelBadRegen > 0)
			{
				player.lifeRegen += cancelBadRegen;
				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
			}
		}

		public override void SetControls()
		{
			for (int k = 0; k < 1000; k++)
			{
				if (Main.projectile[k].active && Main.projectile[k].owner == player.whoAmI && Main.projectile[k].type == mod.ProjectileType("CleanserBeam"))
				{
					player.controlLeft = false;
					player.controlRight = false;
					player.controlUp = false;
					player.controlDown = false;
					player.controlJump = false;
					break;
				}
			}
		}

		public override void PreUpdateBuffs()
		{
			if (saltLamp)
			{
				player.AddBuff(mod.BuffType("SaltLamp"), 2, false);
			}
			if (heroLives > 0)
			{
				bool flag = false;
				for (int k = 0; k < 200; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.active && npc.type == mod.NPCType("PuritySpirit"))
					{
						flag = true;
						PuritySpiritTeleport(npc);
						break;
					}
				}
				for (int k = 0; k < 200; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.active && IsChaosSpirit(npc.type))
					{
						flag = true;
						ChaosSpiritWarning(npc);
						break;
					}
				}
				if (!flag && !Bluemagic.freezeHeroLives)
				{
					heroLives = 0;
				}
				if (heroLives == 1)
				{
					player.AddBuff(mod.BuffType("HeroOne"), 2);
				}
				else if (heroLives == 2)
				{
					player.AddBuff(mod.BuffType("HeroTwo"), 2);
				}
				else if (heroLives == 3)
				{
					player.AddBuff(mod.BuffType("HeroThree"), 3);
				}
			}
			else
			{
				chaosWarningCooldown = 0;
			}
			if (purityDebuffCooldown > 0)
			{
				purityDebuffCooldown--;
			}
			if (terraLives > 0)
			{
				bool flag = false;
				for (int k = 0; k < 200; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.active && IsTerraSpirit(npc.type))
					{
						flag = true;
						TerraSpiritBarrier(npc);
						break;
					}
				}
				if (!flag)
				{
					terraLives = 0;
				}
			}
			if (blushieHealth > 0f)
			{
				if (BlushieBoss.BlushieBoss.Active)
				{
					BlushieBarrier();
				}
				else
				{
					blushieHealth = 0f;
				}
			}
			if (CursedMount())
			{
				if (player.mount.Active)
				{
					player.mount.Dismount(player);
				}
				player.AddBuff(mod.BuffType("NoMount"), 5);
			}
			lastPos = player.position;
		}

		public bool CursedMount()
		{
			return BlushieBoss.BlushieBoss.Players[player.whoAmI] && BlushieBoss.BlushieBoss.Phase == 3 && (BlushieBoss.BlushieBoss.Phase3Attack > 7 || (BlushieBoss.BlushieBoss.Phase3Attack == 7 && BlushieBoss.BlushieBoss.Timer >= 2210));
		}

		public void CheckBadHeal()
		{
			if (prevLife >= 0 && badHeal && player.statLife > prevLife)
			{
				int hurt = 2 * (player.statLife - prevLife);
				player.statLife -= hurt;
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.DamagedFriendly, hurt.ToString(), false, false);
				if (player.statLife <= 0 && player.whoAmI == Main.myPlayer)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was dissolved by holy powers"), hurt, 0, false);
				}
			}
			prevLife = -1;
		}

		public void StartBadHeal()
		{
			if (badHeal)
			{
				prevLife = player.statLife;
			}
		}

		private void PuritySpiritTeleport(NPC npc)
		{
			int halfWidth = PuritySpirit.PuritySpirit.arenaWidth / 2;
			int halfHeight = PuritySpirit.PuritySpirit.arenaHeight / 2;
			Vector2 newPosition = player.position;
			if (player.position.X <= npc.Center.X - halfWidth)
			{
				newPosition.X = npc.Center.X + halfWidth - player.width - 1;
				while (Collision.SolidCollision(newPosition, player.width, player.height))
				{
					newPosition.X -= 16f;
				}
			}
			else if (player.position.X + player.width >= npc.Center.X + halfWidth)
			{
				newPosition.X = npc.Center.X - halfWidth + 1;
				while (Collision.SolidCollision(newPosition, player.width, player.height))
				{
					newPosition.X += 16f;
				}
			}
			else if (player.position.Y <= npc.Center.Y - halfHeight)
			{
				newPosition.Y = npc.Center.Y + halfHeight - player.height - 1;
				while (Collision.SolidCollision(newPosition, player.width, player.height))
				{
					newPosition.Y -= 16f;
				}
			}
			else if (player.position.Y + player.height >= npc.Center.Y + halfHeight)
			{
				newPosition.Y = npc.Center.Y - halfHeight + 1;
				while (Collision.SolidCollision(newPosition, player.width, player.height))
				{
					newPosition.Y += 16f;
				}
			}
			if (newPosition != player.position)
			{
				player.Teleport(newPosition, 1, 0);
				NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, newPosition.X, newPosition.Y, 1, 0, 0);
				PuritySpiritDebuff();
			}
		}

		internal void TerraSpiritBarrier(NPC npc)
		{
			Vector2 offset = player.position - lastPos;
			if (offset.Length() > 32f)
			{
				offset.Normalize();
				offset *= 32f;
				player.position = lastPos + offset;
			}
			int halfWidth = TerraSpirit.TerraSpirit.arenaWidth / 2;
			int halfHeight = TerraSpirit.TerraSpirit.arenaHeight / 2;
			bool spikes = npc.type == mod.NPCType("TerraSpirit2");
			if (player.position.X <= npc.Center.X - halfWidth)
			{
				player.position.X = npc.Center.X - halfWidth;
				if (player.velocity.X < 0f)
				{
					player.velocity.X = 0f;
				}
				if (spikes)
				{
					TerraKill();
				}
			}
			else if (player.position.X + player.width >= npc.Center.X + halfWidth)
			{
				player.position.X = npc.Center.X + halfWidth - player.width;
				if (player.velocity.X > 0f)
				{
					player.velocity.X = 0f;
				}
				if (spikes)
				{
					TerraKill();
				}
			}
			if (player.position.Y <= npc.Center.Y - halfHeight)
			{
				player.position.Y = npc.Center.Y - halfHeight;
				if (player.velocity.Y < 0f)
				{
					player.velocity.Y = 0f;
				}
				if (spikes)
				{
					TerraKill();
				}
			}
			else if (player.position.Y + player.height >= npc.Center.Y + halfHeight)
			{
				player.position.Y = npc.Center.Y + halfHeight - player.height;
				if (player.velocity.Y > 0f)
				{
					player.velocity.Y = 0f;
				}
				if (spikes)
				{
					TerraKill();
				}
			}
		}

		internal void BlushieBarrier()
		{
			Vector2 offset = player.position - lastPos;
			if (offset.Length() > 32f)
			{
				offset.Normalize();
				offset *= 32f;
				player.position = lastPos + offset;
			}
			Vector2 origin = BlushieBoss.BlushieBoss.Origin;
			float arenaSize = BlushieBoss.BlushieBoss.ArenaSize;
			if (player.position.X <= origin.X - arenaSize)
			{
				player.position.X = origin.X - arenaSize;
				if (player.velocity.X < 0f)
				{
					player.velocity.X = 0f;
				}
			}
			else if (player.position.X + player.width >= origin.X + arenaSize)
			{
				player.position.X = origin.X + arenaSize - player.width;
				if (player.velocity.X > 0f)
				{
					player.velocity.X = 0f;
				}
			}
			if (player.position.Y <= origin.Y - arenaSize)
			{
				player.position.Y = origin.Y - arenaSize;
				if (player.velocity.Y < 0f)
				{
					player.velocity.Y = 0f;
				}
			}
			else if (player.position.Y + player.height >= origin.Y + arenaSize)
			{
				player.position.Y = origin.Y + arenaSize - player.height;
				if (player.velocity.Y > 0f)
				{
					player.velocity.Y = 0f;
				}
			}
		}

		public void PuritySpiritDebuff()
		{
			bool flag = true;
			if (Main.rand.Next(2) == 0)
			{
				flag = false;
				for (int k = 0; k < 2; k++)
				{
					int buffType;
					int buffTime;
					switch (Main.rand.Next(5))
					{
						case 0:
							buffType = BuffID.Darkness;
							buffTime = 900;
							break;
						case 1:
							buffType = BuffID.Cursed;
							buffTime = 300;
							break;
						case 2:
							buffType = BuffID.Confused;
							buffTime = 600;
							break;
						case 3:
							buffType = BuffID.Slow;
							buffTime = 900;
							break;
						default:
							buffType = BuffID.Silenced;
							buffTime = 300;
							break;
					}
					if (Main.expertMode)
					{
						buffTime = buffTime * 2 / 3;
					}
					if (!player.buffImmune[buffType])
					{
						player.AddBuff(buffType, buffTime);
						return;
					}
				}
			}
			if (flag || Main.expertMode || Main.rand.Next(2) == 0)
			{
				int buffTime = 1800;
				if (Main.expertMode)
				{
					buffTime = 1200;
				}
				player.AddBuff(mod.BuffType("Undead"), buffTime, false);
			}
			for (int k = 0; k < 25; k++)
			{
				Dust.NewDust(player.position, player.width, player.height, mod.DustType("Negative"), 0f, -1f, 0, default(Color), 2f);
			}
		}

		private void ChaosSpiritWarning(NPC npc)
		{
			float distance = Vector2.Distance(player.Center, npc.Center);
			if (distance > ChaosSpirit.ChaosSpirit.killRadius)
			{
				ChaosKill();
			}
			else if (distance > ChaosSpirit.ChaosSpirit.warningRadius)
			{
				if (chaosWarningCooldown <= 0)
				{
					chaosWarningCooldown = 300;
					Main.NewText("The air feels heavy too far away from the chaos...", 255, 255, 255);
				}
			}
			if (chaosWarningCooldown > 0)
			{
				chaosWarningCooldown--;
			}
		}

		public void ChaosKill()
		{
			int damage = 100 * player.statLifeMax2;
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.DamagedFriendly, damage.ToString(), true, false);
			if (player.whoAmI == Main.myPlayer)
			{
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was crushed by chaotic pressure!"), damage, 0, false);
			}
		}

		public override void PostUpdateBuffs()
		{
			if (nullified)
			{
				Nullify();
			}
			if (BlushieBoss.BlushieBoss.Players[player.whoAmI])
			{
				noGodmode = true;
			}
		}

		public override void PostUpdateEquips()
		{
			if (nullified)
			{
				Nullify();
			}
			if (elementShield)
			{
				if (elementShields > 0)
				{
					elementShieldTimer--;
					if (elementShieldTimer < 0)
					{
						elementShields--;
						elementShieldTimer = 600;
					}
				}
			}
			else
			{
				elementShields = 0;
				elementShieldTimer = 0;
			}
			elementShieldPos++;
			elementShieldPos %= 300;
			if (saltLamp)
			{
				player.statDefense += 4;
			}
			chaosStats.Update(player);
			if (Main.expertMode)
			{
				cataclysmStats.Update(player);
			}
		}

		public override void PostUpdateMiscEffects()
		{
			if (reviveTime > 0)
			{
				reviveTime--;
			}
			if (puriumShieldChargeMax > 0f)
			{
				ChargePuriumShield(0.001f);
				for (int k = 0; k < Player.MaxBuffs; k++)
				{
					if (puriumShieldCharge < buffImmuneCost)
					{
						break;
					}
					if (player.buffType[k] > 0 && player.buffTime[k] > 0 && Main.debuff[player.buffType[k]] && BuffLoader.CanBeCleared(player.buffType[k]) && !buffImmuneBlacklist.Contains(player.buffType[k]))
					{
						buffImmuneCounter[player.buffType[k]] = 600;
						puriumShieldCharge -= buffImmuneCost;
					}
				}
				for (int k = 0; k < buffImmuneCounter.Length; k++)
				{
					if (buffImmuneCounter[k] > 0)
					{
						player.buffImmune[k] = true;
						buffImmuneCounter[k]--;
					}
				}
				player.endurance += PuriumShieldEndurance();
			}
			CheckBadHeal();
			if (terraImmune > 0)
			{
				terraImmune--;
			}
			if (blushieImmune > 0)
			{
				blushieImmune--;
			}
			if (terraKill > 0)
			{
				terraKill--;
				player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was torn apart by the force of Terraria!"), player.statLifeMax2 * 100, 0, false);
			}
			origHealth = player.statLifeMax2;
			if (blushieHealth > 0f)
			{
				player.statLifeMax2 = (int)(blushieHealth * player.statLifeMax2);
			}
		}

		private void ChargePuriumShield(float charge)
		{
			charge *= puriumShieldChargeRate;
			if (puriumShieldCharge + charge > puriumShieldChargeMax)
			{
				charge = puriumShieldChargeMax - puriumShieldCharge;
			}
			if (charge < 0f)
			{
				charge = 0f;
			}
			puriumShieldCharge += charge;
		}

		private float PuriumShieldEndurance()
		{
			float enduranceFill = puriumShieldCharge / reviveCost;
			if (enduranceFill > puriumShieldEnduranceMult)
			{
				enduranceFill = puriumShieldEnduranceMult;
			}
			return 0.2f * enduranceFill;
		}

		public override void PreUpdateMovement()
		{
			if (purityShieldMount)
			{
				if (player.controlLeft)
				{
					player.velocity.X = -Mounts.PurityShield.speed;
					player.direction = -1;
				}
				else if (player.controlRight)
				{
					player.velocity.X = Mounts.PurityShield.speed;
					player.direction = 1;
				}
				else
				{
					player.velocity.X = 0f;
				}
				if (player.controlUp)
				{
					player.velocity.Y = -Mounts.PurityShield.speed;
				}
				else if (player.controlDown)
				{
					player.velocity.Y = Mounts.PurityShield.speed;
					Vector2 test = Collision.TileCollision(player.position, player.velocity, player.width, player.height, true, false, (int)player.gravDir);
					if (test.Y == 0f)
					{
						player.velocity.Y = 0.5f;
					}
				}
				else
				{
					player.velocity.Y = 0f;
				}
				if (player.controlJump)
				{
					player.velocity *= 0.5f;
				}
			}
		}

		public override void PostUpdate()
		{
			StartBadHeal();
			miscTimer++;
			miscTimer %= 60;
			if (worldReaverCooldown > 0)
			{
				worldReaverCooldown--;
			}
			if (purityShieldMount)
			{
				player.hairFrame.Y = 5 * player.hairFrame.Height;
				player.headFrame.Y = 5 * player.headFrame.Height;
				player.legFrame.Y = 5 * player.legFrame.Height;
			}
		}

		public override void FrameEffects()
		{
			if (player.inventory[player.selectedItem].type == mod.ItemType("DarkLightningPack"))
			{
				player.back = mod.GetAccessorySlot("DarkLightningPack_Back", EquipType.Back);
			}
			if (nullified)
			{
				Nullify();
			}
		}

		private void Nullify()
		{
			player.ResetEffects();
			player.head = -1;
			player.body = -1;
			player.legs = -1;
			player.handon = -1;
			player.handoff = -1;
			player.back = -1;
			player.front = -1;
			player.shoe = -1;
			player.waist = -1;
			player.shield = -1;
			player.neck = -1;
			player.face = -1;
			player.balloon = -1;
			nullified = true;
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			return !godmode;
		}

		public override bool CanBeHitByProjectile(Projectile projectile)
		{
			return !godmode;
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if (npc.type == NPCID.DungeonSpirit && Main.rand.Next(4) == 0)
			{
				player.AddBuff(mod.BuffType("EtherealFlames"), Main.rand.Next(120, 180), true);
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit,
			ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (constantDamage > 0 || percentDamage > 0f)
			{
				int damageFromPercent = (int)(player.statLifeMax2 * percentDamage);
				damage = Math.Max(constantDamage, damageFromPercent);
				if (chaosDefense)
				{
					double cap = Main.expertMode ? 75.0 : 50.0;
					int reduction = (int)(cap * (1.0 - Math.Exp(-player.statDefense / 150.0)));
					if (reduction < 0)
					{
						reduction = player.statDefense / 2;
					}
					damage -= reduction;
					if (damage < 0)
					{
						damage = 1;
					}
				}
				customDamage = true;
			}
			else if (defenseEffect >= 0f)
			{
				if (Main.expertMode)
				{
					defenseEffect *= 1.5f;
				}
				damage -= (int)(player.statDefense * defenseEffect);
				if (damage < 0)
				{
					damage = 1;
				}
				customDamage = true;
			}
			constantDamage = 0;
			percentDamage = 0f;
			defenseEffect = -1f;
			chaosDefense = false;
			if (godmode)
			{
				damage = 0;
				customDamage = true;
			}
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			if (elementShield && damage > 1.0)
			{
				if (elementShields < 6)
				{
					int k;
					bool flag = false;
					for (k = 3; k < 8 + player.extraAccessorySlots; k++)
					{
						if (player.armor[k].type == mod.ItemType("SixColorShield"))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("ElementShield"), player.GetWeaponDamage(player.armor[k]), player.GetWeaponKnockback(player.armor[k], 2f), player.whoAmI, elementShields++);
					}
				}
				elementShieldTimer = 600;
			}
			if (crystalCloak)
			{
				for (int l = 0; l < 3; l++)
				{
					Vector2 crystalPos = player.Center + new Vector2(Main.rand.Next(-400, 401), -Main.rand.Next(500, 800));
					Vector2 offset = player.Center - crystalPos;
					offset.X += (float)Main.rand.Next(-50, 51);
					offset *= 28f / offset.Length();
					int proj = Projectile.NewProjectile(crystalPos.X, crystalPos.Y, offset.X, offset.Y, mod.ProjectileType("CrystalStar"), 100, 6f, player.whoAmI, 0f, 0f);
					Main.projectile[proj].ai[1] = player.position.Y;
				}
			}
			if (puriumShieldChargeMax > 0f)
			{
				float effectiveEndurance = player.endurance;
				if (effectiveEndurance >= 0.995f)
				{
					effectiveEndurance = 0.995f;
				}
				double fullDamage = damage / (1f - effectiveEndurance);
				float shieldDamage = (float)(fullDamage * PuriumShieldEndurance() * 0.1f);
				puriumShieldCharge -= shieldDamage;
				if (puriumShieldCharge < 0f)
				{
					puriumShieldCharge = 0f;
				}
			}
			if (heroLives > 0)
			{
				for (int k = 0; k < 200; k++)
				{
					NPC npc = Main.npc[k];
					if (npc.active && npc.type == mod.NPCType("PuritySpirit"))
					{
						PuritySpirit.PuritySpirit modNPC = (PuritySpirit.PuritySpirit)npc.modNPC;
						if (modNPC.attack >= 0)
						{
							double proportion = damage / player.statLifeMax2;
							if (proportion > 1.0)
							{
								proportion = 1.0;
							}
							modNPC.attackWeights[modNPC.attack] += (int)(proportion * 400);
							if (modNPC.attackWeights[modNPC.attack] > PuritySpirit.PuritySpirit.maxAttackWeight)
							{
								modNPC.attackWeights[modNPC.attack] = PuritySpirit.PuritySpirit.maxAttackWeight;
							}
							if (nullified && modNPC.attack != 2)
							{
								modNPC.attackWeights[2] += (int)(proportion * 200);
								if (modNPC.attackWeights[2] > PuritySpirit.PuritySpirit.maxAttackWeight)
								{
									modNPC.attackWeights[2] = PuritySpirit.PuritySpirit.maxAttackWeight;
								}
							}
						}
					}
				}
			}
		}

		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			if (!pvp && crystalCloak)
			{
				int immuneTime = 0;
				if (damage == 1.0)
				{
					immuneTime = 40;
				}
				else
				{
					immuneTime = 80;
				}
				if (player.immuneTime == immuneTime)
				{
					player.immuneTime += 20;
				}
				for (int k = 0; k < player.hurtCooldowns.Length; k++)
				{
					if (player.hurtCooldowns[k] == immuneTime)
					{
						player.hurtCooldowns[k] += 20;
					}
				}
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (godmode)
			{
				player.statLife = player.statLifeMax2;
				return false;
			}
			if (puriumShieldChargeMax > 0f && puriumShieldCharge >= reviveCost)
			{
				puriumShieldCharge -= reviveCost;
				if (player.statLife < 1)
				{
					player.statLife = 1;
				}
				StartBadHeal();
				player.immune = true;
				player.immuneTime = player.longInvince ? 180 : 120;
				if (crystalCloak)
				{
					player.immuneTime += 60;
				}
				for (int k = 0; k < player.hurtCooldowns.Length; k++)
				{
					player.hurtCooldowns[k] = player.longInvince ? 180 : 120;
				}
				return false;
			}
			if (heroLives > 1)
			{
				heroLives--;
				if (Main.netMode == 1)
				{
					ModPacket packet = mod.GetPacket();
					packet.Write((byte)MessageType.HeroLives);
					packet.Write(player.whoAmI);
					packet.Write(heroLives);
					packet.Send();
				}
				player.statLife = player.statLifeMax2;
				player.HealEffect(player.statLifeMax2);
				StartBadHeal();
				player.immune = true;
				player.immuneTime = player.longInvince ? 180 : 120;
				if (crystalCloak)
				{
					player.immuneTime += 60;
				}
				for (int k = 0; k < player.hurtCooldowns.Length; k++)
				{
					player.hurtCooldowns[k] = player.longInvince ? 180 : 120;
				}
				Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 29);
				reviveTime = 60;
				return false;
			}
			if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
			{
				if (healHurt > 0)
				{
					damageSource = PlayerDeathReason.ByCustomReason(player.name + " was dissolved by holy powers");
				}
				else if (chaosPressure > 0)
				{
					damageSource = PlayerDeathReason.ByCustomReason(player.name + " was crushed by chaotic pressure");
				}
				else if (blushieImmune > 0)
				{
					damageSource = PlayerDeathReason.ByCustomReason(player.name + " succumbed to the might of the blushie!");
				}
			}
			return true;
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			Item salt = new Item();
			salt.SetDefaults(mod.ItemType("Salt"));
			int npc = NPC.FindFirstNPC(mod.NPCType("Abomination"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 25 + 25 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			int count = NPC.CountNPCS(mod.NPCType("CaptiveElement2"));
			if (count > 0)
			{
				salt.stack = 50 + 5 * (5 - count);
				player.GetItem(player.whoAmI, salt);
			}
			npc = NPC.FindFirstNPC(mod.NPCType("Phantom"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 50 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			npc = NPC.FindFirstNPC(mod.NPCType("PuritySpirit"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 50 + 50 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			npc = NPC.FindFirstNPC(mod.NPCType("ChaosSpirit"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 50 + 25 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			npc = NPC.FindFirstNPC(mod.NPCType("ChaosSpirit2"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 75 + 50 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			npc = NPC.FindFirstNPC(mod.NPCType("ChaosSpirit3"));
			if (npc >= 0)
			{
				NPC ent = Main.npc[npc];
				salt.stack = 125 + 25 * (ent.lifeMax - ent.life) / ent.lifeMax;
				player.GetItem(player.whoAmI, salt);
			}
			if (NPC.AnyNPCs(mod.NPCType("TerraSpirit")))
			{
				salt.SetDefaults(mod.ItemType("PureSalt"));
				salt.stack = 2 * BluemagicWorld.terraDeaths;
				if (salt.stack > 999)
				{
					salt.stack = 999;
				}
				player.GetItem(player.whoAmI, salt);
			}
		}

		public void TerraKill()
		{
			if (terraLives <= 0 || terraImmune > 0 || Main.netMode == 2)
			{
				return;
			}
			int damage = 100 * player.statLifeMax2;
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.DamagedFriendly, damage.ToString(), true, false);
			terraLives--;
			if (terraLives == 0 && Bluemagic.testing)
			{
				Main.NewText("YOU LOSE! Lucky you're just the dev whose testing her fight.", 255, 25, 25);
				terraLives = 1;
			}
			if (Main.netMode == 1)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((byte)MessageType.TerraLives);
				packet.Write(player.whoAmI);
				packet.Write(terraLives);
				packet.Send();
			}
			if (Main.netMode == 0)
			{
				string text;
				if (terraLives == 1)
				{
					text = Language.GetTextValue("Mods.Bluemagic.LifeLeft", player.name);
				}
				else
				{
					text = Language.GetTextValue("Mods.Bluemagic.LivesLeft", player.name, terraLives);
				}
				Main.NewText(text, 255, 25, 25);
			}
			if (terraLives > 0)
			{
				player.statLife = player.statLifeMax2;
				player.HealEffect(player.statLifeMax2);
				terraImmune = 60;
				if (!player.immune)
				{
					player.immune = true;
					player.immuneTime = 60;
				}
			}
			else if (Main.myPlayer == player.whoAmI)
			{
				terraKill = 10;
				for (int k = 0; k < 10; k++)
				{
					if (player.dead)
					{
						terraKill = 0;
						break;
					}
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was torn apart by the force of Terraria!"), player.statLifeMax2 * 100, 0, false);
				}
			}
		}

		public void BlushieDamage(float bulletDamage)
		{
			if (blushieHealth <= 0f || blushieImmune > 0 || Main.netMode == 2)
			{
				return;
			}
			int oldHealth = player.statLife;
			blushieHealth -= bulletDamage;
			player.statLife = (int)(player.statLifeMax2 * blushieHealth);
			int constDamage = (int)((200f - player.statDefense / 2f) * (1f - player.endurance));
			if (constDamage < 1)
			{
				constDamage = 1;
			}
			if (player.statLife > oldHealth - constDamage)
			{
				player.statLife = oldHealth - constDamage;
			}
			int damage = oldHealth - player.statLife;
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), CombatText.DamagedFriendly, damage.ToString(), true, false);
			if (player.Male)
			{
				Main.PlaySound(1, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
			}
			else
			{
				Main.PlaySound(20, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
			}
			if (blushieHealth > 0f && player.statLife > 0)
			{
				blushieImmune = 60;
				if (!player.immune)
				{
					player.immune = true;
					player.immuneTime = 60;
				}
			}
			else if (Main.myPlayer == player.whoAmI)
			{
				if (Bluemagic.testing)
				{
					Main.NewText("YOU LOSE! Lucky that it's not possible for the dev to defeat herself when testing.");
					blushieHealth = 0.05f;
					player.statLife = 1;
					blushieImmune = 60;
					return;
				}
				bool playSound = true;
				bool genGore = true;
				PlayerDeathReason damageSource = PlayerDeathReason.ByCustomReason(player.name + " succumbed to the might of the blushie!");
				PlayerHooks.PreKill(player, damage, 0, false, ref playSound, ref genGore, ref damageSource);
				damageSource = PlayerDeathReason.ByCustomReason(player.name + " succumbed to the might of the blushie!");
				player.lastDeathPostion = player.Center;
				player.lastDeathTime = DateTime.Now;
				player.showLastDeath = true;
				bool flag;
				int coinsOwned = (int)Utils.CoinsCount(out flag, player.inventory, new int[0]);
				player.lostCoins = coinsOwned;
				player.lostCoinString = Main.ValueToCoins(player.lostCoins);
				Main.mapFullscreen = false;
				player.trashItem.SetDefaults(0, false);
				if (player.difficulty == 0)
				{
					for (int i = 0; i < 59; i++)
					{
						Item item = player.inventory[i];
						if (item.stack > 0 && ((item.type >= 1522 && item.type <= 1527) || item.type == 3643))
						{
							int num = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, item.type, 1, false, 0, false, false);
							Main.item[num].netDefaults(item.netID);
							Main.item[num].Prefix(item.prefix);
							Main.item[num].stack = item.stack;
							Main.item[num].velocity.Y = Main.rand.Next(-20, 1) * 0.2f;
							Main.item[num].velocity.X = Main.rand.Next(-20, 21) * 0.2f;
							Main.item[num].noGrabDelay = 100;
							Main.item[num].favorited = false;
							Main.item[num].newAndShiny = false;
							if (Main.netMode == 1)
							{
								NetMessage.SendData(21, -1, -1, null, num, 0f, 0f, 0f, 0, 0, 0);
							}
							item.SetDefaults(0, false);
						}
					}
				}
				else if (player.difficulty == 1)
				{
					player.DropItems();
				}
				else if (player.difficulty == 2)
				{
					player.DropItems();
					player.KillMeForGood();
				}
				if (playSound)
				{
					Main.PlaySound(5, (int)player.position.X, (int)player.position.Y, 1, 1f, 0f);
				}
				player.headVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
				player.bodyVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
				player.legVelocity.Y = (float)Main.rand.Next(-40, -10) * 0.1f;
				player.headVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f;
				player.bodyVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f;
				player.legVelocity.X = (float)Main.rand.Next(-20, 21) * 0.1f;
				if (player.stoned || !genGore)
				{
					player.headPosition = Vector2.Zero;
					player.bodyPosition = Vector2.Zero;
					player.legPosition = Vector2.Zero;
				}
				if (genGore)
				{
					for (int j = 0; j < 100; j++)
					{
						if (player.stoned)
						{
							Dust.NewDust(player.position, player.width, player.height, 1, 0f, -2f, 0, default(Color), 1f);
						}
						else if (player.frostArmor)
						{
							int num2 = Dust.NewDust(player.position, player.width, player.height, 135, 0f, -2f, 0, default(Color), 1f);
							Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
						}
						else if (player.boneArmor)
						{
							int num3 = Dust.NewDust(player.position, player.width, player.height, 26, 0f, -2f, 0, default(Color), 1f);
							Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(player.ArmorSetDye(), player);
						}
						else
						{
							Dust.NewDust(player.position, player.width, player.height, 5, 0f, -2f, 0, default(Color), 1f);
						}
					}
				}
				player.mount.Dismount(player);
				player.dead = true;
				player.respawnTimer = 600;
				if (Main.expertMode)
				{
					player.respawnTimer = 900;
				}
				PlayerHooks.Kill(player, damage, 0, false, damageSource);
				player.immuneAlpha = 0;
				player.palladiumRegen = false;
				player.iceBarrier = false;
				player.crystalLeaf = false;
				NetworkText deathText = damageSource.GetDeathText(player.name);
				if (Main.netMode == 0)
				{
					Main.NewText(deathText.ToString(), 225, 25, 25, false);
				}
				else if (Main.netMode == 1)
				{
					NetMessage.SendPlayerDeath(player.whoAmI, damageSource, damage, 0, false, -1, -1);
				}
				if (player.difficulty == 0)
				{
					player.DropCoins();
				}
				player.DropTombstone(coinsOwned, deathText, 0);
				try
				{
					WorldGen.saveToonWhilePlaying();
				}
				catch
				{
				}
			}
		}

		public override void MeleeEffects(Item item, Rectangle hitbox)
		{
			if (item.melee && !item.noMelee && !item.noUseGraphic && customMeleeEnchant > 0)
			{
				if (customMeleeEnchant == 1)
				{
					if (Main.rand.Next(2) == 0)
					{
						int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("EtherealFlame"), player.velocity.X * 0.2f + player.direction * 3f, player.velocity.Y * 0.2f, 100, default(Color), 2.5f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
					}
				}
				else if (customMeleeEnchant == 2)
				{
					if (Main.rand.Next(2) == 0)
					{
						int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 135, player.velocity.X * 0.2f + player.direction * 3f, player.velocity.Y * 0.2f, 100, default(Color), 2.5f);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity *= 0.7f;
						Main.dust[dust].velocity.Y -= 0.5f;
					}
				}
			}
		}

		public override bool ConsumeAmmo(Item weapon, Item ammo)
		{
			if (Main.rand.NextFloat() < ammoCost)
			{
				return false;
			}
			return base.ConsumeAmmo(weapon, ammo);
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
		{
			if (suppression > 0f)
			{
				damage = (int)(damage * (1f - suppression));
			}
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if (customMeleeEnchant == 1)
			{
				target.AddBuff(mod.BuffType("EtherealFlames"), 60 * Main.rand.Next(3, 7), false);
			}
			else if (customMeleeEnchant == 2)
			{
				target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7), false);
			}
			if (puriumShieldChargeMax > 0f && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				ChargePuriumShield(damage * puriumShieldDamageEffectiveness);
			}
		}

		public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (suppression > 0f)
			{
				damage = (int)(damage * (1f - suppression));
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (proj.melee && !proj.noEnchantments)
			{
				if (customMeleeEnchant == 1)
				{
					target.AddBuff(mod.BuffType("EtherealFlames"), 60 * Main.rand.Next(3, 7), false);
				}
				else if (customMeleeEnchant == 2)
				{
					target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7), false);
				}
			}
			if (puriumShieldChargeMax > 0f && target.lifeMax > 5 && target.type != NPCID.TargetDummy)
			{
				ChargePuriumShield(damage * puriumShieldDamageEffectiveness);
			}
		}

		public override void OnHitPvp(Item item, Player target, int damage, bool crit)
		{
			if (customMeleeEnchant == 1)
			{
				target.AddBuff(mod.BuffType("EtherealFlames"), 60 * Main.rand.Next(3, 7), true);
			}
			else if (customMeleeEnchant == 2)
			{
				target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7), true);
			}
			if (puriumShieldChargeMax > 0f)
			{
				ChargePuriumShield(damage * puriumShieldDamageEffectiveness);
			}
		}

		public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
		{
			if(proj.melee && !proj.noEnchantments)
			{
				if (customMeleeEnchant == 1)
				{
					target.AddBuff(mod.BuffType("EtherealFlames"), 60 * Main.rand.Next(3, 7), true);
				}
				else if (customMeleeEnchant == 2)
				{
					target.AddBuff(BuffID.Frostburn, 60 * Main.rand.Next(3, 7), true);
				}
			}
			if (puriumShieldChargeMax > 0f)
			{
				ChargePuriumShield(damage * puriumShieldDamageEffectiveness);
			}
		}

		public override void ModifyScreenPosition()
		{
			if (BlushieBoss.BlushieBoss.Players[Main.myPlayer] && BlushieBoss.BlushieBoss.CameraFocus)
			{
				Vector2 origin = BlushieBoss.BlushieBoss.Origin;
				Main.screenPosition = origin - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
			}
		}

		public override void DrawEffects(PlayerDrawInfo drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (eFlames)
			{
				if (Main.rand.Next(4) == 0 && drawInfo.shadow == 0f)
				{
					int dust = Dust.NewDust(drawInfo.position - new Vector2(2f, 2f), player.width + 4, player.height + 4, mod.DustType("EtherealFlame"), player.velocity.X * 0.4f, player.velocity.Y * 0.4f, 100, default(Color), 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 1.8f;
					Main.dust[dust].velocity.Y -= 0.5f;
					Main.playerDrawDust.Add(dust);
				}
				r *= 0.1f;
				g *= 0.2f;
				b *= 0.7f;
				fullBright = true;
			}
		}

		public static readonly PlayerLayer MiscEffectsBack = new PlayerLayer("Bluemagic", "MiscEffectsBack", PlayerLayer.MiscEffectsBack, delegate(PlayerDrawInfo drawInfo)
			{
				if (drawInfo.shadow != 0f)
				{
					return;
				}
				Player drawPlayer = drawInfo.drawPlayer;
				if (drawPlayer.dead)
				{
					return;
				}
				Mod mod = ModLoader.GetMod("Bluemagic");
				BluemagicPlayer modPlayer = drawPlayer.GetModPlayer<BluemagicPlayer>();
				if (modPlayer.reviveTime > 0)
				{
					Texture2D texture = mod.GetTexture("PuritySpirit/Revive");
					int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
					int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 4f - 60f + modPlayer.reviveTime - Main.screenPosition.Y);
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White * (modPlayer.reviveTime / 60f), 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
					Main.playerDrawData.Add(data);
				}
			});
		public static readonly PlayerLayer MiscEffects = new PlayerLayer("Bluemagic", "MiscEffects", PlayerLayer.MiscEffectsFront, delegate(PlayerDrawInfo drawInfo)
			{
				if (drawInfo.shadow != 0f)
				{
					return;
				}
				Player drawPlayer = drawInfo.drawPlayer;
				if (drawPlayer.dead)
				{
					return;
				}
				Mod mod = ModLoader.GetMod("Bluemagic");
				BluemagicPlayer modPlayer = drawPlayer.GetModPlayer<BluemagicPlayer>();
				if (modPlayer.badHeal)
				{
					Texture2D texture = mod.GetTexture("Buffs/PuritySpirit/Skull");
					int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
					int drawY = (int)(drawInfo.position.Y - 4f - Main.screenPosition.Y);
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Lighting.GetColor((int)((drawInfo.position.X + drawPlayer.width / 2f) / 16f), (int)((drawInfo.position.Y - 4f - texture.Height / 2f) / 16f)), 0f, new Vector2(texture.Width / 2f, texture.Height), 1f, SpriteEffects.None, 0);
					Main.playerDrawData.Add(data);
					for (int k = 0; k < 2; k++)
					{
						int dust = Dust.NewDust(new Vector2(drawInfo.position.X + drawPlayer.width / 2f - texture.Width / 2f, drawInfo.position.Y - 4f - texture.Height), texture.Width, texture.Height, mod.DustType("Smoke"), 0f, 0f, 0, Color.Black);
						Main.dust[dust].velocity += drawPlayer.velocity * 0.25f;
						Main.playerDrawDust.Add(dust);
					}
				}
				if (modPlayer.puriumShieldChargeMax > 0f && modPlayer.puriumShieldCharge > 0f && !modPlayer.purityShieldMount)
				{
					Texture2D texture = mod.GetTexture("PuriumShield");
					int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
					int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y);
					float strength = (modPlayer.miscTimer % 30f) / 15f;
					if (strength > 1f)
					{
						strength = 2f - strength;
					}
					strength = 0.1f + strength * 0.2f;
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White * strength, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
					data.shader = drawInfo.bodyArmorShader;
					Main.playerDrawData.Add(data);
				}
				if (modPlayer.purityShieldMount)
				{
					Texture2D texture = mod.GetTexture("Mounts/PurityShield");
					int drawX = (int)(drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X);
					int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y);
					float strength = (modPlayer.miscTimer % 30f) / 15f;
					if (strength > 1f)
					{
						strength = 2f - strength;
					}
					strength = 0.4f + strength * 0.2f;
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White * strength, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0);
					data.shader = drawInfo.drawPlayer.miscDyes[3].dye;
					Main.playerDrawData.Add(data);
				}
				if (modPlayer.blushieHealth > 0f && drawPlayer.whoAmI == Main.myPlayer)
				{
					Texture2D texture = mod.GetTexture(BlushieBoss.BlushieBoss.CameraFocus ? "BlushieBoss/IndicatorBig" : "BlushieBoss/Indicator");
					DrawData data = new DrawData(texture, drawPlayer.Center - new Vector2(texture.Width / 2, texture.Height / 2) - Main.screenPosition, Color.White);
					Main.playerDrawData.Add(data);
				}
			});
		public static readonly PlayerLayer RedLine = new PlayerLayer("Bluemagic", "RedLine", delegate(PlayerDrawInfo drawInfo)
			{
				if (drawInfo.shadow != 0f)
				{
					return;
				}
				Player drawPlayer = drawInfo.drawPlayer;
				if (drawPlayer.whoAmI != Main.myPlayer || drawPlayer.itemAnimation > 0 || Main.gamePaused)
				{
					return;
				}
				Mod mod = ModLoader.GetMod("Bluemagic");
				if (drawPlayer.inventory[drawPlayer.selectedItem].type == mod.ItemType("CleanserBeam"))
				{
					Texture2D texture = mod.GetTexture("RedLine");
					Vector2 origin = drawPlayer.RotatedRelativePoint(drawPlayer.MountedCenter, true);
					Vector2 mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
					Vector2 unit = mousePos - origin;
					float length = unit.Length();
					unit.Normalize();
					float rotation = unit.ToRotation();
					for (float k = 16f; k <= length; k += 32f)
					{
						Vector2 drawPos = origin + unit * k - Main.screenPosition;
						Main.playerDrawData.Add(new DrawData(texture, drawPos, null, Color.White, rotation, new Vector2(2f, 8f), 1f, SpriteEffects.None, 0));
					}
				}
			});
		public static readonly PlayerLayer PreHeldItem = new PlayerLayer("Bluemagic", "PreHeldItem", PlayerLayer.HeldItem, delegate(PlayerDrawInfo drawInfo)
			{
				Mod mod = Bluemagic.Instance;
				Main.itemTexture[mod.ItemType("ChaosCrystal")] = mod.GetTexture("Items/ChaosSpirit/ChaosCrystalNoAnim");
				Main.itemTexture[mod.ItemType("CataclysmCrystal")] = mod.GetTexture("Items/ChaosSpirit/CataclysmCrystalNoAnim");
			});
		public static readonly PlayerLayer PostHeldItem = new PlayerLayer("Bluemagic", "PostHeldItem", PlayerLayer.HeldItem, delegate(PlayerDrawInfo drawInfo)
			{
				Mod mod = Bluemagic.Instance;
				Main.itemTexture[mod.ItemType("ChaosCrystal")] = mod.GetTexture("Items/ChaosSpirit/ChaosCrystal");
				Main.itemTexture[mod.ItemType("CataclysmCrystal")] = mod.GetTexture("Items/ChaosSpirit/CataclysmCrystal");
			});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			RedLine.visible = true;
			layers.Add(RedLine);
			MiscEffectsBack.visible = true;
			layers.Insert(0, MiscEffectsBack);
			MiscEffects.visible = true;
			layers.Add(MiscEffects);
			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k] == PlayerLayer.HeldItem)
				{
					layers.Insert(k, PreHeldItem);
					k++;
					layers.Insert(k + 1, PostHeldItem);
					k++;
				}
			}
		}
	}
}
