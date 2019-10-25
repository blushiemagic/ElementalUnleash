using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Bluemagic.Dusts
{
    public class Particle : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 2, 2);
            dust.rotation = Main.rand.NextFloat() * MathHelper.Pi;
        }

        public override bool Update(Dust dust)
        {
            const float speed = 0.5f;
            Vector2 goal = ((NPC)dust.customData).Center + new Vector2(0f, 15f);
            Vector2 offset = dust.position - goal;
            float radius = offset.Length();
            if (radius < speed)
            {
                dust.active = false;
                return false;
            }
            offset.Normalize();
            float rotation = offset.ToRotation();
            radius -= speed;
            rotation += 0.025f;
            dust.position = goal + radius * rotation.ToRotationVector2();
            return false;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return Color.White;
        }
    }
}