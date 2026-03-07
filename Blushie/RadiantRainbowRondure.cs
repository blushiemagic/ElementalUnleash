using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class RadiantRainbowRondure : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault("Fires a ray of fabulous rainbow!"
                + "\n'Great for impersonating tModLoader devs...?'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.channel = true;
            Item.noMelee = true;
            Item.damage = 1200;
            Item.knockBack = 4f;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Expert;
            Item.DamageType = DamageClass.Magic;
            Item.value = Item.sellPrice(2, 0, 0, 0);
            Item.UseSound = SoundID.Item1;
            Item.shoot = Mod.Find<ModProjectile>("RadiantRainbowRay").Type;
            Item.mana = 10;
            Item.shootSpeed = 0f;
        }
    }
}