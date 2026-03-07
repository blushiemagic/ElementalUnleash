using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Phantom
{
    public class PhantomSoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.width = 16;
            NPC.height = 16;
            NPC.alpha = 100;
            NPC.noTileCollide = true;
            NPC.lifeMax = 100;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.npcSlots = 12f;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            Music = MusicID.Boss3;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.ai[0] < 200f)
            {
                NPC.Center = player.Center;
            }
            else if (NPC.ai[0] < 600f)
            {
                NPC.Center = player.Center - new Vector2(0f, (NPC.ai[0] - 200f) * 0.625f);
            }
            else
            {
                NPC.Center = player.Center - new Vector2(0f, 250f);
            }
            if (NPC.ai[0] < 660f)
            {
                NPC.ai[1] = 0f;
            }
            else if (NPC.ai[0] < 720f)
            {
                NPC.ai[1] = (NPC.ai[0] - 660f) / 60f * 0.6f;
            }
            else
            {
                NPC.ai[1] = 0.6f;
            }
            if (NPC.ai[0] < 750f)
            {
                NPC.ai[2] = 1f;
            }
            else
            {
                NPC.ai[2] = 1f + (NPC.ai[0] - 750f) / 50f;
            }

            int num = 3;
            bool flag = NPC.ai[0] == 899f;
            if (NPC.ai[0] < 120)
            {
                num = 1;
            }
            if (flag)
            {
                num = 200;
            }
            for (int k = 0; k < num; k++)
            {
                int dust = Dust.NewDust(NPC.position, 16, 16, Mod.Find<ModDust>("Phantom").Type);
                if (flag)
                {
                    Main.dust[dust].velocity *= (Main.rand.Next(3) + 1);
                }
            }
            NPC.ai[0] += 1f;
            if (NPC.ai[0] >= 900)
            {
                NPC.active = false;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y + 40, Mod.Find<ModNPC>("Phantom").Type, 0, 0f, 0f, 0f, 0f, NPC.target);
                }
            }
            NPC.rotation += 0.1f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Main.spriteBatch.Draw(texture, NPC.position - Main.screenPosition, Color.White * 0.8f);
            texture = ModContent.Request<Texture2D>("Bluemagic/Phantom/PhantomSoulAura").Value;
            float alpha = NPC.ai[1];
            float scale = NPC.ai[2];
            Main.spriteBatch.Draw(texture, NPC.Center - Main.screenPosition, null, Color.White * alpha, NPC.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}