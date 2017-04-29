using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.UI.Gamepad;

namespace Bluemagic.NPCs
{
	public class HardmodeGuide : ModNPC
	{
		private static int helpText;

		public override bool Autoload(ref string name, ref string texture, ref string[] altTextures)
		{
			name = "Hardmode Guide";
			altTextures = new string[] { texture + "_Alt_1" };
			return mod.Properties.Autoload;
		}

		public override void SetDefaults()
		{
			npc.name = "Hardmode Guide";
			npc.townNPC = true;
			npc.friendly = true;
			npc.width = 18;
			npc.height = 40;
			npc.aiStyle = 7;
			npc.damage = 10;
			npc.defense = 15;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			Main.npcFrameCount[npc.type] = 25;
			NPCID.Sets.ExtraFramesCount[npc.type] = 9;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
			NPCID.Sets.ExtraTextureCount[npc.type] = 1;
			animationType = NPCID.Guide;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int num = npc.life > 0 ? 1 : 5;
			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("Sparkle"));
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return Main.hardMode;
		}

		public override string TownNPCName()
		{
			switch (WorldGen.genRand.Next(5))
			{
				case 0:
					return "Blocky";
				case 1:
					return "Polygon";
				case 2:
					return "Steve";
				case 3:
					return "Alex";
				default:
					return "Nathan";
			}
		}

		public override string GetChat()
		{
			int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
			if (partyGirl >= 0 && Main.rand.Next(4) == 0)
			{
				return "Can you please tell " + Main.npc[partyGirl].displayName + " to stop decorating my house with colors?";
			}
			switch (Main.rand.Next(3))
			{
				case 0:
					return "Sometimes I feel like I'm different from everyone else here.";
				case 1:
					return "What's your favorite color? My favorite colors are white and black.";
				default:
					return "What? I don't have any arms or legs? Oh, don't be ridiculous!";
			}
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Lang.inter[51];
			button2 = Lang.inter[25];
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				Main.PlaySound(12, -1, -1, 1);
				Main.npcChatText = NextHelpText();
			}
			else
			{
				Main.playerInventory = true;
				Main.npcChatText = "";
				Main.PlaySound(12, -1, -1, 1);
				Main.InGuideCraftMenu = true;
				UILinkPointNavigator.GoToDefaultPage(0);
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 30;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = mod.ProjectileType("SparklingBall");
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}

		private string NextHelpText()
		{
			string text = GetHelpText();
			helpText++;
			helpText %= 35;
			return text;
		}

		private string GetHelpText()
		{
			string dark = "";
			if (!WorldGen.crimson)
			{
				dark = "corruption";
			}
			else
			{
				dark = "crimson";
			}
			while (true)
			{
				switch (helpText)
				{
				case 0:
					if (WorldGen.altarCount > 0)
					{
						goto case -1;
					}
					string altar = "";
					if (!WorldGen.crimson)
					{
						altar = "demon";
					}
					else
					{
						altar = "crimson";
					}
					return "If you smash a " + altar + " altar with your Pwnhammer, new treasures will appear underground... everything comes at a cost, however.";
				case 1:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					return "You can gather souls from monsters underground amidst darkness or light. You can also find more high in the skies. Souls make great crafting materials.";
				case 2:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					return "You better get the best gear as soon as you can; it is said powerful machines may attack at night.";
				case 3:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					return "The machines will stop attacking when you defeat them; if you don't want to wait for them, you can summon them with items made from iron and souls.";
				case 4:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					string anvil = "a Mythril or Orichalcum Anvil";
					if (WorldGen.oreTier2 == 108)
					{
						anvil = "a Mythril Anvil";
					}
					if (WorldGen.oreTier2 == 222)
					{
						anvil = "an Orichalcum Anvil";
					}
					return "You will need " + anvil + " in order to craft powerful equipment and rare items.";
				case 5:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					string forge = "an Adamantite or Titanium Forge";
					if (WorldGen.oreTier3 == 111)
					{
						forge = "an Adamantite Forge";
					}
					if (WorldGen.oreTier3 == 223)
					{
						forge = "a Titanium Forge";
					}
					return "You will need " + forge + " in order to forge the best ores into bars. Keep in mind you will need a Hellforge and rare ores to craft " + forge + ".";
				case 6:
					if (NPC.savedWizard)
					{
						goto case -1;
					}
					return "I've heard that a wizard is lost underground; he may join us if you rescue him.";
				case 7:
					if (NPC.downedMechBossAny)
					{
						goto case -1;
					}
					return "Defeating one of the mechanical giants may get the attention of the Steampunker.";
				case 8:
					if (WorldGen.altarCount <= 0 || NPC.downedPirates)
					{
						goto case -1;
					}
					return "There's been reports of a band of pirates nearby; be careful.";
				case 9:
					if (NPC.AnyNPCs(160))
					{
						goto case -1;
					}
					return "If you create a mushroom biome aboveground and build a house there, a living mushroom might arrive!... I'm not joking!";
				case 10:
					if (!NPC.downedMechBossAny || BluemagicWorld.eclipsePassed)
					{
						goto case -1;
					}
					return "If a solar eclipse happens, it is your chance to collect rare and powerful equipment from the unique monsters.";
				case 11:
					if (Main.LocalPlayer.statLifeMax > 400)
					{
						goto case -1;
					}
					return "You can find plants called Life Fruits growing rarely in the jungle. They can increase your health even more!";
				case 12:
					if (!NPC.downedMechBossAny || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3))
					{
						goto case -1;
					}
					return "There are a total of three great machines that must be defeated.";
				case 13:
					if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3 || NPC.downedPlantBoss)
					{
						goto case -1;
					}
					return "In the jungle, sometimes you will see a pink bulb growing. If you break it, a terrible monster will become enraged at you.";
				case 14:
					if (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3 || NPC.downedPlantBoss)
					{
						goto case -1;
					}
					return "You may have noticed some green ore called chlorophyte growing in the jungle. You will need to craft a pickaxe from the three giant machines in order to mine it.";
				case 15:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "Chlorophyte is like no other ore. It can grow and spread to nearby mud as long as it is underground and there is not too much of it around.";
				case 16:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "Once Plantera is defeated, you can use special keys to unlock chests in the dungeon. These chests contain unique weapons.";
				case 17:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "Biome keys are dropped very rarely by monsters in the ice, jungle, " + dark + ", and hallow. If you are really lucky, you may have already found one.";
				case 18:
					if (!NPC.downedPlantBoss || NPC.downedGolemBoss)
					{
						goto case -1;
					}
					return "You can use the temple key dropped by Plantera to open the temple in the underground jungle. This temple is inhabited by monsters called Lizahrds.";
				case 19:
					if (!NPC.downedPlantBoss || NPC.downedGolemBoss)
					{
						goto case -1;
					}
					return "In the jungle temple, you can use a Power Cell on the altar in the end room to summon a powerful golem.";
				case 20:
					if (!NPC.downedGolemBoss || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "You can use the beetle husks dropped by Golem to craft some powerful melee armor.";
				case 21:
					if (!NPC.downedPlantBoss || !NPC.AnyNPCs(160) || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "You can use the autohammer sold by " + NPC.firstNPCName(160) + " to craft shroomite bars. These are materials for some powerful ranged armor.";
				case 22:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedPhantom)
					{
						goto case -1;
					}
					return "You hear the screams echoing from the dungeon? There are some very powerful monsters there now, but they will also give very powerful weapons!";
				case 23:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "Sometimes monsters in the dungeon will drop ectoplasm, which can be used to craft some powerful magic armor.";
				case 24:
					if (!NPC.downedPlantBoss || BluemagicWorld.pumpkinMoonPassed || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "You can use pumpkins from the dryad and ectoplasm to craft a pumpkin moon medallion. When used during the night, it will summon extremely powerful monsters that drop very powerful equipment.";
				case 25:
					if (!NPC.downedPlantBoss || BluemagicWorld.pumpkinMoonPassed)
					{
						goto case -1;
					}
					return "Monsters from the pumpkin moon drop more loot the more waves you have completed. Can you reach the final wave?";
				case 26:
					if (!NPC.downedPlantBoss || BluemagicWorld.snowMoonPassed || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "You can use silk and ectoplasm to craft a naughty present. When used during the night, it will summon extremely powerful monsters that drop extremely powerful equipment.";
				case 27:
					if (!NPC.downedPlantBoss || BluemagicWorld.snowMoonPassed)
					{
						goto case -1;
					}
					return "Monsters from the frost moon drop more loot the more waves you have completed. Can you reach wave 15, or maybe even further?";
				case 28:
					if (!NPC.downedPlantBoss || NPC.downedFishron)
					{
						goto case -1;
					}
					return "You may sometimes find a truffle worm in an underground glowing mushroom area. Catch it with a bug net, but if you're too slow, it will run away!";
				case 29:
					if (!NPC.downedPlantBoss || NPC.downedFishron)
					{
						goto case -1;
					}
					return "If you go fishing with the truffle worm in the ocean, you will catch a very powerful monster.";
				case 30:
					if (!NPC.downedPlantBoss || BluemagicWorld.downedPhantom)
					{
						goto case -1;
					}
					return "You can use ectoplasm to craft an special emblem. When used in the dungeon, it will awaken a very powerful spirit.";
				case 31:
					if (!BluemagicWorld.pumpkinMoonPassed || !BluemagicWorld.snowMoonPassed || !NPC.downedFishron || !BluemagicWorld.downedPhantom || BluemagicWorld.downedAbomination)
					{
						goto case -1;
					}
					return "You can mix together items from powerful monsters of many elements to create something that can summon a hideous beast in the underworld.";
				case 32:
					if (!BluemagicWorld.downedAbomination || !NPC.downedMoonlord || BluemagicWorld.downedAbomination2 > 0)
					{
						goto case -1;
					}
					return "Now that the Moon Lord has been defeated, the elements are upset, and I feel that the Abomination has grown stronger.";
				case 33:
					int count = 0;
					for (int k = 0; k < 200; k++)
					{
						if (Main.npc[k].active && Main.npc[k].townNPC && Main.npc[k].type != 142)
						{
							count++;
						}
					}
					if (count >= 23)
					{
						goto case -1;
					}
					return "I feel that there are still some NPCs that may want to join us. Either build more houses or follow my prior advice.";
				default:
					if (!NPC.downedMechBossAny)
					{
						goto case -1;
					}
					return "You can use the clentaminator and green solution sold by the Steampunker to rid Terraria of the " + dark + ".";
				case -1:
					helpText++;
					helpText %= 35;
					break;
				}
			}
		}
	}
}