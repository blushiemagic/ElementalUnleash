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
		public const bool testing = true;

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

			ModTranslation text = CreateTranslation("PhantomSummon");
			text.SetDefault("You feel something cold leeching your life...");
			AddTranslation(text);
			text = CreateTranslation("ElementalUnleash");
			text.SetDefault("The elements have been unleashed!");
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
		}

		public override void PostSetupContent()
		{
			Mod bossList = ModLoader.GetMod("BossChecklist");
			if (bossList != null)
			{
				bossList.Call("AddBossWithInfo", "The Phantom", 12.05f, (Func<bool>)(() => BluemagicWorld.downedPhantom), string.Format("Use a [i:{0}] in the Dungeon (Plantera must be defeated)", ItemType("PaladinEmblem")));
				bossList.Call("AddBossWithInfo", "The Abomination", 12.8f, (Func<bool>)(() => BluemagicWorld.downedAbomination), string.Format("Use a [i:{0}] in the Underworld (Plantera must be defeated)", ItemType("FoulOrb")));
				bossList.Call("AddBossWithInfo", "The Abomination (Rematch)", 14.5f, (Func<bool>)(() => BluemagicWorld.downedAbomination2 > 0), string.Format("Use a [i:{0}] in the Underworld (Moon Lord must be defeated). [c/FF0000:Starts the Elemental Unleash!]", ItemType("FoulOrb")));
				bossList.Call("AddBossWithInfo", "The Spirit of Purity", 16f, (Func<bool>)(() => BluemagicWorld.downedPuritySpirit), string.Format("Kill a Bunny while the Bunny is standing in front of a placed [i:{0}]", ItemType("ElementalPurge")));
				bossList.Call("AddBossWithInfo", "The Spirit of Chaos", 18f, (Func<bool>)(() => BluemagicWorld.downedChaosSpirit), string.Format("Use a [i:{0}] anytime, anywhere (has infinite reuses)", ItemType("RitualOfEndings")));
			}
		}

		public override void AddRecipes()
		{
			BluemagicRecipes.AddRecipes(this);
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
	}

	enum MessageType : byte
	{
		PuritySpirit,
		HeroLives,
		ChaosSpirit,
		PushChaosArm,
		TerraSpirit,
		TerraLives
	}
}