using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Bluemagic.BlushieBoss;
using Bluemagic.ChaosSpirit;
using Bluemagic.Interface;
using Bluemagic.PuritySpirit;
using Bluemagic.TerraSpirit;
using Bluemagic.Tiles;

namespace Bluemagic
{
	public class Bluemagic : Mod
	{
		public static Mod Instance;
		public static Mod Calamity;
		public static Mod Thorium;
		public static Mod Sushi;
		public const bool testing = false;

		private static Color pureColor = new Color(100, 255, 100);
		private static int pureColorStyle = 0;

		public const string captiveElementHead = "Bluemagic/Abomination/CaptiveElement_Head_Boss_";
		public const string captiveElement2Head = "Bluemagic/Abomination/CaptiveElement2_Head_Boss_";
		public static bool freezeHeroLives = false;

		public static Color PureColor
		{
			get
			{
				return pureColor;
			}
		}

		public Bluemagic()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			Instance = this;
			foreach (string checkMod in ModLoader.GetLoadedMods())
			{
				if (checkMod == "ExampleMod")
				{
					throw new Exception("ExampleMod and Bluemagic cannot be loaded at the same time");
				}
			}
			InterfaceHelper.Initialize();
			for (int k = 1; k <= 4; k++)
			{
				AddBossHeadTexture(captiveElementHead + k);
				AddBossHeadTexture(captiveElement2Head + k);
			}
			Filters.Scene["Bluemagic:PuritySpirit"] = new Filter(new PuritySpiritScreenShaderData("FilterMiniTower").UseColor(0.4f, 0.9f, 0.4f).UseOpacity(0.7f), EffectPriority.VeryHigh);
			SkyManager.Instance["Bluemagic:PuritySpirit"] = new PuritySpiritSky();
			Filters.Scene["Bluemagic:MonolithVoid"] = new Filter(new ScreenShaderData("FilterMoonLord"), EffectPriority.Medium);
			SkyManager.Instance["Bluemagic:MonolithVoid"] = new VoidSky();
			Filters.Scene["Bluemagic:ChaosSpirit"] = new Filter(new ChaosSpiritScreenShaderData("FilterMiniTower").UseColor(0.9f, 0.4f, 0.4f).UseOpacity(0.25f), EffectPriority.VeryHigh);
			SkyManager.Instance["Bluemagic:ChaosSpirit"] = new ChaosSpiritSky();
			Filters.Scene["Bluemagic:TerraSpirit"] = new Filter(new TerraSpiritScreenShaderData("FilterMiniTower").UseColor(0f, 0f, 0f).UseOpacity(0.1f), EffectPriority.VeryHigh);
			SkyManager.Instance["Bluemagic:TerraSpirit"] = new TerraSpiritSky();
			SkyManager.Instance["Bluemagic:BlushieBoss"] = new BlushieSky();

			ModTranslation text = CreateTranslation("PhantomSummon");
			text.SetDefault("You feel something cold leeching your life...");
			AddTranslation(text);
			text = CreateTranslation("ElementalUnleash");
			text.SetDefault("The elements have been unleashed!");
			AddTranslation(text);
			text = CreateTranslation("PuriumOreGen");
			text.SetDefault("Your world has been blessed with Purium!");
			AddTranslation(text);
			text = CreateTranslation("NPCTalk");
			text.SetDefault("<{0}> {1}");
			AddTranslation(text);
			text = CreateTranslation("LivesLeft");
			text.SetDefault("{0} has {1} lives left!");
			AddTranslation(text);
			text = CreateTranslation("LifeLeft");
			text.SetDefault("{0} has 1 life left!");
			AddTranslation(text);
			text = CreateTranslation("ChaosDpsCap");
			text.SetDefault("A heavy pressure protects the chaos from rapid damage.");
			AddTranslation(text);
			text = CreateTranslation("ChaosPressureStart");
			text.SetDefault("The air grows heavy with chaotic pressure");
			AddTranslation(text);
			text = CreateTranslation("ChaosPressureLight");
			text.SetDefault("A protective sphere of light has appeared!");
			AddTranslation(text);
			text = CreateTranslation("CataclysmCountdown");
			text.SetDefault("{0} seconds until the end");
			AddTranslation(text);
			text = CreateTranslation("TerraSpiritExpert");
			text.SetDefault("The Spirit of Purity is losing control!");
			AddTranslation(text);
			text = CreateTranslation("ExtraLives");
			text.SetDefault("Everyone has been granted 3 extra lives!");
			AddTranslation(text);
		}

		public override void PostSetupContent()
		{
			Mod bossList = ModLoader.GetMod("BossChecklist");
			if (bossList != null)
			{
				bossList.Call("AddBossWithInfo", "The Phantom", 12.05f, (Func<bool>)(() => BluemagicWorld.downedPhantom), string.Format("Use a [i:{0}] in the Dungeon (Plantera must be defeated)", ItemType("PaladinEmblem")));
				bossList.Call("AddBossWithInfo", "The Abomination", 12.8f, (Func<bool>)(() => BluemagicWorld.downedAbomination), string.Format("Use a [i:{0}] in the Underworld (Plantera must be defeated)", ItemType("FoulOrb")));
				bossList.Call("AddBossWithInfo", "The Abomination (Rematch)", 14.5f, (Func<bool>)(() => BluemagicWorld.elementalUnleash), string.Format("Use a [i:{0}] in the Underworld (Moon Lord must be defeated). [c/FF0000:Starts the Elemental Unleash!]", ItemType("FoulOrb")));
				bossList.Call("AddBossWithInfo", "The Spirit of Purity", 16f, (Func<bool>)(() => BluemagicWorld.downedPuritySpirit), string.Format("Kill a Bunny while the Bunny is standing in front of a placed [i:{0}]", ItemType("ElementalPurge")));
				bossList.Call("AddBossWithInfo", "The Spirit of Chaos", 18f, (Func<bool>)(() => BluemagicWorld.downedChaosSpirit), string.Format("Use a [i:{0}] anytime, anywhere (has infinite reuses)", ItemType("RitualOfEndings")));
				bossList.Call("AddBossWithInfo", "????? (Phase 1)", 42f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint1 > 0), string.Format("Use a [i:{0}] anytime, anywhere, after all previous bosses have been defeated (has infinite reuses)", ItemType("RitualOfBunnies")));
				bossList.Call("AddBossWithInfo", "????? (Phase 2)", 256f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint2 > 0), string.Format("Defeat the previous phase or use a [i:{0}]", ItemType("Checkpoint1")));
				bossList.Call("AddBossWithInfo", "????? (Phase 3)", 666f, (Func<bool>)(() => BluemagicWorld.terraCheckpoint3 > 0), string.Format("Defeat the previous phase or use a [i:{0}]", ItemType("Checkpoint2")));
				bossList.Call("AddBossWithInfo", "????? (Phase 4)", 1337f, (Func<bool>)(() => BluemagicWorld.terraCheckpointS > 0), string.Format("Defeat the previous phase or use a [i:{0}]", ItemType("Checkpoint3")));
				bossList.Call("AddBossWithInfo", "?????", 9001f, (Func<bool>)(() => BluemagicWorld.downedTerraSpirit), "Overcome all phases and defeat the boss once and for all!");
				bossList.Call("AddBossWithInfo", "blushiemagic (Phases 1-2)", float.MaxValue - 1e37f, (Func<bool>)(() => BluemagicWorld.downedBlushiePhase2), "Please don't do this");
				bossList.Call("AddBossWithInfo", "blushiemagic", float.MaxValue, (Func<bool>)(() => BluemagicWorld.downedBlushie), "Why");
				bossList.Call("AddBossWithInfo", "blushiemagic (no hit)", float.PositiveInfinity, (Func<bool>)(() => BluemagicWorld.noHitBlushie), "You must be a god");
			}
			Calamity = ModLoader.GetMod("CalamityMod");
			Thorium = ModLoader.GetMod("ThoriumMod");
			Sushi = ModLoader.GetMod("imkSushisMod");
			BlushieBoss.BlushieBoss.Load();
		}

		public override void Unload()
		{
			Instance = null;
			Calamity = null;
			Thorium = null;
			Sushi = null;
		}

		public override void AddRecipes()
		{
			BluemagicRecipes.AddRecipes(this);
		}

		public override object Call(object[] args)
		{
			if (args.Length == 0)
			{
				return null;
			}
			if (args[0] == "Set")
			{
				if (args.Length < 4)
				{
					return null;
				}
				if (args[1] is Player && args[2] is string)
				{
					return CallPlayerSet((Player)args[1], (string)args[2], args[3]);
				}
				return null;
			}
			if (args[0] == "Get")
			{
				if (args.Length < 2)
				{
					return null;
				}
				if (args[1] is string)
				{
					return CallGet((string)args[1]);
				}
				return null;
			}
			return null;
		}

		private object CallPlayerSet(Player player, string command, object arg)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			if (command == "puriumShieldChargeMax" && arg is float)
			{
				modPlayer.puriumShieldChargeMax = (float)arg;
				return arg;
			}
			if (command == "puriumShieldChargeRate" && arg is float)
			{
				modPlayer.puriumShieldChargeRate = (float)arg;
				return arg;
			}
			if (command == "puriumShieldEnduranceMult" && arg is float)
			{
				modPlayer.puriumShieldEnduranceMult = (float)arg;
				return arg;
			}
			if (command == "manaMagnet2" && arg is bool)
			{
				modPlayer.manaMagnet2 = (bool)arg;
				return arg;
			}
			if (command == "crystalCloak" && arg is bool)
			{
				modPlayer.crystalCloak = (bool)arg;
				return arg;
			}
			if (command == "lifeMagnet2" && arg is bool)
			{
				modPlayer.lifeMagnet2 = (bool)arg;
				return arg;
			}
			if (command == "noGodmode" && arg is bool)
			{
				modPlayer.noGodmode = (bool)arg;
			}
			return null;
		}

		private object CallGet(string command)
		{
			if (command == "downedPhantom")
			{
				return BluemagicWorld.downedPhantom;
			}
			if (command == "downedAbomination")
			{
				return BluemagicWorld.downedAbomination;
			}
			if (command == "elementalUnleash")
			{
				return BluemagicWorld.elementalUnleash;
			}
			if (command == "downedPuritySpirit")
			{
				return BluemagicWorld.downedPuritySpirit;
			}
			if (command == "downedChaosSpirit")
			{
				return BluemagicWorld.downedChaosSpirit;
			}
			if (command == "downedTerraSpirit")
			{
				return BluemagicWorld.downedTerraSpirit;
			}
			return null;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType type = (MessageType)reader.ReadByte();
			if (type == MessageType.PuritySpirit)
			{
				PuritySpirit.PuritySpirit spirit = Main.npc[reader.ReadInt32()].modNPC as PuritySpirit.PuritySpirit;
				if (spirit != null && spirit.npc.active)
				{
					spirit.HandlePacket(reader);
				}
			}
			else if (type == MessageType.HeroLives)
			{
				Player player = Main.player[reader.ReadInt32()];
				int lives = reader.ReadInt32();
				player.GetModPlayer<BluemagicPlayer>(this).heroLives = lives;
				if (lives > 0)
				{
					NetworkText text;
					if (lives == 1)
					{
						text = NetworkText.FromKey("Mods.Bluemagic.LifeLeft", player.name);
					}
					else
					{
						text = NetworkText.FromKey("Mods.Bluemagic.LivesLeft", player.name, lives);
					}
					NetMessage.BroadcastChatMessage(text, new Color(255, 25, 25));
				}
			}
			else if (type == MessageType.ChaosSpirit)
			{
				NPC npc = Main.npc[reader.ReadInt32()];
				if (npc.active)
				{
					ChaosSpirit.ChaosSpirit spirit = npc.modNPC as ChaosSpirit.ChaosSpirit;
					if (spirit != null)
					{
						spirit.HandlePacket(reader);
					}
					ChaosSpirit2 spirit2 = npc.modNPC as ChaosSpirit2;
					if (spirit2 != null)
					{
						spirit2.HandlePacket(reader);
					}
					ChaosSpirit3 spirit3 = npc.modNPC as ChaosSpirit3;
					if (spirit3 != null)
					{
						spirit3.HandlePacket(reader);
					}
				}
			}
			else if (type == MessageType.PushChaosArm)
			{
				NPC npc = Main.npc[reader.ReadInt32()];
				Vector2 push = new Vector2(reader.ReadSingle(), reader.ReadSingle());
				if (npc.active)
				{
					ChaosSpiritArm arm = npc.modNPC as ChaosSpiritArm;
					if (arm != null)
					{
						arm.offset += push;
						if (Main.netMode == 2)
						{
							ModPacket packet = GetPacket();
							packet.Write((byte)MessageType.PushChaosArm);
							packet.Write(push.X);
							packet.Write(push.Y);
							packet.Send(-1, whoAmI);
						}
					}
				}
			}
			else if (type == MessageType.TerraSpirit)
			{
				NPC npc = Main.npc[reader.ReadInt32()];
				if (npc.active)
				{
					TerraSpirit.TerraSpirit spirit = npc.modNPC as TerraSpirit.TerraSpirit;
					if (spirit != null)
					{
						spirit.HandlePacket(reader);
					}
				}
			}
			else if (type == MessageType.TerraLives)
			{
				Player player = Main.player[reader.ReadInt32()];
				int lives = reader.ReadInt32();
				player.GetModPlayer<BluemagicPlayer>().terraLives = lives;
				if (lives > 0)
				{
					NetworkText text;
					if (lives == 1)
					{
						text = NetworkText.FromKey("Mods.Bluemagic.LifeLeft", player.name);
					}
					else
					{
						text = NetworkText.FromKey("Mods.Bluemagic.LivesLeft", player.name, lives);
					}
					NetMessage.BroadcastChatMessage(text, new Color(255, 25, 25));
				}
			}
			else if (type == MessageType.GoldBlob)
			{
				NPC npc = Main.npc[reader.ReadByte()];
				float value = reader.ReadByte();
				if (npc.active && npc.type == NPCType("GoldBlob"))
				{
					npc.localAI[0] = value;
				}
			}
			else if (type == MessageType.ExtraLives)
			{
				BluemagicPlayer player = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>();
				if (player.terraLives > 0)
				{
					player.terraLives += 3;
				}
			}
			else if (type == MessageType.BulletNegative)
			{
				NPC npc = Main.npc[reader.ReadByte()];
				if (npc.active && npc.type == NPCType("TerraSpirit2") && npc.modNPC is TerraSpirit2)
				{
					var bullets = ((TerraSpirit2)npc.modNPC).bullets;
					int count = reader.ReadByte();
					for (int k = 0; k < count; k++)
					{
						bullets.Add(new BulletNegative(reader.ReadVector2(), reader.ReadVector2()));
					}
				}
			}
			else if (type == MessageType.CustomStats)
			{
				byte byte1 = reader.ReadByte();
				byte byte2 = reader.ReadByte();
				Player player = Main.player[byte1];
				BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
				CustomStats stats = byte2 == 0 ? modPlayer.chaosStats : modPlayer.cataclysmStats;
				stats.NetReceive(reader);
				if (Main.netMode == 2)
				{
					ModPacket packet = GetPacket(512);
					packet.Write(byte1);
					packet.Write(byte2);
					stats.NetSend(packet);
					packet.Send(-1, whoAmI);
				}
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			InterfaceHelper.ModifyInterfaceLayers(layers);
		}

		public static void UpdatePureColor()
		{
			pureColor.R = (byte)(255 - 155f * Math.Abs(Math.Cos(pureColorStyle * Math.PI / 200.0)));
			pureColor.B = pureColor.R;
			pureColorStyle = (pureColorStyle + 1) % 200;
		}

		public static void NewText(string key, int r, int g, int b)
		{
			if (Main.netMode == 0)
			{
				Main.NewText(Language.GetTextValue(key), (byte)r, (byte)g, (byte)b);
			}
			else if (Main.netMode == 2)
			{
				NetworkText text = NetworkText.FromKey(key);
				NetMessage.BroadcastChatMessage(text, new Color(r, g, b));
			}
		}

		public static void NewText(string key, Color color)
		{
			NewText(key, color.R, color.G, color.B);
		}
	}

	enum MessageType : byte
	{
		PuritySpirit,
		HeroLives,
		ChaosSpirit,
		PushChaosArm,
		TerraSpirit,
		TerraLives,
		GoldBlob,
		ExtraLives,
		BulletNegative,
		CustomStats
	}
}