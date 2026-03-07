using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.TerraSpirit
{
    public class NegativeBlob2 : ModNPC
    {
        protected NPC Spirit
        {
            get
            {
                return Main.npc[(int)NPC.ai[0]];
            }
        }

        public override string Texture
        {
            get
            {
                return "Bluemagic/TerraSpirit/NegativeBlob";
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Negative Blob");
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 20000;
            NPC.damage = 0;
            NPC.defense = 100;
            NPC.knockBackResist = 0f;
            NPC.width = 80;
            NPC.height = 80;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = null;
            NPC.dontTakeDamage = true;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            NPC spirit = Spirit;
            if (!spirit.active || !(spirit.ModNPC is TerraSpirit2) || NPC.ai[3] == -1f || spirit.ai[0] == 3f)
            {
                NPC.active = false;
                return;
            }
            NPC.velocity = Vector2.Zero;
            NPC.rotation += 0.1f;
            if (NPC.timeLeft < 600)
            {
                NPC.timeLeft = 600;
            }
            if (NPC.ai[1] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int k = 0; k < 200; k++)
                {
                    if (k != NPC.whoAmI && Main.npc[k].active && Main.npc[k].type == NPC.type)
                    {
                        if (Main.npc[k].ai[1] == 0f)
                        {
                            Main.npc[k].ai[1] = NPC.whoAmI;
                            NPC.ai[1] = k;
                            Main.npc[k].netUpdate = true;
                            NPC.netUpdate = true;
                        }
                        else if (Main.npc[k].ai[2] == 0f)
                        {
                            Main.npc[k].ai[2] = NPC.whoAmI;
                            NPC.ai[1] = k;
                            Main.npc[(int)Main.npc[k].ai[1]].ai[2] = NPC.whoAmI;
                            NPC.ai[2] = Main.npc[k].ai[1];
                            Main.npc[k].netUpdate = true;
                            NPC.netUpdate = true;
                            Main.npc[(int)Main.npc[k].ai[1]].netUpdate = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Bottom.X, (int)NPC.Bottom.Y, Mod.Find<ModNPC>("GoldBlob").Type, 0, NPC.whoAmI, NPC.ai[1], NPC.ai[2]);
                            }
                        }
                    }
                }
            }
            Player player = Main.player[Main.myPlayer];
            if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(NPC.Hitbox))
            {
                player.GetModPlayer<BluemagicPlayer>().TerraKill();
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Color color = Color.White * 0.7f;
            if (NPC.ai[1] > 0f)
            {
                Utils.DrawLine(spriteBatch, NPC.Center, Main.npc[(int)NPC.ai[1]].Center, color, color, 4f);
            }
            if (NPC.ai[2] > 0f)
            {
                Utils.DrawLine(spriteBatch, NPC.Center, Main.npc[(int)NPC.ai[2]].Center, color, color, 4f);
            }
            Main.spriteBatch.Draw(ModContent.Request<Texture2D>("Bluemagic/TerraSpirit/NegativeCircle").Value, NPC.Center - Main.screenPosition, null, Color.White * 0.25f, 0f, new Vector2(120f, 120f), 1f, SpriteEffects.None, 0f);
            return true;
        }
    }
}
