using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;
using Terraria.GameContent.ItemDropRules;
using Bluemagic.Items.Misc1;

namespace Bluemagic
{
    public class BluemagicNPC : GlobalNPC
    {
        public bool eFlames = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            eFlames = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (eFlames)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                npc.lifeRegen -= 16;
                if (damage < 2)
                {
                    damage = 2;
                }
            }
        }

        // Todo: Actually integrate these into the item drop database
        public override void OnKill(NPC npc)
        {
            if (NPC.downedPlantBoss && npc.value > 0f && npc.position.Y < Main.rockLayer * 16.0)
            {
                if (!NPC.BusyWithAnyInvasionOfSorts())
                {
                    if (Main.dayTime)
                    {
                        if (Main.rand.Next(10) == 0)
                        {
                            Item.NewItem(npc.GetSource_Death(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("SolarDrop").Type);
                        }
                    }
                    else if (Main.rand.Next(15) == 0)
                    {
                        Item.NewItem(npc.GetSource_Death(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("LunarDrop").Type);
                    }
                }
                else if (Main.invasionType == 0 && !DD2Event.Ongoing && (Main.bloodMoon || Main.eclipse) && Main.rand.Next(20) == 0)
                {
                    Item.NewItem(npc.GetSource_Death(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>("HorrorDrop").Type);
                }
            }
            if (((npc.type == NPCID.Pumpking && Main.pumpkinMoon) || (npc.type == NPCID.IceQueen && Main.snowMoon)) && NPC.waveNumber > 10)
            {
                int chance = NPC.waveNumber - 10;
                if (Main.expertMode)
                {
                    chance++;
                }
                if (Main.rand.Next(5) < chance)
                {
                    int stack = 1;
                    if (NPC.waveNumber >= 15)
                    {
                        stack = Main.rand.Next(4, 7);
                        if (Main.expertMode)
                        {
                            stack++;
                        }
                    }
                    else if (Main.rand.Next(2) == 0)
                    {
                        stack++;
                    }
                    string type = npc.type == NPCID.Pumpking ? "ScytheBlade" : "Icicle";
                    Item.NewItem(npc.GetSource_Death(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, Mod.Find<ModItem>(type).Type, stack);
                }
            }
            if (npc.type == NPCID.Bunny && npc.AnyInteractions())
            {
                int left = (int)(npc.position.X / 16f);
                int top = (int)(npc.position.Y / 16f);
                int right = (int)((npc.position.X + npc.width) / 16f);
                int bottom = (int)((npc.position.Y + npc.height) / 16f);
                bool flag = false;
                for (int i = left; i <= right; i++)
                {
                    for (int j = top; j <= bottom; j++)
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.HasTile && tile.TileType == Mod.Find<ModTile>("ElementalPurge").Type && !NPC.AnyNPCs(Mod.Find<ModNPC>("PuritySpirit").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit2").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("ChaosSpirit3").Type))
                        {
                            i -= Main.tile[i, j].TileFrameX / 18;
                            j -= Main.tile[i, j].TileFrameY / 18;
                            i = (i * 16) + 16;
                            j = (j * 16) + 24 + 60;
                            for (int k = 0; k < 255; k++)
                            {
                                Player player = Main.player[k];
                                if (player.active && player.position.X > i - NPC.sWidth / 2 && player.position.X + player.width < i + NPC.sWidth / 2 && player.position.Y > j - NPC.sHeight / 2 && player.position.Y < j + NPC.sHeight / 2)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                NPC.NewNPC(npc.GetSource_FromThis(), i, j, Mod.Find<ModNPC>("PuritySpirit").Type);
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
            }
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.DukeFishron)
            {
                npcLoot.Add(new CommonDrop(ModContent.ItemType<Bubble>(), 1, 5, 7));
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (BluemagicWorld.elementalUnleash)
            {
                spawnRate = (int)(spawnRate * 0.8);
                maxSpawns = (int)(maxSpawns * 1.2);
                int x = (int)(player.position.X / 16f);
                int y = (int)(player.position.Y / 16f);
                if (y < Main.worldSurface * 16 && (player.ZoneDesert || x < 250 || x > Main.maxTilesX - 250 || player.ZoneHallow))
                {
                    spawnRate = (int)(spawnRate * 2f / 3f);
                    maxSpawns++;
                }
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (eFlames)
            {
                if (Main.rand.Next(4) < 3)
                {
                    int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, Mod.Find<ModDust>("EtherealFlame").Type, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity *= 1.8f;
                    Main.dust[dust].velocity.Y -= 0.5f;
                    if (Main.rand.Next(4) == 0)
                    {
                        Main.dust[dust].noGravity = false;
                        Main.dust[dust].scale *= 0.5f;
                    }
                }
                Lighting.AddLight(npc.position, 0.1f, 0.2f, 0.7f);
            }
        }
    }
}