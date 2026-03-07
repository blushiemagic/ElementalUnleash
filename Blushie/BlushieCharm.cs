using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class BlushieCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Charm of Blushie");
            // Tooltip.SetDefault("Your hearts shall spell the doom of your enemies"
            //    + "\nCan be used by any class"
            //    + "\nMade in celebration of blushiemagic's love");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 54;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.channel = true;
            Item.noMelee = true;
            Item.damage = 1;
            Item.knockBack = 1f;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.rare = ItemRarityID.Expert;
            Item.expert = true;
            Item.DamageType = DamageClass.Magic;
            Item.value = 100000000; // Porting note: Previously 100000000
            Item.UseSound = SoundID.Item1;
            Item.shoot = Mod.Find<ModProjectile>("BlushieCharmProj").Type;
            Item.mana = 200;
            Item.shootSpeed = 0f;
        }
    }
}