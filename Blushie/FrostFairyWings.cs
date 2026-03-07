using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class FrostFairyWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wings of the Frost Fairy");
            /* Tooltip.SetDefault("Summons Wings of the Frost Fairy to follow behind you"
                + "\nEach player can only summon one set of wings"
                + "\n'Great for impersonating tModLoader devs!'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 900;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 20;
            Item.width = 28;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 100;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(2, 0, 0, 0);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item44;
            Item.shoot = Mod.Find<ModProjectile>("FrostFairyWingsProj").Type;
            Item.shootSpeed = 0f;
            Item.buffType = Mod.Find<ModBuff>("FrostFairyWings").Type;
            Item.buffTime = 3600;
        }
    }
}
