using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Bluemagic;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Bluemagic.Items.Abomination;

namespace Bluemagic.Abomination
{
    [AutoloadBossHead]
    public class CaptiveElement2 : ModNPC
    {
        private static int hellLayer
        {
            get
            {
                return Main.maxTilesY - 200;
            }
        }

        private int difficulty
        {
            get
            {
                int result = 0;
                if (Main.expertMode)
                {
                    result++;
                }
                if (NPC.downedMoonlord)
                {
                    result++;
                }
                return result;
            }
        }

        private int captiveType
        {
            get
            {
                return (int)NPC.ai[0];
            }
            set
            {
                NPC.ai[0] = value;
            }
        }

        private float attackCool
        {
            get
            {
                return NPC.ai[1];
            }
            set
            {
                NPC.ai[1] = value;
            }
        }

        private int run
        {
            get
            {
                return (int)NPC.ai[2];
            }
            set
            {
                NPC.ai[2] = value;
            }
        }

        private int jungleAI
        {
            get
            {
                return (int)NPC.ai[3];
            }
            set
            {
                NPC.ai[3] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Captive Element");
            Main.npcFrameCount[NPC.type] = 5;
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 15000;
            NPC.damage = 100;
            NPC.defense = 55;
            if (NPC.downedMoonlord)
            {
                NPC.lifeMax = 30000;
                NPC.damage = 120;
                NPC.defense = 80;
            }
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 20, 0, 0);
            NPC.npcSlots = 10f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            Music = MusicID.Boss2;
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * balance);
            NPC.damage = (int)(NPC.damage * 0.75f);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }

        public override void AI()
        {
            Player player = Main.player[NPC.target];
            if (NPC.localAI[0] == 0f)
            {
                if (GetDebuff() >= 0f)
                {
                    NPC.buffImmune[GetDebuff()] = true;
                }
                if (captiveType == 3)
                {
                    NPC.buffImmune[20] = true;
                    NPC.ai[3] = 1f;
                }
                if (captiveType == 0)
                {
                    NPC.coldDamage = true;
                }
                if (captiveType == 1)
                {
                    NPC.alpha = 100;
                }
                if (captiveType == 2)
                {
                    NPC.damage += 20;
                    if (Main.expertMode)
                    {
                        NPC.damage += 20;
                    }
                }
                NPC.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.NPCDeath7, NPC.position);
            }
            //run away
            if ((!player.active || player.dead || player.position.Y + player.height < hellLayer * 16) && run < 2)
            {
                NPC.TargetClosest(false);
                player = Main.player[NPC.target];
                if (!player.active || player.dead || player.position.Y + player.height < hellLayer * 16)
                {
                    run = 1;
                }
                else
                {
                    run = 0;
                }
            }
            if (run > 0)
            {
                bool flag = true;
                if (run == 1)
                {
                    for (int k = 0; k < 200; k++)
                    {
                        if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("CaptiveElement2").Type && Main.npc[k].ai[2] == 0f)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    run = 2;
                    NPC.velocity = new Vector2(0f, 10f);
                    NPC.rotation = 0.5f * (float)Math.PI;
                    CreateDust();
                    if (NPC.timeLeft > 10)
                    {
                        NPC.timeLeft = 10;
                    }
                    NPC.netUpdate = true;
                    return;
                }
            }
            if (run < 2 && NPC.timeLeft < 750)
            {
                NPC.timeLeft = 750;
            }
            //move
            int count = 0;
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && Main.npc[k].type == Mod.Find<ModNPC>("CaptiveElement2").Type)
                {
                    count++;
                }
            }
            if (captiveType != 1 && captiveType != 4)
            {
                Vector2 moveTo = player.Center;
                if (captiveType == 0)
                {
                    moveTo.Y -= 320f;
                }
                if (captiveType == 2)
                {
                    moveTo.Y += 320f;
                }
                if (captiveType == 3)
                {
                    if (jungleAI < 0)
                    {
                        moveTo.X -= 320f;
                    }
                    else
                    {
                        moveTo.X += 320f;
                    }
                }
                float minX = moveTo.X - 50f;
                float maxX = moveTo.X + 50f;
                float minY = moveTo.Y;
                float maxY = moveTo.Y;
                if (captiveType == 0)
                {
                    minY -= 50f;
                }
                if (captiveType == 2)
                {
                    maxY += 50f;
                }
                if (captiveType == 3)
                {
                    minY -= 240f;
                    maxY += 240f;
                }
                if (NPC.Center.X >= minX && NPC.Center.X <= maxX && NPC.Center.Y >= minY && NPC.Center.Y <= maxY)
                {
                    NPC.velocity *= 0.98f;
                }
                else
                {
                    Vector2 move = moveTo - NPC.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    float speed = 10f;
                    if (captiveType == 3 && (jungleAI == -5 || jungleAI == 1))
                    {
                        speed = 8f;
                    }
                    if (magnitude > speed)
                    {
                        move *= speed / magnitude;
                    }
                    float inertia = 10f;
                    if (speed == 8f)
                    {
                        inertia = 20f;
                    }
                    NPC.velocity = (inertia * NPC.velocity + move) / (inertia + 1);
                    magnitude = (float)Math.Sqrt(NPC.velocity.X * NPC.velocity.X + NPC.velocity.Y + NPC.velocity.Y);
                    if (magnitude > speed)
                    {
                        NPC.velocity *= speed / magnitude;
                    }
                }
            }
            if (captiveType == 1)
            {
                Vector2 move = player.Center - NPC.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                if (magnitude > 3.5f)
                {
                    move *= 3.5f / magnitude;
                }
                NPC.velocity = move;
            }
            //look and shoot
            if (captiveType != 4)
            {
                LookToPlayer();
                attackCool -= 1f;
                if (difficulty > 1 && attackCool > 0f && attackCool <= 60f && (int)Math.Ceiling(attackCool) % 30 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Shoot();
                }
                if (attackCool <= 0f && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (captiveType == 3)
                    {
                        jungleAI++;
                        if (jungleAI == 0)
                        {
                            jungleAI = 1;
                        }
                        if (jungleAI == 6)
                        {
                            jungleAI = -5;
                        }
                    }
                    attackCool = 150f + 100f * (float)NPC.life / (float)NPC.lifeMax + (float)Main.rand.Next(50);
                    attackCool *= (float)count / 5f;
                    if (captiveType != 3 || (jungleAI != -5 && jungleAI != 1))
                    {
                        Shoot();
                    }
                    NPC.TargetClosest(false);
                    NPC.netUpdate = true;
                }
            }
            else
            {
                attackCool -= 1f;
                if (attackCool <= 0f)
                {
                    if (difficulty > 1)
                    {
                        Shoot();
                    }
                    attackCool = 80f + 40f * (float)NPC.life / (float)NPC.lifeMax;
                    attackCool *= (float)count / 5f;
                    NPC.TargetClosest(false);
                    LookToPlayer();
                    float speed = 12.5f - 2.5f * (float)NPC.life / (float)NPC.lifeMax;
                    NPC.velocity = speed * new Vector2((float)Math.Cos(NPC.rotation), (float)Math.Sin(NPC.rotation));
                    NPC.netUpdate = true;
                }
                else
                {
                    NPC.velocity *= 0.995f;
                }
            }
            CreateDust();
        }

        private void Shoot()
        {
            int damage = NPC.damage / 2;
            if (Main.expertMode)
            {
                damage = (int)(damage / Main.GameModeInfo.EnemyDamageMultiplier);
            }
            float speed = 5f;
            if (captiveType != 1)
            {
                speed = difficulty > 0 ? 9f : 7f;
            }
            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, speed * (float)Math.Cos(NPC.rotation), speed * (float)Math.Sin(NPC.rotation), Mod.Find<ModProjectile>("PixelBall").Type, damage, 3f, Main.myPlayer, GetDebuff(), GetDebuffTime());
        }

        private void CreateDust()
        {
            Color? color = GetColor();
            if (color.HasValue)
            {
                Vector2 unit = new Vector2((float)Math.Cos(NPC.rotation), (float)Math.Sin(NPC.rotation));
                Vector2 center = NPC.Center;
                for (int k = 0; k < 4; k++)
                {
                    int dust = Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("Pixel").Type, 0f, 0f, 0, color.Value);
                    Vector2 offset = Main.dust[dust].position - center;
                    offset.X = (offset.X - (float)NPC.width / 2f) / 2f;
                    Main.dust[dust].position = center + new Vector2(unit.X * offset.X - unit.Y * offset.Y, unit.Y * offset.X + unit.X * offset.Y);
                    Main.dust[dust].velocity += -3f * unit;
                    Main.dust[dust].rotation = NPC.rotation;
                    Main.dust[dust].velocity += NPC.velocity;
                }
            }
        }

        private void LookToPlayer()
        {
            Vector2 look = Main.player[NPC.target].Center - NPC.Center;
            float angle = 0.5f * (float)Math.PI;
            if (look.X != 0f)
            {
                angle = (float)Math.Atan(look.Y / look.X);
            }
            else if (look.Y < 0f)
            {
                angle += (float)Math.PI;
            }
            if (look.X < 0f)
            {
                angle += (float)Math.PI;
            }
            NPC.rotation = angle;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = captiveType * frameHeight;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                if (difficulty > 0)
                {
                    int next = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.position.Y + NPC.height * 3 / 4, Mod.Find<ModNPC>("FreedElement").Type);
                    Main.npc[next].ai[0] = captiveType;
                    Main.npc[next].netUpdate = true;
                }
                else
                {
                    Color? color = GetColor();
                    if (color.HasValue)
                    {
                        for (int k = 0; k < 75; k++)
                        {
                            Dust.NewDust(NPC.position, NPC.width, NPC.height, Mod.Find<ModDust>("PixelHurt").Type, 0f, 0f, 0, color.Value);
                        }
                    }
                }
            }
        }

        public override bool PreKill()
        {
            for (int k = 0; k < 200; k++)
            {
                if (Main.npc[k].active && k != NPC.whoAmI && Main.npc[k].type == NPC.type)
                {
                    return false;
                }
            }
            return true;
        }

        public override void OnKill()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                int centerX = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
                int centerY = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
                int halfLength = NPC.width / 2 / 16 + 1;
                for (int x = centerX - halfLength; x <= centerX + halfLength; x++)
                {
                    for (int y = centerY - halfLength; y <= centerY + halfLength; y++)
                    {
                        if ((x == centerX - halfLength || x == centerX + halfLength || y == centerY - halfLength || y == centerY + halfLength) && !Main.tile[x, y].HasTile)
                        {
                            WorldGen.PlaceTile(x, y, TileID.HellstoneBrick);
                        }
                        Main.tile[x, y].Get<LiquidData>().LiquidType = LiquidID.Water;
                        Main.tile[x, y].LiquidAmount = 0;
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1);
                        }
                        else
                        {
                            WorldGen.SquareTileFrame(x, y, true);
                        }
                    }
                }
            }
            BluemagicWorld.downedAbomination = true;
            if (Main.netMode != NetmodeID.MultiplayerClient && NPC.downedMoonlord)
            {
                BluemagicWorld.elementalUnleash = true;
                Bluemagic.NewText("Mods.Bluemagic.ElementalUnleash", 100, 220, 100);
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.ByCondition(new MLCondition(), Mod.Find<ModItem>("AbominationBag2").Type));
            npcLoot.Add(ItemDropRule.ByCondition(new BMLCondition(), Mod.Find<ModItem>("AbominationBag").Type));
            npcLoot.Add(ItemDropRule.Common(Mod.Find<ModItem>("AbominationTrophy").Type, 10));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("AbominationMask").Type, 7));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("MoltenDrill").Type));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("DimensionalChest").Type));
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), Mod.Find<ModItem>("MoltenBar").Type, 1, 5, 5));
            npcLoot.Add(ItemDropRule.ByCondition(new MLCondition(), ModContent.ItemType<PuriumOreGen>()));
            npcLoot.Add(new LeadingConditionRule(new MLCondition()).OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<ElementalYoyo>(), ModContent.ItemType<ElementalSprayer>(), ModContent.ItemType<EyeballTome>(), ModContent.ItemType<ElementalStaff>(), ModContent.ItemType<EyeballGlove>())));

        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            name = "The Abomination";
            potionType = ItemID.GreaterHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (captiveType == 2 && difficulty > 0)
            {
                cooldownSlot = 1;
            }
            return true;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            if (Main.expertMode || Main.rand.Next(2) == 0)
            {
                int debuff = GetDebuff();
                if (debuff >= 0)
                {
                    target.AddBuff(debuff, GetDebuffTime(), true);
                }
            }
        }

        public int GetDebuff()
        {
            switch (captiveType)
            {
                case 0:
                    return BuffID.Frostburn;
                case 1:
                    return Mod.Find<ModBuff>("EtherealFlames").Type;
                case 3:
                    return BuffID.Venom;
                case 4:
                    return BuffID.Ichor;
                default:
                    return -1;
            }
        }

        public int GetDebuffTime()
        {
            int time;
            switch (captiveType)
            {
                case 0:
                    time = 400;
                    break;
                case 1:
                    time = 300;
                    break;
                case 3:
                    time = 400;
                    break;
                case 4:
                    time = 600;
                    break;
                default:
                    return -1;
            }
            return time;
        }

        public Color? GetColor()
        {
            switch (captiveType)
            {
                case 0:
                    return new Color(0, 230, 230);
                case 1:
                    return new Color(0, 153, 230);
                case 3:
                    return new Color(0, 178, 0);
                case 4:
                    return new Color(230, 192, 0);
                default:
                    return null;
            }
        }

        public override void BossHeadSlot(ref int index)
        {
            if (captiveType > 0)
            {
                index = NPCHeadLoader.GetBossHeadSlot(Bluemagic.captiveElement2Head + captiveType);
            }
        }
    }

    public class MLCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return NPC.downedMoonlord;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Language.GetOrRegister("After defeating the Moon Lord").Value;
        }
    }

    public class BMLCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return !NPC.downedMoonlord;
        }

        public bool CanShowItemDropInUI()
        {
            return true;
        }

        public string GetConditionDescription()
        {
            return Language.GetOrRegister("Before defeating the Moon Lord").Value;
        }
    }
}