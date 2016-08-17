using System;

namespace NigelGott.Terra.Terrain
{
    public class TerrainTile
    {
        public readonly float[,] heights;

        public int Size
        {
            get { return heights.GetLength(0); }
        }

        public TerrainTile(float[,] heights)
        {
            if (heights.Rank != 2 || heights.GetLength(0) != heights.GetLength(1))
            {
                throw new ArgumentException("Cannot construct a non square TerrainTile");
            }
            this.heights = heights;
        }
    }
}