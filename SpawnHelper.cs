using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic
{
	public class SpawnHelper
	{
		public static bool IsJungleBlock(int type)
		{
			return type == TileID.JungleGrass;
		}

		public static bool IsCorruptBlock(int type, Player spawnedOn)
		{
			return (type == TileID.Demonite && spawnedOn.ZoneCorrupt) || type == TileID.CorruptGrass || type == TileID.Ebonstone || type == TileID.Ebonsand || type == TileID.CorruptIce;
		}

		public static bool IsCrimsonBlock(int type, Player spawnedOn)
		{
			return (type == TileID.Crimtane && spawnedOn.ZoneCrimson) || type == TileID.FleshGrass || type == TileID.FleshIce || type == TileID.Crimstone || type == TileID.Crimsand;
		}

		public static bool IsHallowBlock(int type)
		{
			return type == TileID.Pearlsand || type == TileID.Pearlstone || type == TileID.HallowedGrass || type == TileID.HallowedIce;
		}

		public static bool IsSnowBlock(int type)
		{
			return type == TileID.SnowBlock || type == TileID.IceBlock || type == TileID.CorruptIce || type == TileID.HallowedIce || type == TileID.BreakableIce || type == TileID.FleshIce;
		}

		public static bool IsShroomBlock(int type)
		{
			return type == TileID.MushroomGrass;
		}

		public static bool NoInvasion(bool invasion, int y)
		{
			return !invasion && ((!Main.pumpkinMoon && !Main.snowMoon) || y > Main.worldSurface || Main.dayTime) && (!Main.eclipse || y > Main.worldSurface || !Main.dayTime);
		}

		public static bool NoBiome(Player spawnedOn, int tile)
		{
			return !IsJungleBlock(tile) && !spawnedOn.ZoneDungeon && !IsCorruptBlock(tile, spawnedOn) && !IsCrimsonBlock(tile, spawnedOn) && !IsHallowBlock(tile) && !IsSnowBlock(tile) && !spawnedOn.ZoneSnow && !IsShroomBlock(tile);
		}

		public static bool NoZoneAllowWater(NPCSpawnInfo info)
		{
			Tile tile = Main.tile[info.spawnTileX, info.spawnTileY];
			return !info.sky && !info.player.ZoneMeteor && tile.type != TileID.Meteorite && !info.spiderCave && tile.wall != 62;
		}

		public static bool NoZone(NPCSpawnInfo info)
		{
			return NoZoneAllowWater(info) && !info.water;
		}

		public static bool NormalSpawn(NPCSpawnInfo info)
		{
			return !info.playerInTown && NoInvasion(info.invasion, info.spawnTileY);
		}

		public static bool NoZoneNormalSpawn(NPCSpawnInfo info)
		{
			return NormalSpawn(info) && NoZone(info);
		}

		public static bool NoZoneNormalSpawnAllowWater(NPCSpawnInfo info)
		{
			return NormalSpawn(info) && NoZoneAllowWater(info);
		}

		public static bool NoBiomeNormalSpawn(NPCSpawnInfo info)
		{
			return NormalSpawn(info) && NoBiome(info.player, Main.tile[info.spawnTileX, info.spawnTileY].type) && NoZone(info);
		}
	}
}