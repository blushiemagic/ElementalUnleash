using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Bluemagic.Tiles
{
    public class TEElementalPurge : ModTileEntity
    {
        private const int range = 100;

        public override void Update()
        {
            int i = Position.X + Main.rand.Next(-range, range + 1);
            int j = Position.Y + Main.rand.Next(-range, range + 1);
            WorldGen.Convert(i, j, 0, 0, false, false);
        }

        public override bool IsTileValidForEntity(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            return tile.HasTile && tile.TileType == ModContent.TileType<ElementalPurge>() && tile.TileFrameX == 0 && tile.TileFrameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i - 1, j - 2, 2, 3);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - 1, j - 2, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i - 1, j - 2);
        }
    }
}