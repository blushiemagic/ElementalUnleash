using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.BlushieBoss
{
	public class BlushiemagicA : BlushiemagicBase
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("blushiemagic (A)");
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
				for (int k = 0; k < 5; k++)
				{
					float x = 12f + 60f * Main.rand.NextFloat();
					float y = 16f - 0.5f * x;
					if (Main.rand.Next(2) == 0)
					{
						x *= -1f;
					}
					x -= 2f;
					int dust = Dust.NewDust(npc.Center + new Vector2(x, y), 0, 0, 6, 0f, 0f, 0, default(Color), 4f);
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
				npc.life = BlushieBoss.HealthA;
			}
			else
			{
				npc.active = false;
				if (Main.netMode != 1)
				{
					BlushieBoss.AnnaTalk("Wow, you're really strong! It was very fun playing with you~");
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (BlushieBoss.ShieldA >= 300 && BlushieBoss.ShieldBuff(npc))
			{
				Texture2D shield = mod.GetTexture("BlushieBoss/ShieldA");
				spriteBatch.Draw(shield, npc.Center - Main.screenPosition - new Vector2(shield.Width / 2, shield.Height / 2), null, Color.White * 0.5f);
			}
		}

		public override double CalculateDamage(Player player, double damage)
		{
			if (BlushieBoss.ShieldA >= 300 && BlushieBoss.ShieldBuff(npc))
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
			if (Main.netMode != 2 && npc.localAI[0] == 0f && damage < 50000)
			{
				Main.NewText("<blushiemagic (A)> I play with my own rules! If you want to damage me, try having lots of health at the start of the fight and high life regen!", 255, 128, 128);
				npc.localAI[0] = 1f;
			}
			return damage;
		}

		public override void SetHealth(double damage)
		{
			BlushieBoss.HealthA = npc.life - (int)damage;
		}
	}
}