using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
    public class BlushiemagicA : BlushiemagicBase
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("blushiemagic (A)");
            Main.npcFrameCount[NPC.type] = 2;
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
                for (int k = 0; k < 5; k++)
                {
                    float x = 12f + 60f * Main.rand.NextFloat();
                    float y = 16f - 0.5f * x;
                    if (Main.rand.Next(2) == 0)
                    {
                        x *= -1f;
                    }
                    x -= 2f;
                    int dust = Dust.NewDust(NPC.Center + new Vector2(x, y), 0, 0, DustID.Torch, 0f, 0f, 0, default(Color), 4f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].velocity.Y = 0.5f - 8f * Main.rand.NextFloat();
                    Main.dust[dust].velocity.X += 0.1f * x;
                }
            }
        }

        public override bool CheckDead()
        {
            if (BlushieBoss.HealthA > 0)
            {
                NPC.life = BlushieBoss.HealthA;
            }
            else
            {
                NPC.active = false;
                // Todo: Port to item drop database
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (BlushieBoss.BlushieC)
                    {
                        BlushieBoss.ChrisTalk("You are incredibly powerful! It was a pleasure to be able to spar with you.");
                        Item.NewItem(new EntitySource_Death(NPC), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FirePulsar").Type);
                    }
                    else
                    {
                        BlushieBoss.AnnaTalk("Wow, you're really strong! It was very fun playing with you~");
                        Item.NewItem(new EntitySource_Death(NPC), (int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("RadiantRainbowRondure").Type);
                    }
                }
            }
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = BlushieBoss.BlushieC ? frameHeight : 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (BlushieBoss.ShieldA >= 300 && BlushieBoss.ShieldBuff(NPC))
            {
                Texture2D shield = ModContent.Request<Texture2D>(BlushieBoss.BlushieC ? "Bluemagic/BlushieBoss/ShieldC" : "Bluemagic/BlushieBoss/ShieldA").Value;
                Main.spriteBatch.Draw(shield, NPC.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
            }
        }

        public override double CalculateDamage(Player player, double damage)
        {
            if (BlushieBoss.ShieldA >= 300 && BlushieBoss.ShieldBuff(NPC))
            {
                BlushieBoss.ShieldA = 0;
                return 0;
            }
            float healthMult = player.GetModPlayer<BluemagicPlayer>().origHealth / 720f;
            float regenMult = player.lifeRegen / 28f;
            float mult = 0.6f * healthMult + 0.4f * regenMult;
            damage = mult * 100000;
            if (damage > 100000)
            {
                damage = 100000;
            }
            else if (damage < 1)
            {
                damage = 1;
            }
            if (Main.netMode != NetmodeID.Server && NPC.localAI[0] == 0f && damage < 50000)
            {
                if (BlushieBoss.BlushieC)
                {
                    Main.NewText("<blushiemagic (C)> I'll give you a hint: maximum health and high life regen are key to damaging me. If you get hit, you won't be penalized for the max health reduction, but the damage over time you take will matter!", 255, 255, 0);
                }
                else
                {
                    Main.NewText("<blushiemagic (A)> I play with my own rules! If you want to damage me, try having lots of health at the start of the fight and high life regen!", 255, 128, 128);
                }
                NPC.localAI[0] = 1f;
            }
            return damage;
        }

        public override void SetHealth(double damage)
        {
            BlushieBoss.HealthA = NPC.life - (int)damage;
        }
    }
}