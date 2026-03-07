using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class DarkLightningPack : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Zaps enemies across the entire screen"
                + "\n'Great for impersonating tModLoader devs!'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item121;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = 100;
            Item.damage = 500;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.width = 24;
            Item.height = 28;
            Item.shoot = Mod.Find<ModProjectile>("DarkLightningProj").Type;
            Item.shootSpeed = 0f;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(2, 0, 0, 0);
        }
    }
}
