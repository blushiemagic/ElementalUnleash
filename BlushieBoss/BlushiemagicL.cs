using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public class BlushiemagicL : BlushiemagicBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("blushiemagic (L)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            this.Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Phyrnna - Return of the Snow Queen");
        }

        public override void AI()
        {
            if (BlushieBoss.Timer >= 390)
            {
                for (int k = 0; k < 3; k++)
                {
                    float start = MathHelper.Pi / 6f + k * MathHelper.Pi * 2f / 3f;
                    float end = start + 2f * MathHelper.Pi / 3f;
                    Vector2 pos = NPC.Center + 50f * Vector2.Lerp(start.ToRotationVector2(), end.ToRotationVector2(), Main.rand.NextFloat());
                    int dust = Dust.NewDust(pos - new Vector2(2f), 0, 0, Mod.Find<ModDust>("PurpleLightning").Type, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dust].velocity *= 0.25f;
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                }
            }
            NPC.localAI[0] = (NPC.localAI[0] + 1f) % 60f;
        }

        public override bool CheckDead()
        {
            if (BlushieBoss.HealthL > 0)
            {
                NPC.life = BlushieBoss.HealthL;
            }
            else
            {
                // Todo: Port to item drop database
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    BlushieBoss.LunaTalk("Hmph. I will admit I underestimated you. I shall concede defeat...");
                    Item.NewItem(new EntitySource_Death(NPC), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("DarkLightningPack").Type);
                }
                NPC.active = false;
            }
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/LightningCannon").Value;
            Vector2 draw = NPC.Center - Main.screenPosition + new Vector2(0f, 8f);
            Color color;
            if (NPC.localAI[0] < 15f)
            {
                color = Color.Lerp(Color.White, new Color(255, 255, 0), NPC.localAI[0] / 15f);
            }
            else if (NPC.localAI[0] < 30f)
            {
                color = Color.Lerp(new Color(255, 255, 0), Color.White, (NPC.localAI[0] - 15f) / 15f);
            }
            else if (NPC.localAI[0] < 45f)
            {
                color = Color.Lerp(Color.White, new Color(0, 200, 255), (NPC.localAI[0] - 30f) / 15f);
            }
            else
            {
                color = Color.Lerp(new Color(0, 200, 255), Color.White, (NPC.localAI[0] - 45f) / 15f);
            }
            Main.spriteBatch.Draw(texture, draw, new Rectangle(0, 0, 48, 20), Color.White, MathHelper.Pi * 7f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, draw, new Rectangle(0, 20, 48, 20), color, MathHelper.Pi * 7f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, draw, new Rectangle(0, 0, 48, 20), Color.White, MathHelper.Pi * 11f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture, draw, new Rectangle(0, 20, 48, 20), color, MathHelper.Pi * 11f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);

            texture = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/LightningOrb").Value;
            draw = NPC.Center - Main.screenPosition - new Vector2(16f, 16f);
            for (int k = 0; k < 3; k++)
            {
                float rot = MathHelper.Pi / 6f + k * MathHelper.Pi * 2f / 3f;
                Main.spriteBatch.Draw(texture, 50f * rot.ToRotationVector2() + draw, new Color(200, 200, 200));
            }

            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (BlushieBoss.ShieldL >= 300 && BlushieBoss.ShieldBuff(NPC))
            {
                Texture2D shield = ModContent.Request<Texture2D>("Bluemagic/BlushieBoss/ShieldL").Value;
                Main.spriteBatch.Draw(shield, NPC.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
            }
        }

        public override double CalculateDamage(Player player, double damage)
        {
            if (BlushieBoss.ShieldL >= 300 && BlushieBoss.ShieldBuff(NPC))
            {
                BlushieBoss.ShieldL = 0;
                return 0;
            }
            damage *= 50;
            if (damage > 100000)
            {
                damage = 100000;
            }
            if (Main.netMode != NetmodeID.Server && NPC.localAI[0] == 0f && damage < 50000)
            {
                Main.NewText("<blushiemagic (L)> I hope you realize that you need high damage in order to deal high damage to me. Common sense, really...", 128, 0, 128);
                NPC.localAI[0] = 1f;
            }
            return damage;
        }

        public override void SetHealth(double damage)
        {
            BlushieBoss.HealthL = NPC.life - (int)damage;
        }
    }
}