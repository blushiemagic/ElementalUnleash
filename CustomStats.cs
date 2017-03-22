using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic
{
	public class CustomStats
	{
		public int Points = 0;
		public List<CustomStat> Stats = new List<CustomStat>();

		public int UsedPoints
		{
			get
			{
				int total = 0;
				foreach (CustomStat stat in Stats)
				{
					total += stat.Points;
				}
				return total;
			}
		}

		public int MaxPoints
		{
			get
			{
				return CustomStat.MaxPoints * Stats.Count;
			}
		}
	}

	public class CustomStat
	{
		public const int MaxPoints = 5;

		public int Points = 0;
		private Action<Player, int> effect;
		public readonly string Tooltip;

		public CustomStat(Action<Player, int> effect, string tooltip)
		{
			this.effect = effect;
			this.Tooltip = tooltip;
		}

		public void Apply(Player player)
		{
			effect(player, Points);
		}

		public bool CanUpgrade()
		{
			return Points < MaxPoints;
		}
	}

	public static class CustomStatEffects
	{
		public static void MeleeDamage(Player player, int points)
		{
			player.meleeDamage += 0.04f * points;
		}

		public const string MeleeDamageTip = "+ 4% melee damage per level";

		public static void RangedDamage(Player player, int points)
		{
			player.rangedDamage += 0.04f * points;
		}

		public const string RangedDamageTip = "+ 4% ranged damage per level";

		public static void MagicDamage(Player player, int points)
		{
			player.magicDamage += 0.04f * points;
		}

		public const string MagicDamageTip = "+ 4% magic damage per level";

		public static void MinionDamage(Player player, int points)
		{
			player.minionDamage += 0.04f * points;
		}

		public const string MinionDamageTip = "+ 4% minion damage per level";

		public static void ThrownDamage(Player player, int points)
		{
			player.thrownDamage += 0.04f * points;
		}

		public const string ThrownDamageTip = "+ 4% throwing damage per level";

		public static void MeleeCrit(Player player, int points)
		{
			player.meleeCrit += 4 * points;
		}

		public const string MeleeCritTip = "+ 4% melee crit chance per level";

		public static void RangedCrit(Player player, int points)
		{
			player.rangedCrit += 24 * points;
		}

		public const string RangedCritTip = "+ 4% ranged crit chance per level";

		public static void MagicCrit(Player player, int points)
		{
			player.magicCrit += 4 * points;
		}

		public const string MagicCritTip = "+ 4% magic crit chance per level";

		public static void MaxMinions(Player player, int points)
		{
			if (points >= 1)
			{
				player.maxMinions++;
			}
			if (points >= 3)
			{
				player.maxMinions++;
			}
			if (points >= 5)
			{
				player.maxMinions++;
			}
		}

		public const string MaxMinionsTip = "level 1: + 1 max minions\nlevel 3: + 2 max minions\nlevel 5: + 3 max minions";

		public static void ThrownCrit(Player player, int points)
		{
			player.thrownCrit += 4 * points;
		}

		public const string ThrownCritTip = "+ 4% throwing crit chance per level";

		public static void MeleeSpeed(Player player, int points)
		{
			player.meleeSpeed += 0.04f * points;
		}

		public const string MeleeSpeedTip = "+ 4% melee speed per level";

		public static void AmmoCost(Player player, int points)
		{
			Mod mod = ModLoader.GetMod("Bluemagic");
			player.GetModPlayer<BluemagicPlayer>(mod).ammoCost += 0.08f * points;
		}

		public const string AmmoCostTip = "+ 8% chance not to consume ammo per level";
	}
}