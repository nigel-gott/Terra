using System;

namespace NigelGott.Terra.Terrain
{
    public class TerrainTile
    {
        public readonly float[,] heights;

        public readonly int X, Y;

        public int Size
        {
            get { return heights.GetLength(0); }
        }

        public TerrainTile(float[,] heights, int y, int x)
        {
            if (heights.Rank != 2 || heights.GetLength(0) != heights.GetLength(1))
            {
                throw new ArgumentException("Cannot construct a non square TerrainTile");
            }
            this.heights = heights;
            this.Y = y;
            this.X = x;
        }
    }
}