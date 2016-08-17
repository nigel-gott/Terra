namespace NigelGott.Terra.Terrain
{
    public class TerrainState
    {
        private readonly TerrainTile[,] tiles;

        public TerrainState(TerrainTile[,] tiles)
        {
            this.tiles = tiles;
        }
    }
}