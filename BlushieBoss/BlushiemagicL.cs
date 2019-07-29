using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicL : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (L)");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			this.music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Phyrnna - Return of the Snow Queen");
		}

		public override void AI()
		{
			if (BlushieBoss.Timer >= 390)
			{
				for (int k = 0; k < 3; k++)
				{
					float start = MathHelper.Pi / 6f + k * MathHelper.Pi * 2f / 3f;
					float end = start + 2f * MathHelper.Pi / 3f;
					Vector2 pos = npc.Center + 50f * Vector2.Lerp(start.ToRotationVector2(), end.ToRotationVector2(), Main.rand.NextFloat());
					int dust = Dust.NewDust(pos - new Vector2(2f), 0, 0, mod.DustType("PurpleLightning"), 0f, 0f, 100, default(Color), 1f);
					Main.dust[dust].velocity *= 0.25f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
			}
			npc.localAI[0] = (npc.localAI[0] + 1f) % 60f;
		}

		public override bool CheckDead()
		{
			if (BlushieBoss.HealthL > 0)
			{
				npc.life = BlushieBoss.HealthL;
			}
			else
			{
				if (Main.netMode != 1)
				{
					BlushieBoss.LunaTalk("Hmph. I will admit I underestimated you. I shall concede defeat...");
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DarkLightningPack"));
				}
				npc.active = false;
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("BlushieBoss/LightningCannon");
			Vector2 draw = npc.Center - Main.screenPosition + new Vector2(0f, 8f);
			Color color;
			if (npc.localAI[0] < 15f)
			{
				color = Color.Lerp(Color.White, new Color(255, 255, 0), npc.localAI[0] / 15f);
			}
			else if (npc.localAI[0] < 30f)
			{
				color = Color.Lerp(new Color(255, 255, 0), Color.White, (npc.localAI[0] - 15f) / 15f);
			}
			else if (npc.localAI[0] < 45f)
			{
				color = Color.Lerp(Color.White, new Color(0, 200, 255), (npc.localAI[0] - 30f) / 15f);
			}
			else
			{
				color = Color.Lerp(new Color(0, 200, 255), Color.White, (npc.localAI[0] - 45f) / 15f);
			}
			spriteBatch.Draw(texture, draw, new Rectangle(0, 0, 48, 20), Color.White, MathHelper.Pi * 7f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, draw, new Rectangle(0, 20, 48, 20), color, MathHelper.Pi * 7f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, draw, new Rectangle(0, 0, 48, 20), Color.White, MathHelper.Pi * 11f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, draw, new Rectangle(0, 20, 48, 20), color, MathHelper.Pi * 11f / 6f, new Vector2(0f, 10f), 1f, SpriteEffects.None, 0f);

			texture = mod.GetTexture("BlushieBoss/LightningOrb");
			draw = npc.Center - Main.screenPosition - new Vector2(16f, 16f);
			for (int k = 0; k < 3; k++)
			{
				float rot = MathHelper.Pi / 6f + k * MathHelper.Pi * 2f / 3f;
				spriteBatch.Draw(texture, 50f * rot.ToRotationVector2() + draw, new Color(200, 200, 200));
			}

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.ShieldL >= 300 && BlushieBoss.ShieldBuff(npc))
			{
				Texture2D shield = mod.GetTexture("BlushieBoss/ShieldL");
				spriteBatch.Draw(shield, npc.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
			}
		}

		public override double CalculateDamage(Player player, double damage)
		{
			if (BlushieBoss.ShieldL >= 300 && BlushieBoss.ShieldBuff(npc))
			{
				BlushieBoss.ShieldL = 0;
				return 0;
			}
			damage *= 50;
			if (damage > 100000)
			{
				damage = 100000;
			}
			if (Main.netMode != 2 && npc.localAI[0] == 0f && damage < 50000)
			{
				Main.NewText("<blushiemagic (L)> I hope you realize that you need high damage in order to deal high damage to me. Common sense, really...", 128, 0, 128);
				npc.localAI[0] = 1f;
			}
			return damage;
		}

		public override void SetHealth(double damage)
		{
			BlushieBoss.HealthL = npc.life - (int)damage;
		}
	}
}