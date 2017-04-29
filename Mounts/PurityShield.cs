using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Mounts
{
	public class PurityShield : ModMountData
	{
		private const float speed = 10f;

		public override void SetDefaults()
		{
			mountData.spawnDust = 226;
			mountData.spawnDustNoGravity = true;
			mountData.buff = mod.BuffType("PurityShieldMount");
			mountData.heightBoost = 0;
			mountData.flightTimeMax = Int32.MaxValue;
			mountData.fatigueMax = Int32.MaxValue;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = speed;
			mountData.dashSpeed = speed;
			mountData.acceleration = speed;
			mountData.swimSpeed = speed;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 8f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 1;
			int[] array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 0;
			}
			mountData.playerYOffsets = new int[] { 0 };
			mountData.xOffset = 16;
			mountData.bodyFrame = 5;
			mountData.yOffset = 16;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 0;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 0;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 1;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 1;
			mountData.swimFrameDelay = 0;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}

		public override void UpdateEffects(Player player)
		{
			if (player.controlLeft)
			{
				player.velocity.X = -speed;
				player.direction = -1;
			}
			else if (player.controlRight)
			{
				player.velocity.X = speed;
				player.direction = 1;
			}
			else
			{
				player.velocity.X = 0f;
			}
			if (player.controlJump || player.controlUp)
			{
				player.velocity.Y = -speed;
			}
			else if (player.controlDown)
			{
				player.velocity.Y = speed;
			}
			else
			{
				player.velocity.Y = 0f;
			}
		}
	}
}