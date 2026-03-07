using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Items.Misc1
{
    public class Shroomsand : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shroomsand Block");
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = Mod.Find<ModTile>("Shroomsand").Type;
            Item.ammo = AmmoID.Sand;
            Item.notAmmo = true;
        }

        public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
        {
            if (type == ProjectileID.SandBallGun)
            {
                type = Mod.Find<ModProjectile>("ShroomsandGunBall").Type;
            }
        }
    }
}