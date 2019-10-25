using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public class BlushiemagicK : BlushiemagicBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("blushiemagic (K)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
        }

        public override void AI()
        {
            if (BlushieBoss.Timer >= 390 && BlushieBoss.Timer < 600)
            {
                for (int k = 0; k < 5; k++)
                {
                    Dust.NewDust(npc.Center - new Vector2(50f, 50f), 100, 100, mod.DustType("Sparkle"), 0f, 0f, 0, new Color(0, 0, 255), 1f);
                }
            }
        }

        public override bool CheckDead()
        {
            if (BlushieBoss.HealthK > 0)
            {
                npc.life = BlushieBoss.HealthK;
            }
            else
            {
                npc.active = false;
                if (Main.netMode != 1)
                {
                    BlushieBoss.KylieTalk("I knew it. I'm so useless...");
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrostFairyWings"));
                }
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (BlushieBoss.Timer >= 600)
            {
                Texture2D texture = mod.GetTexture("BlushieBoss/BlushiemagicK_Back");
                spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, texture.Height / 2), Color.White);
            }
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (BlushieBoss.ShieldK >= 300 && BlushieBoss.ShieldBuff(npc))
            {
                Texture2D shield = mod.GetTexture("BlushieBoss/ShieldK");
                spriteBatch.Draw(shield, npc.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
            }
        }

        public override double CalculateDamage(Player player, double damage)
        {
            if (BlushieBoss.ShieldK >= 300 && BlushieBoss.ShieldBuff(npc))
            {
                BlushieBoss.ShieldK = 0;
                return 0;
            }
            float defenseMult = player.statDefense / 200f;
            float resistMult = player.endurance / 0.6f;
            float mult = 0.6f * defenseMult + 0.4f * resistMult;
            damage = mult * 100000;
            if (damage > 100000)
            {
                damage = 100000;
            }
            if (Main.netMode != 2 && npc.localAI[0] == 0f && damage < 50000)
            {
                Main.NewText("<blushiemagic (K)> Oh yeah, uh, you need lots of defense and damage reduction if you want to damage me. Sorry...", 0, 128, 255);
                npc.localAI[0] = 1f;
            }
            return damage;
        }

        public override void SetHealth(double damage)
        {
            BlushieBoss.HealthK = npc.life - (int)damage;
        }
    }
}