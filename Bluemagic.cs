using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic.ChaosSpirit;
using Bluemagic.PuritySpirit;
using Bluemagic.Tiles;

namespace Bluemagic
{
	public class Bluemagic : Mod
	{
		public const string captiveElementHead = "Bluemagic/Abomination/CaptiveElement_Head_Boss_";
		public const string captiveElement2Head = "Bluemagic/Abomination/CaptiveElement2_Head_Boss_";
		public static bool freezeHeroLives = false;

		public Bluemagic()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true
			};
		}

		public override void Load()
		{
			foreach (string checkMod in ModLoader.GetLoadedMods())
			{
				if (checkMod == "ExampleMod")
				{
					throw new Exception("ExampleMod and Bluemagic cannot be loaded at the same time");
				}
			}
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
		}

		public override void PostSetupContent()
		{
			Mod bossList = ModLoader.GetMod("BossChecklist");
			if (bossList != null)
			{
				bossList.Call("AddBoss", "The Abomination", 12.5f, (Func<bool>)(() => BluemagicWorld.downedAbomination));
				bossList.Call("AddBoss", "The Spirit of Purity", 16f, (Func<bool>)(() => BluemagicWorld.downedPuritySpirit));
				bossList.Call("AddBoss", "The Spirit of Chaos", 18f, (Func<bool>)(() => BluemagicWorld.downedChaosSpirit));
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
					string text = player.name + " has " + lives;
					if (lives == 1)
					{
						text += " life left!";
					}
					else
					{
						text += " lives left!";
					}
					NetMessage.SendData(25, -1, -1, text, 255, 255, 25, 25);
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
		}

		public override void ModifyInterfaceLayers(List<MethodSequenceListItem> layers)
		{
			
		}
	}

	enum MessageType : byte
	{
		PuritySpirit,
		HeroLives,
		ChaosSpirit,
		PushChaosArm
	}
}