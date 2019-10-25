using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class SkyDragonHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Dragon's Heart");
            Tooltip.SetDefault("Summons the Sky Dragon to fight for you"
                + "\nEach player can only summon one dragon"
                + "\n'Great for impersonating... someone?'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[item.type] = 2;
        }

        public override void SetDefaults()
        {
            item.damage = 812;
            item.summon = true;
            item.mana = 20;
            item.width = 34;
            item.height = 48;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 4;
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(2, 0, 0, 0);
            item.rare = 13;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("SkyDragonHead");
            item.shootSpeed = 0f;
            item.buffType = mod.BuffType("SkyDragon");
            item.buffTime = 3600;
        }
    }
}
