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
            return (Main.pumpkinMoon || Main.snowMoon) && info.spawnTileY <= Main.worldSurface && !Main.dayTime;
        }

        public static bool Eclipse(NPCSpawnInfo info)
        {
            return Main.eclipse && info.spawnTileY <= Main.worldSurface && Main.dayTime;
        }

        public static bool LunarTower(NPCSpawnInfo info)
        {
            Player player = info.player;
            return player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust;
        }

        public static bool NoInvasion(NPCSpawnInfo info)
        {
            return !info.invasion && !DD2Event.Ongoing && !MoonEvent(info) && !Eclipse(info) && !LunarTower(info);
        }

        public static bool NoBiome(NPCSpawnInfo info)
        {
            Player player = info.player;
            return !player.ZoneJungle && !player.ZoneDungeon && !player.ZoneCorrupt && !player.ZoneCrimson && !player.ZoneHoly && !player.ZoneSnow && !player.ZoneUndergroundDesert && info.spawnTileY < Main.maxTilesY - 190;
        }

        public static bool NoZoneAllowWater(NPCSpawnInfo info)
        {
            return !info.sky && !info.player.ZoneMeteor && !info.spiderCave;
        }

        public static bool NoZone(NPCSpawnInfo info)
        {
            return NoZoneAllowWater(info) && !info.water;
        }

        public static bool NormalSpawn(NPCSpawnInfo info)
        {
            return !info.playerInTown && NoInvasion(info);
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
