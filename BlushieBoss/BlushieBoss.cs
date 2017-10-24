using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Bluemagic.BlushieBoss
{
	public static class BlushieBoss
	{
		public const float arenaSize = 800;
		internal static int Phase = 0;
		internal static int Timer = 0;
		internal static List<Bullet> bullets = new List<Bullet>();
		internal static Vector2 Origin;
		internal static bool[] Players = new bool[256];

		internal static void Update()
		{
			if (Phase > 0)
			{
				Timer++;
			}
		}

		internal static void Initialize(NPC npc)
		{
			Phase = 1;
			Timer = 0;
			Origin = npc.Center;
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				Players[k] = player.active && !player.dead && player.position.X >= Origin.X - arenaSize && player.position.X + player.width <= Origin.X + arenaSize && player.position.Y >= Origin.Y - arenaSize && player.position.Y + player.height <= Origin.Y + arenaSize;
			}
		}

		internal static void Reset()
		{
			Phase = 0;
			Timer = 0;
			for (int k = 0; k < 255; k++)
			{
				Players[k] = false;
			}
		}
	}
}