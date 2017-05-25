using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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
		}

		public override void UpdateDead()
		{
			eFlames = false;
			badHeal = false;
			puriumShieldCharge = 0f;
			reviveTime = 0;
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["version"] = 0;
			tag["puriumShieldCharge"] = puriumShieldCharge;
			tag["chaosStats"] = chaosStats.Save();
			tag["cataclysmStats"] = cataclysmStats.Save();
			return tag;
		}

		public override void Load(TagCompound tag)
		{
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

		private bool AnyChaosSpirit()
		{
			return NPC.AnyNPCs(mod.NPCType("ChaosSpirit")) || NPC.AnyNPCs(mod.NPCType("ChaosSpirit2")) || NPC.AnyNPCs(mod.NPCType("ChaosSpirit3"));
		}

		private bool IsChaosSpirit(int type)
		{
			return type == mod.NPCType("ChaosSpirit") || type == mod.NPCType("ChaosSpirit2") || type == mod.NPCType("ChaosSpirit3");
		}

		public override void UpdateBiomeVisuals()
		{
			bool useChaos = AnyChaosSpirit();
			player.ManageSpecialBiomeVisuals("Bluemagic:ChaosSpirit", useChaos);
			bool usePurity = !useChaos && NPC.AnyNPCs(mod.NPCType("PuritySpirit"));
			player.ManageSpecialBiomeVisuals("Bluemagic:PuritySpirit", usePurity);
			bool useVoidMonolith = voidMonolith && !useChaos && !usePurity && !NPC.AnyNPCs(NPCID.MoonLordCore);
			player.ManageSpecialBiomeVisuals("Bluemagic:MonolithVoid", useVoidMonolith, player.Center);
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
					player.KillMe(PlayerDeathReason.ByCustomReason(" was dissolved by holy powers"), hurt, 0, false);
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
				NetMessage.SendData(65, -1, -1, "", 0, player.whoAmI, newPosition.X, newPosition.Y, 1, 0, 0);
				PuritySpiritDebuff();
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
				player.KillMe(PlayerDeathReason.ByCustomReason(" was crushed by chaotic pressure!"), damage, 0, false);
			}
		}

		public override void PostUpdateBuffs()
		{
			if (nullified)
			{
				Nullify();
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
				for (int k = 0; k < Player.maxBuffs; k++)
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

		public override void PostUpdate()
		{
			StartBadHeal();
			miscTimer++;
			miscTimer %= 60;
			if (purityShieldMount)
			{
				player.hairFrame.Y = 5 * player.hairFrame.Height;
				player.headFrame.Y = 5 * player.headFrame.Height;
				player.legFrame.Y = 5 * player.legFrame.Height;
			}
		}

		public override void FrameEffects()
		{
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
			if (puriumShieldChargeMax > 0f && puriumShieldCharge >= reviveCost)
			{
				puriumShieldCharge -= reviveCost;
				player.statLife = 1;
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
			if (heroLives > 0)
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
				if (heroLives > 0)
				{
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
			}
			if (damage == 10.0 && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
			{
				if (healHurt > 0)
				{
					damageSource = PlayerDeathReason.ByCustomReason(" was dissolved by holy powers");
				}
				else if (chaosPressure > 0)
				{
					damageSource = PlayerDeathReason.ByCustomReason(" was crushed by chaotic pressure");
				}
			}
			return true;
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
			}
			if (puriumShieldChargeMax > 0f)
			{
				ChargePuriumShield(damage * puriumShieldDamageEffectiveness);
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
				Mod mod = ModLoader.GetMod("Bluemagic");
				BluemagicPlayer modPlayer = drawPlayer.GetModPlayer<BluemagicPlayer>(mod);
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
				Mod mod = ModLoader.GetMod("Bluemagic");
				BluemagicPlayer modPlayer = drawPlayer.GetModPlayer<BluemagicPlayer>(mod);
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
