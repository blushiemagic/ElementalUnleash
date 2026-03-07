using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class FirePulsar : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Sends pulses of flames across the screen"
                + "\n'Great for impersonating... someone?'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item81;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.damage = 1200;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.width = 30;
            Item.height = 30;
            Item.shoot = Mod.Find<ModProjectile>("FirePulse").Type;
            Item.shootSpeed = 0f;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 4;
            Item.value = Item.sellPrice(2, 0, 0, 0);
        }
    }
}
