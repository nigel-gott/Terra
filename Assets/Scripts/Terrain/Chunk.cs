using System;
using UnityEngine;

namespace NigelGott.Terra.Terrain
{
    public class Chunk
    {
        public readonly float[,] heights;

        public readonly int X, Y;

        public int Size
        {
            get { return heights.GetLength(0); }
        }

        public Vector2 Coord
        {
            get
            {
                return new Vector2(X,Y);
            }
        }

        public Chunk(float[,] heights, int y, int x)
        {
            if (heights.Rank != 2 || heights.GetLength(0) != heights.GetLength(1))
            {
                throw new ArgumentException("Cannot construct a non square Chunk");
            }
            this.heights = heights;
            this.Y = y;
            this.X = x;
        }
    }
}