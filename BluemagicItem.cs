using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic
{
    public class BluemagicItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (item.mountType > -1 && modPlayer.CursedMount())
            {
                return false;
            }
            return base.CanUseItem(item, player);
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if (item.CountsAsClass(DamageClass.Throwing) && Main.rand.NextFloat() < modPlayer.thrownCost)
            {
                return false;
            }
            return base.ConsumeItem(item, player);
        }

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if ((item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum) && modPlayer.manaMagnet2)
            {
                grabRange += 100;
            }
            if ((item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane) && modPlayer.lifeMagnet2)
            {
                grabRange += 50;
            }
        }

        public override bool OnPickup(Item item, Player player)
        {
            BluemagicPlayer modPlayer = player.GetModPlayer<BluemagicPlayer>();
            if ((item.type == ItemID.Star || item.type == ItemID.SoulCake || item.type == ItemID.SugarPlum) && modPlayer.manaMagnet2)
            {
                SoundEngine.PlaySound(SoundID.Grab, player.position);
                player.statMana += 160;
                if (Main.myPlayer == player.whoAmI)
                {
                    player.ManaEffect(160);
                }
                if (player.statMana > player.statManaMax2)
                {
                    player.statMana = player.statManaMax2;
                }
                return false;
            }
            if ((item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane) && modPlayer.lifeMagnet2)
            {
                modPlayer.CheckBadHeal();
                SoundEngine.PlaySound(SoundID.Grab, player.position);
                player.statLife += 32;
                if (Main.myPlayer == player.whoAmI)
                {
                    player.HealEffect(32);
                }
                if (player.statLife > player.statLifeMax2)
                {
                    player.statLife = player.statLifeMax2;
                }
                modPlayer.StartBadHeal();
                return false;
            }
            else if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
            {
                modPlayer.CheckBadHeal();
                player.statLife += 20;
                modPlayer.StartBadHeal();
                player.statLife -= 20;
            }
            return base.OnPickup(item, player);
        }
    }
}