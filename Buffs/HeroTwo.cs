using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Buffs
{
	public class HeroTwo : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffName[this.Type] = "Hero";
			Main.buffTip[this.Type] = "You are a hero of Terraria! (2 Lives)";
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			canBeCleared = false;
		}
	}
}
