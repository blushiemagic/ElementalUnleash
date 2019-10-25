using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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

		public void Update(Player player)
		{
			foreach (CustomStat stat in Stats)
			{
				stat.Apply(player);
			}
		}

		public bool CanUpgrade()
		{
			return Points + UsedPoints < MaxPoints;
		}

		public TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["Points"] = Points;
			List<TagCompound> tagList = new List<TagCompound>();
			foreach (CustomStat stat in Stats)
			{
				TagCompound tagStat = new TagCompound();
				tagStat["Name"] = stat.Name;
				tagStat["Points"] = stat.Points;
				tagStat["Inactive"] = stat.Inactive;
				tagList.Add(tagStat);
			}
			tag["Stats"] = tagList;
			return tag;
		}

		public void Load(TagCompound tag)
		{
			this.Points = tag.GetInt("Points");
			foreach (TagCompound tagStat in tag.GetList<TagCompound>("Stats"))
			{
				string name = tagStat.GetString("Name");
				int points = tagStat.GetInt("Points");
				bool inactive = tagStat.GetBool("Inactive");
				foreach (CustomStat stat in Stats)
				{
					if (stat.Name == name)
					{
						stat.Points = points;
						stat.Inactive = inactive;
					}
				}
			}
		}

		public void NetSend(BinaryWriter writer)
		{
			writer.Write(Points);
			writer.Write((short)Stats.Count);
			foreach (CustomStat stat in Stats)
			{
				writer.Write(stat.Name);
				writer.Write((byte)stat.Points);
				writer.Write(stat.Inactive);
			}
		}

		public void NetReceive(BinaryReader reader)
		{
			Points = reader.ReadInt32();
			int count = reader.ReadInt16();
			for (int k = 0; k < count; k++)
			{
				string name = reader.ReadString();
				int points = reader.ReadByte();
				bool inactive = reader.ReadBoolean();
				foreach (CustomStat stat in Stats)
				{
					if (stat.Name == name)
					{
						stat.Points = points;
						stat.Inactive = inactive;
					}
				}
			}
		}

		public CustomStats Clone()
		{
			CustomStats clone = new CustomStats();
			clone.Points = Points;
			clone.Stats = new List<CustomStat>(Stats.Select(stat => stat.Clone()));
			return clone;
		}

		public override bool Equals(object other)
		{
			CustomStats stats = other as CustomStats;
			if (stats == null || Points != stats.Points || Stats.Count != stats.Stats.Count)
			{
				return false;
			}
			for (int k = 0; k < Stats.Count; k++)
			{
				if (!Stats[k].Equals(stats.Stats[k]))
				{
					return false;
				}
			}
			return true;
		}

		public static CustomStats CreateChaosStats()
		{
			CustomStats stats = new CustomStats();
			stats.Stats.Add(new CustomStat("Melee Damage", CustomStatEffects.MeleeDamage, CustomStatEffects.MeleeDamageTip));
			stats.Stats.Add(new CustomStat("Ranged Damage", CustomStatEffects.RangedDamage, CustomStatEffects.RangedDamageTip));
			stats.Stats.Add(new CustomStat("Magic Damage", CustomStatEffects.MagicDamage, CustomStatEffects.MagicDamageTip));
			stats.Stats.Add(new CustomStat("Minion Damage", CustomStatEffects.MinionDamage, CustomStatEffects.MinionDamageTip));
			stats.Stats.Add(new CustomStat("Thrown Damage", CustomStatEffects.ThrownDamage, CustomStatEffects.ThrownDamageTip));
			stats.Stats.Add(new CustomStat("Melee Crit", CustomStatEffects.MeleeCrit, CustomStatEffects.MeleeCritTip));
			stats.Stats.Add(new CustomStat("Ranged Crit", CustomStatEffects.RangedCrit, CustomStatEffects.MeleeCritTip));
			stats.Stats.Add(new CustomStat("Magic Crit", CustomStatEffects.MagicCrit, CustomStatEffects.MagicCritTip));
			stats.Stats.Add(new CustomStat("Max Minions", CustomStatEffects.MaxMinions, CustomStatEffects.MaxMinionsTip));
			stats.Stats.Add(new CustomStat("Thrown Crit", CustomStatEffects.ThrownCrit, CustomStatEffects.ThrownCritTip));
			stats.Stats.Add(new CustomStat("Melee Speed", CustomStatEffects.MeleeSpeed, CustomStatEffects.MeleeSpeedTip));
			stats.Stats.Add(new CustomStat("Ammo Consumption", CustomStatEffects.AmmoCost, CustomStatEffects.AmmoCostTip));
			stats.Stats.Add(new CustomStat("Mana Consumption", CustomStatEffects.ManaCost, CustomStatEffects.ManaCostTip));
			stats.Stats.Add(new CustomStat("Minion Knockback", CustomStatEffects.MinionKB, CustomStatEffects.MinionKBTip));
			stats.Stats.Add(new CustomStat("Throwing Consumption", CustomStatEffects.ThrownCost, CustomStatEffects.ThrownCostTip));
			stats.Stats.Add(new CustomStat("Defense", CustomStatEffects.Defense, CustomStatEffects.DefenseTip));
			stats.Stats.Add(new CustomStat("Life Regen", CustomStatEffects.LifeRegen, CustomStatEffects.LifeRegenTip));
			stats.Stats.Add(new CustomStat("Movement Speed", CustomStatEffects.MoveSpeed, CustomStatEffects.MoveSpeedTip));
			stats.Stats.Add(new CustomStat("Shield Capacity", CustomStatEffects.PuriumShieldChargeMax, CustomStatEffects.PuriumShieldChargeMaxTip));
			stats.Stats.Add(new CustomStat("Shield Fill Rate", CustomStatEffects.PuriumShieldFillRate, CustomStatEffects.PuriumShieldFillRateTip));
			stats.Stats.Add(new CustomStat("Shield Endurance", CustomStatEffects.PuriumShieldEnduranceMult, CustomStatEffects.PuriumShieldEnduranceMultTip));
			return stats;
		}

		public static CustomStats CreateCataclysmStats()
		{
			CustomStats stats = new CustomStats();
			stats.Stats.Add(new CustomStat("Damage", CustomStatEffects.AllDamage, CustomStatEffects.AllDamageTip));
			stats.Stats.Add(new CustomStat("Crit + Minions", CustomStatEffects.AllCrit, CustomStatEffects.AllCritTip));
			stats.Stats.Add(new CustomStat("Misc Offense", CustomStatEffects.MiscOffense, CustomStatEffects.MiscOffenseTip));
			stats.Stats.Add(new CustomStat("Misc Defense", CustomStatEffects.MiscDefense, CustomStatEffects.MiscDefenseTip));
			stats.Stats.Add(new CustomStat("Endurance", CustomStatEffects.Endurance, CustomStatEffects.EnduranceTip));
			stats.Stats.Add(new CustomStat("Max Health", CustomStatEffects.MaxHealth, CustomStatEffects.MaxHealthTip));
			stats.Stats.Add(new CustomStat("Debuff Immunities", CustomStatEffects.DebuffImmune, CustomStatEffects.DebuffImmuneTip));
			stats.Stats.Add(new CustomStat("Life Regen 2", CustomStatEffects.LifeRegen2, CustomStatEffects.LifeRegen2Tip));
			return stats;
		}
	}

	public class CustomStat
	{
		public const int MaxPoints = 5;

		public int Points = 0;
		public readonly string Name;
		private Action<Player, int> effect;
		public readonly string Tooltip;
		public bool Inactive;

		public CustomStat(string name, Action<Player, int> effect, string tooltip)
		{
			this.Name = name;
			this.effect = effect;
			this.Tooltip = tooltip;
		}

		public void Apply(Player player)
		{
			if (!Inactive)
			{
				effect(player, Points);
			}
		}

		public bool CanUpgrade()
		{
			return Points < MaxPoints;
		}

		public bool Equals(object other)
		{
			CustomStat otherStat = other as CustomStat;
			return otherStat != null && Points == otherStat.Points && Name == otherStat.Name && Tooltip == otherStat.Tooltip && Inactive == otherStat.Inactive;
		}

		public CustomStat Clone()
		{
			return (CustomStat)MemberwiseClone();
		}
	}

	public static class CustomStatEffects
	{
		public static void MeleeDamage(Player player, int points)
		{
			player.meleeDamage += 0.05f * points;
		}

		public const string MeleeDamageTip = "+ 5% melee damage per level";

		public static void RangedDamage(Player player, int points)
		{
			player.rangedDamage += 0.05f * points;
		}

		public const string RangedDamageTip = "+ 5% ranged damage per level";

		public static void MagicDamage(Player player, int points)
		{
			player.magicDamage += 0.05f * points;
		}

		public const string MagicDamageTip = "+ 5% magic damage per level";

		public static void MinionDamage(Player player, int points)
		{
			player.minionDamage += 0.05f * points;
		}

		public const string MinionDamageTip = "+ 5% minion damage per level";

		public static void ThrownDamage(Player player, int points)
		{
			player.thrownDamage += 0.05f * points;
		}

		public const string ThrownDamageTip = "+ 5% throwing damage per level";

		public static void MeleeCrit(Player player, int points)
		{
			player.meleeCrit += 5 * points;
		}

		public const string MeleeCritTip = "+ 5% melee crit chance per level";

		public static void RangedCrit(Player player, int points)
		{
			player.rangedCrit += 5 * points;
		}

		public const string RangedCritTip = "+ 5% ranged crit chance per level";

		public static void MagicCrit(Player player, int points)
		{
			player.magicCrit += 5 * points;
		}

		public const string MagicCritTip = "+ 5% magic crit chance per level";

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

		public const string MaxMinionsTip = "level 1-2: + 1 max minions\nlevel 3-4: + 2 max minions\nlevel 5: + 3 max minions";

		public static void ThrownCrit(Player player, int points)
		{
			player.thrownCrit += 5 * points;
		}

		public const string ThrownCritTip = "+ 5% throwing crit chance per level";

		public static void MeleeSpeed(Player player, int points)
		{
			player.meleeSpeed += 0.06f * points;
		}

		public const string MeleeSpeedTip = "+ 6% melee speed per level";

		public static void AmmoCost(Player player, int points)
		{
			Mod mod = ModLoader.GetMod("Bluemagic");
			player.GetModPlayer<BluemagicPlayer>().ammoCost += 0.08f * points;
		}

		public const string AmmoCostTip = "+ 8% chance not to consume ammo per level";

		public static void ManaCost(Player player, int points)
		{
			player.manaCost *= (1f - 0.1f * points);
		}

		public const string ManaCostTip = "10% reduced mana usage per level";

		public static void MinionKB(Player player, int points)
		{
			player.minionKB += 0.6f * points;
		}

		public const string MinionKBTip = "Increases minion knockback with each level";

		public static void ThrownCost(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.thrownCost += 0.08f * points;
		}

		public const string ThrownCostTip = "+ 8% chance not to consume thrown item per level";

		public static void Defense(Player player, int points)
		{
			player.statDefense += 8 * points;
		}

		public const string DefenseTip = "+ 8 defense per level";

		public static void LifeRegen(Player player, int points)
		{
			player.lifeRegen += 4 * points;
		}

		public const string LifeRegenTip = "Heals 2 health per level per second over time";

		public static void MoveSpeed(Player player, int points)
		{
			player.moveSpeed += 0.06f * points;
		}

		public const string MoveSpeedTip = "+ 6% movement speed per level";

		public static void PuriumShieldChargeMax(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.puriumShieldChargeMax += 100 * points;
		}

		public const string PuriumShieldChargeMaxTip = "+ 100 purity shield capacity per level";

		public static void PuriumShieldFillRate(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.puriumShieldChargeRate += 0.1f * points;
		}

		public const string PuriumShieldFillRateTip = "+ 10% purity shield fill rate per level";

		public static void PuriumShieldEnduranceMult(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.puriumShieldEnduranceMult += 0.1f * points;
		}

		public const string PuriumShieldEnduranceMultTip = "+ 2% purity shield endurance cap per level";

		public static void AllDamage(Player player, int points)
		{
			player.meleeDamage += 0.03f * points;
			player.rangedDamage += 0.03f * points;
			player.magicDamage += 0.03f * points;
			player.minionDamage += 0.03f * points;
			player.thrownDamage += 0.03f * points;
		}

		public const string AllDamageTip = "+ 3% damage per level";

		public static void AllCrit(Player player, int points)
		{
			player.meleeCrit += 3 * points;
			player.rangedCrit += 3 * points;
			player.magicCrit += 3 * points;
			player.thrownCrit += 3 * points;
			if (points >= 2)
			{
				player.maxMinions++;
			}
			if (points >= 5)
			{
				player.maxMinions++;
			}
		}

		public const string AllCritTip = "+ 3% crit chance per level\nlevel 2-4: + 1 max minions\nlevel 5: + 2 max minions";

		public static void MiscOffense(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			player.meleeSpeed += 0.03f * points;
			modPlayer.ammoCost += 0.04f * points;
			player.manaCost *= (1f - 0.05f * points);
			player.minionKB += 0.3f * points;
			modPlayer.thrownCost += 0.04f * points;
		}

		public const string MiscOffenseTip = "+ 3% melee speed per level\n+ 4% chance not to consume ammo or thrown item per level\n5% reduced mana usage per level\nIncreases minion knockback with each level";

		public static void MiscDefense(Player player, int points)
		{
			player.statDefense += 4 * points;
			player.lifeRegen += 2 * points;
			player.moveSpeed += 0.03f * points;
		}

		public const string MiscDefenseTip = "+ 4 defense per level\nHeals 1 health per level per second over time\n+ 3% movement speed per level";

		public static void Endurance(Player player, int points)
		{
			float endurance = 1f - player.endurance;
			endurance *= 1f - 0.05f * points;
			player.endurance = 1f - endurance;
		}

		public const string EnduranceTip = "Reduces damage taken by 5% per level";

		public static void MaxHealth(Player player, int points)
		{
			player.statLifeMax2 += 20 * points;
		}

		public const string MaxHealthTip = "+ 20 max health per level";

		public static void DebuffImmune(Player player, int points)
		{
			if (points >= 1)
			{
				player.buffImmune[BuffID.Suffocation] = true;
			}
			if (points >= 2)
			{
				player.buffImmune[BuffID.Blackout] = true;
			}
			if (points >= 3)
			{
				player.buffImmune[BuffID.Electrified] = true;
			}
			if (points >= 4)
			{
				player.buffImmune[BuffID.Webbed] = true;
			}
			if (points >= 5)
			{
				player.buffImmune[BuffID.VortexDebuff] = true;
			}
		}

		public const string DebuffImmuneTip = "Immune to Suffocation at level 1\nImmune to Blackout at level 2\nImmune to Electrified at level 3\nImmune to Webbed at level 4\nImmune to Distorted at level 5";

		public static void LifeRegen2(Player player, int points)
		{
			BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
			modPlayer.cancelBadRegen += 4 * points;
		}

		public const string LifeRegen2Tip = "Cancels damage over time by 2 health per level per second";
	}
}