using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ModLoader;

namespace Bluemagic
{
    public static class SpawnHelper
    {
        public static bool MoonEvent(NPCSpawnInfo info)
        {
            return (Main.pumpkinMoon || Main.snowMoon) && info.SpawnTileY <= Main.worldSurface && !Main.dayTime;
        }

        public static bool Eclipse(NPCSpawnInfo info)
        {
            return Main.eclipse && info.SpawnTileY <= Main.worldSurface && Main.dayTime;
        }

        public static bool LunarTower(NPCSpawnInfo info)
        {
            Player player = info.Player;
            return player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust;
        }

        public static bool NoInvasion(NPCSpawnInfo info)
        {
            return !info.Invasion && !DD2Event.Ongoing && !MoonEvent(info) && !Eclipse(info) && !LunarTower(info);
        }

        public static bool NoBiome(NPCSpawnInfo info)
        {
            Player player = info.Player;
            return !player.ZoneJungle && !player.ZoneDungeon && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHallow && !player.ZoneSnow && !player.ZoneUndergroundDesert && info.SpawnTileY < Main.maxTilesY - 190;
        }

        public static bool NoZoneAllowWater(NPCSpawnInfo info)
        {
            return !info.Sky && !info.Player.ZoneMeteor && !info.SpiderCave;
        }

        public static bool NoZone(NPCSpawnInfo info)
        {
            return NoZoneAllowWater(info) && !info.Water;
        }

        public static bool NormalSpawn(NPCSpawnInfo info)
        {
            return !info.PlayerInTown && NoInvasion(info);
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
            return NormalSpawn(info) && NoBiome(info) && NoZone(info);
        }
    }
}
