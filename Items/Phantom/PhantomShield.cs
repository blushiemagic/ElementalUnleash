using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Phantom
{
    [AutoloadEquip(EquipType.Shield)]
    public class PhantomShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Phantom's Shield");
            /* Tooltip.SetDefault("Reduces damage taken by 7%"
                + "\nPlayers on your team receive this bonus as well");*/
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Cyan;
            Item.expert = true;
            Item.accessory = true;
            Item.defense = 6;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(Mod.Find<ModBuff>("PhantomShield").Type, 5, true);
            if (player.whoAmI != Main.myPlayer && Main.player[Main.myPlayer].team == player.team && player.team != 0)
            {
                Main.player[Main.myPlayer].AddBuff(Mod.Find<ModBuff>("PhantomShield").Type, 5, true);
            }
        }
    }
}