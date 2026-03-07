using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Bluemagic.Blushie
{
    public class WorldReaver : ModItem
    {
        private static Random rand = new Random();
        private static int timer = 0;
        private static string glitchText = "";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Reaver");
            /* Tooltip.SetDefault("Cleaves the world to erase your enemies"
                + "\nHas a 60 second cooldown"
                + "\n'Great for impersonating... someone?'");*/
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override bool CanUseItem(Player player)
        {
            return player.GetModPlayer<BluemagicPlayer>().worldReaverCooldown <= 0;
        }

        public override bool? UseItem(Player player)
        {
            player.GetModPlayer<BluemagicPlayer>().worldReaverCooldown = 3600;
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                WorldReaverData.Begin(player.whoAmI);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = Mod.GetPacket();
                packet.Write((byte)MessageType.WorldReaver);
                packet.Write(player.whoAmI);
                packet.Send();
            }
            return true;
        }

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Expert;
            // Todo: Uhh this sound doesnt exist? Item.UseSound = new Terraria.Audio.SoundStyle("Bluemagic/Sounds/Item/Slice");
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 666;
            Item.useAnimation = 40;
            Item.useTime = 40;
            Item.width = 138;
            Item.height = 114;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Melee;
            Item.value = Item.sellPrice(2, 0, 0, 0);
        }

        public static void UpdateGlitchText()
        {
            timer++;
            if (timer >= 15)
            {
                timer = 0;
                glitchText = "";
                for (var k = 0; k < 6; k++)
                {
                    int character;
                    if (rand.Next(3) <= 1)
                    {
                        character = 33 + rand.Next(15);
                    }
                    else
                    {
                        character = 97 + rand.Next(26);
                    }
                    glitchText += (char)character;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> lines)
        {
            for (int k = 0; k < lines.Count; k++)
            {
                if (lines[k].Mod == "Terraria" && lines[k].Name == "Damage")
                {
                    lines[k].Text = glitchText + Language.GetTextValue("LegacyTooltip.2");
                }
            }
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.itemAnimation < player.itemAnimationMax * 0.333)
            {
                player.itemLocation.X -= 24f * player.direction;
            }
            else if (player.itemAnimation < player.itemAnimationMax * 0.666)
            {
                player.itemLocation.X -= 24f * player.direction;
            }
            else
            {
                player.itemLocation.X += 24f * player.direction;
            }
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,
            Color itemColor, Vector2 origin, float scale)
        {
            int cooldown = Main.player[Main.myPlayer].GetModPlayer<BluemagicPlayer>().worldReaverCooldown;
            if (cooldown > 0)
            {
                Texture2D texture = TextureAssets.Cd.Value;
                Vector2 slotSize = new Vector2(52f, 52f);
                position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
                Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f/* - texture.Size() * Main.inventoryScale / 2f*/;
                float alpha = 0.1f + 0.9f * (cooldown / 3600f);
                Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                Main.spriteBatch.Draw(texture, drawPos, null, drawColor * alpha, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
            }
        }
    }
}
