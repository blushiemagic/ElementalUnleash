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
				return Main.npc[(int)npc.ai[0]];
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
			DisplayName.SetDefault("Negative Blob");
		}

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 20000;
			npc.damage = 0;
			npc.defense = 100;
			npc.knockBackResist = 0f;
			npc.width = 80;
			npc.height = 80;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = null;
			npc.dontTakeDamage = true;
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}

		public override void AI()
		{
			NPC spirit = Spirit;
			if (!spirit.active || !(spirit.modNPC is TerraSpirit2) || npc.ai[3] == -1f || spirit.ai[0] == 3f)
			{
				npc.active = false;
				return;
			}
			npc.velocity = Vector2.Zero;
			npc.rotation += 0.1f;
			if (npc.timeLeft < 600)
			{
				npc.timeLeft = 600;
			}
			if (npc.ai[1] == 0f && Main.netMode != 1)
			{
				for (int k = 0; k < 200; k++)
				{
					if (k != npc.whoAmI && Main.npc[k].active && Main.npc[k].type == npc.type)
					{
						if (Main.npc[k].ai[1] == 0f)
						{
							Main.npc[k].ai[1] = npc.whoAmI;
							npc.ai[1] = k;
							Main.npc[k].netUpdate = true;
							npc.netUpdate = true;
						}
						else if (Main.npc[k].ai[2] == 0f)
						{
							Main.npc[k].ai[2] = npc.whoAmI;
							npc.ai[1] = k;
							Main.npc[(int)Main.npc[k].ai[1]].ai[2] = npc.whoAmI;
							npc.ai[2] = Main.npc[k].ai[1];
							Main.npc[k].netUpdate = true;
							npc.netUpdate = true;
							Main.npc[(int)Main.npc[k].ai[1]].netUpdate = true;
							if (Main.netMode != 1)
							{
								NPC.NewNPC((int)npc.Bottom.X, (int)npc.Bottom.Y, mod.NPCType("GoldBlob"), 0, npc.whoAmI, npc.ai[1], npc.ai[2]);
							}
						}
					}
				}
			}
			Player player = Main.player[Main.myPlayer];
			if (player.active && !player.dead && player.GetModPlayer<BluemagicPlayer>().terraLives > 0 && player.Hitbox.Intersects(npc.Hitbox))
			{
				player.GetModPlayer<BluemagicPlayer>().TerraKill();
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Color color = Color.White * 0.7f;
			if (npc.ai[1] > 0f)
			{
				Utils.DrawLine(spriteBatch, npc.Center, Main.npc[(int)npc.ai[1]].Center, color, color, 4f);
			}
			if (npc.ai[2] > 0f)
			{
				Utils.DrawLine(spriteBatch, npc.Center, Main.npc[(int)npc.ai[2]].Center, color, color, 4f);
			}
			spriteBatch.Draw(mod.GetTexture("TerraSpirit/NegativeCircle"), npc.Center - Main.screenPosition, null, Color.White * 0.25f, 0f, new Vector2(120f, 120f), 1f, SpriteEffects.None, 0f);
			return true;
		}
	}
}
