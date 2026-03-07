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
            // DisplayName.SetDefault("Sky Dragon's Heart");
            /* Tooltip.SetDefault("Summons the Sky Dragon to fight for you"
                + "\nEach player can only summon one dragon"
                + "\n'Great for impersonating... someone?'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.StaffMinionSlotsRequired[Item.type] = 2;
        }

        public override void SetDefaults()
        {
            Item.damage = 812;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 34;
            Item.height = 48;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(2, 0, 0, 0);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item44;
            Item.shoot = Mod.Find<ModProjectile>("SkyDragonHead").Type;
            Item.shootSpeed = 0f;
            Item.buffType = Mod.Find<ModBuff>("SkyDragon").Type;
            Item.buffTime = 3600;
        }
    }
}
