using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Bluemagic.TerraSpirit
{
    public abstract class Bullet
    {
        public abstract bool Update(TerraSpirit spirit, Rectangle bounds);

        public abstract bool Collides(Rectangle box);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}