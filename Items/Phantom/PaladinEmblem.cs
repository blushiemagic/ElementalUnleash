using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    public class PaladinEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("The dungeon is growing cold...");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (!NPC.downedPlantBoss || !player.ZoneDungeon)
            {
                return false;
            }
            for (int x = 0; x < 200; x++)
            {
                if (Main.npc[x].active && (Main.npc[x].type == Mod.Find<ModNPC>("PhantomSoul").Type || Main.npc[x].type == Mod.Find<ModNPC>("Phantom").Type))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool? UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int npc = NPC.NewNPC(player.GetSource_FromThis(), (int)player.Center.X, (int)player.Center.Y, Mod.Find<ModNPC>("PhantomSoul").Type, 0, 0f, 0f, 0f, 0f, player.whoAmI);
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc);
                }
            }
            Main.NewText(Language.GetTextValue("Mods.Bluemagic.PhantomSummon"), 50, 150, 200);
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SpectreBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}